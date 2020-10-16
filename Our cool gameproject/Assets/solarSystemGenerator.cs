using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
            Debug.Log("New stuff");
            createNewSystem();
        }

    }

    void createNewSystem()
    {
        // Create sun
        GameObject sun = Instantiate(planetPrefab);
        sun.transform.position = transform.position;
        sun.name = "Sun";
        orbitAroundBody sunOrbitScript = sun.GetComponent<orbitAroundBody>();
        Destroy(sunOrbitScript);

        sun.GetComponent<planetScript>().Start();
        sun.GetComponent<planetScript>().generatePlanet();

        addSubPlanets(sun, numberOfPlanets, new int[] { minMoonAmount, maxMoonAmount}, moonDepth);

    }

    public void addSubPlanets(GameObject mainBody, int numberOfPlanets, int[] numberOfMoonsRange, int maxMoonDepth)
    {
        for (int i=0; i< numberOfPlanets; i++)
        {
            // Create planet for each in numberOfPlanet
            GameObject newPlanet = Instantiate(planetPrefab, mainBody.transform);
            newPlanet.name = "Planet: " + i;

            newPlanet.GetComponent<orbitAroundBody>().Start();
            newPlanet.GetComponent<orbitAroundBody>().speed = Random.Range(200, 800);

            newPlanet.GetComponent<planetScript>().Start();
            newPlanet.GetComponent<planetScript>().generatePlanet();
            newPlanet.GetComponent<planetScript>().diameter = mainBody.GetComponent<planetScript>().diameter / 3;

            // set position
            float parentDistanceToGrandparent = 40; //mainBody.GetComponentInParent<orbitAroundBody>().desiredDistance / 4;
            newPlanet.transform.position += new Vector3(parentDistanceToGrandparent * Mathf.Pow(i + 1, 2) * Random.Range(0.75f, 1.25f),
                                                        parentDistanceToGrandparent * Mathf.Pow(i + 1, 2) * Random.Range(0.75f, 1.25f), 0);

            // Recursivly call addMoons
            addMoons(newPlanet, numberOfMoonsRange, 0, maxMoonDepth);
        }
    }

    public void addMoons(GameObject mainBody, int[] numberOfMoonsRange, int currentMoonDepth, int maxMoonDepth)
    {
        int numberOfMoons = Random.Range(numberOfMoonsRange[0], numberOfMoonsRange[1]);
        for (int i = 0; i < numberOfMoons; i++)
        {
            // Create planet for each in numberOfPlanet
            GameObject newPlanet = Instantiate(planetPrefab, mainBody.transform);
            newPlanet.name = GenerateMoonName(mainBody.name, currentMoonDepth);

            newPlanet.GetComponent<orbitAroundBody>().Start();
            newPlanet.GetComponent<orbitAroundBody>().speed = -mainBody.GetComponentInParent<orbitAroundBody>().speed * 1.1f * Random.Range(0.8f, 2f);

            newPlanet.GetComponent<planetScript>().Start();
            newPlanet.GetComponent<planetScript>().generatePlanet();
            newPlanet.GetComponent<planetScript>().diameter = mainBody.GetComponent<planetScript>().diameter / 2;

            // set position
            float parentDistanceToGrandparent = 5;
            Debug.Log(Mathf.Sqrt(i / 2) + " " + currentMoonDepth);
            newPlanet.transform.position += new Vector3(parentDistanceToGrandparent * currentMoonDepth * Random.Range(0.75f, 1.25f),
                                                        parentDistanceToGrandparent * currentMoonDepth * Random.Range(0.75f, 1.25f), 0);

            // Recursivly call addMoons
            int newNumberOfMoons = (int)Mathf.Sqrt(numberOfMoons);

            if (newNumberOfMoons > 0 && currentMoonDepth < maxMoonDepth)
            {
                int newMoonDepth = currentMoonDepth + 1;
                addMoons(newPlanet, new int[] { 0, newNumberOfMoons }, newMoonDepth, maxMoonDepth);
            }
        }
    }

    static string GenerateMoonName(string mainBodyName, int moonDepth)
    {
        //string rawName = mainBodyName.Substring(0, "Planet: 0".Length);
        return string.Concat(Enumerable.Repeat("sub", moonDepth)) + "moon";
    }
}
