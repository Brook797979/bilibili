namespace NoBadConflicts
{
    class Scene_Package : Scene
    {
        Color BGcolor = new Color(210, 180, 130, 255);

        CirRecB centerB;
        bool toCenter = false;
        PackageItemsB[] packitemBs = new PackageItemsB[5];
        Color[] itemColors = { Color.Green, Color.Blue, Color.Purple, Color.Gold, Color.Red};
        int choosedB = -1;
        PackageDoB doB1;
        PackageDoB doB2;
        PackageDoB doB3;

        string armyText = "";
        Rectangle recArmy = new Rectangle(new Vector2(700, 200), new Vector2(150, 250));
        float moveY = 0;

        bool getingAb = false;
        List<AbilityCards> abilityCards;
        int choosedC = -1;
        PackageDoB doB4;
        List<ArmyBase> armyCanEX;
        bool chosingEX = false;
        ArmyBase beEX;
        List<ArmyCapsule> armyCapsules;

        public Scene_Package(GameMG gm)
        {
            centerB = new CirRecB(new Vector2(Raylib.GetScreenWidth() - 60, 40), new Vector2(90, 60), Color.Red);
            centerB.AddText(gm, "返回", 30f, 0, Color.Black);
            foreach(var army in gm.M_a.armyDataOutF)
            {
                armyText += $"{army.name}\nLV{army.rank},{army.num}\n \n";
            }
            Vector2 bpos = new Vector2(300, 200);
            Vector2 bsize = new Vector2(120, 40);
            for(int i = 0;i < 5; i++)
            {
                packitemBs[i] = new PackageItemsB( bpos, bsize, itemColors[i], Color.White, THe.S_package_text[i]);
                bpos.Y += 1.2f * bsize.Y;
            }
            bsize = new Vector2(200, 50);
            doB1 = new PackageDoB(gm, new Vector2(260, 480), bsize, THe.S_package_text[5]);
            doB2 = new PackageDoB(gm, new Vector2(260, 540), bsize, THe.S_package_text[6]);
            doB3 = new PackageDoB(gm, new Vector2(260, 600), bsize, THe.S_package_text[7]);
            doB4 = new PackageDoB(gm, new Vector2(Raylib.GetScreenWidth() / 2 - bsize.X / 2, Raylib.GetScreenHeight() - 100), 
                bsize, THe.S_package_text[8]);
        }
        public override void Run(GameMG gm)
        {
            if (toCenter)
            {
                gm.currentScene = new Scene_Center(gm);
            }
            Raylib.ClearBackground(BGcolor);

            //天赋选择
            if (getingAb)
            {
                //CBAS
                if (choosedB >= 0 && choosedB <= 3)
                {
                    for (int i = 0; i < abilityCards.Count; i++)
                    {
                        abilityCards[i].Act(gm, choosedC == i);
                        if (Raylib.CheckCollisionPointRec(gm.mousePos, abilityCards[i].rec) &&
                            Raylib.IsMouseButtonDown(MouseButton.Left))
                            choosedC = i;
                    }
                    bool d4 = choosedC >= 0 && choosedC < abilityCards.Count;
                    doB4.Act(gm, d4);
                    if (d4 && doB4.IsClick(gm))
                    {
                        Abilities.AbilityValidate(gm, abilityCards[choosedC].ability.str);
                        abilityCards[choosedC].ability.nownum += 1;
                        if (abilityCards[choosedC].ability.nownum >= abilityCards[choosedC].ability.maxnum)
                        {
                            Abilities.AbRemove(abilityCards[choosedC].ability);
                        }
                        armyText = "";
                        foreach (var army in gm.M_a.armyDataOutF)
                        {
                            armyText += $"{army.name}\nLV{army.rank},{army.num}\n \n";
                        }
                        getingAb = false;
                    }
                }
                //EX
                else if(choosedB == 4)
                {
                    //选择觉醒兵种
                    if (!chosingEX)
                    {
                        for (int i = 0; i < armyCapsules.Count; i++)
                        {
                            armyCapsules[i].Act(gm, choosedC == i);
                            if (Raylib.IsMouseButtonPressed(MouseButton.Left) 
                                && Raylib.CheckCollisionPointRec(gm.mousePos, armyCapsules[i].rec))
                            {
                                choosedC = i;
                            }
                        }
                        bool d4 = choosedC >= 0;
                        doB4.Act(gm, d4);
                        if (d4 && doB4.IsClick(gm))
                        {
                            beEX = armyCanEX[choosedC];
                            choosedC = -1;
                            abilityCards = new List<AbilityCards>();
                            EXabiliStruct ex = Abilities.GetAbEX(beEX);
                            if (ex.EXmode == null || ex.EXmode.Count <= 0)
                            {
                                ErroR.Report("无可用觉醒");
                                getingAb = false;
                            }
                            else
                            {
                                float x = 220f;
                                float y1 = 45f;
                                float y2 = 300f;
                                Vector2 pos = Raylib.GetScreenCenter();
                                pos.Y -= (y1 + y2) / 2;
                                pos.X -= ex.EXmode.Count * x / 2 + 0.1f * (x - 1);
                                foreach (var mode in ex.EXmode)
                                {
                                    abilityCards.Add(new AbilityCards(mode, pos, x, y1, y2, itemColors[4]));
                                    pos.X += x * 1.2f;
                                }
                                chosingEX = true;
                            }
                        }
                    }
                    //选择觉醒类型
                    else
                    {
                        for (int i = 0;i < abilityCards.Count; i++)
                        {
                            abilityCards[i].Act(gm, choosedC == i);
                            if (Raylib.CheckCollisionPointRec(gm.mousePos, abilityCards[i].rec) &&
                            Raylib.IsMouseButtonDown(MouseButton.Left))
                                choosedC = i;
                        }
                        bool d4 = choosedC >= 0;
                        doB4.Act(gm, d4);
                        if (d4 && doB4.IsClick(gm))
                        {
                            beEX.EX_mode = choosedC;
                            EXabiliStruct ex = Abilities.GetAbEX(beEX);
                            beEX.name = ex.EXmode[choosedC].title;

                            armyText = "";
                            foreach (var army in gm.M_a.armyDataOutF)
                            {
                                armyText += $"{army.name}\nLV{army.rank},{army.num}\n \n";
                            }
                            getingAb = false;
                        }
                    }
                }
            }
            //背包
            else
            {
                centerB.Act(gm, ref toCenter);
                for(int i = 0; i < packitemBs.Length; i++)
                {
                    if (packitemBs[i].IsClick(gm)) choosedB = i;
                    Vector2 numpos = packitemBs[i].pos;
                    numpos.X *= 1.5f;
                    if(choosedB == i)
                    {
                        Vector2 bp = packitemBs[i].pos;
                        bp.Y += packitemBs[i].rec.Height / 2;
                        bp.X -= packitemBs[i].rec.Width / 2;
                        Vector2 ep = bp;
                        bp.X += packitemBs[i].rec.Width * 2;
                        Raylib.DrawLineV(bp, ep, packitemBs[i].c1);
                        packitemBs[i].Act(gm, true);
                        Raylib.DrawTextEx(gm.ChineseFont, GameMG.items[i].num.ToString(), numpos, 36, 0, Color.White);
                    }
                    else
                    {
                        packitemBs[i].Act(gm, false);
                        Raylib.DrawTextEx(gm.ChineseFont, GameMG.items[i].num.ToString(), numpos, 36, 0, Color.White);
                    }
                }
                if(choosedB >= 0 && choosedB < packitemBs.Length)
                {
                    bool r1 = GameMG.items[choosedB].num >= 3;
                    bool r2 = GameMG.items[choosedB].num >= 2 && choosedB < 4;
                    bool r3 = GameMG.items[choosedB].num >= 1 && choosedB > 0;
                    doB1.Act(gm, r1);
                    doB2.Act(gm, r2);
                    doB3.Act(gm, r3);
                    if(r1 && doB1.IsClick(gm))
                    {
                        GameMG.items[choosedB] = (GameMG.items[choosedB].name, GameMG.items[choosedB].num - 3);
                        getingAb = true;
                        choosedC = -1;
                        if(choosedB <= 3)
                        {
                            GetCards();
                            if (abilityCards.Count <= 0) getingAb = false;
                        }
                        else if(choosedB == 4)
                        {
                            GetCapsules(gm);
                        }
                    }
                    if (r2 && doB2.IsClick(gm))
                    {
                        GameMG.items[choosedB] = (GameMG.items[choosedB].name, GameMG.items[choosedB].num - 2);
                        GameMG.items[choosedB + 1] = (GameMG.items[choosedB + 1].name, GameMG.items[choosedB + 1].num + 1);
                    }
                    if (r3 && doB3.IsClick(gm))
                    {
                        GameMG.items[choosedB] = (GameMG.items[choosedB].name, GameMG.items[choosedB].num - 1);
                        GameMG.items[choosedB - 1] = (GameMG.items[choosedB - 1].name, GameMG.items[choosedB - 1].num + 2);
                    }
                }

                Raylib.DrawRectangleRec(recArmy, Color.LightGray);
                TextDrawing.DrawTextInRectangle(gm.ChineseFont, armyText, recArmy, 30f, 0, moveY, true, Color.Black);
                if (Raylib.CheckCollisionPointRec(gm.mousePos, recArmy))
                    moveY += gm.dt * 1500f * Raylib.GetMouseWheelMove();
                else moveY = 0;
            }
        }
        void GetCards()
        {
            List<AbiliClass> abilities = new List<AbiliClass>();
            List<AbiliClass> gotAbs = new List<AbiliClass>();
            if (choosedB == 0) abilities = Abilities.abilities_C;
            else if (choosedB == 1) abilities = Abilities.abilities_B;
            else if (choosedB == 2) abilities = Abilities.abilities_A;
            else if (choosedB == 3) abilities = Abilities.abilities_S;

            while (true)
            {
                if (gotAbs.Count >= abilities.Count || gotAbs.Count >= 3) break;
                AbiliClass ab = Abilities.GetAb(abilities);
                if (!gotAbs.Contains(ab))
                {
                    gotAbs.Add(ab);
                }
            }
            if (gotAbs.Count <= 0) ErroR.Report("空的天赋池");

            abilityCards = new List<AbilityCards>();
            float x = 220f;
            float y1 = 45f;
            float y2 = 300f;
            Vector2 pos = Raylib.GetScreenCenter();
            pos.Y -= (y1 + y2) / 2;
            pos.X -= gotAbs.Count * x / 2 + 0.1f * x * (gotAbs.Count - 1);
            foreach (var ab in gotAbs)
            {
                abilityCards.Add(new AbilityCards(ab, pos, x, y1, y2, itemColors[choosedB]));
                pos.X += x * 1.2f;
            }
        }
        void GetCapsules(GameMG gm)
        {
            List<ArmyBase> nEXyet = new List<ArmyBase>();
            armyCanEX = new List<ArmyBase>();
            foreach (var army in gm.M_a.armyDataOutF)
            {
                if (army is SpellBase) break;
                if (army.EX_mode < 0) nEXyet.Add(army);
            }
            chosingEX = false;
            if (nEXyet.Count <= 0)
            {
                ErroR.Report("空的待觉醒列表");
                getingAb = false;
            }
            else if (nEXyet.Count <= 3)
            {
                armyCanEX = nEXyet;
            }
            else
            {
                Random r = new Random();
                while (true)
                {
                    if (armyCanEX.Count >= 3) break;
                    int index = r.Next(0, nEXyet.Count);
                    if (!armyCanEX.Contains(nEXyet[index])) armyCanEX.Add(nEXyet[index]);
                }
            }
            Vector2 pos = Raylib.GetScreenCenter();
            Vector2 size = new Vector2(200, 60);
            pos.Y -= size.Y / 2;
            pos.X -= armyCanEX.Count * size.X / 2 + 0.1f * size.X * (armyCanEX.Count - 1);
            armyCapsules = new List<ArmyCapsule>();
            foreach (var army in armyCanEX)
            {
                armyCapsules.Add(new ArmyCapsule(gm, new Rectangle(pos, size), army));
                pos.X += size.X * 1.2f;
            }
        }
    }
}
