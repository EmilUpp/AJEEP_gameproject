using UnityEngine;
/*
 * Class for handling the spaceships movement
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
public class spaceshipController : MonoBehaviour
{
    [Header("Thrusters")]
    public float rotationalThrust;
    public float trimmThrust;
    public float forwardThrust;

    [Header("Cargo")]
    public float fuelCapacity;
    public float fuel;

    public float loadCapacity;
    public float load;

    public float shipMass;

    private bool angularStabilizerOn;

    [Header("Autopilot")]
    public GameObject target;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fuel = fuelCapacity;

        angularStabilizerOn = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
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

        FindCourse(target);

    }

    void FixedUpdate()
    {
        rb.mass = shipMass + fuel/10f + load;

        if (load > loadCapacity)
        {
            load = loadCapacity;
        }


        // Trimm controls
        if (Input.GetAxis("Vertical") != 0)
        {
            FireVerticalTrimEngines();
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            FireHorizontalTrimEngines();
        }

        // Rotation controls
        if (Input.GetKey(KeyCode.Q))
        {
            RotateLeft();
        }
        if (Input.GetKey(KeyCode.E))
        {
            RotateRight();
        }

        // Main engine controls
        if (Input.GetKey(KeyCode.Space) && fuel > 0)
        {
            FireForwardThrust();
        }

        if (angularStabilizerOn)
        {
            AngularStabilizer();
        }

        if (fuel < 0)
        {
            fuel = 0;
        }
    }

    void FireVerticalTrimEngines()
    {

        fuel -= (trimmThrust / 2500);

        rb.AddForce(new Vector2(Input.GetAxis("Vertical") * trimmThrust * Mathf.Cos((90 + rb.rotation) * Mathf.Deg2Rad),
                                Input.GetAxis("Vertical") * trimmThrust * Mathf.Sin((90 + rb.rotation) * Mathf.Deg2Rad)));
    }

    void FireHorizontalTrimEngines()
    {
        fuel -= (trimmThrust / 2500);

        rb.AddForce(new Vector2(Input.GetAxis("Horizontal") * trimmThrust * Mathf.Cos((rb.rotation) * Mathf.Deg2Rad),
                                Input.GetAxis("Horizontal") * trimmThrust * Mathf.Sin((rb.rotation) * Mathf.Deg2Rad)));
    }

    void RotateLeft()
    {
        float rotation = rotationalThrust;
        rb.AddTorque(rotation);

        fuel -= (rotationalThrust / 2500);
    }

    void RotateRight()
    {
        float rotation = rotationalThrust;
        rb.AddTorque(-rotation);

        fuel -= (rotationalThrust / 2500);
    }

    void FireForwardThrust()
    {
        rb.AddForce(new Vector2(forwardThrust * Mathf.Cos((90 + rb.rotation) * Mathf.Deg2Rad),
                        forwardThrust * Mathf.Sin((90 + rb.rotation) * Mathf.Deg2Rad)));

        fuel -= (forwardThrust / 1000);
    }

    Vector2[] GeneratePathList()
    {
        return new Vector2[] {new Vector2(0, 1), new Vector2(0, 2), new Vector2(1, 3) };
    }

    void DebugDrawPath(Vector2[] pathList)
    {
        for (int i=0; i < pathList.Length - 1; i++)
        {
            Debug.DrawLine(pathList[i], pathList[i + 1]);
        }
    }

    void FindCourse(GameObject target)
    {

        //Vector3 direction = new Vector3(Mathf.Cos((90 + rb.rotation) * Mathf.Deg2Rad), Mathf.Sin((90 + rb.rotation) * Mathf.Deg2Rad), 0);
        Vector3 direction = target.transform.position - transform.position;
        Vector3 firingPosition = new Vector3(transform.position.x + 2 * direction.normalized.x, transform.position.y + 2 * direction.normalized.y, 0);

        RaycastHit2D hit = Physics2D.Raycast(firingPosition, direction);

        // Checks if its a hit that's not the ship itself
        if (hit.collider != null
            && hit.collider.transform != transform
            && hit.collider.transform.parent != transform
            && hit.collider.transform != target.transform)
        {
            //float distance = (hit.collider.transform.position - transform.position).magnitude;

            Debug.DrawRay(firingPosition, direction.normalized * hit.distance, Color.red);
        }
        else if (hit.collider.transform == target.transform)
        {
            Debug.DrawRay(firingPosition, direction.normalized * hit.distance, Color.green);
        }
        else
        {
            //Debug.DrawRay(transform.position + direction * 2, direction * maxRange, Color.red);
            Debug.DrawRay(firingPosition, direction.normalized, Color.yellow);
        }
    }

    void AngularStabilizer()
    {
        // Counteracts all angular velocity to help fly straighter

        if (rb.angularVelocity < 0)
        {
            RotateLeft();
        }

        if (rb.angularVelocity > 0)
        {
            RotateRight();
        }
    }
}
