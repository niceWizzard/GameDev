using UnityEngine;
using UnityEngine.Serialization;

public enum GhostState
{
    Patrolling,
    HasTarget,
    LostTarget,
}



public class GhostStateMachine : StateMachine<GhostController, GhostState>
{
    [FormerlySerializedAs("patrollingState")] [SerializeField] private PatrollingState patrollingState;
    [SerializeField] private State<GhostController, GhostState> hasTargetState;
    [SerializeField] private State<GhostController, GhostState> lostTargetState;

    public override void InitializeStates()
    {
        StatesMap.Add(GhostState.Patrolling, patrollingState);
        // StatesMap.Add(GhostState.HasTarget, hasTargetState);
        // StatesMap.Add(GhostState.LostTarget, lostTargetState);
    }
}
