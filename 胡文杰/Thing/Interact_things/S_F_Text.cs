using NoBadConflicts;

class S_F_Text
{
    public Rectangle rec;
    Rectangle rec1;
    Rectangle rec2;
    public string text_title = "";
    public string text_main = "";
    float moy1;
    float moy2;
    Color CardBG = new Color(245, 245, 245, 235);
    public S_F_Text(Vector2 position, float x, float y1, float y2)
    {
        rec = new Rectangle(position, x, y1 + y2);
        Vector2 vsize = new Vector2(x, y1);
        rec1 = new Rectangle(position, vsize);
        position.Y += y1;
        vsize.Y = y2;
        rec2 = new Rectangle(position, vsize);
    }
    public void Act(GameMG gm, ref bool onT)
    {
        Raylib.DrawRectangleRec(rec1, CardBG);
        Raylib.DrawRectangleRec(rec2, CardBG);
        Raylib.DrawRectangleLinesEx(rec1, 1.5f, Color.Black);
        Raylib.DrawRectangleLinesEx(rec2, 1.5f, Color.Black);
        if (Raylib.CheckCollisionPointRec(gm.mousePos, rec))
            onT = true;
        else 
            onT = false;
        if (Raylib.CheckCollisionPointRec(gm.mousePos, rec1))
            moy1 += gm.dt * 300f * Raylib.GetMouseWheelMove();
        else moy1 = 0f;
        if (Raylib.CheckCollisionPointRec(gm.mousePos, rec2))
            moy2 += gm.dt * 900f * Raylib.GetMouseWheelMove();
        else moy2 = 0f;
        TextDrawing.DrawTextInRectangle(gm.ChineseFont, text_title, rec1, 36, 0, moy1, true, Color.Black);
        TextDrawing.DrawTextInRectangle(gm.ChineseFont, text_main, rec2, 36, 0, moy2, true, Color.Black);
    }
}
