using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orbitAroundBody : MonoBehaviour
{
    public Transform bodyToOrbit;
    public float speed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.forward, speed * Time.deltaTime / 100);
        Debug.Log(Vector2.Distance(transform.position, bodyToOrbit.position));
    }
}
