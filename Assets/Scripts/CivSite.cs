/// <summary>
/// ��ʾһ�������ص���ࡣ
/// </summary>
public class CivSite
{
    /// <summary>
    /// X���ꡣ
    /// </summary>
    public int X { get; private set; }

    /// <summary>
    /// Y���ꡣ
    /// </summary>
    public int Y { get; private set; }

    /// <summary>
    /// �ص����
    /// </summary>
    public string Category { get; private set; }

    /// <summary>
    /// �ص��Ƿ����ˡ�
    /// </summary>
    public bool Suitable { get; private set; }

    /// <summary>
    /// �˿����ޡ�
    /// </summary>
    public int PopCap { get; private set; }

    /// <summary>
    /// ȫ�־�̬��������ǰ�˿�����
    /// </summary>
    public int Population { get; set; } = 0;

    /// <summary>
    /// ȫ�־�̬������ָʾ�Ƿ�Ϊ�׶���
    /// </summary>
    public bool IsCapital { get; set; } = false;

    /// <summary>
    /// ��ʼ�� CivSite �����ʵ����
    /// </summary>
    /// <param name="x">X���ꡣ</param>
    /// <param name="y">Y���ꡣ</param>
    /// <param name="category">�ص����</param>
    /// <param name="suitable">�ص��Ƿ����ˡ�</param>
    /// <param name="popcap">�˿����ޡ�</param>
    public CivSite(int x, int y, string category, bool suitable, int popcap)
    {
        X = x;
        Y = y;
        Category = category;
        Suitable = suitable;
        PopCap = popcap;
    }
}
