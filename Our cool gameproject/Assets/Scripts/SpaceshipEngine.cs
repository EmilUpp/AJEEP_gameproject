using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A class for handling the spaceships engines and how they behave
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
public class SpaceshipEngine
{
    SpaceshipController spaceship;
    Rigidbody2D rb;
    public float trimmThrust;
    public float rotationalThrust;
    public float forwardThrust;

    public float fuelCapacity;
    public float fuel;


    public SpaceshipEngine(SpaceshipController spaceship, float trimmThrust, float rotationalThrust, float forwardThrust, float fuelCapacity)
    {
        this.spaceship = spaceship;
        this.trimmThrust = trimmThrust;
        this.rotationalThrust = rotationalThrust;
        this.forwardThrust = forwardThrust;
        this.fuelCapacity = fuelCapacity;
        rb = spaceship.rb;
    }

    // Use this if component but add monobehavour then
    /*
    private void Start()
    {
        Debug.Log("Engine created");

        rb = GetComponent<SpaceshipController>().gameObject.GetComponent<Rigidbody2D>();
        fuel = fuelCapacity;

        trimmThrust = GetComponent<SpaceshipController>().trimmThrust;
        rotationalThrust = GetComponent<SpaceshipController>().rotationalThrust;
        forwardThrust = GetComponent<SpaceshipController>().forwardThrust;
        fuelCapacity = GetComponent<SpaceshipController>().fuelCapacity;

    }
    */

    public void FireVerticalTrimEngines(float direction)
    {
        rb.AddForce(new Vector2(direction * trimmThrust * Mathf.Cos((90 + rb.rotation) * Mathf.Deg2Rad),
                                direction * trimmThrust * Mathf.Sin((90 + rb.rotation) * Mathf.Deg2Rad)));

        spaceship.fuel -= (trimmThrust / 2500);
    }

    public void FireHorizontalTrimEngines(float direction)
    {
        rb.AddForce(new Vector2(direction * trimmThrust * Mathf.Cos((rb.rotation) * Mathf.Deg2Rad),
                                direction * trimmThrust * Mathf.Sin((rb.rotation) * Mathf.Deg2Rad)));

        spaceship.fuel -= (trimmThrust / 2500);
    }

    /*
     * Rotates the spaceship
     * direction of 1 means left
     * Direction of -1 means right
     */
    public void RotateSpaceship(float direction)
    {
        float rotation = rotationalThrust * direction;
        rb.AddTorque(rotation);

        spaceship.fuel -= Mathf.Abs((rotation / 2500));
    }

    /*
     * The powerfactor is used when autopilot turns to minimize wobbling
     */
    public void RotateSpaceship(float direction, float powerFactor)
    {
        float rotation = rotationalThrust * direction * powerFactor;
        rb.AddTorque(rotation);

        spaceship.fuel -= Mathf.Abs((rotation / 2500));
    }

    public void FireForwardThrust()
    {
        rb.AddForce(new Vector2(forwardThrust * Mathf.Cos((90 + rb.rotation) * Mathf.Deg2Rad),
                        forwardThrust * Mathf.Sin((90 + rb.rotation) * Mathf.Deg2Rad)));

        spaceship.fuel -= (forwardThrust / 1000);
    }
}
