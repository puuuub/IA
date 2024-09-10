
public static class StaticText 
{
    public const string SIZEHEAD = "<size=";
    public const string SIZETAIL = "</size>";
    public const string COLORHEAD = "<color=";
    public const string COLORTAIL = "</color>";
    public const string CLOSE = ">";
    
    
    public const string PERCENT = "%";
    public const string BAR = "-";
    public const string EMPTY = " ";

    public const string OPEN_S = "(";
    public const string CLOSE_S = ")";

    public const string SLASH = "/";
    public const string ENTER = "\n";

    public const string T1Code = "166";
    public const string CACode = "668";
    public const string T2Code = "168";


    public const string ON = "시작";
    public const string OFF = "종료";


    public const string ZERO = "0";

    public const string SIK = "식";

    public const string DATEFORMAT_0 = "yyyy-MM-dd HH:mm:ss";
    public const string DATEFORMAT_1 = "yyyy-MM-dd";
    public const string DATEFORMAT_2 = "HH:mm:ss";

    public static string SetSize(int size, string str)
    {
        return SIZEHEAD + size.ToString() + CLOSE + str + SIZETAIL;
    }

    public static string SetColor(string color, string str)
    {
        return COLORHEAD + color + CLOSE + str + COLORTAIL;
    }
        
}
