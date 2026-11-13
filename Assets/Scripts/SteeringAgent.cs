using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SteeringAgent : MonoBehaviour
{
    public float mass = 1.0f;
    public float maxSpeed = 3.0f;
    public float maxForce = 10.0f;
    public Vector3 velocity = Vector3.zero;

    private List<SteeringBehaviourBase> steeringBehaviours = new List<SteeringBehaviourBase>();

    public float angularDampeningTime = 5.0f;
    public float deadZone = 10.0f;

    public bool reachedGoal = false;
    public bool disable_Y_Velocity = true;
    public bool useRootMotion = true;
    public bool useGravity = true;

    private Animator animator;
    private CharacterController characterController;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            useRootMotion = false;
        }

        characterController = GetComponent<CharacterController>();

        steeringBehaviours.AddRange(GetComponentsInChildren<SteeringBehaviourBase>());
        foreach (SteeringBehaviourBase behaviour in steeringBehaviours)
        {
            behaviour.steeringAgent = this;
        }
    }

    private Vector3 CalculateSteeringForce()
    {
        Vector3 totalForce = Vector3.zero;

        foreach (SteeringBehaviourBase behaviour in steeringBehaviours)
        {
            if (behaviour.enabled)
            {
                totalForce = totalForce + (behaviour.CalculateForce() * behaviour.weight);
                totalForce = Vector3.ClampMagnitude(totalForce, maxForce);
            }
        }
        return totalForce;
    }

    private void Update()
    {
        Vector3 steeringForce = CalculateSteeringForce();

        if (disable_Y_Velocity)
        {
            steeringForce.y = 0.0f;
        }

        if (reachedGoal == true)
        {
            velocity = Vector3.zero;
            if (animator != null)
            {
                animator.SetFloat("Speed", 0.0f);
            }
        }
        else
        {
            Vector3 acceleration = steeringForce / mass;
            velocity = velocity + (acceleration * Time.deltaTime);
            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

            float speed = velocity.magnitude;
            if (animator != null)
            {
                animator.SetFloat("Speed", speed);
            }

            if (useRootMotion == false)
            {
                if (characterController != null)
                {
                    characterController.Move(velocity * Time.deltaTime);
                }
                else
                {
                    transform.position += (velocity * Time.deltaTime);
                }

                if (useGravity == true && characterController != null)
                {
                    characterController.Move(Physics.gravity * Time.deltaTime);
                }
            }

            if (velocity.magnitude > 0.0f)
            {
                Vector3 vector3 = new Vector3(velocity.x, 0.0f, velocity.z);
                float angle = Vector3.Angle(transform.forward, vector3);

                if (Mathf.Abs(angle) <= deadZone)
                {
                    transform.LookAt(transform.position + vector3);
                }
                else if (vector3 != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation,
                        Quaternion.LookRotation(vector3),
                        Time.deltaTime * angularDampeningTime);
                }
            }
        }
    }

    private void OnAnimatorMove()
    {
        if (Time.deltaTime > 0.0f && useRootMotion == true)
        {
            Vector3 animationVelocity = animator.deltaPosition / Time.deltaTime;

            if (characterController != null)
            {
                characterController.Move((transform.forward * animationVelocity.magnitude) * Time.deltaTime);
            }
            else
            {
                transform.position += (transform.forward * animationVelocity.magnitude) * Time.deltaTime;
            }

            if (useGravity == true && characterController != null)
            {
                characterController.Move(Physics.gravity * Time.deltaTime);
            }
        }
    }
}