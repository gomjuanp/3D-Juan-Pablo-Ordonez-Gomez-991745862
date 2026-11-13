using UnityEngine;
using UnityEngine.InputSystem;

public abstract class SteeringBehaviourBase : MonoBehaviour
{
    public float weight = 1.0f;
    public Vector3 target = Vector3.zero;
    public abstract Vector3 CalculateForce();

    [HideInInspector] public SteeringAgent steeringAgent;

    public bool useMouseInput = true;
    protected bool mouseClicked = false;
    protected bool checkMouseClick = true;

    protected void CheckMouseInput()
    {
        mouseClicked = false;
        if (Mouse.current != null && Mouse.current.leftButton.isPressed && useMouseInput && checkMouseClick)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                target = hit.point;
                mouseClicked = true;
            }
        }
    }
}