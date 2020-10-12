using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class planetScript : MonoBehaviour
{
    public int diameter = 2;
    public int density = 1;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(diameter, diameter);
        float volume = 4 * Mathf.PI * Mathf.Pow((diameter / 2f), 3) / 3;

        rb.mass = volume * density;
    }
}
