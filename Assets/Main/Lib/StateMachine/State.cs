#nullable enable

using System;
using UnityEngine;

namespace Main.Lib.StateMachine
{
    public class State<T, K> : MonoBehaviour 
        where T : MonoBehaviour
        where K : Enum
    {
        protected T? controller;
        protected StateMachine<T, K>? sm; 
        public virtual void Initialize(T? controller, StateMachine<T, K>? stateMachine)
        {
            this.controller = controller;
            sm = stateMachine;
        }
        public virtual void Prepare()
        {

        }

        public virtual void Do()
        {
        }

        public virtual void FixedDo()
        {
        }

        public virtual void Exit()
        {
        }

        public virtual void Enter()
        {
        }

        protected void ChangeState(K state)
        {
            sm.ChangeState(state);
        }
    }
}
