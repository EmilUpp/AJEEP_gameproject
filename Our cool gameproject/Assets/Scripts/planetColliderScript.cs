using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class for generating the collider to the planet
 */
public class planetColliderScript : MonoBehaviour
{

    private PolygonCollider2D polyCollider;
    private Mesh mesh;

    void Start()
    {
        polyCollider = GetComponent<PolygonCollider2D>();
        mesh = GetComponent<MeshFilter>().mesh;
        polyCollider.pathCount = 1;

        recalcuateCollider();
    }

    public void recalcuateCollider()
    {
        // Add the vertice to the polygon path list
        polyCollider.SetPath(0, Vector3ArrayToVector2(mesh.vertices));
    }

    public Vector2[] Vector3ArrayToVector2(Vector3[] v3)
    {
        // Converts vector3 array to vector2 array

        Vector2[] v2 = new Vector2[v3.Length - 1];
        for (int i = 1; i < v3.Length; i++)
        {
            Vector3 tempV3 = v3[i];
            v2[i - 1] = new Vector2(tempV3.x, tempV3.y);
        }

        return v2;
    }
}
