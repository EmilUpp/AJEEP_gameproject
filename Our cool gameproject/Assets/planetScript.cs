using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class planetScript : MonoBehaviour
{
    //TODO Comment class

    public int diameter = 2;
    public int density = 1;

    private Rigidbody2D rb;

    [Range(0,0.5f)]
    public float amplitude;
    [Range(0, 4)]
    public float scale;
    int seed;
    public bool generateSeed;

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
        if (generateSeed)
        {
            seed = Random.Range(0, 100000);
        }
        generateSeed = false;

        transform.localScale = new Vector3(diameter, diameter);
        float volume = 4 * Mathf.PI * Mathf.Pow((diameter / 2f), 3) / 3;

        rb.mass = volume * density;

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

    Vector3[] createCircleVerteciesList(int verticesAmount)
    {
        //TODO add perlinnoise
        float step = (2 * Mathf.PI) / verticesAmount;

        Vector3[] vertices = new Vector3[verticesAmount + 1];
;
        vertices[0] = new Vector2(0, 0);
        for (int i=1; i < verticesAmount + 1; i++)
        {
            float xCoord = Mathf.Cos((i - 1) * step);
            float yCoord = Mathf.Sin((i - 1) * step);
            float noise = amplitude * 2 * (Mathf.PerlinNoise((seed + xCoord)*scale, (seed + yCoord)*scale) - 0.5f);
            vertices[i] = new Vector3(noise + xCoord, noise + yCoord, 0);
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

        return triangles.ToArray();
    }
}
