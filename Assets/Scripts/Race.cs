/// <summary>
/// 表示一个种族的类。
/// </summary>
public class Race
{
    /// <summary>
    /// 种族的名称。
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 种族偏好的生物群落。
    /// </summary>
    //public string PrefBiome { get; set; }
    public BiomeType[] PrefBiomes { get; set; }

    /// <summary>
    /// 种族的力量值。
    /// </summary>
    public int Strength { get; set; }

    /// <summary>
    /// 种族的体积大小。
    /// </summary>
    public float Size { get; set; }

    /// <summary>
    /// 种族的繁殖速度。
    /// </summary>
    public float ReproductionSpeed { get; set; }

    /// <summary>
    /// 种族的攻击性等级。
    /// </summary>
    public int Aggressiveness { get; set; }

    /// <summary>
    /// 种族的形态描述。
    /// </summary>
    public string Form { get; set; }

    /// <summary>
    /// 初始化 Race 类的新实例。
    /// </summary>
    /// <param name="name">种族的名称</param>
    /// <param name="prefBiomes">偏好的生物群落</param>
    /// <param name="strength">力量值</param>
    /// <param name="size">体积大小</param>
    /// <param name="reproductionSpeed">繁殖速度</param>
    /// <param name="aggressiveness">攻击性等级</param>
    /// <param name="form">形态描述</param>
    public Race(string name,
                BiomeType[] prefBiomes,
                int strength,
                float size,
                float reproductionSpeed,
                int aggressiveness,
                string form)
    {
        Name = name;
        PrefBiomes = prefBiomes;
        Strength = strength;
        Size = size;
        ReproductionSpeed = reproductionSpeed;
        Aggressiveness = aggressiveness;
        Form = form;
    }
}
