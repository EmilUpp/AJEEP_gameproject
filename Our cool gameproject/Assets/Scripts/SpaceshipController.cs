using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/*
 * Class for handling the spaceships input controls and pathfinding
 * 
 * rotationalThrust, how fast it can rotate
 * 
 * trimmThrust, how fast it can move up, down, left, right
 * 
 * forwardThrust, how fast it can accelerate forward
 * 
 * fuelCapacity, maximum fuel
 * 
 * fuel, current fuel level, set to max at start
 */
public class SpaceshipController : MonoBehaviour
{
    [Header("Player")]
    public bool isEntered;

    [Header("Thrusters")]
    public float rotationalThrust;
    public float trimmThrust;
    public float forwardThrust;
    public SpaceshipEngine engine;

    [Header("Cargo")]
    public float fuelCapacity;
    public float fuel;

    public float loadCapacity;
    public float load;

    public float shipMass;

    [Header("Autopilot")]
    public GameObject target;
    public float baseSafeDistance = 10;
    public float cruiseSpeed = 5;

    public float speed;
    public float speedTowardsTarget;
    private bool angularStabilizerOn;
    public List<Vector2> pathPointList;
    private bool isPathfinding;

    [Header("Debug")]
    public bool showPath;
    public bool showRays;

    [HideInInspector]
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        fuel = fuelCapacity;
        engine = new SpaceshipEngine(this, trimmThrust, rotationalThrust, forwardThrust, fuelCapacity);

        // Use this if component
        //engine = gameObject.AddComponent<SpaceshipEngine>();
        //engine = GetComponent<SpaceshipEngine>();

        angularStabilizerOn = false;

        if (target != null)
        {
            StartCoroutine(nameof(UpdatePath), 3);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        isEntered = GetComponent<Enterable>().isEntered;

        if (Input.GetKeyDown(KeyCode.X) && isEntered)
        {
            if (angularStabilizerOn)
            {
                angularStabilizerOn = false;
            }
            else
            {
                angularStabilizerOn = true;
            }
        }

        if (target != null)
        {
            //pathPointList = PathFinder.GeneratePathList(gameObject, target, baseSafeDistance, showRays);

            //Debug.Log("The path is " + pathPointList.Count + " nodes long");

            if (showPath) {
                DebugDrawPath(pathPointList);
            }
        }

        if (!isPathfinding && target != null)
        {
            StartCoroutine(nameof(UpdatePath), 3);
        }
    }

    /*
     * Fixed update for all physic and force actions
     */
    void FixedUpdate()
    {
        rb.mass = shipMass + fuel/100f + load;

        if (load > loadCapacity)
        {
            load = loadCapacity;
        }

        if (isEntered)
        {
            CheckPlayerNavigationActions();
        }

        if (pathPointList.Count > 0)
        {
            FollowPath(pathPointList, 5, cruiseSpeed);
        }

        if (angularStabilizerOn)
        {
            AngularStabilizer();
        }
    }

    /*
     * Listens for player inputs and fire the engines accordingly
     */
    void CheckPlayerNavigationActions()
    {
        // Trimm controls
        if (Input.GetAxis("Vertical") != 0)
        {
            engine.FireVerticalTrimEngines(Input.GetAxis("Vertical"));
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            engine.FireHorizontalTrimEngines(Input.GetAxis("Horizontal"));
        }

        // Rotation controls
        if (Input.GetKey(KeyCode.Q))
        {
            engine.RotateSpaceship(1);
        }
        if (Input.GetKey(KeyCode.E))
        {
            engine.RotateSpaceship(-1);
        }

        // Main engine controls
        if (Input.GetKey(KeyCode.Space) && fuel > 0)
        {
            engine.FireForwardThrust();
        }
    }

    /*
     * Only updates the path every seconds
     */
    IEnumerator UpdatePath(int updateRate)
    {
        isPathfinding = true;
        while(true)
        {
            pathPointList = PathFinder.GeneratePathList(gameObject, target, baseSafeDistance, showRays);

            yield return new WaitForSeconds(updateRate);
        }
    }

    /*
     * Draws a line between points in space
     */
    void DebugDrawPath(List<Vector2> pathList)
    {
        if (pathList.Count == 0)
        {
            return;
        }

        Debug.DrawLine(transform.position, pathList[0], Color.green);
        for (int i=0; i < pathList.Count - 1; i++)
        {
            Debug.DrawLine(pathList[i], pathList[i + 1], Color.green);
        }
    }

    /*
     * Make the spaceship follow a list of points in space by firing it's thrusters
     * 
     * pointList: List of points to follow
     * 
     * proximityThreshold: How close the spaceship needs to be a point in order to have "reached it"
     * 
     * cruiseSpeed: The maximum speed towards the target
     */
    void FollowPath(List<Vector2> pointList, float proximityThreshold, float cruiseSpeed)
    {
        Vector2 nextPoint = pointList[0];

        // Check if closer than threshold to first point in list
        if (Vector2.Distance(transform.position, nextPoint) < proximityThreshold)
        {
            pointList.RemoveAt(0);
            return;
        }

        // Else call rotate towards point
        RotateTowardsPoint(nextPoint);


        Vector2 dirToPoint = nextPoint - new Vector2(transform.position.x, transform.position.y);
        
        // Calcluates how much of the velocity is towards the target
        float dirFactor = Vector2.Dot(dirToPoint.normalized, rb.velocity.normalized);


        float[] angleAndRotation = GetRotationAndAngleToPoint(transform, nextPoint);
        float angleDifference = Mathf.Abs(angleAndRotation[0] - angleAndRotation[1]);

        speedTowardsTarget = rb.velocity.magnitude * dirFactor;
        speed = rb.velocity.magnitude;

        // Accelerates if speed is lowwer than set speed
        if (rb.velocity.magnitude * dirFactor < cruiseSpeed && angleDifference < 30)
        {
            engine.FireForwardThrust();
        }

        // This section calculates and counteract circular motion by firing side thrusters
        float angleToVelocity = Vector2.SignedAngle(dirToPoint.normalized, rb.velocity.normalized);

        // Left
        if (angleToVelocity > 0)
        {
            engine.FireHorizontalTrimEngines(1);
        }
        // Right
        else if (angleToVelocity < 0)
        {
            engine.FireHorizontalTrimEngines(-1);
        }

    }

    /*
     * Rotates the spaceship towards a point in space
     * 
     * Uses the AngularStabilizer() to minimize wobbling by counteracting the turning
     */
    void RotateTowardsPoint(Vector2 point)
    {
        float[] angleAndRotation = GetRotationAndAngleToPoint(transform, point);

        float angleToPoint = angleAndRotation[0];
        float currentAngle = angleAndRotation[1];


        // This helps to deaccelerate and reduce wobbling
        if (Mathf.Abs(currentAngle - angleToPoint) < 90)
        {
            AngularStabilizer();
        }

        float angleDifference = currentAngle - angleToPoint;

        // Adds an addional powerfactor to prevent the AngularStabilizer to stop it completely
        // Left
        if (angleDifference < 0 && angleDifference >= -180)
        {
            engine.RotateSpaceship(1, 1.1f);
        }
        // Right
        else if (angleDifference <= 180 || angleDifference < -180)
        {
            engine.RotateSpaceship(-1, 1.1f);
        }
        // Left
        else if (angleDifference > 180)
        {
            engine.RotateSpaceship(1, 1.1f);
        }

    }

    /*
     * Calcuates the angle to point in space and own rotation
     * 
     * return float[angleToPoint, currentAngle]
     * 
     * both values are in range 180 -> -180
     */
    static float[] GetRotationAndAngleToPoint(Transform objectTransform, Vector2 point)
    {
        float angleToPoint = Mathf.Atan2(point.y - objectTransform.position.y, point.x - objectTransform.position.x) * Mathf.Rad2Deg;

        // Offsets 90 because of prefab orientation
        float currentAngle = objectTransform.eulerAngles.z + 90;

        // Wraps the angle to 180 -> -180
        currentAngle %= 360;
        if (currentAngle > 180)
            currentAngle -= 360;

        return new float[] { angleToPoint, currentAngle };
    }

    /*
     * Counteracts angular velocity to make it easier to fly straight
     * and reduce wobbling when autopilot is on
     */
    void AngularStabilizer()
    {
        if (rb.angularVelocity < 0)
        {
            engine.RotateSpaceship(1);
        }

        if (rb.angularVelocity > 0)
        {
            engine.RotateSpaceship(-1);
        }
    }

    /*
     * Asserts value are positive
     */
    private void OnValidate()
    {
        if (baseSafeDistance < 0)
        {
            baseSafeDistance = 0;
        }

        if (fuelCapacity < 0)
        {
            fuelCapacity = 0;
        }

        if (loadCapacity < 0)
        {
            loadCapacity = 0;
        }
    }
}
