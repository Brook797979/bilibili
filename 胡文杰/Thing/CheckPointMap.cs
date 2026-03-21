namespace NoBadConflicts
{
    enum E_PointType
    {
        None = -1,
        Test = 0,
        Easy = 1,
        Normal = 2,
        Difficult = 3,
        Boss = 11
    }
    class pointStr
    {
        public E_PointType type = E_PointType.None;
        public string name = "null";
        public string path = "null";
        public int starN;
        public int[] rewards;
        public pointStr() { }
        public pointStr(E_PointType pt, string n, string p)
        {
            type = pt;
            name = n;
            path = p;
            starN = 0;
            rewards = new int[5];
        }
    }
    class CheckPointMap
    {
        public List<List<pointStr>> checkPoints;
        public static List<pointStr> donePoints;
        public static int pointP = 1;
        public static bool pointCanIn;
        public void Init()
        {
            checkPoints = new List<List<pointStr>>();
            donePoints = new List<pointStr>();
            Random r = new Random();
            List<(string lp, string name)> names = new List<(string lp, string name)>();
            List<E_PointType> types = new List<E_PointType>();
            types.Add(E_PointType.Easy);
            types.Add(E_PointType.Normal);
            types.Add(E_PointType.Difficult);

            string fp = @"ASSET\Json\Con_Map\";
            List<pointStr> plist_0 = new List<pointStr>();
#if DEBUG
            plist_0.Add(new pointStr(E_PointType.Test, "测试用", fp + "Test.json"));
#endif
            checkPoints.Add(plist_0);

            #region 关卡1，2，3
            List<pointStr> plist_1 = new List<pointStr>();
            plist_1.Add(new pointStr(E_PointType.Normal, "新手教程？", fp + "n1.json"));
            checkPoints.Add(plist_1);

            List<pointStr> plist_2 = new List<pointStr>();
            plist_2.Add(new pointStr(E_PointType.Normal, "初来炸到", fp + "n2.json"));
            checkPoints.Add(plist_2);

            List<pointStr> plist_3 = new List<pointStr>();
            plist_3.Add(new pointStr(E_PointType.Boss, "偷心贼", fp + "n3.json"));
            checkPoints.Add(plist_3);
            #endregion
            #region 关卡4，5，6
            types.Clear();
            types.AddRange(new E_PointType[] { E_PointType.Easy, E_PointType.Normal, E_PointType.Difficult });
            names.Clear();
            names.AddRange(new (string lp, string name)[]
            {
                ("n4_1.json", "强化基础"),
                ("n4_2.json", "巨人杀手"),
                ("n4_3.json", "缤纷魔法"),
                ("n4_4.json", "射程优势"),

                ("n4_1.json", "强化基础"),
                ("n4_2.json", "巨人杀手"),
                ("n4_3.json", "缤纷魔法"),
                ("n4_4.json", "射程优势"),
            });

            List<pointStr> plist_4 = new List<pointStr>();
            for (int i = 0; i < 3; i++)
            {
                int rn = r.Next(0, names.Count);
                plist_4.Add(new pointStr(types[i], names[rn].name, fp + names[rn].lp));
                names.RemoveAt(rn);
            }
            checkPoints.Add(plist_4);
            List<pointStr> plist_5 = new List<pointStr>();
            for (int i = 0; i < 3; i++)
            {
                int rn = r.Next(0, names.Count);
                plist_5.Add(new pointStr(types[i], names[rn].name, fp + names[rn].lp));
                names.RemoveAt(rn);
            }
            checkPoints.Add(plist_5);

            List<pointStr> plist_6 = new List<pointStr>();
            plist_6.Add(new pointStr(E_PointType.Boss, "齐步走", fp + "n5.json"));
            checkPoints.Add(plist_6);
            #endregion
            List<pointStr> plist_7 = new List<pointStr>();
            plist_7.Add(new pointStr(E_PointType.Boss, "再创的第七天", fp + "n6.json"));
            checkPoints.Add(plist_7);
        }
        public void DrawPoints(GameMG gm)
        {
            float size = 30f;
            float sw = Raylib.GetScreenWidth();
            Scene_Center.moveX = Scene_Center.moveX <= sw / 3 ? Scene_Center.moveX : sw / 3;
            Scene_Center.moveX = Scene_Center.moveX >= -(sw / 3 + checkPoints.Count * size * 4f) ? 
                Scene_Center.moveX : -(sw / 3 + checkPoints.Count * size * 4f);
            for (int i = 0; i < checkPoints.Count; i++)
            {
                Vector2 pos = Raylib.GetScreenCenter();
                pos.X += i * size * 4f + Scene_Center.moveX;
                pos.Y -= checkPoints[i].Count / 2 * size * 2.4f;
                for (int j = 0; j < checkPoints[i].Count; j++)
                {
                    if (Vector2.Distance(gm.mousePos, pos) < size && Raylib.IsMouseButtonPressed(MouseButton.Left))
                    {
                        pointCanIn = i == pointP || i == 0;
                        if (Scene_Center.chooseP == checkPoints[i][j])
                        {
                            Scene_Center.chooseP = new pointStr();
                        }
                        else
                        {
                            Scene_Center.chooseP = checkPoints[i][j];
                            Scene_Center.numP = i;
                        }
                    }

                    if (i == 0) continue;
                    if (donePoints.Contains(checkPoints[i][j]))
                    {
                        Raylib.DrawCircleV(pos, size, Color.Gray);
                        Raylib.DrawCircleLinesV(pos, size, Color.Black);
                        Vector2[] spos = { pos, pos, pos };
                        spos[0].X -= size * 0.40f;
                        spos[2].X += size * 0.40f;
                        spos[0].Y += size * 0.85f;
                        spos[1].Y += size * 0.40f;
                        spos[2].Y += size * 0.85f;
                        for (int ii = 0; ii < 3; ii++)
                        {
                            if (checkPoints[i][j].starN > ii)
                                Raylib.DrawCircleV(spos[ii], size * 0.3f, Color.Gold);
                            else
                                Raylib.DrawCircleV(spos[ii], size * 0.3f, Color.LightGray);
                            Raylib.DrawCircleLinesV(spos[ii], size * 0.3f, Color.Black);
                        }
                    }
                    else if (i == pointP)
                    {
                        Raylib.DrawCircleV(pos, size, Color.White);
                        if (Scene_Center.chooseP == checkPoints[i][j])
                        {
                            Raylib.DrawCircleLinesV(pos, size, Color.Red);
                            Raylib.DrawCircleV(pos, size * 0.8f, Color.Red);
                        }
                        else
                            Raylib.DrawCircleLinesV(pos, size, Color.Black);
                    }
                    else
                    {
                        Raylib.DrawCircleV(pos, size, Color.DarkGray);
                        Raylib.DrawCircleLinesV(pos, size, Color.Black);
                    }
                    pos.Y += size * 2.4f;
                }
            }
#if DEBUG
            pointCanIn = true;
#endif
        }
    }
}
