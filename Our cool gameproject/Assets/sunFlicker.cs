using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sunFlicker : MonoBehaviour
{
    float timer;
    private void FixedUpdate()
    {
        if (timer % 16 == 0)
        {
            gameObject.GetComponent<planetScript>().lacunarity = Random.Range(4.5f, 5);
        }
        timer++;
    }
}
