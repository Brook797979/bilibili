namespace NoBadConflicts
{
    internal class Scene_Center : Scene
    {
        Color BGcolor = new Color(225, 225, 225, 255);

        CirRecB packageB;
        bool toPackage = false;
        CirRecB fightB;
        bool toFight = false;

        Vector2 lastFrameMP;
        public static float moveX = 0;
        public static pointStr chooseP;
        public static float numP = -1;
        public Scene_Center(GameMG gm)
        {
            packageB = new CirRecB(new Vector2(Raylib.GetScreenWidth() * 0.2f, Raylib.GetScreenHeight() - 80),
                new Vector2(120, 80), Color.DarkBrown);
            packageB.AddText(gm, "背包", 36, 0, Color.White);
            fightB = new CirRecB(new Vector2(Raylib.GetScreenWidth() * 0.8f, Raylib.GetScreenHeight() - 80),
                new Vector2(120, 80), Color.DarkBrown);
            fightB.AddText(gm, "战斗", 36, 0, Color.White);

            lastFrameMP = Raylib.GetMousePosition();
            chooseP = new pointStr();
        }
        public override void Run(GameMG gm)
        {
            if (toFight)
            {
                gm.currentScene = new Scene_Fighting(gm);
            }
            else if (toPackage)
            {
                gm.currentScene = new Scene_Package(gm);
            }
            Raylib.ClearBackground(BGcolor);

            packageB.Act(gm, ref toPackage);

            float changeX = Raylib.GetMousePosition().X - lastFrameMP.X;
            lastFrameMP = Raylib.GetMousePosition();
            if (Raylib.IsMouseButtonDown(MouseButton.Left))
                moveX += changeX;
            gm.CPM.DrawPoints(gm);
            if (chooseP.type != E_PointType.None)
            {
                Vector2 pos = new Vector2(Raylib.GetScreenWidth() * 0.7f, Raylib.GetScreenHeight() - 220);
                Raylib.DrawTextEx(gm.ChineseFont, chooseP.type.ToString(), pos, 36, 0, Color.Black);
                pos.Y += 60f;
                Raylib.DrawTextEx(gm.ChineseFont, chooseP.name, pos, 36, 0, Color.Black);
                if(CheckPointMap.donePoints.Contains(chooseP))
                {
                    pos.Y += 40f;
                    Raylib.DrawTextEx(gm.ChineseFont, "获得的奖励：", pos, 36, 0, Color.Black);
                    pos.Y -= 40f;
                    pos.X += 200f;
                    Color[] itemColors = { Color.Green, Color.Blue, Color.Purple, Color.Gold, Color.Red };
                    for (int i = 0;i < 5; i++)
                    {
                        Raylib.DrawTextEx(gm.ChineseFont, chooseP.rewards[i].ToString(), pos, 24, 0, itemColors[i]);
                        pos.Y += 24f;
                    }
                }
                if(CheckPointMap.pointCanIn)
                {
                    fightB.Act(gm, ref toFight);
                }
            }
        }
    }
}

