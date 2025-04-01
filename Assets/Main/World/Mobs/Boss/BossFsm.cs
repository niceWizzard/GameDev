using System;
using System.Collections.Generic;
using Main.Lib.FSM;
using Main.World.Mobs.Boss;
using UnityEngine;

public class BossFsm : StateMachine<BossFsm, BossController>
{
    [SerializeField] private BossController bossController;
    private void Start()
    {
        
        var idle = typeof(IdleState);
        var states = new List<Type>()
        {
            idle,
        };
        
        var transitions = new List<Transition>();
        Setup(
            bossController,
            states,
            transitions,    
            idle,
            this
        );
    }
}
