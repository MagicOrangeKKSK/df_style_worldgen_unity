/// <summary>
/// ��ʾһ�����ӵ��ࡣ
/// </summary>
public class Army
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
    /// ����������
    /// </summary>
    public string Civ { get; private set; }

    /// <summary>
    /// ���ӵĹ�ģ��
    /// </summary>
    public int Size { get; private set; }

    /// <summary>
    /// ��ʼ�� Army �����ʵ����
    /// </summary>
    /// <param name="x">���ӵ�X���ꡣ</param>
    /// <param name="y">���ӵ�Y���ꡣ</param>
    /// <param name="civ">����������������</param>
    /// <param name="size">���ӵĹ�ģ��</param>
    public Army(int x, int y, string civ, int size)
    {
        X = x;
        Y = y;
        Civ = civ;
        Size = size;
    }
}
