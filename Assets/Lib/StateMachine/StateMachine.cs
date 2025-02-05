#nullable enable

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine<T> : MonoBehaviour where T : MonoBehaviour
{
    private List<State<T>> states;
    [SerializeField]
    private State<T>? currentState = null;
    public State<T>? CurrentState { get { return currentState; } }

    
    public T? controller;

    private void SetState(State<T>? newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        if (currentState != null) {
            currentState.Enter();
        }
    }

    void Start()
    {
        states = GetComponentsInChildren<State<T>>().ToList();
        states.ForEach(state => state.Initialize(controller));
        states.ForEach(state => state.Prepare());
    }

    void Update()
    {
        var next = currentState?.Do();
        if (next != null)
        {
            SetState(next);
        }
    }

    private void FixedUpdate()
    {
        var next = currentState?.FixedDo();
        if (next != null)
        {
            SetState(next);
        }
    }
}
