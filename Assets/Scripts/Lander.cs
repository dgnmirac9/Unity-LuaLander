using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    public event EventHandler OnUpForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnBeforeForce;

    private Rigidbody2D rbLander2D;

    private void Awake()
    {
        rbLander2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);
        
        if(Keyboard.current.upArrowKey.isPressed)
        {
            float force = 700f;
            rbLander2D.AddForce(force * transform.up * Time.deltaTime);
            OnUpForce?.Invoke(this, EventArgs.Empty);
        }

        if(Keyboard.current.leftArrowKey.isPressed)
        {
            float turnSpeed = 100f;
            rbLander2D.AddTorque(turnSpeed * Time.deltaTime);
            OnLeftForce?.Invoke(this, EventArgs.Empty);
        }

        if(Keyboard.current.rightArrowKey.isPressed)
        {
            float turnSpeed = -100f;
            rbLander2D.AddTorque(turnSpeed * Time.deltaTime);
            OnRightForce?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        //çarpışma nesnesi kontrolü
        if (!collision2D.gameObject.TryGetComponent<LandingPad>(out LandingPad landingPad))
        {
            Debug.Log("Crashed!");
            return;
        }

        //bağıl hız kontrolü
        float softLandingVelocitySpeed = 4f;
        float relativeVelocityMagnitude = collision2D.relativeVelocity.magnitude;

        if(relativeVelocityMagnitude > softLandingVelocitySpeed)
        {
            Debug.Log("Landed too hard!");
            return;
        }

        //iniş açısı kontrolü
        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        float minDotVector = .9f;
        if(dotVector < minDotVector)
        {
            Debug.Log("Bad landing angle!");
            return;
        }

        Debug.Log("Successfull Landing!");

        float maxScoreAmountLandingAngle = 100f;
        float scoreDotVectorMultiplier =  10f;
        float landingAngleScore = maxScoreAmountLandingAngle - Mathf.Abs(dotVector - 1f) * scoreDotVectorMultiplier * maxScoreAmountLandingAngle;
        
        float maxScoreAmountLandingSpeed = 100f;
        float landingSpeedScore = (softLandingVelocitySpeed - relativeVelocityMagnitude) * maxScoreAmountLandingSpeed;

        Debug.Log("landingAngleScore: " + landingAngleScore);
        Debug.Log("landingSpeedScore: " + landingSpeedScore);

        int score = Mathf.RoundToInt(landingAngleScore + landingSpeedScore) * landingPad.GetScoreMultiplier();
        Debug.Log($"Score: {score}");
    }
}
