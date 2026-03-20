namespace NoBadConflicts
{
    internal class Scene_Fighting : Scene
    {
        Color BGcolor = new Color(245, 245, 245, 255);
        float pastTime;
        public float moveX = 0;
        List<ArmyCards> armyCards;
        int cardNum = 0;
        ACard_Pointer pointer = null;
        int armyIndex;
        public Vector2 lastFrameMP = Raylib.GetMousePosition();
        Rectangle cardArea = new Rectangle(0, Raylib.GetScreenHeight() - 90, Raylib.GetScreenWidth(), 90);
        bool onCardArea;
        bool onButton;
        bool nearC;
        bool canCreateArmy;
        bool canCreateSpell;
        bool isFightStart = false;
        bool isFightPause = false;
        bool isFightEnd = false;
        float endT;
        CirRecB quitB;
        PauseB pauseB;
        bool showT = false;
        S_F_Text describeT;
        bool onDT = false;
        CirRecB quitDTB;
        public Scene_Fighting(GameMG gm)
        {
            gm.camera = new Camera2D
            {
                Target = new Vector2(0, 0),
                Offset = Raylib.GetScreenCenter(),
                Rotation = 0.0f,
                Zoom = 0.8f
            };
            endT = 0;
            pastTime = 0;

            #region 选兵相关初始化
            armyIndex = -1;
            gm.M_a.InitArmyData();
            armyCards = new List<ArmyCards>();
            Vector2 cardSize = new Vector2(90, 120);
            for(int i = 0;i  < gm.M_a.armyDataOutF.Count(); i++)
            {
                var army = gm.M_a.armyDataOutF[i];
                armyCards.Add(new ArmyCards(army, cardSize, i));
            }
            #endregion

            #region 建筑初始化
            gm.M_c.conList = new List<ConBase>();
            List<Con_Load> conloads = JsonSerializer.Deserialize<List<Con_Load>>(File.ReadAllText(Scene_Center.chooseP.path));
            gm.M_c.Load(conloads);
            switch (Scene_Center.chooseP.type)
            {
                case E_PointType.Test:
                    for (int i = 0; i < gm.M_a.armyDataInF.Count; i++)
                        gm.M_a.armyDataInF[i] += 10000;
                    break;
                case E_PointType.Easy:
                    for (int i = 0; i < gm.M_c.conList.Count; i++)
                    {
                        gm.M_c.conList[i].maxHealth *= 0.6f;
                        gm.M_c.conList[i].attackPower *= 0.6f;
                    }
                    break;
                case E_PointType.Normal:
                    break;
                case E_PointType.Difficult:
                    for (int i = 0; i < gm.M_c.conList.Count; i++)
                    {
                        gm.M_c.conList[i].maxHealth *= 1.45f;
                        gm.M_c.conList[i].attackPower *= 1.45f;
                    }
                    break;
            }
            #endregion

            gm.casts = new List<CastBase>();

            #region 按键初始化
            quitB = new CirRecB(new Vector2(Raylib.GetScreenWidth() - 60, 40), new Vector2(90, 60), Color.Red);
            quitB.AddText(gm, "放弃", 30f, 0, Color.Black);
            pauseB = new PauseB(new Vector2(Raylib.GetScreenWidth() - 180, 10), new Vector2(60, 60));

            describeT = new S_F_Text(new Vector2(Raylib.GetScreenWidth() - 220f, 80f), 220f, 45f, 270f);
            quitDTB = new CirRecB(new Vector2(Raylib.GetScreenWidth() - 20, 130), new Vector2(40, 40), Color.Red);
            quitDTB.AddText(gm, "关闭", 24f, 0, Color.Black);
            #endregion
        }
        public override void Run(GameMG gm)
        {
            #region 战斗结束
            if (gm.M_a.IsEmpty() || gm.M_c.IsEmpty()) isFightEnd = true;

            if (isFightEnd)
            {
                if(endT <= 0.5f || (endT <= 2f && gm.M_c.percent.starN == 3))
                {
                    endT += gm.dt;
                }
                else
                {
                    GetRT(gm.M_c.percent.starN);
                    if (Scene_Center.chooseP.type != E_PointType.Test)
                    {
                        CheckPointMap.pointP += 1;
                        Scene_Center.chooseP.starN = gm.M_c.percent.starN;
                        CheckPointMap.donePoints.Add(Scene_Center.chooseP);
                    }
                    gm.currentScene = new Scene_Center(gm);
                }  
            }
            #endregion

            #region 相机控制
            if (true)
            {
                if (!onDT)
                {
                    float wheelMove = Raylib.GetMouseWheelMove();
                    if (wheelMove > 0 && gm.camera.Zoom < 3.5f)
                        gm.camera.Zoom *= 1.1f;
                    else if (wheelMove < 0 && gm.camera.Zoom > 0.35f)
                        gm.camera.Zoom /= 1.1f;
                }

                float moved = 500 * gm.dt / gm.camera.Zoom;
                if (Raylib.IsKeyDown(KeyboardKey.W) && gm.camera.Target.Y >= -1000) gm.camera.Target.Y -= moved;
                if (Raylib.IsKeyDown(KeyboardKey.S) && gm.camera.Target.Y <= 1000) gm.camera.Target.Y += moved;
                if (Raylib.IsKeyDown(KeyboardKey.A) && gm.camera.Target.X >= -1200) gm.camera.Target.X -= moved;
                if (Raylib.IsKeyDown(KeyboardKey.D) && gm.camera.Target.X <= 1200) gm.camera.Target.X += moved;
            }
            #endregion

            #region 选兵界面控制
            bool onCardBack = false;
            float changeX = Raylib.GetMousePosition().X - lastFrameMP.X;
            lastFrameMP = Raylib.GetMousePosition();
            if(Raylib.CheckCollisionPointRec(gm.mousePos, cardArea))
            {
                if(Raylib.IsMouseButtonDown(MouseButton.Left) &&
                    moveX + changeX <= Raylib.GetScreenWidth() / 2 && moveX + changeX >= -(cardNum - 1) * 90)
                    moveX += changeX;
                onCardBack = true;
            }
            #endregion

            Raylib.BeginMode2D(gm.camera);

            Raylib.ClearBackground(BGcolor);

            #region 部队释放
            bool doingC = Raylib.IsMouseButtonPressed(MouseButton.Left) || gm.M_Input.GetMouseDownTime() >= 0.15f;
            if (armyIndex >= 0 && doingC)
            {
                if (gm.M_a.armyDataOutF[armyIndex] is SpellBase)
                {
                    if (canCreateSpell)
                    {
                        isFightStart = true;
                        gm.M_a.armyDataOutF[armyIndex].Create(gm, armyIndex);
                        gm.M_Input.mouseDownTime -= 0.2f;
                    }
                }
                else
                {
                    if (canCreateArmy)
                    {
                        isFightStart = true;
                        gm.M_a.armyDataOutF[armyIndex].Create(gm, armyIndex);
                        gm.M_Input.mouseDownTime -= 0.025f;
                    }
                }
            }
            #endregion

            #region 建筑周围
            bool nC = false;
            float nDis = 100f;
            foreach (var con in gm.M_c.conList)
            {
                if (con is TrapBase) continue;
                if(Vector2.Distance(gm.worldPos, con.position) <= nDis) nC = true;
            }
            nearC = nC;
            #endregion

            #region 单位行为和绘制
            if (!isFightPause && isFightStart && !isFightEnd)
            {
                pastTime += gm.dt;
                List<CastBase> castMove = new List<CastBase>();
                foreach (var cast in gm.casts)
                {
                    cast.Behavior(gm);
                    if (cast.pastTime >= cast.maxTime)
                        castMove.Add(cast);
                }
                foreach (var cast in castMove)
                {
                    cast.OnDestroy(gm);
                    gm.casts.Remove(cast);
                }

                gm.M_c.ConAction(gm);
                gm.M_a.ArmyAction(gm);
            }

            foreach (var cast in gm.casts)
            {
                if (cast.type == E_CastType.Ground)
                    cast.Draw();
            }
            foreach (var con in gm.M_c.conList)
            {
                con.DrawSelf(gm);
            }
            foreach (var army in gm.M_a.armyList)
            {
                army.DrawSelf();
            }
            foreach (var cast in gm.casts)
            {
                if (cast.type == E_CastType.Air)
                    cast.Draw();
            }
            foreach (var army in gm.M_a.armyList)
            {
                army.DrawCondition();
            }
            foreach (var con in gm.M_c.conList)
            {
                if (con.lostHealth < con.maxHealth)
                    con.DrawCondition();
            }
            foreach (var con in gm.M_c.conList)
            {
                bool canshow = con.canBeTrack || con.lostHealth >= con.maxHealth;
#if DEBUG
                canshow = true;
#endif
                if (canshow && con.MouseOn(gm))
                {
                    Raylib.DrawCircleLinesV(con.position, con.attackRanger, Color.Red);
                    Raylib.DrawCircleLinesV(con.position, con.attackRangeR, Color.Red);
                    if (InputMG.mulClick)
                    {
                        showT = true;
                        describeT.text_title = con.name + "\n" + $"LV {con.rank}";
                        describeT.text_main = con.description;
                    }
                    if (con.lostHealth >= con.maxHealth) continue;
                    foreach(var army in con.targets)
                    {
                        Raylib.DrawLineV(con.position, army.position, Color.Red);
                    }
                }
            }
            #endregion

            #region 药水范围预览
            if (armyIndex >= 0 && gm.M_a.armyDataOutF[armyIndex] is SpellBase)
            {
                if (gm.M_a.armyDataInF[armyIndex] > 0)
                    Raylib.DrawCircleLinesV(gm.worldPos, gm.M_a.armyDataOutF[armyIndex].attackRange, gm.M_a.armyDataOutF[armyIndex].tcolor);
            }
            #endregion

            Raylib.EndMode2D();

            #region 释放指示
            if (canCreateArmy)
                Raylib.DrawCircleLinesV(gm.mousePos, 5f, Color.Green);
            else
                Raylib.DrawCircleLinesV(gm.mousePos, 5f, Color.Red);
            #endregion

            #region 界面绘制
            //Raylib.DrawText(pastTime.ToString("F1"), 30, 30, 30, Color.Black);
            quitB.Act(gm, ref isFightEnd);
            pauseB.Act(gm, ref isFightPause);
            if (Raylib.IsKeyPressed(KeyboardKey.Space)) isFightPause = !isFightPause;
            if (quitB.IsHangOn(gm) || pauseB.IsHangOn(gm)) onButton = true;
            else onButton = false;

            Raylib.DrawRectangleRec(cardArea, new Color(80, 80, 80, 155));
            bool onCardFront = false;
            int cardI = 0;
            for(int i = 0;i < armyCards.Count(); i++)
            {
                var card = armyCards[i];
                if (gm.M_a.armyDataInF[i] > 0)
                {
                    card.Draw(gm, moveX, 15, cardI, pointer);
                    if (armyCards[i].IsHangOn(gm, moveX, 15, cardI))
                    {
                        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                        {
                            if (pointer != null && pointer.armyCard == armyCards[i])
                            {
                                pointer = new ACard_Pointer(new ArmyCards());
                                armyIndex = -1;
                            }
                            else
                            {
                                pointer = new ACard_Pointer(armyCards[i]);
                                armyIndex = i;
                            }
                        }
                        onCardFront = true;
                        if (InputMG.mulClick)
                        {
                            showT = true;
                            describeT.text_title = card.army.name;
                            describeT.text_main = card.army.description;
                            if(card.army.EX_mode >= 0)
                            {
                                foreach(var a in Abilities.abilities_EX)
                                {
                                    if (a.army == card.army)
                                        describeT.text_main += " \n \n" + a.EXmode[card.army.EX_mode].description;
                                }
                            }
                        }
                    }
                    cardI++;
                }
            }
            cardNum = cardI;

            if (showT)
            {
                describeT.Act(gm, ref onDT);
                quitDTB.Act(gm, ref showT);
            }
            else
                onDT = false;

            Raylib.DrawTextEx(gm.ChineseFont, "摧毁率" + gm.M_c.percent.GetPercent(),
                new Vector2(Raylib.GetScreenWidth() - 150, 400), 32, 0, Color.Black);
            Vector2 starP = new Vector2(Raylib.GetScreenWidth() - 120, 450);
            for(int i = 0; i < 3; i++)
            {
                if (gm.M_c.percent.starN > i)
                    Raylib.DrawCircleV(starP, 18, Color.Gold);
                else
                    Raylib.DrawCircleV(starP, 18, Color.DarkGray);
                Raylib.DrawCircleLinesV(starP, 18, Color.Black);
                starP.X += 45;
            }
            #endregion

            #region 能否释放部队
            if (!onCardBack && !onCardFront)
                onCardArea = false;
            else
                onCardArea = true;

            if(!onCardArea && !onButton && !onDT)
            {
                canCreateSpell = true;
                if(!nearC)
                    canCreateArmy = true;
                else
                    canCreateArmy = false;
            }
            else
            {
                canCreateSpell = false;
                canCreateArmy = false;
            }
            #endregion
        }
        void GetReward(int c, int b, int a, int s,int ex,int num)
        {
            Random r = new Random();
            int[] addn = new int[5] { 0, 0, 0, 0, 0};
            int[] needn = new int[5];
            needn[0] = c;
            needn[1] = needn[0] + b;
            needn[2] = needn[1] + a;
            needn[3] = needn[2] + s;
            needn[4] = needn[3] + ex;
            for(int i = 0; i < num; i++)
            {
                int n = r.Next(0, needn[4]);
                if (n <= needn[0]) addn[0] += 1;
                else if (n <= needn[1]) addn[1] += 1;
                else if (n <= needn[2]) addn[2] += 1;
                else if (n <= needn[3]) addn[3] += 1;
                else if (n <= needn[4]) addn[4] += 1;
            }
            for (int i = 0; i < 5; i++)
            {
                GameMG.items[i] = new(GameMG.items[i].name, GameMG.items[i].num + addn[i]);
                Scene_Center.chooseP.rewards[i] += addn[i];
            }
        }
        void GetRT(int starN)
        {
#if DEBUG
            starN = 3;
#endif
            if(Scene_Center.chooseP.type == E_PointType.Test)
            {

                GetReward(30, 20, 20, 20, 10, 10000);
                return;
            }
            int getnum = 0;
            switch (Scene_Center.numP)
            {
                case 1:
                    GetReward(100, 0, 0, 0, 0, 2);
                    GetReward(0, 100, 0, 0, 0, 2);
                    GetReward(75, 25, 0, 0, 0, 6 + 2 * starN);
                    break;
                case 2:
                    GetReward(25, 75, 0, 0, 0, 6 + 2 * starN);
                    break;
                case 3:
                    if (starN == 2) getnum = 6;
                    else if (starN == 3) getnum = 12;
                    GetReward(0, 60, 30, 10, 0, getnum);
                    GetReward(0, 0, 0, 0, 100, 6);
                    break;
                case 4:
                case 5:
                    if (Scene_Center.chooseP.type == E_PointType.Easy) getnum = 3 * starN;
                    else if(Scene_Center.chooseP.type == E_PointType.Normal) getnum = 5 * starN;
                    else if(Scene_Center.chooseP.type == E_PointType.Difficult) getnum = 7 * starN;
                    GetReward(10, 50, 25, 15, 0, getnum);
                    break;
                case 6:
                    if (starN == 2) getnum = 12;
                    else if(starN == 3) getnum = 18;
                    GetReward(0, 30, 40, 30, 0, getnum);
                    GetReward(0, 0, 0, 0, 100, 3);

                    GetReward(0, 0, 0, 100, 0, starN * 8);
                    break;
            }
        }
    }
}
