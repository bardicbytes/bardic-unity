//alex@bardicbytes.com

using System.Collections.Generic;
using UnityEngine;

namespace BardicBytes.BardicUnity
{
    /// <summary>
    /// BardicBytes Finite State Machine
    /// </summary>
    public class BardicStateMachine : MonoBehaviour
    {
        [SerializeField] private List<BardicState> states;

        private int currentStateIndex;
        //private Dictionary<int, int[]> validTransitions;

        private BardicState CurrentState => states[currentStateIndex];

        private void OnValidate()
        {
            if(states == null || states.Count == 0) states = new List<BardicState>(GetComponentsInChildren<BardicState>());
        }

        private void Awake()
        {
            //validTransitions = new Dictionary<int, int[]>();
            for (int stateIndex = 0; stateIndex < states.Count; stateIndex++)
            {
                var s = states[stateIndex];

                s.Initialize();

                //if (!validTransitions.ContainsKey(stateIndex))
                //{
                //    validTransitions.Add(stateIndex, new int[s.ValidTransitionsLength]);
                //}

                //for(int transIndex = 0; transIndex < s.ValidTransitionsLength; transIndex++)
                //{
                //    var ts = s.GetValidTransitonState(transIndex);
                //    var tsi = states.IndexOf(ts);

                //    validTransitions[stateIndex][transIndex] = tsi;
                //}
            }
        }

        private void Start()
        {
            CurrentState.Enter();
        }

        public void GoToNextState()
        {
            CurrentState.Exit();
            currentStateIndex++;
            CurrentState.Enter();
        }

        public void TransitionTo(BardicState state)
        {
            //if (Debug.isDebugBuild) Debug.Assert(CurrentState.IsValidTransition(allStates));
            CurrentState.Exit();
            currentStateIndex = states.IndexOf(state);
            CurrentState.Enter();
        }
    }
}
