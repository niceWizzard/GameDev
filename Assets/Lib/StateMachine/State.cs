using System;
using UnityEditor.UI;
using UnityEngine;
#nullable enable

public class State<T> : MonoBehaviour where T : MonoBehaviour
{
    private T? controller;
    public virtual void Initialize(T? controller)
    {
        this.controller = controller;
    }
    public virtual void Prepare()
    {

    }

    public virtual State<T>? Do()
    {
        return null;
    }

    public virtual State<T>? FixedDo()
    {
        return null;
    }

    public virtual void Exit()
    {
    }

    public virtual void Enter()
    {
    }
}
