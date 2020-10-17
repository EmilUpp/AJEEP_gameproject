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
            newSystem = false;
            createNewSystem();
        }

    }

    void createNewSystem()
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
        sun.GetComponent<planetScript>().Start();

        // Adds planets orbiting itself
        addSubPlanets(sun, numberOfPlanets, new int[] { minMoonAmount, maxMoonAmount}, moonDepth);

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
            newPlanet.GetComponent<orbitAroundBody>().speed = Random.Range(200, 800);

            // Start the planetScript
            newPlanet.GetComponent<planetScript>().Start();
            newPlanet.GetComponent<planetScript>().diameter = (mainBody.GetComponent<planetScript>().diameter / 3) * Random.Range(0.5f, 1.5f);

            // set position, each planet gets expoentially further out
            float parentDistanceToGrandparent = 40;
            newPlanet.transform.position += new Vector3(parentDistanceToGrandparent * Mathf.Pow(i + 1, 2) * Random.Range(0.75f, 1.25f),
                                                        parentDistanceToGrandparent * Mathf.Pow(i + 1, 2) * Random.Range(0.75f, 1.25f), 0);

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
            newPlanet.GetComponent<orbitAroundBody>().speed = -mainBody.GetComponentInParent<orbitAroundBody>().speed * 1.1f * Random.Range(0.8f, 2f);

            // Start the planetScript
            newPlanet.GetComponent<planetScript>().Start();
            newPlanet.GetComponent<planetScript>().diameter = (mainBody.GetComponent<planetScript>().diameter / 2) * Random.Range(0.5f, 1.5f);

            // set position
            float parentDistanceToGrandparent = 5;
            newPlanet.transform.position += new Vector3(parentDistanceToGrandparent * currentMoonDepth * Random.Range(0.5f, 2f),
                                                        parentDistanceToGrandparent * currentMoonDepth * Random.Range(0.5f, 2f), 0);

            newPlanet.GetComponent<orbitAroundBody>().desiredDistance = newPlanet.transform.position.magnitude;

            // Lower the amount of submoons possible
            int newNumberOfMoons = (int)Mathf.Sqrt(numberOfMoons);

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
}
