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
    private bool canWalk;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics2D.OverlapCircle(groundCheck.transform.position, 0.01f, planetsLayer))
        {
            canWalk = true;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddRelativeForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
        }
        else
        {
            canWalk = false;
        }
    }

    private void FixedUpdate()
    {
        if (canWalk)
        {
            rb.AddRelativeForce(new Vector2(Input.GetAxisRaw("Horizontal") * walkSpeed, -0.3f), ForceMode2D.Force);
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        // If in atmosphere
        if (collision.gameObject.CompareTag("Atmosphere"))
        {
            // Find planet of atmosphere (atmosphere's parent)
            Transform closestPlanet = collision.transform.parent;

            // Rotate Player to stand upright
            transform.rotation = Quaternion.Euler(0, 0, 90 + Mathf.Atan2(closestPlanet.position.y - transform.position.y, closestPlanet.position.x - transform.position.x) * 180 / Mathf.PI);
        }
    }
}
