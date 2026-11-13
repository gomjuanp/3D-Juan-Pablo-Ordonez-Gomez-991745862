using UnityEngine;

public class SeekSteeringBehaviour : SteeringBehaviourBase
{
    public Transform seekTarget;
    protected Vector3 desiredVelocity;

    public override Vector3 CalculateForce()
    {
        if (seekTarget != null)
        {
            target = seekTarget.position;
        }
        else
        {
            CheckMouseInput();
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