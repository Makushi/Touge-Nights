using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float currentSteeringAngle;
    private float currentBreakingForce;

    private bool isDrifting = false;
    private bool isAccelerating = false;
    private bool isBreaking;
    private bool isInReverse = false;

    private float totalEnergy = 100.0f;
    private float currentEnergy = 100.0f;
    private float energyDecreaseRate = -1.0f;
    private float energyIncreaseRate = 5.0f;

    private float driftAngleVelocity = 25.0f;
    private float currentSpeed = 0;

    private AudioSource engineSource = null;
    private AudioSource driftSource = null;

    [Header("Car Settings")]
    [SerializeField] private float motorForce;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float reverseSpeed;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteeringAngle;
    [SerializeField] private float decelerationSpeed;

    [Header("Object References")]
    [SerializeField] private Rigidbody rigidBody;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    [SerializeField] private Light RearLeftBreakLight;
    [SerializeField] private Light RearRightBreakLight;

    [SerializeField] private TrailRenderer RearLeftBreakLightTrail;
    [SerializeField] private TrailRenderer RearRightBreakLightTrail;

    [Header("Particles")]
    [SerializeField] private GameObject LeftTireSmoke;
    [SerializeField] private GameObject RightTireSmoke;
    private ParticleSystem LeftTireSmokePs;
    private ParticleSystem RightTireSmokePs;

    private void OnEnable()
    {
        EventManager.onRetry += DestroyPlayer;
        EventManager.onQuitToMainMenu += DestroyPlayer;
        EventManager.onPause += HandlePause;
        EventManager.onContinue += HandlePause;
        EventManager.onLapCompleted += HandlePause;
        EventManager.onGameOver += HandlePause;
    }

    private void OnDisable()
    {
        EventManager.onRetry -= DestroyPlayer;
        EventManager.onQuitToMainMenu -= DestroyPlayer;
        EventManager.onPause += HandlePause;
        EventManager.onContinue += HandlePause;
        EventManager.onLapCompleted -= HandlePause;
        EventManager.onGameOver -= HandlePause;
    }

    private void Awake()
    {
        LeftTireSmokePs = LeftTireSmoke.GetComponent<ParticleSystem>();
        RightTireSmokePs = RightTireSmoke.GetComponent<ParticleSystem>();

        AudioSource[] sources = GetComponents<AudioSource>();
        engineSource = sources[0];
        driftSource = sources[1];
    }

    private void Start()
    {
        rigidBody.centerOfMass = new Vector3(0.0f, 0.1f, 0.5f);
        EventManager.PlayerSpawned();
        engineSource.Play();
    }

    private void Update()
    {
        if (!GameController.Instance.isGameRunning)
            return;
        
        currentSpeed = rigidBody.velocity.magnitude * 3.6f;
        EventManager.UpdateSpeedometer(rigidBody.velocity.magnitude);

        GetInput();
        HandleSteering();

        SwitchBreakLights(isBreaking);
        HandleEngineSound();

        if (!isDrifting)
        {
            UpdateEnergy(energyDecreaseRate);
        }
    }
    private void FixedUpdate()
    {
        if (!GameController.Instance.isGameRunning)
            return;

        HandleMotor();
        UpdateWheels();
        CheckDrifting();
    }

    private void CheckDrifting()
    {
        Vector3 forward = transform.forward;
        forward.y = 0.0f;
        Vector3 velocity = rigidBody.velocity;
        velocity.y = 0.0f;
        float angleVelocityOffset = Vector3.Angle(forward, velocity);

        if (angleVelocityOffset >= driftAngleVelocity && (int)currentSpeed > 0 && rearLeftWheelCollider.rpm > 0)
        {
            isDrifting = true;
            UpdateEnergy(energyIncreaseRate);
            DriftingFeedback(true);
        }
        else
        {
            isDrifting = false;
            DriftingFeedback(false);
        }
    }

    private void HandleMotor()
    {
        float breakSpeed = 0.0f;
        float force = 0.0f;

        if (isAccelerating)
        {
            if (rearLeftWheelCollider.rpm < 0)
            {
                breakSpeed = breakForce;
            }

            force = currentSpeed < maxSpeed ? motorForce : 0.0f;
        }
        else
        {
            breakSpeed = decelerationSpeed;
            force = 0.0f;
        }

        if (isBreaking)
        {
            if (currentSpeed > 0 && rearLeftWheelCollider.rpm > 0)
            {
                breakSpeed = breakForce;
            }
            else
            {
                breakSpeed = 0.0f;
                force = -motorForce;
            }
        }

        rearLeftWheelCollider.motorTorque = force;
        rearRightWheelCollider.motorTorque = force;
        currentBreakingForce = breakSpeed;

       
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontLeftWheelCollider.brakeTorque = currentBreakingForce;
        frontRightWheelCollider.brakeTorque = currentBreakingForce;
        rearLeftWheelCollider.brakeTorque = currentBreakingForce;
        rearRightWheelCollider.brakeTorque = currentBreakingForce;
    }

    private void HandleSteering()
    {
        currentSteeringAngle = maxSteeringAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteeringAngle;
        frontRightWheelCollider.steerAngle = currentSteeringAngle;
    }

    private void UpdateWheels()
    {
        UpdateWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateWheel(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateEnergy(float energyDelta)
    {
        currentEnergy += energyDelta * Time.deltaTime;

        if (currentEnergy < 0.0f)
        {
            currentEnergy = 0.0f;
            OutOfEnergy();
        }
        else if (currentEnergy > totalEnergy)
        {
            currentEnergy = totalEnergy;
        }

        EventManager.UpdateEnergyMeter((float)currentEnergy / totalEnergy);
    }
    private void UpdateWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        isBreaking = Input.GetKey(KeyCode.S);
        isAccelerating = Input.GetKey(KeyCode.W);
    }

    private void SwitchBreakLights(bool braking)
    {
        float lightIntensity = braking ? 10f : 5;
        RearLeftBreakLight.intensity = lightIntensity;
        RearRightBreakLight.intensity = lightIntensity;
    }

    private void HandleEngineSound()
    {
        engineSource.pitch = currentSpeed / 180;
    }

    private void DriftingFeedback(bool enabled)
    {
        if (enabled)
        {
            if (!driftSource.isPlaying)
            {
                driftSource.Play();
            }

            if (!LeftTireSmokePs.isPlaying)
            {
                LeftTireSmokePs.Play();
                RightTireSmokePs.Play();
            }

            RearRightBreakLightTrail.enabled = enabled;
            RearLeftBreakLightTrail.enabled = enabled;
        }
        else
        {
            driftSource.Stop();
            LeftTireSmokePs.Stop();
            RightTireSmokePs.Stop();
            RearRightBreakLightTrail.enabled = enabled;
            RearLeftBreakLightTrail.enabled = enabled;
        }
    }

    private void OutOfEnergy()
    {
        EventManager.GameOver();
    }

    private void HandlePause()
    {
        if (GameController.Instance.isGameRunning)
        {
            if (engineSource)
                engineSource.Play();
        }
        else
        {
            if (engineSource)
                engineSource.Pause();

            if (driftSource)
                driftSource.Stop();
        }
    }

    private void DestroyPlayer()
    {
        Destroy(this.gameObject);
    }
}
