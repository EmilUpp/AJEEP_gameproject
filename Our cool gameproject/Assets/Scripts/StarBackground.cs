using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBackground : MonoBehaviour
{
    
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
        transform.localScale = new Vector3(Camera.main.orthographicSize * 1.5f, Camera.main.orthographicSize * 1.5f, 1);
    }
}
