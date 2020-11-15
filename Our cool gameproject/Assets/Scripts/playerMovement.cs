using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float walkSpeed;
    public float jumpForce;
    public GameObject groundCheck;
    public LayerMask planetsLayer;
    private Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        // If standing on planet
        if (collision.gameObject.CompareTag("Planet"))
        {
            // Walking
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                rb.AddRelativeForce(new Vector2(Input.GetAxisRaw("Horizontal"), 0));
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Unparent player from planet while not in atmosphere
        if (collision.gameObject.CompareTag("Atmosphere"))
        {
            transform.parent = null;
        }
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        // If in atmosphere
        if (collision.gameObject.CompareTag("Atmosphere"))
        {
            // Make player a child of the planet who's atmosphere they are within
            transform.parent = collision.transform.parent;
            
            // Find planet of atmosphere (atmosphere's parent)
            Transform closestPlanet = collision.transform.parent;

            // Rotate Player to stand upright
            transform.rotation = Quaternion.Euler(0, 0, 90 + Mathf.Atan2(closestPlanet.position.y - transform.position.y, closestPlanet.position.x - transform.position.x) * 180 / Mathf.PI);
        }
    }
}
