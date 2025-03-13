using Main.Lib.FSM;
using UnityEngine;

namespace Main.World.Mobs.Ghost
{
    public class ChaseState : State<GhostFsm>
    {
        private GhostController _ghost;
        

        public override void OnSetup(GameObject agent, GhostFsm executor)
        {
            base.OnSetup(agent, executor);
            _ghost = agent.GetComponent<GhostController>();
        }

        public override void OnExit()
        {
            base.OnExit();
            Debug.Log("EXIT!" + _ghost.detectedPlayer);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            Vector2 toPlayer = (_ghost.detectedPlayer.transform.position - _ghost.transform.position);
            var dir = toPlayer.normalized;
            _ghost.Rigidbody2D.linearVelocity = dir * _ghost.MovementSpeed;
        }
    }
}