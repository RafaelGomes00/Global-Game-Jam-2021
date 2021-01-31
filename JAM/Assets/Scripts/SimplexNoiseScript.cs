using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimplexNoise;

class Map
{
    public static int NUMBER_OF_ROOMS = 8;

    public int[,] matrix;

    public Map(int x, int y)
    {
        matrix = new int[x, y];
    }
}

public class SimplexNoiseScript : MonoBehaviour
{
    public int[,] gerarMatriz(int tamanhoX, int tamanhoZ) 
    {
        SimplexNoise.Noise.Seed = Random.Range(1, 497980922);
        Map map = new Map(tamanhoX, tamanhoZ);
        int length = tamanhoX, width = tamanhoZ;
        float scale = 0.1f;
        float[,] noise_values = SimplexNoise.Noise.Calc2D(length, width, scale);

        for (int i = 0; i < tamanhoX; i++)
        {
            for (int j = 0; j < tamanhoZ; j++)
            {
                map.matrix[i, j] = (int)noise_values[i, j] / (int)(255 / Map.NUMBER_OF_ROOMS);
            }
        }

        return map.matrix;
    }
}
   
