namespace NoBadConflicts
{
    internal class S2C
    {
        static public Color Do(string s) 
        {
            Color color;
            if (s.Length != 8)
            {
                color = Color.Black;
                ErroR.Report("不合规S2C字符串: " + s);
            }
            else
            {
                try
                {
                    color = new Color(
                        Convert.ToInt32(s.Substring(0, 2), 16),
                        Convert.ToInt32(s.Substring(2, 2), 16),
                        Convert.ToInt32(s.Substring(4, 2), 16),
                        Convert.ToInt32(s.Substring(6, 2), 16)
                        );
                }
                catch
                {
                    ErroR.Report("不合规S2C字符串: " + s);
                    return Color.Black;
                }
            }
            return color;
        }
    }
}
