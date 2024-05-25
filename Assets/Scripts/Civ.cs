

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 表示一个文明的类。
/// </summary>
public class Civ
{
    /// <summary>
    /// 文明的名称。
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// 文明的种族。
    /// </summary>
    public Race Race { get; private set; }

    /// <summary>
    /// 文明的政府形式。
    /// </summary>
    public Government Government { get; private set; }

    /// <summary>
    /// 文明的颜色。
    /// </summary>
    public Color Color { get; private set; }

    /// <summary>
    /// 文明的旗帜。
    /// </summary>
    public string Flag { get; private set; }

    /// <summary>
    /// 文明的侵略性。
    /// </summary>
    public int Aggression { get; private set; }

    /// <summary>
    /// 文明所有的地点。
    /// </summary>
    public List<CivSite> Sites { get; } = new List<CivSite>();

    /// <summary>
    /// 文明适合的地点。
    /// </summary>
    public List<CivSite> SuitableSites { get; } = new List<CivSite>();

    /// <summary>
    /// 文明是否在战争中。
    /// </summary>
    public bool AtWar { get; set; } = false;

    /// <summary>
    /// 文明的军队。
    /// </summary>
    public Army Army { get; set; }

    /// <summary>
    /// 文明的总人口。
    /// </summary>
    public int TotalPopulation { get; set; } = 0;

    /// <summary>
    /// 初始化 Civ 类的新实例。
    /// </summary>
    /// <param name="race">文明的种族。</param>
    /// <param name="name">文明的名称。</param>
    /// <param name="government">文明的政府形式。</param>
    /// <param name="color">文明的颜色。</param>
    /// <param name="flag">文明的旗帜。</param>
    /// <param name="aggression">基于种族和政府形式计算的侵略性。</param>
    public Civ(Race race, string name, Government government, Color color, string flag, int aggression)
    {
        Name = name;
        Race = race;
        Government = government;
        Color = color;
        Flag = flag;
        Aggression = Race.Aggressiveness + Government.Aggressiveness;
    }

    /// <summary>
    /// 打印文明的详细信息。
    /// </summary>
    public void PrintInfo()
    {
        string text = $"名称:{Name}\n" +
                               $"种族:{Race.Name}\n" +
                               $"政府:{Government.Name}\n" +
                               $"侵略性:{Aggression}\n" +
                               $"宜居点数量:{SuitableSites.Count}";
     Debug.Log(text);
    }
}
