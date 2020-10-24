using System.Collections;
using System.Collections.Generic;
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
    public float rotationalThrust;
    public float trimmThrust;
    public float forwardThrust;

    public float fuelCapacity;

    public float fuel;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fuel = fuelCapacity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Trimm controls
        if (Input.GetAxis("Vertical") != 0)
        {

            fuel -= (trimmThrust / 2500);

            rb.AddForce(new Vector2(Input.GetAxis("Vertical") * trimmThrust * Mathf.Cos((90 + rb.rotation) * Mathf.Deg2Rad),
                                    Input.GetAxis("Vertical") * trimmThrust * Mathf.Sin((90 + rb.rotation) * Mathf.Deg2Rad)));
        }

        if (Input.GetAxis("Horizontal") != 0)
        {

            fuel -= (trimmThrust / 2500);

            rb.AddForce(new Vector2(Input.GetAxis("Horizontal") * trimmThrust * Mathf.Cos((rb.rotation) * Mathf.Deg2Rad),
                                    Input.GetAxis("Horizontal") * trimmThrust * Mathf.Sin((rb.rotation) * Mathf.Deg2Rad)));
        }

        // Rotation controls
        if (Input.GetKey(KeyCode.Q))
        {
            float rotation = rotationalThrust;
            rb.AddTorque(rotation);

            fuel -= (rotationalThrust / 2500);
        }
        if (Input.GetKey(KeyCode.E))
        {
            float rotation = rotationalThrust;
            rb.AddTorque(-rotation);

            fuel -= (rotationalThrust / 2500);
        }


        // Main engine controls
        if (Input.GetKey(KeyCode.Space) && fuel > 0)
        {
            rb.AddForce(new Vector2(forwardThrust * Mathf.Cos((90 + rb.rotation) * Mathf.Deg2Rad),
                                    forwardThrust * Mathf.Sin((90 + rb.rotation) * Mathf.Deg2Rad)));

            fuel -= (forwardThrust / 1000);
        }

        if (fuel < 0)
        {
            fuel = 0;
        }
    }
}
