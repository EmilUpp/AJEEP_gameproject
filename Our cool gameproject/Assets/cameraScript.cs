using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
    //public float posSmoothness;
    public float rotSmoothness;

    public float zoomSpeed;
    public float targetOrtho;
    public float minOrtho;
    public float maxOrtho;

    Transform player;

    [HideInInspector]
    public bool inAtmosphere;

    private Vector2 acceleration;
    private Vector2 lastVelocity;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        targetOrtho = Camera.main.orthographicSize;
    }
    
    void Update()
    {
        /* 
        //----------------- Smooth Position Movement ---------------------
        // Make the smoothness more rigid when player is further away from the camera (so the player never leaves the camera's vision)
        posSmoothness = Vector2.Distance(transform.position, player.position) * 2 + 3;

        // Set the desired position to the player's position
        Vector3 desPos = player.position + new Vector3(0, 0, -10); 

        // Change the camera's position based on a smoothed position of the desired position
        transform.position = Vector3.Lerp(transform.position, desPos, posSmoothness * Time.deltaTime);
        */
        
        transform.position = new Vector3(player.position.x, player.position.y, -10);

        Quaternion desRot;

        if (inAtmosphere) desRot = player.rotation; // Ayy, thank you atmosphereScript for letting me know, I will set my desired rotation to the player's rotation while in the atmosphere

        else desRot = Quaternion.Euler(0, 0, 0);

        // Change camera's rotation based on a smoothed rotation of the desired rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, desRot, rotSmoothness * Time.deltaTime); 

        // Zoom stuff:
        zoomSpeed = Camera.main.orthographicSize; // zoom speed increases with current zoom amount
        float scroll = Input.GetAxis("Mouse ScrollWheel"); // Get scroll input
        if (scroll != 0.0f)
        {
            targetOrtho -= scroll * zoomSpeed; // set target orthographic size based on mouse wheel scroll amount and zoomspeed
            targetOrtho = Mathf.Clamp(targetOrtho, minOrtho, maxOrtho); // Limit zoom to a min and max
        }

        Camera.main.orthographicSize = targetOrtho; // Apply the zoom to the camera
    }
}
