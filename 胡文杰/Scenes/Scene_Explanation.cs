namespace NoBadConflicts
{
    internal class Scene_Explanation : Scene
    {
        Color BGcolor = new Color(210, 180, 130, 245);
        float passt = 0;

        CirRecB nextB;
        bool toN = false;
        List<ChooseB> choBs = new List<ChooseB>();
        ChooseB beChoB = new ChooseB();
        string Rtext = "";

        Rectangle rec = new Rectangle(new Vector2(400, 50), new Vector2(650, 480));
        float moveY = 0f;
        public Scene_Explanation(GameMG gm)
        {
#if DEBUG
            passt = 3f;
#endif
            nextB = new CirRecB(new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() - 60),
                new Vector2(120, 80), Color.DarkBrown);
            nextB.AddText(gm, "下一步", 36, 0, Color.White);

            Color c = new Color(45, 39, 30, 0);
            Vector2 posV = new Vector2(80, 50);
            Vector2 sizeV = new Vector2(120, 72);
            for (int i = 0; i < THe.S_explanation_title.Length; i++)
            {
                choBs.Add(new ChooseB(posV, sizeV, c, Color.DarkBrown, THe.S_explanation_title[i]));
                posV.Y += sizeV.Y * 1.2f;
            }
        }
        public override void Run(GameMG gm)
        {
            if (toN)
            {
                gm.currentScene = new Scene_Selects(gm);
            }
            Raylib.ClearBackground(BGcolor);

            passt += gm.dt;
            if (choBs.Contains(beChoB) || passt >= 3f)
                nextB.Act(gm, ref toN);
            for (int i = 0; i < choBs.Count; i++)
            {
                if (choBs[i].IsClick(gm))
                {
                    beChoB = choBs[i];
                    moveY = 0f;
                    Rtext = THe.S_explanation_main[i];
                }
                choBs[i].Act(gm, beChoB);
            }

            Raylib.DrawRectangleLinesEx(rec, 3f, Color.DarkBrown);
            if(Raylib.CheckCollisionPointRec(gm.mousePos, rec))
            {
                moveY += gm.dt * 1000f * Raylib.GetMouseWheelMove();
            }
            TextDrawing.DrawTextInRectangle(gm.ChineseFont, Rtext, rec, 36, 0, moveY, true, Color.White);
        }
    }
}
