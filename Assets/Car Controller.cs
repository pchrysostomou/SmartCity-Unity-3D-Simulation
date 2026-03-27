//

using UnityEngine;

public class CarController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentBreakForce;
    private bool isBreaking, isBoosting;
    private float currentSpeed;
    private int currentGear = 1;

    // Settings
    [SerializeField] private float baseMotorForce = 2000f;
    [SerializeField] private float breakForce = 6000f;
    [SerializeField] private float maxSteerAngle = 45f;
    [SerializeField] private float boostMultiplier = 2.5f;
    [SerializeField] private float maxSpeed = 200f; // Maximum speed in km/h

    // Acceleration curve
    [SerializeField] private AnimationCurve accelerationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    // Gear settings
    [SerializeField] private int maxGears = 5;
    [SerializeField] private float[] gearRatios = new float[] { 1f, 1.5f, 2f, 2.5f, 3f };

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    private Rigidbody carRigidbody;

    private void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
        if (carRigidbody == null)
        {
            Debug.LogError("Rigidbody component missing!");
        }

        // Validate gear ratios
        if (gearRatios.Length != maxGears)
        {
            Debug.LogError($"Gear ratios array length ({gearRatios.Length}) does not match max gears ({maxGears}).");
        }

        // Set initial rigidbody drag settings
        carRigidbody.drag = 0.05f;
        carRigidbody.angularDrag = 0.1f;
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        UpdateSpeed();
        HandleGearShift();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBreaking = Input.GetKey(KeyCode.Space);
        isBoosting = Input.GetKey(KeyCode.LeftShift);

        // Manual gear shift
        if (Input.GetKeyDown(KeyCode.E) && currentGear < maxGears)
        {
            currentGear++;
        }
        if (Input.GetKeyDown(KeyCode.Q) && currentGear > 1)
        {
            currentGear--;
        }
    }

    private void HandleMotor()
    {
        float motorForce = baseMotorForce;

        // Apply acceleration curve
        float accelerationFactor = accelerationCurve.Evaluate(Mathf.Abs(verticalInput));

        // Apply gear ratio
        motorForce *= gearRatios[currentGear - 1];

        // Apply boost (limited to lower gears)
        if (isBoosting && currentGear <= 3)
        {
            motorForce *= boostMultiplier;
        }

        float wheelMotorForce = verticalInput * motorForce * accelerationFactor;
        ApplyMotorForce(wheelMotorForce);

        // Handle breaking
        currentBreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();

        // Speed limiter
        if (currentSpeed >= maxSpeed)
        {
            ApplyMotorForce(0);
        }
    }

    private void ApplyMotorForce(float force)
    {
        frontLeftWheelCollider.motorTorque = force;
        frontRightWheelCollider.motorTorque = force;
        rearLeftWheelCollider.motorTorque = force;
        rearRightWheelCollider.motorTorque = force;
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentBreakForce;
        frontLeftWheelCollider.brakeTorque = currentBreakForce;
        rearLeftWheelCollider.brakeTorque = currentBreakForce;
        rearRightWheelCollider.brakeTorque = currentBreakForce;
    }

    private void HandleSteering()
    {
        // Reduce steering angle at high speeds for better control
        float speedFactor = 1f - (currentSpeed / maxSpeed * 0.5f);
        currentSteerAngle = maxSteerAngle * horizontalInput * speedFactor;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    private void UpdateSpeed()
    {
        // Convert rigidbody velocity to km/h
        currentSpeed = carRigidbody.velocity.magnitude * 3.6f;
    }

    private void HandleGearShift()
    {
        // Auto gear shifting based on speed
        if (currentSpeed < 20f) currentGear = 1;
        else if (currentSpeed < 40f) currentGear = 2;
        else if (currentSpeed < 70f) currentGear = 3;
        else if (currentSpeed < 100f) currentGear = 4;
        else currentGear = 5;
    }
}
