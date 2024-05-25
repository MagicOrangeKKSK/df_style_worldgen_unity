using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constant;
using Random = UnityEngine.Random;


public class NoiseMap : IDisposable
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public float[,] Values { get; private set; }
    public float NoiseSize { get; private set; }
    public int Ocatves { get; private set; }
    public int Seed { get; private set; }

    public float this[int x, int y]
    {
        get
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
                throw new System.IndexOutOfRangeException($"Out Bounds! x: {x} ,y: {y}");
            return Values[x, y];
        }
    }

    public NoiseMap(int width, int height, float noiseSize = 1, int ocatves = 4, int seed = 0, int variation = 0)
    {
        InitializeProperties(width, height, noiseSize, ocatves, seed);
        GenerateHeightMap(variation);
    }

    private void GenerateHeightMap(int variation)
    {
        Random.InitState(Seed);
        float noiseSeed = Random.Range(0, 10000) + variation * 10;
        Vector2 mapSize = new Vector2(Width, Height);
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Vector2 normalize = NormalizePosition(x, y, mapSize);
                Vector2 noisePosition = CalculateNoisePosition(noiseSeed, normalize);
                Values[x, y] = SamplePoint(noisePosition.x, noisePosition.y, Ocatves);
            }
        }
    }

    private Vector2 CalculateNoisePosition(float noiseSeed, Vector2 normalize)
    {
        return new Vector2(
            normalize.x * NoiseSize + noiseSeed,
            normalize.y * NoiseSize + noiseSeed
            );
    }

    private Vector2 NormalizePosition(float x, float y, Vector2 mapSize)
    {
        return new Vector2(((x / mapSize.x) - 0.5f) * 2f, ((y / mapSize.y) - 0.5f) * 2f);
    }

    private void InitializeProperties(int width, int height, float noiseSize, int ocatves, int seed)
    {
        this.Width = width;
        this.Height = height;
        this.NoiseSize = noiseSize;
        this.Ocatves = ocatves;
        this.Seed = seed == 0 ? Random.Range(100000, 999999) : seed;
        this.Values = new float[width, height];
    }

    /// <summary>
    /// 简单噪声
    /// </summary>
    /// <param name="octaves">层数</param>
    /// <param name="persistence">每一层噪声的贡献度</param>
    /// <param name="lacunarity">每一层噪声的频率</param>
    /// <returns></returns>
    private static float SamplePoint(float x, float y, int octaves = 1, float persistence = 0.5f, float lacunarity = 2)
    {
        persistence = Mathf.Clamp01(persistence);
        float result = 0;
        float frequency = 1;
        float amplitude = 1;
        float sumOfAmplitudes = 0;

        for (int i = 0; i < octaves; i++)
        {
            result += Mathf.PerlinNoise(x * frequency, y * frequency) * amplitude;

            sumOfAmplitudes += amplitude;
            frequency *= lacunarity;
            amplitude *= persistence;
        }
        return result / sumOfAmplitudes;
    }

    public void Dispose()
    {
        Values = null;
    }
}