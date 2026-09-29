using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    private Rigidbody2D rbLander2D;

    private void Awake()
    {
        rbLander2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(Keyboard.current.upArrowKey.isPressed)
        {
            float force = 700f;
            rbLander2D.AddForce(force * transform.up * Time.deltaTime);
        }

        if(Keyboard.current.leftArrowKey.isPressed)
        {
            float turnSpeed = 100f;
            rbLander2D.AddTorque(turnSpeed * Time.deltaTime);
        }

        if(Keyboard.current.rightArrowKey.isPressed)
        {
            float turnSpeed = -100f;
            rbLander2D.AddTorque(turnSpeed * Time.deltaTime);
        }
    }
}
