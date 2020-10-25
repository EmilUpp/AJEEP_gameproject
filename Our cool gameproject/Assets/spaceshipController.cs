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
    }

    void FixedUpdate()
    {
        rb.mass = shipMass + load;


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
