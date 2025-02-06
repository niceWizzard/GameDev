#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class StateMachine<T, K> : MonoBehaviour,  IStateMachineInitializer
    where T : MonoBehaviour where K : Enum
{
    [SerializeField]
    private State<T, K>? currentState = null;
    public State<T,K>? CurrentState { get { return currentState; } }

    public Dictionary<K, State<T,K>> StatesMap { get; private set; } =  new();
    
    public T? controller;
    public List<State<T, K>> AllStates => StatesMap.Values.ToList();

    protected virtual void Start()
    {
        InitializeStates();
        AllStates.ForEach(state => state.Initialize(controller, this));
        AllStates.ForEach(state => state.Prepare());
    }

    void Update()
    {
        currentState?.Do();
    }

    private void FixedUpdate()
    {
       currentState?.FixedDo();
    }

    public abstract void InitializeStates();
    public void ChangeState(K newState)
    {
        var s = StatesMap[newState];
        currentState?.Exit();
        currentState = s;
        currentState?.Enter();
    }
}


public interface IStateMachineInitializer
{
    void InitializeStates();
}
