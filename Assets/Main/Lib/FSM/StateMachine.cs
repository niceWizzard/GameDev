#nullable  enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Lib.FSM
{
    public abstract class StateMachine<T> : MonoBehaviour where T : MonoBehaviour
    {
        private Dictionary<Type, State<T>> _statesMap = new();
        private List<Transition> _transitions = null!;

        public State<T>? CurrentState { get; private set; }

        /// <summary>
        /// Sets the required values for the StateMachine
        /// </summary>
        /// <param name="agent">The object which this StateMachine is attached to. ex. Player</param>
        /// <param name="stateTypes">The list types of the States for this StateMachine</param>
        /// <param name="transitions">The list of transitions</param>
        /// <param name="initialStateType">The initial state to set</param>
        /// <param name="executor">The StateMachine itself (this)</param>
        public void Setup(
            GameObject agent, 
            List<Type> stateTypes, 
            List<Transition> transitions, 
            Type initialStateType,
            T executor
        )
        {
            _transitions = transitions;

            // Instantiate and setup states
            foreach (var stateType in stateTypes)
            {
                if (Activator.CreateInstance(stateType) is State<T> stateInstance)
                {
                    stateInstance.Setup(agent, executor);
                    _statesMap[stateType] = stateInstance;
                }
                else
                {
                    Debug.LogError($"Failed to create instance of {stateType.Name}");
                }
            }

            // Set initial state
            SetState(initialStateType);
        }

        protected virtual void Update()
        {
            if (CurrentState == null) return;

            // Check for valid transition
            foreach (var transition in _transitions)
            {
                if (!transition.CanTransitionFrom(CurrentState.GetType()) || !transition.Condition()) 
                    continue;

                SetState(transition.To);
                return;
            }

            // Continue executing current state
            CurrentState.OnUpdate();
        }
        
        public void SetState(Type newStateType)
        {
            if (!_statesMap.TryGetValue(newStateType, out var newState))
            {
                Debug.LogError($"State {newStateType.Name} not found in the state machine.");
                return;
            }

            CurrentState?.OnExit();
            CurrentState = newState;
            CurrentState?.OnEnter();
        }
    }
}
