using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class for handling the planet objects
 * Allows for resizing planets and automatic calculation os mass
 * 
 * Creates a mesh using perlin noise to make the surface "bumpy"
 * Amplitude, how much the perlin noise should be scaled, higher values means higher max
 * Scale, the size of the sample area, hiher values means smaller bumps meaning more jagged surface
 * Seed, how much to offset the sample points on and infinite plane, cahnges all the noise values
 * 
 * Mesh
 * verticesAmount, amount of vertecies used, more vertecies means higher resolution of the planet
 * 
 */
[ExecuteInEditMode]
public class planetScript : MonoBehaviour
{
    [Header("Planet dimensions")]
    public float diameter = 2;
    public float density = 1;
    float baseScale;

    public int planetsOrbitingThisBody;

    private Rigidbody2D rb;

    [Header("Noise properties")]
    public bool generateNewPlanet;
    [Range(0,0.5f)]
    public float amplitude;
    [Range(0.1f, 5)]
    public float sampleSize;
    [Range(1, 10)]
    public int octaves;
    [Range(0, 0.99f)]
    public float persitence;
    [Range(1, 5)]
    public float lacunarity;
    //public bool updateWithVa
    [SerializeField]
    int seed;
    public bool generateSeed;

    public float atmosphereScale = 0.3f;
    public float atmosphereDensity = 1;

    [Header("Mesh")]
    Mesh mesh;
    public int verticesAmount;
    public Vector3[] vertices;
    public int[] triangles;

    public void InstanstiatePlanet()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

        mesh = new Mesh();
        gameObject.GetComponent<MeshFilter>().mesh = mesh;

        generateSeed = false;
        generateNewPlanet = false;
    }

    // Update is called once per frame
    void Update()
    {
        planetsOrbitingThisBody = countSubPlanets();

        // Allows to generate new seed in editor
        if (generateSeed)
        {
            seed = Random.Range(0, 100000);
        }
        generateSeed = false;

        // Allows to generate new planet in editor
        if (generateNewPlanet)
        {
            generatePlanet();
        }
        generateNewPlanet = false;

        // Updates mass
        transform.localScale = new Vector3(diameter, diameter);
        float volume = 4 * Mathf.PI * Mathf.Pow((diameter / 2f), 3) / 3;
        rb.mass = volume * density;
    }

    public void generatePlanet()
    {
        // Randomizes all values and creates a new mesh

        amplitude = Random.Range(0.2f, 0.35f);
        sampleSize = Random.Range(0.5f, 3);
        octaves = Random.Range(2, 5);
        persitence = Random.Range(0.3f, 0.7f);
        lacunarity = Random.Range(1.5f, 4);
        seed = Random.Range(0, 100000);

        // Creates the mesh
        createShape(verticesAmount);
        UpdateMesh();
    }

    public void UpdateMesh()
    {
        // Clears old values
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

    public void createShape(int verticesAmount)
    {
        // Updates vertices and triangles

        vertices = createCircleVerteciesList(verticesAmount);
        triangles = createCircleTriangleList(verticesAmount);
    }

    Vector3[] createCircleVerteciesList(int verticesAmount)
    {
        // Create the list of vertecies forming the circle and adds perlinnoise
        float[] noiseMap = Noise.generate1DNoiseMap(verticesAmount, sampleSize, octaves, persitence, lacunarity, seed);

        // Distance between points on the edge in radians
        float step = (2 * Mathf.PI) / verticesAmount;

        // List of vertices one extra is added for the center
        Vector3[] vertices = new Vector3[verticesAmount + 1];
        
        // Adds the center of the circle for drawing purpose
        vertices[0] = new Vector2(0, 0);
        for (int i=1; i < verticesAmount + 1; i++)
        {
            float xCoord = Mathf.Cos((i - 1) * step);
            float yCoord = Mathf.Sin((i - 1) * step);

            if (Mathf.Abs(xCoord) < 0.001f)
            {
                xCoord = 0;
            }

            if (Mathf.Abs(yCoord) < 0.001f)
            {
                yCoord = 0;
            }

            // Calculates the noise, offset by seed
            // Ranges from -1 to 1
            float noise = 1 + amplitude * noiseMap[i - 1];
            vertices[i] = new Vector3(noise * xCoord, noise * yCoord, 0);
        }



        return vertices;
    }

    static int[] createCircleTriangleList(int verticesAmount)
    {
        // Creates the list of order vertecies are connected to draw the triangles

        List<int> triangles = new List<int>();
        int counter = 1;

        // Makes sure it starts of clockwise
        triangles.Add(0);

        while (counter < verticesAmount)
        {
            triangles.Add(counter);

            // Every third vertex the center is added
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

        // Adds the last triangles manually close the loop
        triangles.Add(counter);
        triangles.Add(0);
        triangles.Add(counter);
        triangles.Add(1);

        return triangles.ToArray();
    }

    private void OnValidate()
    {
        if (verticesAmount < 4)
        {
            verticesAmount = 4;
        }
    }

    int countSubPlanets()
    {
        int childCount = 0;
        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            if (child.GetComponent<planetScript>() != null && child != transform)
            {
                childCount += 1 + child.GetComponent<planetScript>().countSubPlanets();
            }
        }

        return childCount;
    }
}
