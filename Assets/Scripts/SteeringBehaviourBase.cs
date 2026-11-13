using UnityEngine;

public abstract class SteeringBehaviourBase : MonoBehaviour
{
    public float weight = 1.0f;
    public Vector3 target = Vector3.zero;
    public abstract Vector3 CalculateForce();

    [HideInInspector] public SteeringAgent steeringAgent;
}