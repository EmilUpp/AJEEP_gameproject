using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class atmosphereScript : MonoBehaviour
{
    
    private Vector2 oldpos;
    private Vector2 newpos;

    public Vector2 velocity;
    public float density;
    public LayerMask planetsLayer;

    [Header("Atmosphere Composition")]
    public float co2;
    public float helium;
    public float hydrogen;
    public float nitrogen;
    public float oxygen;

    public float sum;

    // Start is called before the first frame update
    void Start()
    {
        // Get values from parent's planet script and apply them to self
        transform.localScale = new Vector3(transform.parent.GetComponent<planetScript>().atmosphereScale, transform.parent.GetComponent<planetScript>().atmosphereScale, 1);
        oldpos = transform.position;
        density = transform.parent.GetComponent<planetScript>().atmosphereDensity;
        GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.9f, 1, density / 3);

        // Apply atmosphere composition from parent's planet script to self
        float totalComp = transform.parent.GetComponent<planetScript>().atmoCO2 +
                          transform.parent.GetComponent<planetScript>().atmoHelium +
                          transform.parent.GetComponent<planetScript>().atmoHydrogen +
                          transform.parent.GetComponent<planetScript>().atmoNitrogen +
                          transform.parent.GetComponent<planetScript>().atmoOxygen;
        co2 = transform.parent.GetComponent<planetScript>().atmoCO2 / totalComp;
        helium = transform.parent.GetComponent<planetScript>().atmoHelium / totalComp;
        hydrogen = transform.parent.GetComponent<planetScript>().atmoHydrogen / totalComp;
        nitrogen = transform.parent.GetComponent<planetScript>().atmoNitrogen / totalComp;
        oxygen = transform.parent.GetComponent<planetScript>().atmoOxygen / totalComp;

        sum = co2 + helium + hydrogen + nitrogen + oxygen;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate Atmosphere's velocity
        newpos = transform.position;
        var media = (newpos - oldpos);
        velocity = media / Time.deltaTime;
        oldpos = newpos;
        newpos = transform.position;
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // If player is in atmosphere
        if (collision.gameObject.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic && collision.gameObject.CompareTag("Player"))
        {
            // Get players height from planet center
            float heightRemapped = -1 * Vector2.Distance(collision.transform.position, transform.position) / ((transform.lossyScale.x / 2)) + 10;
            
            if(heightRemapped < 0)
            {
                heightRemapped = 0;
            }

            // Get a reversed vector for the players velocity relative to the atmosphere's velocity 
            Vector2 reverseVector = -new Vector2((collision.attachedRigidbody.velocity.x - velocity.x) / 5, (collision.attachedRigidbody.velocity.y - velocity.y) / 5);
            
            // Apply high drag on the player while on ground, acting as friction to ground
            if (Physics2D.OverlapCircle(collision.gameObject.transform.Find("Ground Check").transform.position, 0.05f, planetsLayer))
            {
                collision.attachedRigidbody.velocity += reverseVector * 0.5f;
            }

            // Apply drag to player in atmosphere based on atmosphere's density at player's current height
            collision.attachedRigidbody.velocity += (reverseVector*heightRemapped*density) / 300;

            // Yo camera, player is in atmosphere
            Camera.main.GetComponent<cameraScript>().inAtmosphere = true;
            
        }
    }
}
