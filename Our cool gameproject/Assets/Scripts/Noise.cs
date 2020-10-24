using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class for creating and generating noise map using perlin noise
 * 
 * scale, zoom level of the sampla size, smaller means smalelr distances betweens points and smoother surface
 * 
 * octaves, times the process is repeated, higher number gives more realistic and jagged ridges
 * 
 * persistance, how much effect each subsequent layer of noise impact the result
 * 
 * lacunarity, how much the frequency changes, higher means more detailed noise
 * 
 * seed, the offset on the infinite plane, thus randomizing the noise
 */
public class Noise : MonoBehaviour
{
    public static float[] generate1DNoiseMap(int length, float scale, int octaves, float persistance, float lacunarity, int seed)
    {
        float[] noiseMap = new float[length];

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        if (scale < 0)
        {
            scale = 0.0001f;
        }

        for (int i=0; i < length; i++)
        {
            float amplitude = 1;
            float frequency = 1;
            float noiseheight = 0;

            float step = (2 * Mathf.PI) / length;

            // Loop through each layer of noise
            for (int j = 0; j < octaves; j++)
            {
                // Calculates the coordinates on the edge of an circle
                // Scaled and amplified by scale and frequency
                float xCoord = Mathf.Cos(i * step)/scale * frequency;
                float yCoord = Mathf.Sin(i * step)/scale * frequency;

                // Offset value to range -1->1
                float perlinValue = Mathf.PerlinNoise(seed + xCoord, seed + yCoord) * 2 - 1;

                noiseheight += perlinValue * amplitude;

                // Decreases amplitude to lower impact of next mask
                amplitude *= persistance;

                // Increases frequency to add more details
                frequency *= lacunarity;
            }

            noiseMap[i] = noiseheight;

            // Set min max
            if (noiseheight > maxNoiseHeight)
            {
                maxNoiseHeight = noiseheight;
            }
            if (noiseheight < minNoiseHeight)
            {
                minNoiseHeight = noiseheight;
            }
        }

        // Interpolates values to 0->1
        /*
        for (int i = 0; i < length; i++)
        {
            noiseMap[i] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[i]) * 2 - 1;
        }
        */

        return noiseMap;
    }
}
