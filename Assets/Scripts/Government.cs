/// <summary>
/// ��ʾһ���������͵��ࡣ
/// </summary>
public class Government
{
    /// <summary>
    /// �������͵����ơ�
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// �������͵�������
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// �����������ԡ�
    /// </summary>
    public int Aggressiveness { get; private set; }

    /// <summary>
    /// �����ľ��»��̶ȡ�
    /// </summary>
    public float Militarization { get; private set; }

    /// <summary>
    /// �����ļ���������
    /// </summary>
    public float TechBonus { get; private set; }

    /// <summary>
    /// ��ʼ�� GovernmentType �����ʵ����
    /// </summary>
    /// <param name="name">�������͵����ơ�</param>
    /// <param name="description">�������͵�������</param>
    /// <param name="aggressiveness">�����������ԡ�</param>
    /// <param name="militarization">�����ľ��»��̶ȡ�</param>
    /// <param name="techBonus">�����ļ���������</param>
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
