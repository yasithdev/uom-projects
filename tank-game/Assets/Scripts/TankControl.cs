using UnityEngine;

public class TankControl : MonoBehaviour
{
    private float currentRotation;
    private Vector3 destination;
    public float motionSpeed;

    // Private properties
    private Quaternion quaternion;
    public float rotationSpeed;
    // Public properties
    public float stepDistance;

    private void Start()
    {
        quaternion = Quaternion.identity;
    }

    // Update is called once per frame
    private void Update()
    {
        // Forward Movement
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Accelerate();
        }

        // Backward Movement
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Reverse();
        }

        // Left Rotation
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            TurnLeft();
        }

        // Right Rotation
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            TurnRight();
        }

        // Apply transformation if present
        transform.rotation = Quaternion.RotateTowards(transform.rotation, quaternion, rotationSpeed*Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, destination, motionSpeed*Time.deltaTime);
    }

    // Tank Movement Logic
    private void Accelerate()
    {
        destination += transform.up*stepDistance;
    }

    private void Reverse()
    {
        destination -= transform.up*stepDistance;
    }

    private void TurnLeft()
    {
        currentRotation += 90;
        quaternion = Quaternion.Euler(new Vector3(quaternion.x, quaternion.y, currentRotation));
    }

    private void TurnRight()
    {
        currentRotation -= 90;
        quaternion = Quaternion.Euler(new Vector3(quaternion.x, quaternion.y, currentRotation));
    }
}