//alex@bardicbytes.com

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BardicBytes.BardicUnity
{
    public class UpdateAggregator : MonoBehaviour
    {
        public enum AggregationMode { Manual = 0, AutoSelf = 1, AutoScene = 1}

        [field:SerializeField]
        [field:Tooltip("AutoSelf/AutoScene = empty receivers arrays are automatically populated. ")]
        public AggregationMode Mode { get; private set; }

        [SerializeField]
        private List<MonoBehaviour> serializedReceivers = new List<MonoBehaviour>();

        private List<IManagedUpdateReceiver> updateReceivers = new List<IManagedUpdateReceiver>();
        private List<IManagedFixedUpdateReceiver> fixedUpdateReceivers = new List<IManagedFixedUpdateReceiver>();
        private List<IManagedLateUpdateReceiver> lateUpdateReceivers = new List<IManagedLateUpdateReceiver>();

        private Queue<ISortableBehaviour> toRemove = new Queue<ISortableBehaviour>();

        /// <summary>
        /// receivers added to the list will have their managed update method called automatically
        /// </summary>
        public void AddReceiver<T>(T receiver) where T : ISortableBehaviour
        {
            // Determine the correct list based on the type of T
            List<T> receiverList = GetReceiverList<T>();

            if (receiverList == null)
            {
                Debug.LogWarning($"Unsupported receiver type: {typeof(T)}");
                return;
            }

            if (receiverList.Contains(receiver))
            {
                Debug.LogWarning($"Attempted to add a duplicate receiver of type {typeof(T)}.");
                return;
            }

            int index = receiverList.FindIndex(r => r.SortValue > receiver.SortValue);
            if (index < 0) receiverList.Add(receiver); // Add to the end if no larger Sort value is found
            else receiverList.Insert(index, receiver);
        }

        private List<T> GetReceiverList<T>() where T : ISortableBehaviour
        {
            if (typeof(T) == typeof(IManagedUpdateReceiver))
                return updateReceivers as List<T>;
            if (typeof(T) == typeof(IManagedFixedUpdateReceiver))
                return fixedUpdateReceivers as List<T>;
            if (typeof(T) == typeof(IManagedLateUpdateReceiver))
                return lateUpdateReceivers as List<T>;

            return null; // or throw an exception if unsupported type
        }

        public void RemoveReceiver<T>(T receiver) where T : ISortableBehaviour
        {
            // Determine the correct list based on the type of T
            List<T> receiverList = GetReceiverList<T>();

            if (receiverList == null)
            {
                Debug.LogWarning($"Unsupported receiver type: {typeof(T)}");
                return;
            }

            if (!receiverList.Remove(receiver)) Debug.LogWarning($"Attempted to remove an {typeof(T)} that was not in the list.");
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            for(int i = 0; i < serializedReceivers.Count; i++)
            {
                if (serializedReceivers[i] != null) continue;
                if (!(serializedReceivers[i] is ISortableBehaviour)) continue;

                serializedReceivers.RemoveAt(i);
                i--;
            }

            if (Mode == AggregationMode.Manual) return;
            else if (Mode == AggregationMode.AutoSelf) RefreshReceiversSelf();
            else if (Mode == AggregationMode.AutoScene) RefreshReceiversScene();

            ExtendedOnValidate();
        }

        protected virtual void ExtendedOnValidate() { }

        [ContextMenu("Add New Receivers from Self")]
        public void RefreshReceiversSelf()
        {
            var found = GetComponentsInChildren(typeof(ISortableBehaviour),true);

            for (int i = 0; i < found.Length; i++)
            {
                if (found[i] is ISortableBehaviour r)
                {
                    if (serializedReceivers.Contains(r as MonoBehaviour)) continue;
                    serializedReceivers.Add(r as MonoBehaviour);
                }
            }

            SortSerializedReceivers();
        }

        /// <summary>
        /// Searches the entire scene for IManagedReceiver components and puts them in the receivers list.
        /// </summary>
        [ContextMenu("Add New Receivers from Scene")]
        public void RefreshReceiversScene()
        {
            var found = FindObjectsOfType(typeof(ISortableBehaviour));
            // if we don't find any receivers with their own aggregators, we can use the original array
            
            for(int i = 0; i < found.Length; i++)
            {
                if (!(found[i] is ISortableBehaviour)) continue;
                var r = (ISortableBehaviour)found[i];

                //avoid adding receivers in the scene if the receiver is parented to an update aggregator
                if (r.gameObject.GetComponentInParent<UpdateAggregator>() != null) continue;

                if (serializedReceivers.Contains(r as MonoBehaviour)) continue;

                serializedReceivers.Add(r as MonoBehaviour);
            }

            SortSerializedReceivers();
        }

        private void SortSerializedReceivers()
        {
            serializedReceivers.Sort((a, b) =>
            {
                var ra = a as ISortableBehaviour;
                var rb = b as ISortableBehaviour;
                if (ra != null && rb != null)
                {
                    return ra.SortValue.CompareTo(rb.SortValue);
                }
                return 0;
            });
        }
#endif

        private void Awake()
        {
            for(int i =0; i < serializedReceivers.Count; i++)
            {
                var r = serializedReceivers[i];
                if (r is IManagedFixedUpdateReceiver fur) fixedUpdateReceivers.Add(fur);
                if (r is IManagedLateUpdateReceiver lur) lateUpdateReceivers.Add(lur);
                if (r is IManagedUpdateReceiver ur) updateReceivers.Add(ur);
            }
        }

        private void Update()
        {
            UpdateAnyReceivers(updateReceivers, (IManagedUpdateReceiver receiver) => { receiver.ManagedUpdate(); });
            UpdateAnyReceivers(fixedUpdateReceivers, (IManagedFixedUpdateReceiver receiver) => { receiver.ManagedFixedUpdate(); });
            UpdateAnyReceivers(lateUpdateReceivers, (IManagedLateUpdateReceiver receiver) => { receiver.ManagedLateUpdate(); });

            while (toRemove.Count > 0)
            {
                var r = toRemove.Dequeue();
                if (r is IManagedFixedUpdateReceiver fur) fixedUpdateReceivers.Remove(fur);
                if (r is IManagedLateUpdateReceiver lur) lateUpdateReceivers.Remove(lur);
                if (r is IManagedUpdateReceiver ur) updateReceivers.Remove(ur);
            }
        }

        private void UpdateAnyReceivers<T>(List<T> receivers, Action<T> updateAction) where T : ISortableBehaviour
        {
            for (int i = 0; i < receivers.Count; i++)
            {
                if (receivers[i] == null)
                {
                    toRemove.Enqueue(receivers[i]);
                    continue;
                }

                if (!receivers[i].isActiveAndEnabled) continue;
                updateAction.Invoke(receivers[i]);
            }
        }
    }
}
