using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    // Put script onto the obstacle/GameObject you want to move and/or rotate
    // ToDo: see if the public bools can grey out other editor options | add a RigidBody.Force multiplier if needed | try to switch transforms to local space (especially the rotations)
    [Tooltip("Activates the rotation logic")]
    public bool isRotating = false;
    [Tooltip("Activates the moving logic")]
    public bool isMoving = false;
    [Tooltip("Use this to freely spin 360 degrees without switching directions (higher numbers are faster)")]
    public bool justSpin = false;
    [Tooltip("Rotation speed")]
    public float rotationSpeed = 1;
    [Tooltip("X-axis rotation (switches directions after reaching that degree)")]
    public float rotationX = 0;
    [Tooltip("Y-axis rotation (switches directions after reaching that degree)")]
    public float rotationY = 0;
    [Tooltip("Z-axis rotation (switches directions after reaching that degree)")]
    public float rotationZ = 0;
    [Tooltip("Movement speed (coordinate with travel time)")]
    public float distanceSpeed = 0;
    [Tooltip("How long the object is moving in one direction before coming back (coordinate with distance speed)")]
    public float distanceTravelTime = 0;
    [Tooltip("X-axis movement distance")]
    public float distanceX = 0;
    [Tooltip("Y-axis movement distance")]
    public float distanceY = 0;
    [Tooltip("Z-axis movement distance")]
    public float distanceZ = 0;
    //public float forceApplied = 0;
    private Vector3 obstaclePosition;
    private Vector3 rotationCalc;
    private Vector3 distanceCalc;
    private int rotateTimer = 0;
    private int distanceTimer = 0;

    void Start()
    {
        obstaclePosition = gameObject.transform.position;
        distanceCalc = new Vector3(distanceX, distanceY, distanceZ);
        distanceCalc += gameObject.transform.position;
        distanceTravelTime *= 50;
        rotationSpeed = 50 / rotationSpeed;
        rotationCalc = new Vector3(rotationX / rotationSpeed, rotationY / rotationSpeed, rotationZ / rotationSpeed);
    }

    void FixedUpdate()
    {
        if (isRotating)
        {
            if (rotateTimer < rotationSpeed)
            {
                gameObject.transform.Rotate(rotationCalc);
                rotateTimer += 1;
            }
            else if (rotateTimer < rotationSpeed * 2 && !justSpin)
            {
                gameObject.transform.Rotate(rotationCalc * -1);
                rotateTimer += 1;
            }
            else
            {
                rotateTimer = 0;
            }
        }

        if (isMoving)
        {
            if (distanceTimer < distanceTravelTime)
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, distanceCalc, distanceSpeed * Time.deltaTime);
                distanceTimer += 1;
            }
            else if (distanceTimer < distanceTravelTime * 2)
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, obstaclePosition, distanceSpeed * Time.deltaTime);
                distanceTimer += 1;
            }
            else
            {
                distanceTimer = 0;
            }
        }
    }
}