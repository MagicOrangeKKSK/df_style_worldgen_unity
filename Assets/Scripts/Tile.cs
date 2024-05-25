using System;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// ��ʾһ���ؿ���ࡣ
/// </summary>
public class Tile
{
    /// <summary>
    /// ��ʼ�� Tile �����ʵ����
    /// </summary>
    /// <param name="height">�߶ȡ�</param>
    /// <param name="temp">�¶ȡ�</param>
    /// <param name="precip">��ˮ����</param>
    /// <param name="drainage">��ˮ�����</param>
    /// <param name="biome">����Ⱥ�����͡�</param>
    public Tile(float height, float temp, float precip, float drainage, string biome)
    {
        Height = height;
        Temp = temp;
        Precip = precip;
        Drainage = drainage;
        Biome = biome;
    }

 
    /// <summary>
    /// �¶ȡ�
    /// </summary>
    public float Temp { get; set; }

    /// <summary>
    /// �߶ȡ�
    /// </summary>
    public float Height { get; set; }

    /// <summary>
    /// ��ˮ����
    /// </summary>
    public float Precip { get; set; }

    /// <summary>
    /// ��ˮ�����
    /// </summary>
    public float Drainage { get; set; }

    /// <summary>
    /// ����Ⱥ�����͡�
    /// </summary>
    public string Biome { get; set; }

    /// <summary>
    /// �Ƿ��к�����
    /// </summary>
    public bool HasRiver { get; set; } = false;

    /// <summary>
    /// �Ƿ���������
    /// </summary>
    public bool IsCiv { get; set; } = false;

    /// <summary>
    /// ����Ⱥ��ID��
    /// </summary>
    public int BiomeID { get; set; } = 0;

    /// <summary>
    /// ���ٶȡ�
    /// </summary>
    public float Prosperity { get; set; } = 0;

    //�򵥼��㷱�ٶ�
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
