/// <summary>
/// 表示一个政府类型的类。
/// </summary>
public class Government
{
    /// <summary>
    /// 政府类型的名称。
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// 政府类型的描述。
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// 政府的侵略性。
    /// </summary>
    public int Aggressiveness { get; private set; }

    /// <summary>
    /// 政府的军事化程度。
    /// </summary>
    public float Militarization { get; private set; }

    /// <summary>
    /// 政府的技术奖励。
    /// </summary>
    public float TechBonus { get; private set; }

    /// <summary>
    /// 初始化 GovernmentType 类的新实例。
    /// </summary>
    /// <param name="name">政府类型的名称。</param>
    /// <param name="description">政府类型的描述。</param>
    /// <param name="aggressiveness">政府的侵略性。</param>
    /// <param name="militarization">政府的军事化程度。</param>
    /// <param name="techBonus">政府的技术奖励。</param>
    public Government(
        string name,
        string description,
        int aggressiveness,
        float militarization,
        float techBonus)
    {
        Name = name;
        Description = description;
        Aggressiveness = aggressiveness;
        Militarization = militarization;
        TechBonus = techBonus;
    }
}
