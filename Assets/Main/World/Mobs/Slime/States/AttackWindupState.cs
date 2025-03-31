using System.Collections;
using Main.Lib.FSM;
using UnityEngine;

namespace Main.World.Mobs.Slime.States
{
    public class AttackWindupState: State<SlimeFsm, SlimeController>
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Executor.AttackWindupStarted = true;
            Executor.AttackWindupFinished = false;
            Agent.Velocity *= 0;
            Executor.StartCoroutine(AttackWindupTimer());
        }

        public override void OnExit()
        {
            base.OnExit();
            Executor.AttackWindupFinished = false;
            Executor.AttackWindupStarted = false;
        }

        private IEnumerator AttackWindupTimer()
        {
            yield return new WaitForSeconds(1f);
            Executor.AttackWindupFinished = true;
        }
    }
}