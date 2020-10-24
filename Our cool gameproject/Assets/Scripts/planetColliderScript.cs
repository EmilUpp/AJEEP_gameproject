using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planetColliderScript : MonoBehaviour
{
    private PolygonCollider2D polyCollider;
    private Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        polyCollider = GetComponent<PolygonCollider2D>();
        mesh = GetComponent<MeshFilter>().mesh;
        polyCollider.pathCount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
        polyCollider.SetPath(0, ConvertArray(mesh.vertices));
    }

    Vector2[] ConvertArray(Vector3[] v3)
    {
        Vector2[] v2 = new Vector2[v3.Length];
        for (int i = 0; i < v3.Length; i++)
        {
            Vector3 tempV3 = v3[i];
            v2[i] = new Vector2(tempV3.x, tempV3.y);
        }
        return v2;
    }
}
