using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using static Constant;
using Random = UnityEngine.Random;

public class World : ISave
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public Tile[,] Tiles { get; private set; }

    public Tile this[Vector2Int pos]
    {
        get
        {
            return Tiles[pos.x, pos.y];
        }
    }

    public Tile this[int x, int y]
    {
        get
        {
            if (Tiles == null)
                throw new System.NullReferenceException("Tiles is NULL");
            if (Tiles[x, y] == null)
                throw new System.NullReferenceException($"Tile is NULL, x:{x} ,y:{y}");
            if (x < 0 || y < 0 || x >= Width || y >= Height)
                throw new System.IndexOutOfRangeException($"Pos not in bounds, x:{x} ,y:{y}");
            return Tiles[x, y];
        }
    }

    public World(int width, int height)
    {
        Width = width;
        Height = height;
        Tiles = new Tile[Width, Height];
    }

    public World(int worldWidth, int worldHeight, HeightMap heightMap, HeightMap tempHeightMap, HeightMap precipHeightMap, HeightMap drainageHeightMap)
    {
        Width = worldWidth;
        Height = worldHeight;
        Tiles = new Tile[Width, Height];
        if (IsValidHeightMap(heightMap) && IsValidHeightMap(tempHeightMap) && IsValidHeightMap(precipHeightMap) && IsValidHeightMap(drainageHeightMap))
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    float height = heightMap[x, y];
                    float temp = tempHeightMap[x, y];
                    float precip = precipHeightMap[x, y];
                    float drainage = drainageHeightMap[x, y];
                    Tiles[x, y] = new Tile(height, temp, precip, drainage, "0");
                }
            }
        }
        else
        {
            //if (!IsValidHeightMap(heightMap))
            //    throw new Exception("HeightMap is not valid!");
            //if(!IsValidHeightMap(tempHeightMap))
            //    throw new Exception("tempHeightMap is not valid!")；
        }
    }

    /// <summary>
    /// 计算繁荣度
    /// </summary>
    public void Prosperity()
    {
        for (int x = 0; x < WORLD_WIDTH; x++)
        {
            for (int y = 0; y < WORLD_HEIGHT; y++)
            {
                Tiles[x, y].CalculationProsperity();
            }
        }
    }

    /// <summary>
    /// 生成生态环境
    /// </summary>
    public void BiomeIDsAttributed()
    {
        for (int x = 0; x < WORLD_WIDTH; x++)
        {
            for (int y = 0; y < WORLD_HEIGHT; y++)
            {
                Tiles[x, y].CalculationBiomeID();
            }
        }
    }

    public void AddRiver(bool onlyCardinalDirections = true)
    {
        int x = Random.Range(0, WORLD_WIDTH);
        int y = Random.Range(0, WORLD_HEIGHT);
        int tries = 0;
        while (Tiles[x, y].Height < 0.8f)
        {
            tries++;
            x = Random.Range(0, WORLD_WIDTH);
            y = Random.Range(0, WORLD_HEIGHT);
            if (tries > 2000)
                return;
        }
        //TODO 记得之后删掉
        Tiles[x, y].BiomeID = 7;

        List<Vector2Int> coor = new List<Vector2Int>();
        coor.Add(new Vector2Int(x, y));
        while (Tiles[x, y].Height >= 0.2f)
        {
            Vector2Int pos = GetLowestNeighbour(x, y, onlyCardinalDirections);
            //TODO 记得之后删除
            Tiles[pos.x, pos.y].BiomeID = 7;
            if (pos.x == x && pos.y == y)
            {
                Debug.Log("-----1");
                return;
            }
            if (HasRiverInAdjacent(x, y, onlyCardinalDirections))
            {
                Debug.Log("-----2");
                return;
            }

            if (coor.Contains(pos))
            {
                Debug.Log("-----3");
                break;
            }
            coor.Add(pos);
            x = pos.x;
            y = pos.y;
        }

        Debug.Log("-------河流长度:" + coor.Count);
        //检查河流长度
        if (coor.Count <= MIN_RIVER_LENGTH)
            return;

        for (int i = 0; i < coor.Count; i++)
        {
            x = coor[i].x;
            y = coor[i].y;
            if (Tiles[x, y].Height < 0.2f)
                break;
            Tiles[x, y].HasRiver = true;
            //如果湖泊的最后没有流入海里 则变为湖泊
            if (Tiles[x, y].Height >= 0.2f && i == coor.Count - 1)
                Tiles[x, y].HasRiver = true;  //改成湖泊
        }
    }

    /// <summary>
    /// 检查HeightMap是否合法
    /// </summary>
    /// <param name="heightMap"></param>
    /// <returns></returns>
    private bool IsValidHeightMap(HeightMap heightMap)
    {
        if (heightMap != null && Width == heightMap.Width && Height == heightMap.Height)
            return true;
        return false;
    }

    private static int[] dx = new int[8] { -1, 0, 1, 0, -1, -1, 1, 1 };
    private static int[] dy = new int[8] { 0, 1, 0, -1, 1, -1, 1, -1 };
    public Vector2Int GetLowestNeighbour(int x, int y, bool onlyCardinalDirections)
    {
        Debug.Log($"========> get lowest : {x},{y}");
        Vector2Int lowest = new Vector2Int(x, y);
        float minHeight = Tiles[x, y].Height;
        int nX, nY;
        int range = onlyCardinalDirections ? 4 : 8;
        for (int i = 0; i < range; i++)
        {
            nX = x + dx[i];
            nY = y + dy[i];
            if (nX < 0 || nY < 0 || nX >= Width || nY >= Height)
                continue;
            if (Tiles[nX, nY].Height <= minHeight)
            {
                Debug.Log($"----- {i}  {nX},{nY} now height:{Tiles[nX, nY].Height}   min height:{minHeight}");
                minHeight = Tiles[nX, nY].Height;
                lowest.x = nX;
                lowest.y = nY;
            }
            else
            {
                Debug.Log($"ooooooo {i}  {nX},{nY} now height:{Tiles[nX, nY].Height}   min height:{minHeight}");
            }
        }

        return lowest;
    }

    public bool HasRiverInAdjacent(int x, int y, bool onlyCardinalDirections)
    {
        int range = onlyCardinalDirections ? 4 : 8;
        int nX, nY;
        if (Tiles[x, y].HasRiver)
            return true;

        for (int i = 0; i < range; i++)
        {
            nX = x + dx[i];
            nY = y + dy[i];
            if (nX < 0 || nY < 0 || nX >= Width || nY >= Height)
                continue;

            if (Tiles[nX, nY].HasRiver)
                return true;
        }
        return false;
    }

    /// <summary>
    /// 保存成图片
    /// </summary>
    public Color[,] GetColorMap()
    {
        int widthSize = 3;
        int heightSize = 5;

        Color[,] colors = new Color[Width * widthSize, Height * heightSize];

        for (int x = 0; x < Width * widthSize; x++)
        {
            for (int y = 0; y < Height * heightSize; y++)
            {
                int BiomeID = Tiles[x / widthSize, y / heightSize].BiomeID;

                Color color = COLOR_UNKNOW;
                if (COLOR_MAP.ContainsKey(BiomeID))
                    color = COLOR_MAP[BiomeID];
                color.a = 1f;

                colors[x,y] = color; 
            }
        }
        return colors;
    }


    public void Save(string filename)
    {
        int widthSize = 3;
        int heightSize = 5;

        string path = Path.Combine(Application.streamingAssetsPath, filename);
        Texture2D texture = new Texture2D(Width * widthSize, Height * heightSize, TextureFormat.ARGB32, false);
        for (int x = 0; x < Width * widthSize; x++)
        {
            for (int y = 0; y < Height * heightSize; y++)
            {
                int BiomeID = Tiles[x / widthSize, y / heightSize].BiomeID;

                Color color = COLOR_UNKNOW;
                if (COLOR_MAP.ContainsKey(BiomeID))
                    color = COLOR_MAP[BiomeID];
                color.a = 1f;

                texture.SetPixel(x, y, color);
            }
        }
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(path, bytes);
    }

}