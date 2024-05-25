using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static Constant;

public class CivManager
{
    public List<Civ> Civs;
    public List<Race> Races;
    public List<Government> Governments;

    public CivManager(List<Race> races, List<Government> governs)
    {
        Races = races;
        Governments = governs;
        Civs = new List<Civ>();


        //��ʼ������
        for (int i = 0; i < CIVILIZED_CIVS; i++)
        {
            Race race;
            do
            {
                race = Races.GetRandom();
            } while (race.Form != "������");



            Government government = Governments.GetRandom();
            Color color = PALETTE.GetRandom();
            Civ civ = new Civ(race, $"��������[{i}]", government, color, "", 0);
            Civs.Add(civ);
        }

        //��ʼ�Ĳ���
        for (int i = 0; i < TRIBAL_CIVS; i++)
        {
            Race race;
            do
            {
                race = Races.GetRandom();
            } while (race.Form != "�����");

            Government government = new Government("����", "* ���� *", 2, 50, 0);
            Color color = new Color(Random.value, Random.value, Random.value, 1f);
            Civ civ = new Civ(race, $"���Բ���[{i}]", government, color, "", 0);
            Civs.Add(civ);
        }

        Debug.Log("- ����������� -");
    }

    public void SetupCivs(World world)
    {

        foreach (var civ in Civs)
        {
            civ.Sites.Clear();
            civ.SuitableSites.Clear();
            civ.SuitableSites.Clear();

            for (int x = 0; x < WORLD_WIDTH; x++)
            {
                for (int y = 0; y < WORLD_HEIGHT; y++)
                {
                    //���ÿ�������Ƿ���ϸò����ס
                    for (int g = 0; g < civ.Race.PrefBiomes.Length; g++)
                    {
                        if ((int)civ.Race.PrefBiomes[g] == world[x, y].BiomeID)
                            civ.SuitableSites.Add(new CivSite(x, y, "", true, 0));
                    }
                }
            }
            //�ҵ�һ��û�еľۼ���
            var site = civ.SuitableSites.GetRandom();
            while (world[site.X, site.Y].IsCiv)
            {
                civ.SuitableSites.Remove(site);
                site = civ.SuitableSites.GetRandom();
            }
            world[site.X, site.Y].IsCiv = true;

            var finalProsperity = world[site.X, site.Y].Prosperity * 150;
            if (world[site.X, site.Y].HasRiver)
                finalProsperity *= 1.5f;

            float PopCap = 4f * civ.Race.ReproductionSpeed + finalProsperity;
            PopCap *= 2f;

            civ.Sites.Add(new CivSite(site.X, site.Y, "Village", false, (int)PopCap));
            civ.Sites[0].IsCapital = true;
            civ.Sites[0].Population = 20;

        }

        Debug.Log("- Civs Setup -");
        Debug.Log("* Civ Gen DONE *");

    }


    bool[,] mask = new bool[3, 5]
{
                { false,true,false,false,false},
                { true,true,true,true, true},
                { false,true,false,false,false},
};

    public void DrawCivs(World world)
    {
        Color[,] colors = world.GetColorMap();
        foreach (var civ in Civs)
        {
            for (int t = 0; t < civ.Sites.Count; t++)
            {

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (mask[i, j])
                            colors[civ.Sites[t].X * 3 + i, civ.Sites[t].Y * 5 + j] = civ.Color;
                    }
                }
            }
        }

        int width = colors.GetLength(0);
        int height = colors.GetLength(1);
        string path = Path.Combine(Application.streamingAssetsPath, "Civ.png");
        Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = colors[x, y];
                texture.SetPixel(x, y, color);
            }
        }
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(path, bytes);
    }
}

public static class Ex
{

    public static T GetRandom<T>(this List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }
}
