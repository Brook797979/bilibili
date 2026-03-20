namespace NoBadConflicts
{
    internal class GameMG : ManagerBase<GameMG>
    {
        public ArmyMG M_a;
        public ConstructionMG M_c;
        public CheckPointMap CPM = new CheckPointMap();
        public SoundMG M_Sound;
        public InputMG M_Input;
        public Abilities M_Abilities;
        public List<CastBase> casts;
        public Font ChineseFont;
        public Camera2D camera;
        public Scene currentScene;
        public float dt;
        public Vector2 mousePos;
        public Vector2 worldPos;
        public static List<(string name, int num)> items;

        public GameMG() //游戏管理器构造函数，初始化窗口和字体
        {
            #region 基础设置
            Raylib.InitWindow(1080, 660, "不赖冲突");
            Raylib.SetTargetFPS(60);
            Raylib.InitAudioDevice();
            Raylib.SetMasterVolume(0.4f);
            #endregion

            #region 单例初始化
            M_a = ArmyMG.Instance;
            M_c = ConstructionMG.Instance;
            M_Sound = SoundMG.Instance;
            M_Input = InputMG.Instance;
            M_Abilities = Abilities.Instance;
            #endregion

            #region 单例数据加载
            //加载兵种数据
            try
            {
                M_a.armyDataOutF = JsonSerializer.Deserialize<List<ArmyBase>>(File.ReadAllText(@"ASSET\Json\ArmyParas.json"));
                SpellBase.AddAllSpell(M_a.armyDataOutF);
            }
            catch (Exception e)
            {
                ErroR.Report("兵种数据加载失败：" + e.Message);
            }
            for(int i = 0;i < M_a.armyDataOutF.Count; i++)
            {
                if (M_a.armyDataOutF[i] is SpellBase) break;
                M_a.armyDataOutF[i].tcolor = S2C.Do(M_a.armyDataOutF[i].color);
            }
            M_a.armyDataOutF[0].num += 25;
            M_a.armyDataOutF[1].num += 25;
            //天赋加载
            M_Abilities.Init();
            M_Abilities.InitEX(this);
            //投射物组
            casts = new List<CastBase>();
            //地图生成
            CPM.Init();
            #endregion

            #region 物品加载
            items = new List<(string name, int num)>();
            items.Add((THe.S_package_text[0], 0));
            items.Add((THe.S_package_text[1], 0));
            items.Add((THe.S_package_text[2], 0));
            items.Add((THe.S_package_text[3], 0));
            items.Add((THe.S_package_text[4], 0));
            #endregion

            #region 字体加载
            string[] someTexts = { "点击任意处开始游戏","放弃","下一步","背包","战斗","返回:：,","关闭","获得的奖励：",
                1234567890.ToString(),"QqWwEeRrTtYyUuIiOoPpAaSsDdFfGgHhJjKkLlZzXxCcVvBbNnMm","摧毁率%",
                };
            string combinedText = string.Concat(someTexts);
            THe.AddAlltext(ref combinedText);
            foreach (var army in M_a.armyDataOutF)
            {
                combinedText += army.name;
                combinedText += army.description;
            }
            M_c.InitAllCon();
            foreach(var con in M_c.conList)
            {
                combinedText += con.name;
                combinedText += con.description;
            }
            foreach (var cps in CPM.checkPoints)
            {
                foreach (var cp in cps)
                    combinedText += cp.name;
            }
            foreach (var ab in Abilities.abilities_C)
            {
                combinedText += ab.title;
                combinedText += ab.description;
            }
            foreach (var ab in Abilities.abilities_B)
            {
                combinedText += ab.title;
                combinedText += ab.description;
            }
            foreach (var ab in Abilities.abilities_A)
            {
                combinedText += ab.title;
                combinedText += ab.description;
            }
            foreach (var ab in Abilities.abilities_S)
            {
                combinedText += ab.title;
                combinedText += ab.description;
            }
            foreach (var ab in Abilities.abilities_EX)
            {
                foreach (var mode in ab.EXmode)
                {
                    combinedText += mode.title;
                    combinedText += mode.description;
                }
            }
            int[] glyphs = combinedText.Distinct().Select(c => (int)c).ToArray();
            ChineseFont = Raylib.LoadFontEx("STKAITI.TTF", 64, glyphs, glyphs.Length);
            Console.WriteLine(ChineseFont.GlyphCount);
            #endregion
        }
        public void Fresh()
        {
            dt = Raylib.GetFrameTime();
            mousePos = Raylib.GetMousePosition();
            worldPos = Raylib.GetScreenToWorld2D(mousePos, camera);
            M_Input.Fresh(this);
        }
    }
}
