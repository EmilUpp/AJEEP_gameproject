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

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(transform.parent.GetComponent<planetScript>().atmosphereScale, transform.parent.GetComponent<planetScript>().atmosphereScale, 1);
        oldpos = transform.position;
        density = transform.parent.GetComponent<planetScript>().atmosphereDensity;
        GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.9f, 1, density / 3);
    }

    // Update is called once per frame
    void Update()
    {
        newpos = transform.position;
        var media = (newpos - oldpos);
        velocity = media / Time.deltaTime;
        oldpos = newpos;
        newpos = transform.position;
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic && collision.gameObject.CompareTag("Player"))
        {
            float heightRemapped = -1 * Vector2.Distance(collision.transform.position, transform.position) / ((transform.lossyScale.x / 2)) + 10;
            
            if(heightRemapped < 0)
            {
                heightRemapped = 0;
            }
            Vector2 reverseVector = -new Vector2((collision.attachedRigidbody.velocity.x - velocity.x) / 5, (collision.attachedRigidbody.velocity.y - velocity.y) / 5);
            
            if (Physics2D.OverlapCircle(collision.gameObject.transform.Find("Ground Check").transform.position, 0.01f, planetsLayer))
            {
                collision.attachedRigidbody.velocity += reverseVector * 0.5f;
            }
            collision.attachedRigidbody.velocity += (reverseVector*heightRemapped*density) / 300;
            
        }
    }
}
