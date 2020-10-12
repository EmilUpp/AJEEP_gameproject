using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class planetScript : MonoBehaviour
{
    public int diameter = 2;
    public int density = 1;

    private Rigidbody2D rb;

    public int verticesAmount;
    Mesh mesh;
    public Vector3[] vertices;
    public int[] triangles;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(diameter, diameter);
        float volume = 4 * Mathf.PI * Mathf.Pow((diameter / 2f), 3) / 3;

        rb.mass = volume * density;

        Debug.Log("Updated");
        createShape(verticesAmount);
        UpdateMesh();
        mesh.RecalculateBounds();
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }

    void createShape(int verticesAmount)
    {
        vertices = createCircleVerteciesList(verticesAmount);
        triangles = createCircleTriangleList(verticesAmount);
    }

    static Vector3[] createCircleVerteciesList(int verticesAmount)
    {
        float step = (2 * Mathf.PI) / verticesAmount;

        Vector3[] vertices = new Vector3[verticesAmount + 1];
;
        vertices[0] = new Vector2(0, 0);
        for (int i=1; i < verticesAmount + 1; i++)
        {
            vertices[i] = new Vector3(Mathf.Cos((i-1) * step), Mathf.Sin((i-1) * step), 0);
        }

        return vertices;
    }

    static int[] createCircleTriangleList(int verticesAmount)
    {
        List<int> triangles = new List<int>();
        int counter = 1;

        triangles.Add(0);

        while (counter < verticesAmount)
        {
            triangles.Add(counter);

            if (triangles.Count % 3 == 0)
            {
                triangles.Add(0);
                triangles.Add(counter);
                counter++;
            }
            else
            {
                counter++;
            }
        }
        triangles.Add(counter);
        triangles.Add(0);
        triangles.Add(counter);
        triangles.Add(1);

        foreach (int triangle in triangles)
        {
            Debug.Log(triangle);
        }

        return triangles.ToArray();
    }
}
