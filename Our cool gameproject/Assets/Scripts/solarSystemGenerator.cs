using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * Class for generating a solar system according to set parameters
 * 
 * numberOfPlanets, number of planets in the system
 * 
 * minMoonAmount, lowest amount of moons a planet can have
 * 
 * maxMoonAmount, maximum amount of moons a planet can have
 * 
 * nestSystem, creates a new system
 */
public class solarSystemGenerator : MonoBehaviour
{
    public GameObject planetPrefab;

    public int numberOfPlanets;
    public int minMoonAmount;
    public int maxMoonAmount;
    public int moonDepth;
    public GameObject sun;

    public bool newSystem;


    // Start is called before the first frame update
    void Start()
    {
        newSystem = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (newSystem)
        {
            if (sun != null)
            {
                Destroy(sun);
            }

            newSystem = false;
            sun = createNewSystem();
        }

        //ModifySpeed(speedFactor);
    }

    GameObject createNewSystem()
    {
        // Creates the system

        // Create sun as base object
        GameObject sun = Instantiate(planetPrefab);
        sun.transform.position = transform.position;
        sun.name = "Sun";

        // Removes orbit from prefab
        orbitAroundBody sunOrbitScript = sun.GetComponent<orbitAroundBody>();
        Destroy(sunOrbitScript);

        // Starts its script
        sun.GetComponent<planetScript>().InstanstiatePlanet();

        sun.GetComponent<planetScript>().amplitude = 0.01f;
        // Creates the mesh
        sun.GetComponent<planetScript>().createShape(sun.GetComponent<planetScript>().verticesAmount);
        sun.GetComponent<planetScript>().UpdateMesh();

        sun.GetComponent<planetScript>().diameter = 20;
        sun.GetComponent<planetScript>().density = 0.05f;

        sun.GetComponent<planetScript>().atmosphereDensity = 0;

        // Adds planets orbiting itself
        addSubPlanets(sun, numberOfPlanets, new int[] { minMoonAmount, maxMoonAmount}, moonDepth);

        return sun;
    }


    public void addSubPlanets(GameObject mainBody, int numberOfPlanets, int[] numberOfMoonsRange, int maxMoonDepth)
    {
        // Adds planets to the sun

        for (int i=0; i< numberOfPlanets; i++)
        {
            // Create planet for each in numberOfPlanet
            GameObject newPlanet = Instantiate(planetPrefab, mainBody.transform);
            newPlanet.name = "Planet " + i;

            // Start the orbitAroundBody script
            newPlanet.GetComponent<orbitAroundBody>().Start();
            newPlanet.GetComponent<orbitAroundBody>().speed = Random.Range(.5f, 2);

            // Start the planetScript
            newPlanet.GetComponent<planetScript>().InstanstiatePlanet();
            newPlanet.GetComponent<planetScript>().generatePlanet();
            newPlanet.GetComponent<planetScript>().diameter = (mainBody.GetComponent<planetScript>().diameter / 40) * Random.Range(0.5f, 1.5f);
            newPlanet.GetComponent<planetScript>().density = Random.Range(80f, 120f);
            newPlanet.GetComponent<planetScript>().atmosphereDensity = Random.Range(0f, 1f);

            // set position, each planet gets expoentially further out
            float parentDistanceToGrandparent = 40;
            newPlanet.transform.position += new Vector3(parentDistanceToGrandparent * Mathf.Pow(i + 2, 2) * Random.Range(0.75f, 1.25f),
                                                        parentDistanceToGrandparent * Mathf.Pow(i + 2, 2) * Random.Range(0.75f, 1.25f), 0);


            newPlanet.GetComponent<orbitAroundBody>().desiredDistance = newPlanet.transform.position.magnitude;

            // Add moons to the planet
            addMoons(newPlanet, numberOfMoonsRange, 0, maxMoonDepth);
        }
    }

    public void addMoons(GameObject mainBody, int[] numberOfMoonsRange, int currentMoonDepth, int maxMoonDepth)
    {
        // Adds moons to planets, very similiar to addSubPlanets but with somewhat changed values and recusivly adding submoons

        int numberOfMoons = Random.Range(numberOfMoonsRange[0], numberOfMoonsRange[1]);

        for (int i = 0; i < numberOfMoons; i++)
        {
            GameObject newPlanet = Instantiate(planetPrefab, mainBody.transform);
            newPlanet.name = GenerateMoonName(mainBody.name, currentMoonDepth);

            // Start the orbitAroundBody script
            newPlanet.GetComponent<orbitAroundBody>().Start();
            

            // Start the planetScript
            newPlanet.GetComponent<planetScript>().InstanstiatePlanet();
            newPlanet.GetComponent<planetScript>().generatePlanet();
            newPlanet.GetComponent<planetScript>().diameter = (mainBody.GetComponent<planetScript>().diameter / 4) * Random.Range(0.5f, 1.5f);
            newPlanet.GetComponent<planetScript>().density = Random.Range(80f, 120f);
            newPlanet.GetComponent<planetScript>().atmosphereDensity = Random.Range(0f, 0.2f);

            // set position
            float parentDistanceToGrandparent;
            if (currentMoonDepth == 0)
            {
                parentDistanceToGrandparent = 30 * Random.Range(0.5f, 2f);
                newPlanet.GetComponent<orbitAroundBody>().speed = Random.Range(0.1f, 2);
            } else
            {
                parentDistanceToGrandparent = (mainBody.GetComponent<orbitAroundBody>().distanceToTarget() / (4 * currentMoonDepth)) * Random.Range(0.5f, 2f);
                newPlanet.GetComponent<orbitAroundBody>().speed = mainBody.GetComponent<orbitAroundBody>().speed * 1.5f * Random.Range(0.8f, 2f);
            }

            newPlanet.transform.position += new Vector3(parentDistanceToGrandparent, parentDistanceToGrandparent, 0);

            // Lower the amount of submoons possible
            int newNumberOfMoons = (int)(Mathf.Sqrt(numberOfMoons) * Random.Range(0.5f, 2f));

            // Recursivly call addMoons
            if (newNumberOfMoons > 0 && currentMoonDepth < maxMoonDepth)
            {
                int newMoonDepth = currentMoonDepth + 1;
                addMoons(newPlanet, new int[] { 0, newNumberOfMoons }, newMoonDepth, maxMoonDepth);
            }
        }
    }

    static string GenerateMoonName(string mainBodyName, int moonDepth)
    {
        // Creates a string with appropriate amounts of sub

        return string.Concat(Enumerable.Repeat("sub", moonDepth)) + "moon";
    }

    void ModifySpeed(float newSpeed)
    {
        if (sun != null)
        {
            foreach (Transform child in sun.transform.GetComponentsInChildren<Transform>())
            {
                if (child.GetComponent<orbitAroundBody>() != null && child != transform)
                {
                    child.GetComponent<orbitAroundBody>().speedFactor = newSpeed;
                }
            }
        }
    }
}
