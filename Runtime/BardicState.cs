//alex@bardicbytes.com

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BardicBytes.BardicUnity
{
    public class BardicState : MonoBehaviour
    {
        /*
         * Serialized/Inspector AutoProeprties
         */
        [field:Header("Transition Config")]
        [field: SerializeField]
        public bool ExitOnInitialize { get; protected set; } = true;
        [field: SerializeField]
        public UnityEvent OnEntered { get; protected set; } = default;
        
        [field: SerializeField]
        public UnityEvent OnExited { get; protected set; } = default;

        //[field:Space]
        //[field:SerializeField]
        //public List<BardicState> ValidTransitions { get; protected set; } = default;

        [Space]
        [Tooltip("Components in this list will be enabled when the state is entered and disabled when the state is exited.")]
        [SerializeField]
        protected List<Component> enabledWhenEntered = new List<Component>();

        /*
         * Public methods
         */
        public void Initialize()
        {
            PreInitialize();
            if (ExitOnInitialize) Exit();
        }
        public bool IsEntered { get; protected set; } = false;

        //public virtual BardicState Next => ValidTransitions[0];

        //public BardicState GetValidTransitonState(int index) => ValidTransitions[index];
        //public int ValidTransitionsLength => ValidTransitions.Count;

        //public bool IsValidTransition(BardicState state) => ValidTransitions.Contains(state);

        public void Add(Component component)
        {
            if (enabledWhenEntered.Contains(component)) throw new System.Exception("component already in list");
            enabledWhenEntered.Add(component);
        }

        public void Remove(Component component)
        {
            if (!enabledWhenEntered.Contains(component)) throw new System.Exception("component not in list");
            enabledWhenEntered.Remove(component);
        }

        /// <summary>
        /// Called by BardicStateMachine when the state begins entering
        /// </summary>
        public void Enter()
        {
            PreEnter();
            IsEntered = true;
            OnEntered?.Invoke();
        }

        /// <summary>
        /// Called by BardicStateMachine when the state begins exiting
        /// </summary>
        public void Exit()
        {
            PreExit();
            IsEntered = false;
            OnExited?.Invoke();
        }

        /*
         * protection methods
         */

        //for extensibility
        protected virtual void PreInitialize() { }
        protected virtual void PreEnter() { }
        protected virtual void PreExit() { }
    }
}
