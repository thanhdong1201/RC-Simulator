using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RCController : MonoBehaviour
{
    [Header("Helicopter Components")]
    [SerializeField] private AudioSource helicopterAudio;
    [SerializeField] private Rigidbody helicopterRigidbody;
    [SerializeField] private HeliRotorController mainRotor;
    [SerializeField] private HeliRotorController tailRotor;

    [Header("Flight Parameters")]
    [SerializeField] private float turnSpeed = 3f;
    [SerializeField] private float forwardSpeed = 10f;
    [SerializeField] private float forwardTilt = 20f;
    [SerializeField] private float turnTilt = 30f;
    [SerializeField] private float maxAltitude = 100f;

    [Header("Engine Power Settings")]
    [SerializeField] private float maxEnginePower = 100f;
    [SerializeField] private float enginePowerChangeRate = 0.5f;
    [SerializeField] private float minTakeoffPower = 20f;
    [SerializeField] private float enginePower;

    [Header("Sway Settings")]
    [SerializeField] private float swayAmplitude = 3.0f;
    [SerializeField] private float swaySpeed = 1.5f;
    [SerializeField] private float swayLerpSpeed = 0.3f;

    public float EnginePower
    {
        get => enginePower;
        set
        {
            enginePower = Mathf.Clamp(value, 0f, maxEnginePower);
            mainRotor.RotarSpeed = enginePower * 80;
            tailRotor.RotarSpeed = enginePower * 60;
            helicopterAudio.pitch = Mathf.Clamp(enginePower, 0, 1.2f);

            enginePowerEvent?.RaiseEvent(enginePower / maxEnginePower);
        }
    }

    [Header("Control Inputs")]
    [SerializeField] private InputReaderSO inputReader;

    [Header("Events")]
    [SerializeField] private FloatEventChannelSO enginePowerEvent;

    private Vector2 moveInput = Vector2.zero;
    private Vector2 powerInput = Vector2.zero;
    private Vector2 tiltInput = Vector2.zero;
    private float turnForce = 0f;
    private float swayTimer = 0f;
    private bool isLanded = true;

    private void OnEnable()
    {
        inputReader.MoveEvent += UpdateMoveInput;
        inputReader.PowerEvent += UpdatePowerInput;
    }

    private void OnDisable()
    {
        inputReader.MoveEvent -= UpdateMoveInput;
        inputReader.PowerEvent -= UpdatePowerInput;
    }
    private void UpdateMoveInput(Vector2 value) => moveInput = value;
    private void UpdatePowerInput(Vector2 value) => powerInput = value;

    private void FixedUpdate()
    {
        ProcessFlightControls();
        ApplyLiftForce();
        ApplyMovement();

        if (isLanded) return;
        ApplyTilt();
        ApplySwayEffect();
    }

    private void ApplyMovement()
    {
        float turnAmount = turnSpeed * Mathf.Lerp(moveInput.x, moveInput.x * (1.5f - Mathf.Abs(moveInput.y)), Mathf.Max(0f, moveInput.y));
        turnForce = Mathf.Lerp(turnForce, turnAmount, Time.fixedDeltaTime * turnSpeed);

        helicopterRigidbody.AddRelativeTorque(0f, turnForce * helicopterRigidbody.mass, 0f);
        helicopterRigidbody.AddRelativeForce(Vector3.forward * Mathf.Max(0f, moveInput.y * forwardSpeed * helicopterRigidbody.mass));
    }

    private void ApplyLiftForce()
    {
        float altitudeFactor = 1 - Mathf.Clamp(helicopterRigidbody.transform.position.y / maxAltitude, 0, 1);
        float liftForce = Mathf.Lerp(0f, EnginePower, altitudeFactor) * helicopterRigidbody.mass;

        if (EnginePower >= minTakeoffPower)
        {
            helicopterRigidbody.AddRelativeForce(Vector3.up * liftForce);
        }
        else
        {
            helicopterRigidbody.AddRelativeForce(Vector3.down * 2000f);
        }
    }

    private void ApplyTilt()
    {
        tiltInput.x = Mathf.Lerp(tiltInput.x, moveInput.x * turnTilt, Time.deltaTime);
        tiltInput.y = Mathf.Lerp(tiltInput.y, moveInput.y * forwardTilt, Time.deltaTime);
        helicopterRigidbody.transform.localRotation = Quaternion.Euler(tiltInput.y, helicopterRigidbody.transform.localEulerAngles.y, -tiltInput.x);
    }
    private void ApplySwayEffect()
    {
        swayTimer += Time.deltaTime;
        float swayAmount = Mathf.Sin(swayTimer * swaySpeed) * swayAmplitude;
        float swayRotation = Mathf.Cos(swayTimer * swaySpeed) * swayAmplitude;

        Quaternion swayTilt = Quaternion.Euler(swayAmount, helicopterRigidbody.transform.localEulerAngles.y, swayRotation);
        helicopterRigidbody.transform.localRotation = Quaternion.Lerp(helicopterRigidbody.transform.localRotation, swayTilt, Time.deltaTime * swayLerpSpeed);
    }
    private void ProcessFlightControls()
    {
        if (powerInput.y > 0)
        {
            EnginePower += enginePowerChangeRate;
        }
        else if (powerInput.y < 0)
        {
            EnginePower = Mathf.Max(0, EnginePower - enginePowerChangeRate);
        }

        if (!isLanded)
        {
            float yawForce = (1.3f - Mathf.Abs(moveInput.y)) * helicopterRigidbody.mass * powerInput.x;
            helicopterRigidbody.AddRelativeTorque(0f, yawForce, 0f);
        }
    }
    private void OnCollisionEnter()
    {
        isLanded = true;
    }

    private void OnCollisionExit()
    {
        isLanded = false;
    }
}
