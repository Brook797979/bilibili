namespace NoBadConflicts
{
    class ChooseB : ButtonBase
    {
        Rectangle rec;
        Color lineC;
        string text;
        public ChooseB() { }
        public ChooseB(Vector2 pos, Vector2 size, Color bkc, Color lc, string text)
        {
            this.pos = pos;
            this.size = size;
            rec = new Rectangle(pos, size);
            this.color = bkc;
            this.lineC = lc;
            this.text = text;
        }
        public void Act(GameMG gm,ChooseB chooseB)
        {
            if (this.IsHangOn(gm)) color.A = 255;
            else color.A = 155;

            Raylib.DrawRectangleRec(rec, color);
            Raylib.DrawRectangleLinesEx(rec, 2f, lineC);
            if (chooseB == this)
                Raylib.DrawTextEx(gm.ChineseFont, text, pos, 36, 0, Color.White);
            else
                Raylib.DrawTextEx(gm.ChineseFont, text, pos, 36, 0, Color.DarkGray);
        }
    }
    class PackageItemsB : ButtonBase
    {
        public Rectangle rec;
        public Color c1;
        public Color c2;
        string text;
        public PackageItemsB(Vector2 pos, Vector2 size, Color c1, Color c2, string text)
        {
            this.pos = pos;
            this.size = size;
            rec = new Rectangle(pos, size);
            this.c1 = c1;
            this.c2 = c2;
            this.text = text;
        }
        public void Act(GameMG gm, bool beChoose)
        {
            if (beChoose)
            {
                Raylib.DrawRectangleRec(rec, c1);
                Raylib.DrawTextEx(gm.ChineseFont, text, pos, 36, 0, c2);
            }
            else
            {
                Raylib.DrawRectangleRec(rec, c2);
                Raylib.DrawTextEx(gm.ChineseFont, text, pos, 36, 0, c1);
            }
               
        }
    }
    class PackageDoB : ButtonBase
    {
        public Rectangle rec;
        string text;
        Vector2 tpos;
        public PackageDoB(GameMG gm, Vector2 pos, Vector2 size, string text)
        {
            this.pos = pos;
            this.size = size;
            rec = new Rectangle(pos, size);
            this.text = text;
            tpos = pos;
            Vector2 takes = Raylib.MeasureTextEx(gm.ChineseFont, text, 36, 0);
            tpos.X += size.X / 2 - takes.X / 2;
            tpos.Y += size.Y / 2 - takes.Y / 2;
        }
        public void Act(GameMG gm, bool cando)
        {
            Color color = cando ? Color.Purple : Color.DarkGray;

            if (this.IsHangOn(gm)) color.A = 255;
            else color.A = 155;

            Raylib.DrawRectangleRounded(rec, 0.3f, 6, color);
            Raylib.DrawRectangleRoundedLines(rec, 0.3f, 6, Color.Brown);
            Raylib.DrawTextEx(gm.ChineseFont, text, tpos, 36, 0, Color.White);
        }
    }
    class CirRecB : ButtonBase
    {
        Vector2 cPos;
        VCenterText? text;
        public CirRecB(Vector2 center, Vector2 Rsize, Color color) 
        { 
            cPos = center;
            size = Rsize;
            pos = cPos - Rsize / 2;
            this.color = color;
            rect = new Rectangle(pos, size);
        }
        public void AddText(GameMG gm, string t, float size, float spacing, Color col)
        {
            text = new VCenterText(gm.ChineseFont, t, (int)cPos.X, (int)cPos.Y, size, spacing, col);
        }
        public void Act(GameMG gm, ref bool which2change)
        {
            if (this.IsHangOn(gm)) color.A = 255;
            else color.A = 155;

            if(this.IsClick(gm)) which2change = !which2change;

            Raylib.DrawRectangleRounded(rect, 0.3f, 6, color);
            if(text != null) 
                text.Draw();
        }
    }
    class PauseB : ButtonBase
    {
        Vector2 v1;
        Vector2 v2;
        Vector2 v3;
        Rectangle rl;
        Rectangle rr;
        public PauseB(Vector2 pos, Vector2 size)
        {
            this.pos = pos;
            this.size = size;
            rect = new Rectangle(pos, size);
            v1 = new Vector2(pos.X + size.X * 0.25f, pos.Y + size.Y * 0.2f);
            v2 = new Vector2(pos.X + size.X * 0.25f, pos.Y + size.Y * 0.8f);
            v3 = new Vector2(pos.X + size.X * 0.75f, pos.Y + size.Y * 0.5f);
            rl = new Rectangle(new Vector2(pos.X + size.X * 0.2f, pos.Y + size.Y * 0.2f), new Vector2(size.X * 0.2f, size.Y * 0.6f));
            rr = new Rectangle(new Vector2(pos.X + size.X * 0.6f, pos.Y + size.Y * 0.2f), new Vector2(size.X * 0.2f, size.Y * 0.6f));
        }
        public void Act(GameMG gm, ref bool which2change)
        {
            if (this.IsHangOn(gm)) color.A = 255;
            else color.A = 155;

            if (this.IsClick(gm)) which2change = !which2change;

            Raylib.DrawRectangleRounded(rect, 0.3f, 6, Color.Black);
            if (!which2change)
            {
                Raylib.DrawRectangleRec(rl, Color.White);
                Raylib.DrawRectangleRec(rr, Color.White);
            }
            else
            {
                Raylib.DrawTriangle(v1, v2, v3, Color.White);
            }
        }
    }
}
