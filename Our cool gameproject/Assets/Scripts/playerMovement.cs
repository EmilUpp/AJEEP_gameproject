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

        // Find Closest Planet
        Transform closestPlanet = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject p in GameObject.FindGameObjectsWithTag("Planet"))
        {
            float dist = Vector3.Distance(p.transform.position, currentPos);
            if (dist < minDist)
            {
                closestPlanet = p.transform;
                minDist = dist;
            }
        }
        // Rotate Player to stand upright
        if(closestPlanet != null)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90 + Mathf.Atan2(closestPlanet.position.y - transform.position.y, closestPlanet.position.x - transform.position.x) * 180 / Mathf.PI);
        }

        if(Input.GetAxisRaw("Horizontal") != 0 && Physics2D.OverlapCircle(groundCheck.transform.position, 0.1f, planetsLayer))
        {
            rb.AddRelativeForce(new Vector2(Input.GetAxisRaw("Horizontal"), 0));
        }
    }
}
