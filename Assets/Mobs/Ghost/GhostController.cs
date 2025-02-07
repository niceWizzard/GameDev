using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GhostController : MobController
{
    [SerializeField] private Rigidbody2D rigidbody2d;
    
    public Rigidbody2D Rigidbody2D => rigidbody2d;
    
}
