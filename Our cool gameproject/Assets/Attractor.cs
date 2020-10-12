using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * The class makes it possible for rigidbodies to apply gravity to on another
 * Is uses the mass of the rigibody
 */
public class Attractor : MonoBehaviour
{
    // Gravinational constant, used to scale
    const float G = 6.674f;

    public static List<Attractor> attractors;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        foreach(Attractor attractor in attractors)
        {
            if (attractor != this)
            {
                Attract(attractor);
            }
        }
    }


    private void OnEnable()
    {
        // If it's not initilized
        if (attractors == null)
        {
            attractors = new List<Attractor>();
        }

        attractors.Add(this);
    }

    private void OnDisable()
    {
        attractors.Remove(this);
    }

    void Attract (Attractor objToAttract)
    {
        Rigidbody2D rbToAttract = objToAttract.rb;

        Vector2 direcetion = rb.position - rbToAttract.position;
        float distance = direcetion.magnitude;

        // Newtons law of gravity
        float forceMagnitude = G * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);
        Vector2 force = direcetion.normalized * forceMagnitude;

        rbToAttract.AddForce(force);
    }
    
}
