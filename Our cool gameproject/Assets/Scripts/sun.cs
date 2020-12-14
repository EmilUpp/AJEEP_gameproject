using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sun : MonoBehaviour
{
    public int emittedTemperature;
    // Start is called before the first frame update
    void Start()
    {
        //generate the sun's surface temperature
        emittedTemperature = generateEmittingTemperature();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    * Randomises a integer between 2000 and 18000 according to gaussian distribution
    * it does this by maping randomly generated floats to values which has that exact probability in a gaussian distribution curve
    * if the function for example randomises the value 0.12, it will return what in the bell curve has exactly the probability of 12%
    */
    private int generateEmittingTemperature()
    {
        //define constants affecting the distribution
        const double averageTemperature = 10.0; //*10^3 degrees kelvin
        const double standardDeviation = 3.0; //*10^3 degrees kelvin
        const double minimumTemperature = 2.0; //the minimum temperature for a star, also decides the maxiumum temperature

        //get the ranges of randomisation by putting the minimum and maxiumum values in to the function of gaussian distribution (bell curve)
        //minimum value is defined above
        float randMin = Convert.ToSingle(1.0 / (standardDeviation * Math.Sqrt(2 * Math.PI)) * Math.Exp(-0.5 * Math.Pow((minimumTemperature-averageTemperature) / standardDeviation, 2)));
        //maximum value is the average, as when we invert the function we will only be able to get values from one half of the curve at once
        float randMax = Convert.ToSingle(1.0 / (standardDeviation * Math.Sqrt(2 * Math.PI)));
        

        //randomise a float in this range
        float rand = UnityEngine.Random.Range(randMin, randMax);
        
        //run the randomised number through the inverse of the bell curve
        double temperatureValue = standardDeviation * Math.Sqrt(-2 * Math.Log(rand * standardDeviation * Math.Sqrt(2 * Math.PI)));
        
        //as the inversed function only has the range 0 < x < average we will also need to decide if the value we just got belongs to the right or left side of the curve
        //this is decided by if the randomly generated float is divisible by two or not
        return (rand * 1000) % 2 > 1 ? Convert.ToInt32((averageTemperature + temperatureValue) * 1000) : Convert.ToInt32((averageTemperature - temperatureValue) * 1000);
    }
}
