using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sunFlicker : MonoBehaviour
{
    float value = 0.05f;
    float timer;
    private void FixedUpdate()
    {
        if (timer % 16 == 0)
        {
            gameObject.GetComponent<planetScript>().lacunarity -= value;
            value *= -1;
        }
        timer++;
    }
}
