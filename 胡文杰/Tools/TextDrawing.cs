class VCenterText
{
    Font font;
    string text;
    int x;
    int y;
    float size;
    float spacing;
    public Color color;
    public VCenterText(Font font, string text, int x, int y, float size, float spacing, Color color)
    {
        this.font = font;
        this.text = text;
        this.x = x;
        this.y = y;
        this.size = size;
        this.spacing = spacing;
        this.color = color;
    }
    public void Draw()
    {
        Vector2 tsize = Raylib.MeasureTextEx(font, text, size, spacing);
        int tx = x - (int)(tsize.X / 2);
        int ty = y - (int)(tsize.Y / 2);
        Raylib.DrawTextEx(font, text, new Vector2(tx, ty), size, spacing, color);
    }
}

public static class TextDrawing
{
    public static void DrawTextInRectangle(
        Font font,
        string text,
        Rectangle rect,
        float fontSize,
        float spacing,
        float moveY,
        bool wordWrap,
        Color color)
    {
        Raylib.BeginScissorMode((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);

        if (!wordWrap)
            Raylib.DrawTextEx(font, text, new Vector2(rect.X, rect.Y), fontSize, spacing, color);
        else
        {
            float lineHeight = fontSize + spacing;
            float maxWidth = rect.Width;

            string[] paragraphs = text.Split('\n');
            List<string> lines = new List<string>();

            foreach (string para in paragraphs)
            {
                string templine = new string("");
                for (int i = 0;i < para.Length;i++)
                {
                    templine += para[i];
                    Vector2 testSize = Raylib.MeasureTextEx(font, templine, fontSize, spacing);
                    if (testSize.X + fontSize * 0.5f >= maxWidth)
                    {
                        lines.Add(templine);
                        templine = new string("");
                    }
                }
                if (templine.Length > 0) 
                    lines.Add(templine);
            }

            float yPos = rect.Y + moveY;
            foreach (string line in lines)
            {
                if (yPos + fontSize < rect.Y)
                {
                    yPos += lineHeight;
                    continue;
                }
                if (yPos - fontSize > rect.Y + rect.Height) break;

                Raylib.DrawTextEx(font, line, new Vector2(rect.X + fontSize * 0.2f, yPos), fontSize, spacing, color);
                yPos += lineHeight;
            }
        }

        Raylib.EndScissorMode();
    }
}
