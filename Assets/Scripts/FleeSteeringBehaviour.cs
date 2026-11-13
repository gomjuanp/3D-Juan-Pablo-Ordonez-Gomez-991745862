using UnityEngine;

public class FleeSteeringBehaviour : SteeringBehaviourBase
{
    public Transform enemyTarget;
    public float fleeDistance = 10.0f;
    public bool shouldReachGoal = true;

    private Vector3 desiredVelocity;
    private bool showGizmoArrows = true;

    public override Vector3 CalculateForce()
    {
        if (enemyTarget != null)
        {
            target = enemyTarget.position;
        }

        float distance = (transform.position - target).magnitude;
        if (distance > fleeDistance)
        {
            steeringAgent.reachedGoal = shouldReachGoal;
            showGizmoArrows = false;
            return Vector3.zero;
        }

        steeringAgent.reachedGoal = false;
        showGizmoArrows = true;

        desiredVelocity = (transform.position - target).normalized;
        desiredVelocity *= steeringAgent.maxSpeed;
        return (desiredVelocity - steeringAgent.velocity);
    }

    private void OnDrawGizmos()
    {
        if (!showGizmoArrows || steeringAgent == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, desiredVelocity);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, steeringAgent.velocity);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(target, fleeDistance);
    }
}