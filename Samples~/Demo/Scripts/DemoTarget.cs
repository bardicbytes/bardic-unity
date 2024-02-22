//alex@bardicbytes.com

using BardicBytes.BardicUnity;
using UnityEngine;

namespace BardicBytes.BardicUnitySamples
{
    /// <summary>
    /// Examples for BardicState/BardicStateMachine and UpdateAggregator
    /// </summary>
    [RequireComponent(typeof(BardicState))]
    public class DemoTarget : MonoBehaviour, IManagedUpdateReceiver
    {
        public Renderer target;
        public int offset = 1;
        public bool subscribeToStateOnAwake = false;
        private float startTime = -1;

        public int SortValue => 0;

        private void OnValidate()
        {
            if(target == null) target = GetComponent<Renderer>();
        }

        private void Awake()
        {
            var state = GetComponent<BardicState>();
            if (state == null) return;

            if (subscribeToStateOnAwake)
            {
                // Subscribe to BardicState events
                state.OnEntered.AddListener(HandleStateEntered);
                state.OnExited.AddListener(HandleStateExited);
            }
        }

        public void HandleStateEntered() => enabled = true;
        public void HandleStateExited() => enabled = false;

        private void OnEnable() => startTime = Time.time;

        public void ManagedUpdate()
        {
            // target's color changes overtime
            target.material.color = new Color(
                Mathf.Sin(offset + startTime + Time.realtimeSinceStartup),
                Mathf.Sin(offset + startTime + Time.realtimeSinceStartup * 2f),
                Mathf.Sin(offset + startTime + Time.realtimeSinceStartup * 1.5f));
        }
    }
}