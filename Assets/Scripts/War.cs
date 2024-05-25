/// <summary>
/// 表示一场战争的类。
/// </summary>
public class War
{
    /// <summary>
    /// 战争的一方。
    /// </summary>
    public Civ Side1 { get; private set; }

    /// <summary>
    /// 战争的对立面。
    /// </summary>
    public Civ Side2 { get; private set; }

    /// <summary>
    /// 初始化 War 类的新实例。
    /// </summary>
    /// <param name="side1">参战的第一方文明。</param>
    /// <param name="side2">参战的第二方文明。</param>
    public War(Civ side1, Civ side2)
    {
        Side1 = side1;
        Side2 = side2;
    }
}
