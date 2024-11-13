using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Helicopter : MonoBehaviour
{
    [Header("Physics")]
    public Rigidbody _rb;
    public Transform CM;
    public Transform EnginePoint;
    public float RoterPower;
    public float TorquePower;
    public Vector3 inertiaTensor;

    [Header("Inputs")]
    [Range(0, 1)]
    public float Throttel;
    [Range(-1, 1)]
    public float InputX;
    [Range(-1, 1)]
    public float InputY;
    [Range(-1, 1)]
    public float InputYaw;

    public Vector3 Velocity;
    public Vector3 LocalVelocity;
    public Vector3 LocalAngularVelocity;

    [Header("Instruments")]
    public float Speed;
    public float Altitude;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.centerOfMass = CM.localPosition;
        _rb.inertiaTensor = inertiaTensor;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void FixedUpdate()
    {
        Speed = Velocity.magnitude * 5.76f;
        Altitude = transform.position.y;
        CalculateState();

        InputYaw = Input.GetAxis("Yaw");
        InputY = Input.GetAxis("Vertical");
        InputX = -Input.GetAxis("Horizontal");
        Throttel = Mathf.Clamp(Throttel + Input.mouseScrollDelta.y * 0.05f, 0f, 1f);

        _rb.AddForceAtPosition(transform.up * RoterPower * Throttel, EnginePoint.position);
        _rb.AddTorque(transform.forward * TorquePower * InputX);
        _rb.AddTorque(transform.right * TorquePower * InputY);
        _rb.AddTorque(transform.up * TorquePower * InputYaw);
    }

    void CalculateState()
    {
        Quaternion invRotation = Quaternion.Inverse(_rb.rotation);
        Velocity = _rb.velocity;
        LocalVelocity = invRotation * Velocity;
        LocalAngularVelocity = invRotation * _rb.angularVelocity;
    }
}
