

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ʾһ���������ࡣ
/// </summary>
public class Civ
{
    /// <summary>
    /// ���������ơ�
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// ���������塣
    /// </summary>
    public Race Race { get; private set; }

    /// <summary>
    /// ������������ʽ��
    /// </summary>
    public Government Government { get; private set; }

    /// <summary>
    /// ��������ɫ��
    /// </summary>
    public Color Color { get; private set; }

    /// <summary>
    /// ���������ġ�
    /// </summary>
    public string Flag { get; private set; }

    /// <summary>
    /// �����������ԡ�
    /// </summary>
    public int Aggression { get; private set; }

    /// <summary>
    /// �������еĵص㡣
    /// </summary>
    public List<CivSite> Sites { get; } = new List<CivSite>();

    /// <summary>
    /// �����ʺϵĵص㡣
    /// </summary>
    public List<CivSite> SuitableSites { get; } = new List<CivSite>();

    /// <summary>
    /// �����Ƿ���ս���С�
    /// </summary>
    public bool AtWar { get; set; } = false;

    /// <summary>
    /// �����ľ��ӡ�
    /// </summary>
    public Army Army { get; set; }

    /// <summary>
    /// ���������˿ڡ�
    /// </summary>
    public int TotalPopulation { get; set; } = 0;

    /// <summary>
    /// ��ʼ�� Civ �����ʵ����
    /// </summary>
    /// <param name="race">���������塣</param>
    /// <param name="name">���������ơ�</param>
    /// <param name="government">������������ʽ��</param>
    /// <param name="color">��������ɫ��</param>
    /// <param name="flag">���������ġ�</param>
    /// <param name="aggression">���������������ʽ����������ԡ�</param>
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
    /// ��ӡ��������ϸ��Ϣ��
    /// </summary>
    public void PrintInfo()
    {
        string text = $"����:{Name}\n" +
                               $"����:{Race.Name}\n" +
                               $"����:{Government.Name}\n" +
                               $"������:{Aggression}\n" +
                               $"�˾ӵ�����:{SuitableSites.Count}";
     Debug.Log(text);
    }
}
