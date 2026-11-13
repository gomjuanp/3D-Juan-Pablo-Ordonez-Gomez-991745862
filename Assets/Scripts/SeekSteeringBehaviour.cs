using UnityEngine;

public class SeekSteeringBehaviour : SteeringBehaviourBase
{
    public Transform seekTarget;
    public float stopDistance = 1.5f;
    public bool shouldReachGoal = true;

    protected Vector3 desiredVelocity;

    public override Vector3 CalculateForce()
    {
        if (seekTarget != null)
        {
            target = seekTarget.position;
        }

        Vector3 toTarget = target - transform.position;
        toTarget.y = 0.0f;
        float distance = toTarget.magnitude;

        if (shouldReachGoal && distance <= stopDistance)
        {
            steeringAgent.reachedGoal = true;
            desiredVelocity = Vector3.zero;
            return Vector3.zero;
        }

        if (shouldReachGoal)
        {
            steeringAgent.reachedGoal = false;
        }

        desiredVelocity = (target - transform.position).normalized;
        desiredVelocity = desiredVelocity * steeringAgent.maxSpeed;
        return (desiredVelocity - steeringAgent.velocity);
    }

    protected virtual void OnDrawGizmos()
    {
        if (steeringAgent != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, desiredVelocity);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, steeringAgent.velocity);
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(target, 0.2f);
        }
    }
}