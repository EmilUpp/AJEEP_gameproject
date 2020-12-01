using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
    public float posSmoothness;
    public float rotSmoothness;

    public float zoomSpeed;
    public float targetOrtho;
    public float minOrtho;
    public float maxOrtho;

    Transform player;

    [HideInInspector]
    public bool inAtmosphere;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        targetOrtho = Camera.main.orthographicSize;
    }
    
    void Update()
    {
        posSmoothness = Vector2.Distance(transform.position, player.position) * 2 + 3;
        

        Vector3 desPos = player.position + new Vector3(0, 0, -10);
        transform.position = Vector3.Lerp(transform.position, desPos, posSmoothness * Time.deltaTime);

        Quaternion desRot;

        if (inAtmosphere) desRot = player.rotation;
        else desRot = Quaternion.Euler(0, 0, 0);

        transform.rotation = Quaternion.Slerp(transform.rotation, desRot, rotSmoothness * Time.deltaTime);

        zoomSpeed = Camera.main.orthographicSize;
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            targetOrtho -= scroll * zoomSpeed;
            targetOrtho = Mathf.Clamp(targetOrtho, minOrtho, maxOrtho);
        }

        Camera.main.orthographicSize = targetOrtho;
    }
}
