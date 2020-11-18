using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class atmosphereScript : MonoBehaviour
{
    
    private Vector2 oldpos;
    private Vector2 newpos;

    public Vector2 velocity;


    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(transform.parent.GetComponent<planetScript>().atmosphereScale, transform.parent.GetComponent<planetScript>().atmosphereScale, 1);
        oldpos = transform.position;
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
            float heightAbovePlanet = Vector2.Distance(collision.transform.position, transform.position) - (transform.parent.GetComponent<planetScript>().diameter / 2);
            float height01 = heightAbovePlanet / (transform.lossyScale.x - (transform.parent.GetComponent<planetScript>().diameter / 2));
            Debug.Log(velocity);
            //Debug.Log(Mathf.Exp(height01 * transform.parent.GetComponent<planetScript>().atmosphereDensity) * (1 - height01)); 
        }
    }
}
