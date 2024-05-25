/// <summary>
/// 表示一个文明地点的类。
/// </summary>
public class CivSite
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
    /// 地点类别。
    /// </summary>
    public string Category { get; private set; }

    /// <summary>
    /// 地点是否适宜。
    /// </summary>
    public bool Suitable { get; private set; }

    /// <summary>
    /// 人口上限。
    /// </summary>
    public int PopCap { get; private set; }

    /// <summary>
    /// 全局静态变量，当前人口数。
    /// </summary>
    public int Population { get; set; } = 0;

    /// <summary>
    /// 全局静态变量，指示是否为首都。
    /// </summary>
    public bool IsCapital { get; set; } = false;

    /// <summary>
    /// 初始化 CivSite 类的新实例。
    /// </summary>
    /// <param name="x">X坐标。</param>
    /// <param name="y">Y坐标。</param>
    /// <param name="category">地点类别。</param>
    /// <param name="suitable">地点是否适宜。</param>
    /// <param name="popcap">人口上限。</param>
    public CivSite(int x, int y, string category, bool suitable, int popcap)
    {
        X = x;
        Y = y;
        Category = category;
        Suitable = suitable;
        PopCap = popcap;
    }
}
