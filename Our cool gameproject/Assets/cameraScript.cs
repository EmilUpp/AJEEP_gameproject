using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
    public float posSmoothness;
    public float rotSmoothness;

    Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    void Update()
    {
        posSmoothness = Vector2.Distance(transform.position, player.position) * 2 + 3;
        Debug.Log(posSmoothness);

        Vector3 desPos = player.position + new Vector3(0, 0, -10);
        transform.position = Vector3.Lerp(transform.position, desPos, posSmoothness * Time.deltaTime);

        Quaternion desRot = player.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, desRot, rotSmoothness * Time.deltaTime);
    }
}
