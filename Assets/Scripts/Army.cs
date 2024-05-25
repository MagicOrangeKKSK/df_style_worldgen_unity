/// <summary>
/// 表示一个军队的类。
/// </summary>
public class Army
{
    /// <summary>
    /// X坐标。
    /// </summary>
    public int X { get; private set; }

    /// <summary>
    /// Y坐标。
    /// </summary>
    public int Y { get; private set; }

    /// <summary>
    /// 所属文明。
    /// </summary>
    public string Civ { get; private set; }

    /// <summary>
    /// 军队的规模。
    /// </summary>
    public int Size { get; private set; }

    /// <summary>
    /// 初始化 Army 类的新实例。
    /// </summary>
    /// <param name="x">军队的X坐标。</param>
    /// <param name="y">军队的Y坐标。</param>
    /// <param name="civ">军队所属的文明。</param>
    /// <param name="size">军队的规模。</param>
    public Army(int x, int y, string civ, int size)
    {
        X = x;
        Y = y;
        Civ = civ;
        Size = size;
    }
}
