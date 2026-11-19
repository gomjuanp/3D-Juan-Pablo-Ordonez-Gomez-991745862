using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints;
    public float angularDampeningTime = 5.0f;
    public float deadZone = 10.0f;

    private NavMeshAgent agent;
    private Animator animator;
    private int currentWaypointIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.updateRotation = false;

        if (waypoints.Count > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    void Update()
    {
        if (waypoints.Count == 0) return;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }

        if (agent.desiredVelocity != Vector3.zero)
        {
            float speed = Vector3.Project(agent.desiredVelocity, transform.forward).magnitude;
            animator.SetFloat("Speed", speed);

            float angle = Vector3.Angle(transform.forward, agent.desiredVelocity);
            if (Mathf.Abs(angle) <= deadZone)
            {
                transform.LookAt(transform.position + agent.desiredVelocity);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(agent.desiredVelocity),
                    Time.deltaTime * angularDampeningTime);
            }
        }
        else
        {
            animator.SetFloat("Speed", 0.0f);
        }
    }

    private void OnAnimatorMove()
    {
        if (Time.deltaTime > 0.0f)
        {
            agent.velocity = animator.deltaPosition / Time.deltaTime;
        }
    }
}