namespace NoBadConflicts
{
    class AbiliClass
    {
        public string str;
        public string title;
        public string description;
        public int weight;
        public int maxnum;
        public int nownum;
        public AbiliClass() { }
        public AbiliClass(string str, string title, string description, int w, int max)
        {
            this.str = str;
            this.title = title;
            this.description = description;
            this.weight = w;
            this.maxnum = max;
            nownum = 0;
        }
    }
    struct EXabiliStruct
    {
        public ArmyBase army;
        public List<AbiliClass> EXmode;
        public EXabiliStruct(ArmyBase army)
        {
            this.army = army;
            EXmode = new List<AbiliClass>();
        }
    }
    class Abilities : ManagerBase<Abilities>
    {
        public static List<AbiliClass> abilities_C = new List<AbiliClass>();
        public static List<AbiliClass> abilities_B = new List<AbiliClass>();
        public static List<AbiliClass> abilities_A = new List<AbiliClass>();
        public static List<AbiliClass> abilities_S = new List<AbiliClass>();
        public static List<EXabiliStruct> abilities_EX = new List<EXabiliStruct>();
        #region 天赋增加数量
        static int[] A01num = { 25, 50, 100, 0 }; //野蛮人
        static int[] A02num = { 25, 50, 100, 0 }; //弓箭手
        static int[] A03num = { 4, 8, 16, 0 }; //巨人
        //rank c2 b2 a1 s0
        static int[] A04num = { 0, 3, 6, 12 }; //皮卡超人
        static int[] A05num = { 0, 1, 2, 4 }; //戈仑石人
        static int[] A06num = { 0, 12, 24, 0 }; //炸弹人
        static int[] A07num = { 0, 10, 20, 40 }; //法师
        //rank c0 b1 a1 s1

        static int[] S01num = { 1, 2, 3, 0 }; //狂暴药水
        static int[] S02num = { 0, 3, 6, 0 }; //急速药水
        static int[] S03num = { 1, 0, 4, 0 }; //治疗药水
        static int[] S04num = { 1, 0, 4, 0 }; //痊愈药水
        static int[] S05num = { 0, 2, 3, 4 }; //冰冻药水
        static int[] S06num = { 0, 2, 3, 4 }; //裂伤药水
        static int[] S07num = { 0, 2, 3, 4 }; //干扰药水
        static int[] S66num = { 16, 36, 66, 126 }; //鞭炮
        #endregion

        public void Init()
        {
            #region C
            abilities_C.Add(new AbiliClass("A01_num_c", "更多野蛮人", $"野蛮人数量增加{A01num[0]}", 10, 99));
            abilities_C.Add(new AbiliClass("A02_num_c", "更多弓箭手", $"弓箭手数量增加{A02num[0]}", 10, 99));
            abilities_C.Add(new AbiliClass("A03_num_c", "更多巨人", $"巨人数量增加{A03num[0]}", 10, 99));
            abilities_C.Add(new AbiliClass("S01_num_c", "更多狂暴药水", $"狂暴药水数量增加{S01num[0]}", 10, 1));
            abilities_C.Add(new AbiliClass("S03_num_c", "更多治疗药水", $"治疗药水数量增加{S03num[0]}", 10, 1));
            abilities_C.Add(new AbiliClass("S04_num_c", "更多痊愈药水", $"痊愈药水数量增加{S04num[0]}", 10, 1));
            abilities_C.Add(new AbiliClass("S66_num_c", "更多鞭炮", $"鞭炮数量增加{S66num[0]}", 10, 6));

            abilities_C.Add(new AbiliClass("A01_rank_c", "更强野蛮人", "野蛮人等级增加1", 10, 2));
            abilities_C.Add(new AbiliClass("A02_rank_c", "更强弓箭手", "弓箭手等级增加1", 10, 2));
            abilities_C.Add(new AbiliClass("A03_rank_c", "更强巨人", "巨人等级增加1", 10, 2));
            #endregion

            #region B
            abilities_B.Add(new AbiliClass("A01_num_b", "更多野蛮人", $"野蛮人数量增加{A01num[1]}", 10, 99));
            abilities_B.Add(new AbiliClass("A02_num_b", "更多弓箭手", $"弓箭手数量增加{A02num[1]}", 10, 99));
            abilities_B.Add(new AbiliClass("A03_num_b", "更多巨人", $"巨人数量增加{A03num[1]}", 10, 99));
            abilities_B.Add(new AbiliClass("A04_num_b", "更多皮卡超人", $"皮卡超人数量增加{A04num[1]}", 10, 99));
            abilities_B.Add(new AbiliClass("A05_num_b", "更多戈仑石人", $"戈仑石人数量增加{A05num[1]}", 10, 99));
            abilities_B.Add(new AbiliClass("A06_num_b", "更多炸弹人", $"炸弹人数量增加{A06num[1]}", 10, 99));
            abilities_B.Add(new AbiliClass("A07_num_b", "更多法师", $"法师数量增加{A07num[1]}", 10, 99));
            abilities_B.Add(new AbiliClass("S01_num_b", "更多狂暴药水", $"狂暴药水数量增加{S01num[1]}", 10, 1));
            abilities_B.Add(new AbiliClass("S02_num_b", "更多急速药水", $"急速药水数量增加{S02num[1]}", 10, 1));
            abilities_B.Add(new AbiliClass("S05_num_b", "更多冰冻药水", $"冰冻药水数量增加{S05num[1]}", 10, 1));
            abilities_B.Add(new AbiliClass("S06_num_b", "更多裂伤药水", $"裂伤药水数量增加{S06num[1]}", 10, 1));
            abilities_B.Add(new AbiliClass("S07_num_b", "更多干扰药水", $"干扰药水数量增加{S07num[1]}", 10, 1));
            abilities_B.Add(new AbiliClass("S66_num_b", "更多鞭炮", $"鞭炮数量增加{S66num[1]}", 10, 6));

            abilities_B.Add(new AbiliClass("A01_rank_b", "更强野蛮人", "野蛮人等级增加2", 10, 2));
            abilities_B.Add(new AbiliClass("A02_rank_b", "更强弓箭手", "弓箭手等级增加2", 10, 2));
            abilities_B.Add(new AbiliClass("A03_rank_b", "更强巨人", "巨人等级增加2", 10, 2));
            abilities_B.Add(new AbiliClass("A04_rank_b", "更强皮卡超人", "皮卡超人等级增加2", 10, 1));
            abilities_B.Add(new AbiliClass("A05_rank_b", "更强戈仑石人", "戈仑石人等级增加2", 10, 1));
            abilities_B.Add(new AbiliClass("A06_rank_b", "更强炸弹人", "炸弹人等级增加2", 10, 1));
            abilities_B.Add(new AbiliClass("A07_rank_b", "更强法师", "法师等级增加2", 10, 1));
            #endregion

            #region A
            abilities_A.Add(new AbiliClass("A01_num_a", "更多野蛮人", $"野蛮人数量增加{A01num[2]}", 10, 99));
            abilities_A.Add(new AbiliClass("A02_num_a", "更多弓箭手", $"弓箭手数量增加{A02num[2]}", 10, 99));
            abilities_A.Add(new AbiliClass("A03_num_a", "更多巨人", $"巨人数量增加{A03num[2]}", 10, 99));
            abilities_A.Add(new AbiliClass("A04_num_a", "更多皮卡超人", $"皮卡超人数量增加{A04num[2]}", 10, 99));
            abilities_A.Add(new AbiliClass("A05_num_a", "更多戈仑石人", $"戈仑石人数量增加{A05num[2]}", 10, 99));
            abilities_A.Add(new AbiliClass("A06_num_a", "更多炸弹人", $"炸弹人数量增加{A06num[2]}", 10, 99));
            abilities_A.Add(new AbiliClass("A07_num_a", "更多法师", $"法师数量增加{A07num[2]}", 10, 99));
            abilities_A.Add(new AbiliClass("S01_num_a", "更多狂暴药水", $"狂暴药水数量增加{S01num[2]}", 10, 2));
            abilities_A.Add(new AbiliClass("S02_num_a", "更多急速药水", $"急速药水数量增加{S02num[2]}", 10, 1));
            abilities_A.Add(new AbiliClass("S03_num_a", "更多治疗药水", $"治疗药水数量增加{S03num[2]}", 10, 2));
            abilities_A.Add(new AbiliClass("S04_num_a", "更多痊愈药水", $"痊愈药水数量增加{S04num[2]}", 10, 2));
            abilities_A.Add(new AbiliClass("S05_num_a", "更多冰冻药水", $"冰冻药水数量增加{S05num[2]}", 10, 1));
            abilities_A.Add(new AbiliClass("S06_num_a", "更多裂伤药水", $"裂伤药水数量增加{S06num[2]}", 10, 1));
            abilities_A.Add(new AbiliClass("S07_num_a", "更多干扰药水", $"干扰药水数量增加{S07num[2]}", 10, 1));
            abilities_A.Add(new AbiliClass("S66_num_a", "更多鞭炮", $"鞭炮数量增加{S66num[2]}", 10, 6));

            abilities_A.Add(new AbiliClass("A01_rank_a", "更强野蛮人", "野蛮人等级增加3", 10, 1));
            abilities_A.Add(new AbiliClass("A02_rank_a", "更强弓箭手", "弓箭手等级增加3", 10, 1));
            abilities_A.Add(new AbiliClass("A03_rank_a", "更强巨人", "巨人等级增加3", 10, 1));
            abilities_A.Add(new AbiliClass("A04_rank_a", "更强皮卡超人", "皮卡超人等级增加3", 10, 1));
            abilities_A.Add(new AbiliClass("A05_rank_a", "更强戈仑石人", "戈仑石人等级增加3", 10, 1));
            abilities_A.Add(new AbiliClass("A06_rank_a", "更强炸弹人", "炸弹人等级增加3", 10, 1));
            abilities_A.Add(new AbiliClass("A07_rank_a", "更强法师", "法师等级增加3", 10, 1));
            #endregion

            #region S
            abilities_S.Add(new AbiliClass("A04_num_s", "更多皮卡超人", $"皮卡超人数量增加{A04num[3]}", 10, 99));
            abilities_S.Add(new AbiliClass("A05_num_s", "更多戈仑石人", $"戈仑石人数量增加{A05num[3]}", 10, 99));
            abilities_S.Add(new AbiliClass("A07_num_s", "更多法师", $"法师数量增加{A07num[3]}", 10, 99));
            abilities_S.Add(new AbiliClass("S05_num_s", "更多冰冻药水", $"冰冻药水数量增加{S05num[3]}", 10, 1));
            abilities_S.Add(new AbiliClass("S06_num_s", "更多裂伤药水", $"裂伤药水数量增加{S06num[3]}", 10, 1));
            abilities_S.Add(new AbiliClass("S07_num_s", "更多干扰药水", $"干扰药水数量增加{S07num[3]}", 10, 1));
            abilities_S.Add(new AbiliClass("S66_num_s", "更多鞭炮", $"鞭炮数量增加{S66num[3]}", 10, 6));

            abilities_S.Add(new AbiliClass("A04_rank_s", "更强皮卡超人", "皮卡超人等级增加4", 10, 1));
            abilities_S.Add(new AbiliClass("A05_rank_s", "更强戈仑石人", "戈仑石人等级增加4", 10, 1));
            abilities_S.Add(new AbiliClass("A06_rank_s", "更强炸弹人", "炸弹人等级增加4", 10, 1));
            abilities_S.Add(new AbiliClass("A07_rank_s", "更强法师", "法师等级增加4", 10, 1));
            #endregion
        }
        public void InitEX(GameMG gm)
        {
            ArmyBase army;
            foreach (var a in gm.M_a.armyDataOutF)
            {
                if (a is SpellBase) break;
                EXabiliStruct exS = new EXabiliStruct(a);
                string mt0 = "";
                string mt1 = "";
                switch (a.id)
                {
                    case "A01":
                        mt0 = "  生命值小幅提高\n" + "  攻击力小幅提高\n" + "  初始获得较长时间的狂暴效果\n"
                            + "  生命值降至0时，一定时间内不会倒下（此状态下不会成为攻击目标，无法获得治疗）";
                        mt1 = "  生命值大幅提高\n" + "  攻击力小幅降低\n" + "  移动速度略微降低\n"
                            + "不攻击时受到伤害大幅降低";
                        exS.EXmode.Add(new AbiliClass("A01_EX_0", "狂暴野蛮人", mt0, 0, 1));
                        exS.EXmode.Add(new AbiliClass("A01_EX_1", "骑士", mt1, 0, 1));
                        abilities_EX.Add(exS);
                        break;
                    case "A02":
                        mt0 = "  射程提高\n" + "  释放后一定时间内不会成为攻击目标";
                        exS.EXmode.Add(new AbiliClass("A02_EX_0", "隐匿弓箭手", mt0, 0, 1));
                        //exS.EXmode.Add(new AbiliStruct("A02_EX_1", "bbb", mt1, 0, 1));
                        abilities_EX.Add(exS);
                        break;
                    case "A03":
                        mt0 = "  生命值小幅提高\n" + "  攻击力大幅降低，但攻击间隔缩短\n"
                            + "  连续对同一目标造成的伤害逐次提高";
                        mt1 = "  生命值大幅提高\n" + "  攻击不再造成伤害，而是治疗自身\n"
                            + "  治疗可使血量溢出上限（溢出上限的血量随时间比例流失）";
                        exS.EXmode.Add(new AbiliClass("A03_EX_0", "斗士巨人", mt0, 0, 1));
                        exS.EXmode.Add(new AbiliClass("A03_EX_1", "和平巨人", mt1, 0, 1));
                        abilities_EX.Add(exS);
                        break;
                    case "A04":
                        mt0 = "  生命值小幅增加\n" + "  攻击力小幅增加\n" + "  初始获得较长时间的狂暴效果\n"
                            + "  生命值降至0时，一定时间内不会倒下（此状态下不会成为攻击目标，无法获得治疗）";
                        mt1 = "  生命值小幅降低\n" + "  攻击力小幅降低\n" + "  攻击间隔略微缩短\n"
                            + "  变为远程攻击，移动速度提升";
                        exS.EXmode.Add(new AbiliClass("A04_EX_0", "狂暴皮卡", mt0, 0, 1));
                        exS.EXmode.Add(new AbiliClass("A04_EX_1", "剑气皮卡", mt1, 0, 1));
                        abilities_EX.Add(exS);
                        break;
                    case "A05":
                        mt0 = "  生命值小幅降低\n" + "  攻击力极大幅降低\n" + "  攻击间隔大幅缩短\n" + "  移动速度提高，体型减小\n"
                            + "  储存受到的伤害，并在攻击时等比造成额外伤害\n" + "  受到治疗量提高";
                        mt1 = "  生命值大幅提高\n" + "  攻击力小幅提高\n" + "  移动速度降低，体型增大\n"
                            + "  随时间恢复生命值\n" + "  获得对寒冷、冰冻效果的抗性";
                        exS.EXmode.Add(new AbiliClass("A05_EX_0", "复仇戈仑", mt0, 0, 1));
                        exS.EXmode.Add(new AbiliClass("A05_EX_1", "高山戈仑", mt1, 0, 1));
                        abilities_EX.Add(exS);
                        break;
                    case "A06":
                        mt0 = "  生命值中幅提高\n" + "  体型增大\n" + "  首次攻击后攻击间隔大幅提高，攻击力大幅降低，移动速度降低\n"
                            + "  攻击不再为自爆式，改为损失一定比例生命值\n" + "  无法获得治疗";
                        mt1 = "  攻击力小幅提高\n" + "  阵亡时，在原地留下弱化版的治疗/痊愈药水光环\n";
                        exS.EXmode.Add(new AbiliClass("A06_EX_0", "连环炸弹人", mt0, 0, 1));
                        exS.EXmode.Add(new AbiliClass("A06_EX_1", "药罐炸弹人", mt1, 0, 1));
                        abilities_EX.Add(exS);
                        break;
                    case "A07":
                        mt0 = "  生命值小幅增加\n" + "  攻击力略微降低\n"
                            + "  攻击造成寒冷效果，倒下时对一定范围内建筑施加短暂冻结";
                        mt1 = "  攻击力小幅增加\n" + "  持续损失生命值（半血以下时失效）\n"
                            + "  依据损失生命值强化攻击力、攻击速度、移动速度";
                        exS.EXmode.Add(new AbiliClass("A07_EX_0", "寒冰法师", mt0, 0, 1));
                        exS.EXmode.Add(new AbiliClass("A07_EX_1", "怒炎法师", mt1, 0, 1));
                        abilities_EX.Add(exS);
                        break;
                    default:
                        ErroR.Report($"不存在觉醒的单位{a.id}");
                        break;
                }
            }
        }
        public static AbiliClass GetAb(List<AbiliClass> abilities)
        {
            if (abilities == null || abilities.Count == 0)
            {
                ErroR.Report("传入了空的天赋列表");
                return new AbiliClass();
            }
            int maxRnum = 0;
            foreach (AbiliClass ability in abilities)
                maxRnum += ability.weight;
            Random rn = new Random();
            int num = rn.Next(0, maxRnum);
            for (int i = 0; i < abilities.Count; i++)
            {
                if (abilities[i].weight > num)
                    return abilities[i];
                num -= abilities[i].weight;
            }
            ErroR.Report("未正确获取天赋");
            return new AbiliClass();
        }
        public static EXabiliStruct GetAbEX(ArmyBase army)
        {
            foreach (var ex in abilities_EX)
            {
                if (ex.army == army)
                {
                    if (ex.EXmode.Count > 0)
                        return ex;
                    else
                        break;
                }
            }
            ErroR.Report($"{army.id}-{army.name}不存在觉醒");
            return new EXabiliStruct();
        }
        public static void AbilityValidate(GameMG gm, string str)
        {
            string[] strs = str.Split("_");
            if (strs.Length != 3)
            {
                ErroR.Report($"错误的天赋格式:{str}");
                return;
            }
            ArmyBase army = null;
            foreach (var a in gm.M_a.armyDataOutF)
            {
                if (a.id == strs[0])
                {
                    army = a;
                    break;
                }
            }
            if (army == null)
            {
                ErroR.Report($"没有对应天赋实现的对象:{str}");
                return;
            }
            bool aboutNum = false;
            bool aboutRank = false;
            if (strs[1] == "num") aboutNum = true;
            else if (strs[1] == "rank") aboutRank = true;
            if (aboutNum)
            {
                switch (strs[0])
                {
                    case "A01":
                        switch (strs[2])
                        {
                            case "c":
                                army.num += A01num[0];
                                break;
                            case "b":
                                army.num += A01num[1];
                                break;
                            case "a":
                                army.num += A01num[2];
                                break;
                            case "s":
                                army.num += A01num[3];
                                break;
                        }
                        break;
                    case "A02":
                        switch (strs[2])
                        {
                            case "c":
                                army.num += A02num[0];
                                break;
                            case "b":
                                army.num += A02num[1];
                                break;
                            case "a":
                                army.num += A02num[2];
                                break;
                            case "s":
                                army.num += A02num[3];
                                break;
                        }
                        break;
                    case "A03":
                        switch (strs[2])
                        {
                            case "c":
                                army.num += A03num[0];
                                break;
                            case "b":
                                army.num += A03num[1];
                                break;
                            case "a":
                                army.num += A03num[2];
                                break;
                            case "s":
                                army.num += A03num[3];
                                break;
                        }
                        break;
                    case "A04":
                        switch (strs[2])
                        {
                            case "c":
                                army.num += A04num[0];
                                break;
                            case "b":
                                army.num += A04num[1];
                                break;
                            case "a":
                                army.num += A04num[2];
                                break;
                            case "s":
                                army.num += A04num[3];
                                break;
                        }
                        break;
                    case "A05":
                        switch (strs[2])
                        {
                            case "c":
                                army.num += A05num[0];
                                break;
                            case "b":
                                army.num += A05num[1];
                                break;
                            case "a":
                                army.num += A05num[2];
                                break;
                            case "s":
                                army.num += A05num[3];
                                break;
                        }
                        break;
                    case "A06":
                        switch (strs[2])
                        {
                            case "c":
                                army.num += A06num[0];
                                break;
                            case "b":
                                army.num += A06num[1];
                                break;
                            case "a":
                                army.num += A06num[2];
                                break;
                            case "s":
                                army.num += A06num[3];
                                break;
                        }
                        break;
                    case "A07":
                        switch (strs[2])
                        {
                            case "c":
                                army.num += A07num[0];
                                break;
                            case "b":
                                army.num += A07num[1];
                                break;
                            case "a":
                                army.num += A07num[2];
                                break;
                            case "s":
                                army.num += A07num[3];
                                break;
                        }
                        break;
                    case "S01":
                        switch (strs[2])
                        {
                            case "c":
                                army.num += S01num[0];
                                break;
                            case "b":
                                army.num += S01num[1];
                                break;
                            case "a":
                                army.num += S01num[2];
                                break;
                            case "s":
                                army.num += S01num[3];
                                break;
                        }
                        break;
                    case "S02":
                        switch (strs[2])
                        {
                            case "c":
                                army.num += S02num[0];
                                break;
                            case "b":
                                army.num += S02num[1];
                                break;
                            case "a":
                                army.num += S02num[2];
                                break;
                            case "s":
                                army.num += S02num[3];
                                break;
                        }
                        break;
                    case "S03":
                        switch (strs[2])
                        {
                            case "c":
                                army.num += S03num[0];
                                break;
                            case "b":
                                army.num += S03num[1];
                                break;
                            case "a":
                                army.num += S03num[2];
                                break;
                            case "s":
                                army.num += S03num[3];
                                break;
                        }
                        break;
                    case "S04":
                        switch (strs[2])
                        {
                            case "c":
                                army.num += S04num[0];
                                break;
                            case "b":
                                army.num += S04num[1];
                                break;
                            case "a":
                                army.num += S04num[2];
                                break;
                            case "s":
                                army.num += S04num[3];
                                break;
                        }
                        break;
                    case "S05":
                        switch (strs[2])
                        {
                            case "c":
                                army.num += S05num[0];
                                break;
                            case "b":
                                army.num += S05num[1];
                                break;
                            case "a":
                                army.num += S05num[2];
                                break;
                            case "s":
                                army.num += S05num[3];
                                break;
                        }
                        break;
                    case "S06":
                        switch (strs[2])
                        {
                            case "c":
                                army.num += S06num[0];
                                break;
                            case "b":
                                army.num += S06num[1];
                                break;
                            case "a":
                                army.num += S06num[2];
                                break;
                            case "s":
                                army.num += S06num[3];
                                break;
                        }
                        break;
                    case "S07":
                        switch (strs[2])
                        {
                            case "c":
                                army.num += S07num[0];
                                break;
                            case "b":
                                army.num += S07num[1];
                                break;
                            case "a":
                                army.num += S07num[2];
                                break;
                            case "s":
                                army.num += S07num[3];
                                break;
                        }
                        break;
                    case "S66":
                        switch (strs[2])
                        {
                            case "c":
                                army.num += S66num[0];
                                break;
                            case "b":
                                army.num += S66num[1];
                                break;
                            case "a":
                                army.num += S66num[2];
                                break;
                            case "s":
                                army.num += S66num[3];
                                break;
                        }
                        break;
                }
            }
            else if (aboutRank)
            {
                switch (strs[0])
                {
                    case "A01":
                    case "A02":
                    case "A03":
                        switch (strs[2])
                        {
                            case "c":
                                army.rank += 1;
                                break;
                            case "b":
                                army.rank += 2;
                                break;
                            case "a":
                                army.rank += 3;
                                break;
                        }
                        break;
                    case "A04":
                    case "A05":
                    case "A06":
                    case "A07":
                        switch (strs[2])
                        {
                            case "b":
                                army.rank += 2;
                                break;
                            case "a":
                                army.rank += 3;
                                break;
                            case "s":
                                army.rank += 4;
                                break;
                        }
                        break;
                }
            }
        }
        public static void AbRemove(AbiliClass ab)
        {
            string s = ab.str;
            foreach (var ability in abilities_C)
            {
                if (ability.str == s)
                {
                    abilities_C.Remove(ability);
                    return;
                }
            }
            foreach (var ability in abilities_B)
            {
                if (ability.str == s)
                {
                    abilities_B.Remove(ability);
                    return;
                }
            }
            foreach (var ability in abilities_A)
            {
                if (ability.str == s)
                {
                    abilities_A.Remove(ability);
                    return;
                }
            }
            foreach (var ability in abilities_S)
            {
                if (ability.str == s)
                {
                    abilities_S.Remove(ability);
                    return;
                }
            }
        }
    }
}
