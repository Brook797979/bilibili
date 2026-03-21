namespace NoBadConflicts
{
    class AbilityCards
    {
        public AbiliClass ability;
        public Rectangle rec;
        Rectangle rec1;
        Rectangle rec2;
        float moy1;
        float moy2;
        Color linec;
        Color CardBG = new Color(245, 245, 245, 245);
        public AbilityCards(AbiliClass ability, Vector2 position, float x, float y1, float y2, Color linec)
        {
            this.ability = ability;
            rec = new Rectangle(position, x, y1 + y2);
            Vector2 vsize = new Vector2(x, y1);
            rec1 = new Rectangle(position, vsize);
            position.Y += y1;
            vsize.Y = y2;
            rec2 = new Rectangle(position, vsize);
            this.linec = linec;
        }
        public void Act(GameMG gm, bool bechoosed)
        {
            if (bechoosed)
            {
                Vector2 pos = rec.Position - rec.Size * 0.05f;
                Raylib.DrawRectangleV(pos, rec.Size / 2, Color.White);
                pos += rec.Size * 0.6f;
                Raylib.DrawRectangleV(pos, rec.Size / 2, Color.White);
            }
            Raylib.DrawRectangleRec(rec1, CardBG);
            Raylib.DrawRectangleRec(rec2, CardBG);
            Raylib.DrawRectangleLinesEx(rec1, 4f, linec);
            Raylib.DrawRectangleLinesEx(rec2, 2f, linec);
            if (Raylib.CheckCollisionPointRec(gm.mousePos, rec1))
                moy1 += gm.dt * 300f * Raylib.GetMouseWheelMove();
            else moy1 = 0f;
            if (Raylib.CheckCollisionPointRec(gm.mousePos, rec2))
                moy2 += gm.dt * 900f * Raylib.GetMouseWheelMove();
            else moy2 = 0f;
            TextDrawing.DrawTextInRectangle(gm.ChineseFont, ability.title, rec1, 36, 0, moy1, true, Color.Black);
            TextDrawing.DrawTextInRectangle(gm.ChineseFont, ability.description, rec2, 36, 0, moy2, true, Color.Black);
        }
    }
    
}
