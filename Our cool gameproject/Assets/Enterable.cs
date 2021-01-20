using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A class for allowing the player to enter objects
 * 
 * Disables the player for the duration and handles the controll from this class
 */
public class Enterable : MonoBehaviour
{
    GameObject player;

    public bool inRange;
    public bool isEntered;
    public float range;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        inRange = false;
        isEntered = false;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {

        // So you don't enter and leave in same frame
        timer += Time.deltaTime;

        // Enter
        if (inRange && timer > 0.1f)
        {
            if (Input.GetKeyDown(KeyCode.F) && player.activeInHierarchy)
            {
                player.SetActive(false);
                isEntered = true;
                timer = 0;
            }
        }

        // Exit
        if (isEntered && timer > 0.1f)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log(transform.right);
                player.transform.position = transform.position + transform.right * 2;
                player.SetActive(true);
                isEntered = false;
                timer = 0;
            }
        }

        if (!isEntered)
        {
            inRange = IsInRange();
        }
    }

    /*
     * Checks proximity with OverlapCircle
     */
    private bool IsInRange()
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, range);

        foreach (Collider2D collider in collisions)
        {
            if (collider.gameObject.tag == "Player")
            {
                return true;
            }
        }

        return false;
    }
}
