/// <summary>
/// ��ʾһ��������ࡣ
/// </summary>
public class Race
{
    /// <summary>
    /// ��������ơ�
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// ����ƫ�õ�����Ⱥ�䡣
    /// </summary>
    //public string PrefBiome { get; set; }
    public BiomeType[] PrefBiomes { get; set; }

    /// <summary>
    /// ���������ֵ��
    /// </summary>
    public int Strength { get; set; }

    /// <summary>
    /// ����������С��
    /// </summary>
    public float Size { get; set; }

    /// <summary>
    /// ����ķ�ֳ�ٶȡ�
    /// </summary>
    public float ReproductionSpeed { get; set; }

    /// <summary>
    /// ����Ĺ����Եȼ���
    /// </summary>
    public int Aggressiveness { get; set; }

    /// <summary>
    /// �������̬������
    /// </summary>
    public string Form { get; set; }

    /// <summary>
    /// ��ʼ�� Race �����ʵ����
    /// </summary>
    /// <param name="name">���������</param>
    /// <param name="prefBiomes">ƫ�õ�����Ⱥ��</param>
    /// <param name="strength">����ֵ</param>
    /// <param name="size">�����С</param>
    /// <param name="reproductionSpeed">��ֳ�ٶ�</param>
    /// <param name="aggressiveness">�����Եȼ�</param>
    /// <param name="form">��̬����</param>
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
