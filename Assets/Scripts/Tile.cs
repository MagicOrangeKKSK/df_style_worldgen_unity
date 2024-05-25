using System;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 表示一个地块的类。
/// </summary>
public class Tile
{
    /// <summary>
    /// 初始化 Tile 类的新实例。
    /// </summary>
    /// <param name="height">高度。</param>
    /// <param name="temp">温度。</param>
    /// <param name="precip">降水量。</param>
    /// <param name="drainage">排水情况。</param>
    /// <param name="biome">生物群落类型。</param>
    public Tile(float height, float temp, float precip, float drainage, string biome)
    {
        Height = height;
        Temp = temp;
        Precip = precip;
        Drainage = drainage;
        Biome = biome;
    }

 
    /// <summary>
    /// 温度。
    /// </summary>
    public float Temp { get; set; }

    /// <summary>
    /// 高度。
    /// </summary>
    public float Height { get; set; }

    /// <summary>
    /// 降水量。
    /// </summary>
    public float Precip { get; set; }

    /// <summary>
    /// 排水情况。
    /// </summary>
    public float Drainage { get; set; }

    /// <summary>
    /// 生物群落类型。
    /// </summary>
    public string Biome { get; set; }

    /// <summary>
    /// 是否有河流。
    /// </summary>
    public bool HasRiver { get; set; } = false;

    /// <summary>
    /// 是否有文明。
    /// </summary>
    public bool IsCiv { get; set; } = false;

    /// <summary>
    /// 生物群落ID。
    /// </summary>
    public int BiomeID { get; set; } = 0;

    /// <summary>
    /// 繁荣度。
    /// </summary>
    public float Prosperity { get; set; } = 0;

    //简单计算繁荣度
    public float CalculationProsperity()
    {
        Prosperity = (1f - Mathf.Abs(Precip - 0.6f) + 1f - Mathf.Abs(Temp - 0.5f) + Drainage) / 3f;
        return Prosperity;
    }

    public int CalculationBiomeID()
    {
        if(Precip >= 0.1f && Precip < 0.33 && Drainage < 0.5f)
        {
            BiomeID = 3;
            if (Random.value > 0.5f)
                BiomeID = 16;
        }

        if(Precip >= 0.1f && Precip > 0.33f)
        {
            BiomeID = 2;
            if (Precip >= 0.66f)
                BiomeID = 1;
        }

        if(Precip >= 0.33f && Precip < 0.66f && Drainage >= 0.33f)
        {
            BiomeID = 15;
            if (Random.value > 0.8f)
                BiomeID = 5;
        }

        if(Precip >= 0.66f && Temp > 0.2f && Drainage > 0.33f)
        {
            BiomeID = 5;
            if (Precip >= 0.75f)
                BiomeID = 6;
            if (Random.value > 0.8f)
                BiomeID = 15;
        }

        if(Precip >= 0.1f && Precip < 0.33f && Drainage >= 0.5f)
        {
            BiomeID = 16;
            if (Random.value > 0.5f)
                BiomeID = 14;
        }

        if(Precip < 0.1f)
        {
            BiomeID = 4;
            if(Drainage > 0.5f)
            {
                BiomeID = 16;
                if (Random.value > 0.5f)
                    BiomeID = 14;
            }
            if (Drainage >= 0.66f)
                BiomeID = 8;
        }

        if (Height <= 0.2f)
            BiomeID = 0;

        if (Temp <= 0.2 && Height > 0.15f)
            BiomeID = Random.Range(11, 14); //11 12 13

        if (Height > 0.6f)
            BiomeID = 9;

        if (Height > 0.9f)
            BiomeID = 10;

        return BiomeID;
    }
}
