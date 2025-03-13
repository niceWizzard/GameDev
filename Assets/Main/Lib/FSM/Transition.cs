using System;
using System.Collections.Generic;

namespace Main.Lib.FSM
{
    public class Transition
    {
        public HashSet<Type> FromStates { get; }
        public Type To { get; }
        public Func<bool> Condition { get; }

        private Transition(IEnumerable<Type> fromStates, Type to, Func<bool> condition)
        {
            FromStates = new HashSet<Type>(fromStates);
            To = to;
            Condition = condition;
        }

        public bool CanTransitionFrom(Type stateType)
        {
            return FromStates.Contains(stateType) || FromStates.Contains(States.AnyState);
        }

        public static Transition Create(Type fromState, Type to, Func<bool> condition)
        {
            return new Transition(new [] {fromState}, to, condition);
        }

        public static Transition MultiFrom(Type to, Func<bool> condition, params Type[] fromStates)
        {
            return new Transition(fromStates, to, condition);
        }
    }
}