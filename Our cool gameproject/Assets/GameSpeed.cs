using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpeed : MonoBehaviour
{
    [Range(0, 10)]
    public float gameSpeed = 1;

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = gameSpeed;
    }
}
