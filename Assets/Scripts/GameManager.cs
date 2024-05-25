using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using static Constant;

public class GameManager : MonoBehaviour
{
    private HeightMap _heightMap = new HeightMap(WORLD_WIDTH, WORLD_HEIGHT);
    private HeightMap _noiseMap = new HeightMap(WORLD_WIDTH, WORLD_HEIGHT); //噪声
    private HeightMap _tempMap = new HeightMap(WORLD_WIDTH, WORLD_HEIGHT);  //气温
    private HeightMap _precipitaionMap = new HeightMap(WORLD_WIDTH, WORLD_HEIGHT);//降水
    private HeightMap _drainageMap = new HeightMap(WORLD_WIDTH,WORLD_HEIGHT); //排水

    private World _world;
    private List<Race> _races;
    private List<Government> _governs;
    private int _month = 0;

    public List<War> wars = new List<War>();
    public List<RaceSO> raceSOList;
    public List<GovernmentSO> governSOList;
    public CivManager civManager;

    public static GameManager Instance;
    public void Start()
    {
        Instance = this;
        MasterWorldGenerator();
    }
    /// <summary>
    /// 主要世界生成代码
    /// </summary>
    public void MasterWorldGenerator()
    {
        Debug.Log("* World Gen Start *");
        Stopwatch startTime = Stopwatch.StartNew();
        AddMainHills();
        AddSmallHills();
        ApplySimplex();
        PoleGenerator();
        TectonicGenerator();
        RainErosion();
        _heightMap.Clamp(0f, 1f);
        //_heightMap.Normalize(0f, 1f);
        TemperatureCalculation();
        PercipitaionCalculation();
        DrainageCalculation();
        //火山活动 - 在海中形成新岛屿较罕见（？），在高度超过0.9的山区较罕见（？），在板块边界较罕见（？）
        startTime.Stop();

        Debug.Log("* World Gen DONE *       in: " + ((float)startTime.ElapsedMilliseconds / 1000f) + " 's");
        _heightMap.Save("height_map.png");
        _tempMap.Save("temp_map.png");
        _precipitaionMap.Save("percipitaion_map.png");
        _drainageMap.Save("drainage_map.png");

        //世界信息
        _world = new World(WORLD_WIDTH, WORLD_HEIGHT, _heightMap, _tempMap, _precipitaionMap, _drainageMap);
        Debug.Log("- Tiles Initialized -");
        _world.Prosperity();
        _world.BiomeIDsAttributed();
        for (int i = 0; i < 10; i++)
            _world.AddRiver(true);
        _world.Save("biome.png");

        _races = new List<Race>();
        for (int i = 0; i < raceSOList.Count; i++)
        {
            RaceSO so = raceSOList[i];
            Race race = new Race(so.Name, so.Biomes, so.Sterngth, so.Size, so.ReproductionSpeed, so.Aggressiveness, so.From);
            _races.Add(race);
        }

        _governs = new List<Government>();
        for (int i = 0; i < governSOList.Count; i++)
        {
            GovernmentSO so = governSOList[i];
            Government government = new Government(so.Name,so.Description,so.Aggressiveness,so.Militarization,so.TechBonus);
            _governs.Add(government);
        }

        civManager = new CivManager(_races,_governs);
        civManager.SetupCivs(_world);

    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ProcessCivs(_month);

            _month++;
            Debug.Log("Month:"+_month);
        }

        if (Input.GetKeyDown(KeyCode.F1))
            civManager.DrawCivs(_world);
    }

   

    void ProcessCivs(int month)
    {
        for (int i = 0; i < CIVILIZED_CIVS + TRIBAL_CIVS; i++)
        {
            var civ1 = civManager.Civs[i];
            Debug.Log(civ1.Name + "  " + civ1.Race.Name);
            civ1.TotalPopulation = 0;

            //Site
            for (int j = 0; j < civ1.Sites.Count; j++)
            {
                var site1 = civ1.Sites[j];
                //Population
                int NewPop = (int)((float)site1.Population * civ1.Race.ReproductionSpeed / 1500f);
                if (site1.Population > site1.PopCap / 2)
                {
                    NewPop /= 6;
                }
                site1.Population += NewPop;
                //Expand
                if (site1.Population > site1.PopCap)
                {
                    site1.Population = (int)site1.PopCap;
                    if(civ1.Sites.Count < CIV_MAX_SITES)
                    {
                        site1.Population = site1.PopCap / 2;
                        civ1 = NewSite(civ1, site1, _world);
                    }
                }
                civ1.TotalPopulation += site1.Population;
            
                //Diplomacy
                for(int k = 0; k < CIVILIZED_CIVS + TRIBAL_CIVS; k++)
                {
                    var civ2 = civManager.Civs[k];
                    for(int l = 0; l < civ1.Sites.Count; l++)
                    {
                        if (civ1 == civ2)
                            break;
                        var site2 = civ1.Sites[l];
                       if(PointDistRound(site1,site2) < WAR_DISTANCE)
                        {
                            bool alreadyWar = false;
                            foreach (var war in wars)
                            {
                                if ((war.Side1 == civ1 && war.Side2 == civ2) ||
                                    (war.Side1 == civ2 && war.Side2 == civ1))
                                    alreadyWar = true;
                            }

                            if (!alreadyWar)
                            {
                                wars.Add(new War(civ1, civ2));
                                if (civ1.AtWar == false)
                                {
                                    civ1.Army = new Army(civ1.Sites[0].X, civ1.Sites[0].Y,
                                                                         civ1.Name, (int)(civ1.TotalPopulation * civ1.Government.Militarization / 100));
                                    civ1.AtWar = true;
                                }

                                if(civ2.AtWar == false)
                                {
                                    civ2.Army = new Army(civ2.Sites[0].X, civ2.Sites[0].Y,
                                                                           civ2.Name, (int)(civ2.TotalPopulation * civ2.Government.Militarization / 100));
                                    civ2.AtWar = true;
                                }
                            }
                        }
                    }
                }

                Debug.Log($"X: {site1.X} Y: {site1.Y}, Population: {site1.Population}");
            }
            Debug.Log($"Army == x: {civ1.Army.X}  y:{civ1.Army.Y}  size:{civ1.Army.Size}");
        }
    }

    private int PointDistRound(CivSite site1, CivSite site2)
    {
        return PointDistRound(site1.X,site1.Y,site2.X, site2.Y);
    }

    private int PointDistRound(int x1, int y1, int x2, int y2)
    {
        return Mathf.Abs(x1 - x2)+ Mathf.Abs(y1 - y2);
    }

    private Civ NewSite(Civ civ, CivSite origin, World world)
    {
       int index = Random.Range(0,civ.SuitableSites.Count);
        int tries = 0;
        while (PointDistRound(origin.X, origin.Y, civ.SuitableSites[index].X, civ.SuitableSites[index].Y) > EXPANSION_DISTANCE ||
                world[civ.SuitableSites[index].X, civ.SuitableSites[index].Y].IsCiv)
        {
            if (tries > 200)
                return civ;
            tries += 1;
            index = Random.Range(0, civ.SuitableSites.Count);
        }

        int x = civ.SuitableSites[index].X;
        int y = civ.SuitableSites[index].Y;
        world[x, y].IsCiv = true;
        var finalProsperity = world[x, y].Prosperity * 150;
        if (world[x, y].HasRiver)
        {
            finalProsperity *= 1.5f;
        }
        var popCap = 3 * civ.Race.ReproductionSpeed + finalProsperity;

        civ.Sites.Add(new CivSite(x, y, "村庄", false,(int)popCap));
        civ.Sites[civ.Sites.Count - 1].Population = 20;
        return civ;
    }



    private void DrainageCalculation()
    {
        _drainageMap.AddFBM(4,4);
        _drainageMap.Normalize(0f, 1f);
        Debug.Log("- Drainage Calculation -");
    }

    private void PercipitaionCalculation()
    {   
        _precipitaionMap.Add(2f);
        _precipitaionMap.AddFBM(4,4);
        _precipitaionMap.Normalize(0f, 1f);
        Debug.Log("- Precipitaion Calculation -");
    }

    private void TemperatureCalculation()
    {
        for (int x = 0; x < WORLD_WIDTH; x++)
        {
            for (int y = 0; y < WORLD_HEIGHT; y++)
            {
                float heightEffect = 0;
                if (y > WORLD_HEIGHT / 2)  //纬度也高 越冷
                    _tempMap.Values[x, y] = WORLD_HEIGHT - y - heightEffect;
                else
                    _tempMap.Values[x, y] = y - heightEffect;
                heightEffect = _heightMap.Values[x, y];
                if (heightEffect > 0.8f)
                {
                    heightEffect *= 5; //高度系数  越高越冷
                    if (y > WORLD_HEIGHT / 2)
                        _tempMap.Values[x, y] = WORLD_HEIGHT - y - heightEffect;
                    else
                        _tempMap.Values[x, y] = y - heightEffect;
                }
                else if(heightEffect < 0.25f)
                {
                    heightEffect *= 10;
                    if (y > WORLD_HEIGHT / 2)
                        _tempMap.Values[x, y] = WORLD_HEIGHT - y - heightEffect;
                    else
                        _tempMap.Values[x, y] = y - heightEffect;
                }
            }
        }

        _tempMap.Normalize(0f, 1f);
        Debug.Log("- Temperature Calculation -");
    }

    private void RainErosion()
    {
        _heightMap.RainErosion(WORLD_WIDTH * WORLD_HEIGHT, 0.07f, 0);
        Debug.Log("- Erosion -");
    }

    private void TectonicGenerator()
    {
        _heightMap.TectonicGenerator(true);  //添加横向山脉
        _heightMap.TectonicGenerator(false); //添加纵向山脉
        Debug.Log("- Tectonic Gen -");
    }

    private void PoleGenerator()
    {
        _heightMap.PoleGenerator(true);
        Debug.Log("- South Pole -");
        _heightMap.PoleGenerator(false);
        Debug.Log("- North Pole -");
    }

    private void ApplySimplex()
    {
        _heightMap.Normalize(0f, 1f);
        _noiseMap.AddFBM(1, 4);
        _noiseMap.Normalize(0, 1f);
        _heightMap.Multiply(_noiseMap);
        Debug.Log("- Apply Smiplex -");
        //_heightMap.SaveToPNG();
    }

    private void AddSmallHills()
    {
        for (int i = 0; i < 1000; i++)
        {
            int randomX = Random.Range(WORLD_WIDTH / 10, WORLD_WIDTH - WORLD_WIDTH / 10 + 1);
            int randomY = Random.Range(WORLD_HEIGHT / 10, WORLD_HEIGHT - WORLD_HEIGHT / 10+1);
            int radius = Random.Range(2, 5);
            int height = Random.Range(6, 11);
            _heightMap.AddHill(randomX, randomY, radius, height);
        }
        Debug.Log("- Small Hills -");
    }

    private void AddMainHills()
    {
        int dx = WORLD_WIDTH / 10;
        int dy = WORLD_HEIGHT / 10;
        for (int i = 0; i < 250; i++)
        {
            int randomX = Random.Range(dx, WORLD_WIDTH -dy + 1);
            int randomY = Random.Range(dx, WORLD_HEIGHT - dy + 1);
            int radius = Random.Range(12, 17);
            int height = Random.Range(6, 11);
            _heightMap.AddHill(randomX, randomY, radius, height);
        }
        Debug.Log("- Main Hills -");
    }
}



/// <summary>
/// 存储游戏中使用的常量值。
/// </summary>
public class Constant
{
    /// <summary>
    /// 世界的宽度。
    /// </summary>
    public static int WORLD_WIDTH = 200;

    /// <summary>
    /// 世界的高度。
    /// </summary>
    public static int WORLD_HEIGHT = 80;

    /// <summary>
    /// 屏幕的宽度。
    /// </summary>
    public static int SCREEN_WIDTH = 200;

    /// <summary>
    /// 屏幕的高度。
    /// </summary>
    public static int SCREEN_HEIGHT = 80;

    /// <summary>
    /// 开局文明化的文明数量。
    /// </summary>
    public static int CIVILIZED_CIVS = 2;

    /// <summary>
    /// 开局部落型的文明数量。
    /// </summary>
    public static int TRIBAL_CIVS = 2;

    /// <summary>
    /// 河流的最小长度。
    /// </summary>
    public static int MIN_RIVER_LENGTH = 3;

    /// <summary>
    /// 每个文明最多可控制的地点数量。
    /// </summary>
    public static int CIV_MAX_SITES = 20;

    /// <summary>
    /// 文明扩张的最大距离（单位：格）。
    /// </summary>
    public static int EXPANSION_DISTANCE = 10;

    /// <summary>
    /// 触发战争的最小距离（单位：格）。
    /// </summary>
    public static int WAR_DISTANCE = 8;


    public static Color COLOR_BAD_LANDS = new Color32(204,159,81,255);
    public static Color COLOR_ICE = new Color32(176,223,215,255);
    public static Color COLOR_DARK_GREEN = new Color32(68,158,53,255);
    public static Color COLOR_LIGHT_GREEN = new Color32(131,212,82,255);
    public static Color COLOR_WATER = new Color32(13,103,196,255);
    public static Color COLOR_MOUTAIN = new Color32(185,192,162,255);
    public static Color COLOR_DESERT = new Color32(255,218,90,255);
    public static Color COLOR_UNKNOW = new Color32(255,0, 255,255);
    public static Color COLOR_LIGHT_BLUE = new Color32(114, 114, 255, 255);

    public static Dictionary<int, Color> COLOR_MAP = new Dictionary<int, Color>()
    {
        [0] =COLOR_WATER,
        [1] =COLOR_DARK_GREEN,
        [2] =COLOR_LIGHT_GREEN,
        [3] = COLOR_LIGHT_GREEN,
        [4] =COLOR_DESERT,
        [5] =COLOR_DARK_GREEN,
        [6] = COLOR_DARK_GREEN,
        [7] =COLOR_UNKNOW,
        [8] =COLOR_BAD_LANDS,
        [9] =COLOR_MOUTAIN,
        [10] =COLOR_MOUTAIN,
        [11] =COLOR_ICE,
        [12] = COLOR_ICE,
        [13] = COLOR_ICE,
        [14] = COLOR_DARK_GREEN,
        [15] = COLOR_LIGHT_GREEN,
        [16] = COLOR_DARK_GREEN,
    };

    public static List<Color> PALETTE = new List<Color>()
    {
        new Color32(255,45,33,255),  //RED
        new Color32(254,80,0,255),//Orange
        new Color32(0,35,156,255),//Blue
        new Color32(71,45,96,255),//Purple
        new Color32(0,135,199,255),//Ocean Blue
        new Color32(254,221,0,255),//Yellow
        new Color32(255,255,255,255),//White
        new Color32(99,102,106,255), //Gray
    };

}

