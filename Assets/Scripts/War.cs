/// <summary>
/// ��ʾһ��ս�����ࡣ
/// </summary>
public class War
{
    /// <summary>
    /// ս����һ����
    /// </summary>
    public Civ Side1 { get; private set; }

    /// <summary>
    /// ս���Ķ����档
    /// </summary>
    public Civ Side2 { get; private set; }

    /// <summary>
    /// ��ʼ�� War �����ʵ����
    /// </summary>
    /// <param name="side1">��ս�ĵ�һ��������</param>
    /// <param name="side2">��ս�ĵڶ���������</param>
    public War(Civ side1, Civ side2)
    {
        Side1 = side1;
        Side2 = side2;
    }
}
