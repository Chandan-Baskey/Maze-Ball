using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float rotationSpeed;
    public float maxRotation;
    public float smoothTime = 0.08f; // lower = snappier, higher = smoother
    public bool usePhysicsRotation = true; // rotate via Rigidbody when possible to keep physics stable
    public Rigidbody mazeRigidbody; // optional - will try to get one from this GameObject

    // internal state
    private float targetAngleX = 0f; // pitch
    private float targetAngleZ = 0f; // roll
    private float currentAngleX = 0f;
    private float currentAngleZ = 0f;
    private float velAngleX = 0f;
    private float velAngleZ = 0f;

    private Transform _t;
    private Quaternion _desiredRotation;

    // Start is called before the first frame update
    void Start()
    {
        _t = transform;
        Vector3 e = _t.localEulerAngles;
        currentAngleX = NormalizeAngle(e.x);
        currentAngleZ = -NormalizeAngle(e.z);
        targetAngleX = currentAngleX;
        targetAngleZ = currentAngleZ;

        if (usePhysicsRotation && mazeRigidbody == null)
        {
            mazeRigidbody = GetComponent<Rigidbody>();
        }
        // initialize desired rotation to current
        _desiredRotation = _t.localRotation;
    }

    // Update is called once per frame � use for mouse input
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RotateMaze();
        }
        else
        {
            // still smooth towards target if needed
            ApplySmoothedRotation();
        }
    }

    private void RotateMaze()
    {
        // read raw or smoothed axis (GetAxis has built-in smoothing)
        float dx = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float dy = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        // accumulate target angles and clamp the total angles
        targetAngleZ = Mathf.Clamp(targetAngleZ + dx, -maxRotation, maxRotation);
        targetAngleX = Mathf.Clamp(targetAngleX + dy, -maxRotation, maxRotation);

        ApplySmoothedRotation();
    }

    private void ApplySmoothedRotation()
    {
        // smooth current angles toward target angles
        currentAngleX = Mathf.SmoothDampAngle(currentAngleX, targetAngleX, ref velAngleX, smoothTime);
        currentAngleZ = Mathf.SmoothDampAngle(currentAngleZ, targetAngleZ, ref velAngleZ, smoothTime);

        // preserve yaw (y) from transform
        float yaw = _t.eulerAngles.y;
        _desiredRotation = Quaternion.Euler(currentAngleX, yaw, -currentAngleZ);
    }

    // Apply the computed rotation in the physics step when possible to keep collisions stable
    void FixedUpdate()
    {
        if (usePhysicsRotation && mazeRigidbody != null)
        {
            mazeRigidbody.MoveRotation(_desiredRotation);
        }
        else
        {
            _t.localRotation = _desiredRotation;
        }
    }

    private float NormalizeAngle(float a)
    {
        if (a > 180f) a -= 360f;
        return a;
    }
}