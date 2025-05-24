using Main.Lib.FSM;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main.Player.States
{
    public class MoveState : State<PlayerFsm, PlayerController>
    {
        private Vector2 _input;
        public override void OnEnter()
        {
            base.OnEnter();
            Agent.Animator.Play("MoveGun");
            Agent.WalkAudioSource.pitch = Random.Range(0.9f, 1.1f);
            Agent.WalkAudioSource.Play();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            Executor.ProcessAttackInputs();
            _input = Agent.GetMovementInput();
            Agent.RotateGun();
        }

        public override void OnExit()
        {
            base.OnExit();
            if(Agent.WalkAudioSource.isPlaying)
                Agent.WalkAudioSource.Stop();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            var vel = Agent.Velocity;
            vel = Vector2.Lerp(vel, _input.normalized * Agent.MovementSpeed, Agent.friction);
            Agent.Velocity = vel;
        }
    }
}