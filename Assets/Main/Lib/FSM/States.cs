using System;
using UnityEngine;

namespace Main.Lib.FSM
{
    /// <summary>
    /// This is a base state for the Finite State Machine (FSM) 
    /// </summary>
    /// <typeparam name="T">the script which inherits from <see cref="StateMachine"/></typeparam>
    public abstract class State<T> where T : MonoBehaviour
    {
        protected T Executor { get; private set; } = null!;

        public void Setup(Component agent, T executor)
        {
            Executor = executor;
            OnSetup(agent, executor);
        }

        /// <summary>
        /// Use this to setup the needed variables/components in your state
        /// </summary>
        /// <param name="agent">The object which the FSM is attached to.</param>
        /// <param name="executor">The StateMachine</param>
        public virtual void OnSetup(Component agent, T executor) { }
        /// <summary>
        /// The Update Loop
        /// </summary>
        public virtual void OnUpdate() { }
        /// <summary>
        /// Called when the State enters
        /// </summary>
        public virtual void OnEnter() { }
        /// <summary>
        /// Called when the State exits
        /// </summary>
        public virtual void OnExit() { }
    }

    public static class States
    {
        private sealed class AnyStatePlaceholder { }
        public static readonly Type AnyState = typeof(AnyStatePlaceholder);
    }
}