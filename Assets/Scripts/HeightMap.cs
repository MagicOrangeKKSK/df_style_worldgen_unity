using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static Constant;
using Random = UnityEngine.Random;


/// <summary>
/// 高度图
/// </summary>
public class HeightMap : ISave
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public float[,] Values { get; private set; }
    public float this[int x, int y]
    {
        get
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
                throw new System.IndexOutOfRangeException($"Out Bounds! x: {x} ,y: {y}");
            return Values[x, y];
        }
    }

    public HeightMap(int width, int height)
    {
        this.Width = width;
        this.Height = height;
        this.Values = new float[width, height];
    }

    /// <summary>
    /// 添加一个小山
    /// </summary>
    public void AddHill(int centerX, int centerY, int radius, float height)
    {
        int radius2 = radius * radius;
        float coef = height / radius;
        int minX = Mathf.Max(centerX - radius, 0);
        int minY = Mathf.Max(centerY - radius, 0);
        int maxX = Mathf.Min(centerX + radius, Width);
        int maxY = Mathf.Min(centerY + radius, Height);
        int yDist, xDist, z;
        for (int x = minX; x < maxX; x++)
        {
            xDist = (x - centerX) * (x - centerX);
            for (int y = minY; y < maxY; y++)
            {
                yDist = (y - centerY) * (y - centerY);
                z = radius2 - xDist - yDist;
                if (z > 0)
                    Values[x, y] += z * coef;
            }
        }
    }

    /// <summary>
    /// 生成南北极
    /// </summary>
    /// <param name="NS"></param>
    public void PoleGenerator(bool NS)
    {
        int rng = Random.Range(2, 6);
        for (int x = 0; x < WORLD_WIDTH; x++)
        {
            for (int y = 0; y < rng; y++)
            {
                Values[x, (NS ? (WORLD_HEIGHT - 1 - y) : y)] = 0.31f;
            }
            rng += Random.Range(1, 4) - 2;
            rng = Mathf.Clamp(rng, 2, 5);
        }
    }

    /// <summary>
    /// 生成山脉
    /// </summary>
    /// <param name="horizontal"></param>
    public void TectonicGenerator(bool horizontal)
    {
        int startX = WORLD_WIDTH / 10;
        int endX = WORLD_WIDTH - WORLD_WIDTH / 10;
        int startY = WORLD_HEIGHT / 10;
        int endY = WORLD_HEIGHT - WORLD_HEIGHT / 10;
        int[,] tecTiles = new int[WORLD_WIDTH, WORLD_HEIGHT];

        if (horizontal)
        {
            int pos = Random.Range(startY, endY + 1);
            for (int x = 0; x < WORLD_WIDTH; x++)
            {
                tecTiles[x, pos] = 1;
                pos += Random.Range(1, 6) - 3; //-2~2
                pos = Mathf.Clamp(pos, 0, WORLD_HEIGHT - 1);
            }
        }
        else
        {
            int pos = Random.Range(startX, endX + 1);
            for (int y = 0; y < WORLD_HEIGHT; y++)
            {
                tecTiles[pos, y] = 1;
                pos += Random.Range(1, 6) - 3;
                pos = Mathf.Clamp(pos, 0, WORLD_WIDTH - 1);
            }
        }

        for (int x = startX; x < endX; x++)
        {
            for (int y = startY; y < endY; y++)
            {
                if (tecTiles[x, y] == 1 && Values[x, y] > 0.3f)
                {
                    int radius = Random.Range(2, 5);
                    float height = Random.Range(0.15f, 0.18f);
                    //float height = 100;
                    AddHill(x, y, radius, height);
                }
            }
        }
    }

    private static int[] dx = new int[8] { -1, 0, 1, -1, 1, -1, 0, 1 };
    private static int[] dy = new int[8] { -1, -1, -1, 0, 0, 1, 1, 1 };
    //下落数量 侵蚀强度 聚合系数 
    public void RainErosion(int drops, float erosionCoef, float aggregationCoef)
    {
        while (drops-- > 0)
        {
            int currentX = Random.Range(0, WORLD_WIDTH);
            int currentY = Random.Range(0, WORLD_HEIGHT);
            float sediment = 0.0f;

            int maxStep = 100000;
            do
            {
                int nextX = 0, nextY = 0;
                float value = Values[currentX, currentY];
                float slope = float.MinValue;
                for (int i = 0; i < 8; i++)
                {
                    int nx = currentX + dx[i];
                    int ny = currentY + dy[i];
                    if (!InBounds(nx, ny))
                        continue;
                    float nSlope = value - Values[nx, ny];
                    if (nSlope > slope)
                    {
                        slope = nSlope;
                        nextX = nx;
                        nextY = ny;
                    }
                }

                if (slope > 0f)
                {
                    Values[currentX, currentY] -= erosionCoef * slope;
                    currentX = nextX;
                    currentY = nextY;
                    sediment += slope; //累计
                }
                else
                {
                    Values[currentX, currentY] += aggregationCoef * sediment;
                    break;
                }
            } while (maxStep-- > 0);
        }
    }


    public bool InBounds(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }

    public void AddFBM(float noiseSize, int ocatves, int seed = 0, int variation = 0)
    {
        using (NoiseMap noiseMap = new NoiseMap(Width, Height, noiseSize, ocatves, seed, variation))
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Values[x, y] += noiseMap[x, y];
                }
            }
        }
    }


    public void Multiply(HeightMap other)
    {
        if (other.Width != Width || other.Height != Height)
            throw new System.Exception("2 heightmap diff");
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                this.Values[x, y] *= other.Values[x, y];
            }
        }
    }

    public void Normalize(float min, float max)
    {
        float currentMin = float.MaxValue;
        float currentMax = float.MinValue;
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                currentMin = Mathf.Min(Values[x, y], currentMin);
                currentMax = Mathf.Max(Values[x, y], currentMax);
            }
        }

        float normalizeScale = (max - min) / (currentMax - currentMin);
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Values[x, y] = min + (Values[x, y] - currentMin) * normalizeScale;
            }
        }
    }

    public void Clamp(float min, float max)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Values[x, y] = Mathf.Clamp(Values[x, y], min, max);
            }
        }
    }

    public void Add(float value)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Values[x, y] += value;
            }
        }
    }


    public void Save(string fileName)
    {
        string path = Path.Combine(Application.streamingAssetsPath, fileName);
        Texture2D texture = new Texture2D(Width * 3, Height * 3, TextureFormat.ARGB32, false);
        for (int x = 0; x < Width * 3; x++)
        {
            for (int y = 0; y < Height * 3; y++)
            {
                Color color = Color.white * Values[x / 3, y / 3];
                color.a = 1f;
                texture.SetPixel(x, y, color);
            }
        }
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(path, bytes);
    }
}
