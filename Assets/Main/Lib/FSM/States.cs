using System;
using UnityEngine;

namespace Main.Lib.FSM
{
    public abstract class State<T> where T : MonoBehaviour
    {
        protected T Executor { get; private set; } = null!;

        public void Setup(GameObject agent, T executor)
        {
            Executor = executor;
            OnSetup(agent, executor);
        }

        public virtual void OnSetup(GameObject agent, T executor) { }
        public virtual void OnUpdate() { }
        public virtual void OnEnter() { }
        public virtual void OnExit() { }
    }

    public static class States
    {
        private sealed class AnyStatePlaceholder { }
        public static readonly Type AnyState = typeof(AnyStatePlaceholder);
    }
}