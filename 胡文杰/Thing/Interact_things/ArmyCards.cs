namespace NoBadConflicts
{
    internal class ArmyCards : ButtonBase
    {
        public ArmyBase army;
        public string name;
        public int id;
        public Color darkColor;

        public ArmyCards() { }
        public ArmyCards(ArmyBase army, Vector2 size, int id)
        {
            this.army = army;
            this.name = army.name;
            this.color = army.tcolor;
            this.color.A = 200;
            this.darkColor = new Color((int)army.tcolor.R / 2, army.tcolor.G / 2, army.tcolor.B / 2, 255);
            this.size = size;
            this.id = id;
        }

        public void Draw(GameMG gm, float moveX, float interX, int p, ACard_Pointer pointer)
        {
            pos = new Vector2(0 + p * size.X, Raylib.GetScreenHeight() - size.Y);
            Vector2 lessSize = new Vector2(size.X, size.Y / 4);
            Vector2 moreSize = new Vector2(size.X, size.Y - lessSize.Y);
            Raylib.DrawRectangleV(new Vector2(pos.X + moveX + p * interX, pos.Y), lessSize, darkColor);
            float x = pos.X + moveX + p * interX;
            float y = pos.Y;
            Raylib.DrawTextEx(gm.ChineseFont, name, new Vector2(x, y), 210 / (name.Length + 4), 0, Color.White);
            Raylib.DrawRectangleV(new Vector2(pos.X + moveX + p * interX, pos.Y + lessSize.Y), 
                moreSize, IsHangOn(gm, moveX, interX, p) ? darkColor : color);
            Raylib.DrawText(gm.M_a.armyDataInF[id].ToString(),
                (int)(pos.X + moveX + p * interX), (int)(pos.Y + lessSize.Y + 10), 24, Color.White);
            Raylib.DrawText($"LV {gm.M_a.armyDataOutF[id].rank}",
                (int)(pos.X + moveX + p * interX), (int)(pos.Y + lessSize.Y + 30), 16, Color.White);

            if (pointer != null && pointer.armyCard == this)
            {
                int ex_dis = 5;
                int lr_dis = 55;
                Vector2 uP = new Vector2(pos.X + moveX + p * interX - ex_dis, pos.Y + size.Y - 25);
                Vector2 dP = new Vector2(pos.X + moveX + p * interX - ex_dis, pos.Y + size.Y);
                Vector2 cP = new Vector2(pos.X + moveX + p * interX - ex_dis + lr_dis, pos.Y + size.Y);
                Raylib.DrawTriangle(uP, dP, cP, new Color(255, 255, 255, 185));
                uP = new Vector2(pos.X + moveX + p * interX + size.X + ex_dis, pos.Y + size.Y - 25);
                dP = new Vector2(pos.X + moveX + p * interX + size.X + ex_dis, pos.Y + size.Y);
                cP = new Vector2(pos.X + moveX + p * interX + size.X - lr_dis, pos.Y + size.Y);
                Raylib.DrawTriangle(uP, cP, dP, new Color(255, 255, 255, 185));
            }
        }
        public bool IsHangOn(GameMG gm, float moveX, float interX, int p)
        {
            Vector2 mp = gm.mousePos;
            mp.X -= moveX + p * interX;
            if (mp.X >= pos.X && mp.X <= pos.X + size.X && mp.Y >= pos.Y && mp.Y <= pos.Y + size.Y)
                return true;
            return false;
        }
    }
    class ACard_Pointer
    {
        public ArmyCards armyCard;
        public int index;
        public ACard_Pointer(ArmyCards ac)
        {
            armyCard = ac;
        }
    }

}
