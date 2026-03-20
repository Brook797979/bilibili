namespace NoBadConflicts
{
    class CPercent
    {
        public int connum_total;
        public int connum_conquer;
        public bool maindone;
        public bool halfdone;
        public bool alldone;
        public int starN;
        public CPercent()
        {
            connum_total = 0;
            connum_conquer = 0;
            maindone = false;
            halfdone = false;
            alldone = false;
            starN = 0;
        }
        public string GetPercent()
        {
            return (connum_conquer * 100 / connum_total).ToString() + "%";
        }
    }
    class ConstructionMG : ManagerBase<ConstructionMG>
    {
        public List<ConBase> conList;
        public CPercent percent;
        public void InitAllCon()
        {
            Con_Load cl = new Con_Load();
            conList = new List<ConBase>();
            conList.Add(new C_DBY(cl));
            conList.Add(new C_FSGT(cl));

            conList.Add(new C_GJT(cl));
            conList.Add(new C_SSJT(cl));
            conList.Add(new C_DCJT(cl));
            conList.Add(new C_RRJT(cl));
            conList.Add(new C_JNP(cl));
            conList.Add(new C_ZXPT(cl));
            conList.Add(new C_FST(cl));
            conList.Add(new C_HBFST(cl));
            conList.Add(new C_XFFST(cl));
            conList.Add(new C_BFFST(cl));
            conList.Add(new C_SZT(cl));
            conList.Add(new C_XTSZT(cl));

            conList.Add(new C_PZJD(cl));

            conList.Add(new T_ZD(cl));
            conList.Add(new T_QLZD(cl));
            conList.Add(new T_BDXJ(cl));
        }
        public bool IsEmpty()
        {
            foreach(var con in conList)
            {
                if (!con.hasDeadOnce && con.countIn)
                    return false;
            }
            return true;
        }
        public void Load(List<Con_Load> Js)
        {
            percent = new CPercent();
            foreach (Con_Load js in Js)
            {
                switch (js.name)
                {
                    case "大本营":
                        conList.Add(new C_DBY(js));
                        break;
                    case "复苏高塔":
                        conList.Add(new C_FSGT(js));
                        break;
                    //建筑
                    case "弓箭塔":
                        conList.Add(new C_GJT(js));
                        break;
                    case "速射箭塔":
                        conList.Add(new C_SSJT(js));
                        break;
                    case "多重箭塔":
                        conList.Add(new C_DCJT(js));
                        break;
                    case "熔融箭塔":
                        conList.Add(new C_RRJT(js));
                        break;
                    case "加农炮":
                        conList.Add(new C_JNP(js));
                        break;
                    case "重型炮台":
                        conList.Add(new C_ZXPT(js));
                        break;
                    case "法师塔":
                        conList.Add(new C_FST(js));
                        break;
                    case "寒冰法师塔":
                        conList.Add(new C_HBFST(js));
                        break;
                    case "旋风法师塔":
                        conList.Add(new C_XFFST(js));
                        break;
                    case "冰封法师塔":
                        conList.Add(new C_BFFST(js));
                        break;
                    case "射爪塔":
                        conList.Add(new C_SZT(js));
                        break;
                    case "玄铁射爪塔":
                        conList.Add(new C_XTSZT(js));
                        break;
                    //屏障
                    case "屏障节点":
                        conList.Add(new C_PZJD(js));
                        break;
                    //陷阱
                    case "炸弹":
                        conList.Add(new T_ZD(js));
                        break;
                    case "强力炸弹":
                        conList.Add(new T_QLZD(js));
                        break;
                    case "冰冻陷阱":
                        conList.Add(new T_BDXJ(js));
                        break;
                    default:
                        ErroR.Report($"未录入建筑名：{js.name}");
                        break;
                }
            }
            foreach( var con in conList)
            {
                if (con.countIn) 
                    percent.connum_total += 1;
            }
        }
        public void ConAction(GameMG gm)
        {
            if(conList == null) return;
            foreach (var con in conList)
            {
                if(con.lostHealth < con.maxHealth)
                    con.Behaviors(gm);
                else
                {
                    if (!con.hasDeadOnce)
                    {
                        con.hasDeadOnce = true;
                        con.WhenDie(gm);

                        if(con.countIn)
                            percent.connum_conquer += 1;
                        if(con.isMain && !percent.maindone)
                        {
                            bool allMainDone = true;
                            foreach (var c in conList)
                            {
                                if(c.isMain && !c.hasDeadOnce)
                                    allMainDone = false;
                            }
                            if (allMainDone)
                            {
                                percent.maindone = true;
                                percent.starN += 1;
                            }
                        }
                        if(percent.connum_conquer * 2 >= percent.connum_total && !percent.halfdone)
                        {
                            percent.halfdone = true;
                            percent.starN += 1;
                        }
                        if(percent.connum_conquer >= percent.connum_total && !percent.alldone)
                        {
                            percent.alldone = true;
                            percent.starN += 1;
                        }
                    }
                }
            }
        }
    }
}
