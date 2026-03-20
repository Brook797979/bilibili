namespace NoBadConflicts 
{
    class C_DBY : ConBase
    {
        public C_DBY(Con_Load con)
        {
            isMain = true;
            name = "大本营";
            description = "大本营";
            showInterval = false;
            position = new Vector2(con.x, con.y);
            maxHealth = 10000f;
            size = 50;
            mPosition = position - new Vector2(size * 0.8f, size * 0.8f);
            this.rank = con.rank;
            for (int i = 0; i < rank; i++)
            {
                maxHealth *= 1.2f;
            }
        }
    }
    class C_FSGT : ConBase
    {
        public C_FSGT(Con_Load con)
        {
            isMain = true;
            name = "复苏高塔";
            description = "大本营\n自愈\n每隔一段时间，为其它建筑恢复一定比例生命值\n被摧毁时，重建被摧毁的建筑";
            position = new Vector2(con.x, con.y);
            maxHealth = 30000f;
            attackInterval = 12f;
            size = 55;
            mPosition = position - new Vector2(size * 0.8f, size * 0.8f);
            this.rank = con.rank;
            for (int i = 0; i < rank; i++)
            {
                maxHealth *= 1.2f;
            }
        }
        public override void Behaviors(GameMG gm)
        {
            float dt = gm.dt * RateCalculate(gm.dt);
            if (lostHealth >= maxHealth) return;
            float heal = gm.dt * maxHealth * 0.05f;
            if (lostHealth > heal)
                lostHealth -= heal;
            else lostHealth = 0;
            intervalCounter += dt;
            if (intervalCounter > attackInterval)
            {
                Attack(gm);
                intervalCounter -= attackInterval;
            }
        }
        public override void Attack(GameMG gm)
        {
            foreach (var con in gm.M_c.conList)
            {
                if (con == this || con.hasDeadOnce || con is TrapBase) continue;
                if (con.lostHealth <= con.maxHealth * 0.25f)
                    con.lostHealth = 0;
                else
                    con.lostHealth -= con.maxHealth * 0.25f;
            }
        }
        public override void WhenDie(GameMG gm)
        {
            this.Attack(gm);
            foreach (var con in gm.M_c.conList)
            {
                if (con == this || con is TrapBase || con.isMain) continue;
                if (con.hasDeadOnce && con.lostHealth >= con.maxHealth)
                    con.lostHealth = con.maxHealth * 0.5f;
            }
        }
    }

    class C_GJT : ConBase
    {
        public C_GJT(Con_Load con)
        {
            name = "弓箭塔";
            description = "Ⅰ类建筑\n基础防御建筑";
            position = new Vector2(con.x, con.y);
            maxHealth = 800f;
            attackPower = 40f;
            attackRangeR = 320f;
            attackInterval = 0.8f;
            targetNum = 1;
            size = 30;
            mPosition = position - new Vector2(size * 0.8f, size * 0.8f);
            this.rank = con.rank;
            for (int i = 0; i < rank; i++)
            {
                maxHealth *= 1.2f;
                attackPower *= 1.2f;
            }
        }
        public override void Attack(GameMG gm)
        {
            Cast_Arrow cast = new Cast_Arrow(this, targets[0], 9f, 2f, 200f, attackPower, Color.Purple);
            gm.casts.Add(cast);
        }
    }
    class C_SSJT : ConBase
    {
        public C_SSJT(Con_Load con)
        {
            name = "速射箭塔";
            description = "Ⅱ类建筑\n高速射击";
            position = new Vector2(con.x, con.y);
            maxHealth = 2400f;
            attackPower = 30f;
            attackRangeR = 320f;
            attackInterval = 0.15f;
            targetNum = 1;
            size = 30;
            mPosition = position - new Vector2(size * 0.8f, size * 0.8f);
            this.rank = con.rank;
            for (int i = 0; i < rank; i++)
            {
                maxHealth *= 1.2f;
                attackPower *= 1.2f;
            }
        }
        public override void Attack(GameMG gm)
        {
            Cast_Arrow cast = new Cast_Arrow(this, targets[0], 9f, 2f, 200f, attackPower, Color.Purple);
            gm.casts.Add(cast);
        }
    }
    class C_DCJT : ConBase
    {
        public C_DCJT(Con_Load con)
        {
            name = "多重箭塔";
            description = "Ⅱ类建筑\n同时锁定多个目标";
            position = new Vector2(con.x, con.y);
            maxHealth = 2400f;
            attackPower = 40f;
            attackRangeR = 320f;
            attackInterval = 0.8f;
            targetNum = 5;
            size = 30;
            mPosition = position - new Vector2(size * 0.8f, size * 0.8f);
            this.rank = con.rank;
            for (int i = 0; i < rank; i++)
            {
                maxHealth *= 1.2f;
                attackPower *= 1.2f;
            }
        }
        public override void Attack(GameMG gm)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                Cast_Arrow cast = new Cast_Arrow(this, targets[i], 9f, 2f, 200f, attackPower, Color.Purple);
                gm.casts.Add(cast);
            }
        }
    }
    class C_RRJT : ConBase
    {
        int atC;
        float atI;
        public C_RRJT(Con_Load con)
        {
            name = "熔融箭塔";
            description = "Ⅲ类建筑\n优先锁定剩余生命值最高的部队\n初始攻击速度较慢，持续对同一目标攻击时攻击速度提升";
            position = new Vector2(con.x, con.y);
            maxHealth = 6000f;
            attackPower = 150f;
            attackRangeR = 320f;
            attackInterval = 2f;
            atI = attackInterval;
            targetNum = 1;
            size = 35;
            mPosition = position - new Vector2(size * 0.8f, size * 0.8f);
            this.rank = con.rank;
            for (int i = 0; i < rank; i++)
            {
                maxHealth *= 1.2f;
                attackPower *= 1.2f;
            }
        }
        public override void Attack(GameMG gm)
        {
            atC++;
            Cast_Arrow cast = new Cast_Arrow(this, targets[0], 9f, 3f, 240f, attackPower, Color.Red);
            gm.casts.Add(cast);
            attackInterval = atC <= 3 ? atI - atC * 0.6f : 0.1f;
        }
        public override void FindTarget(GameMG gm)
        {
            atC = 0;
            List<(ArmyBase a, float dis, float health)> disL = new List<(ArmyBase, float, float)>();
            foreach (var army in gm.M_a.armyList)
            {
                if (army == null || !army.canBeTrack || army.ShouldDie()) continue;
                disL.Add((army, Vector2.Distance(this.position, army.position), army.maxHealth - army.lostHealth));
            }
            disL.Sort((a, b) => a.dis.CompareTo(b.dis));
            if (disL.Count <= 0 || disL[0].dis > attackRangeR) return;
            List<(ArmyBase a, float dis, float health)> disL2 = new List<(ArmyBase, float, float)>();
            foreach (var item in disL)
            {
                if (item.dis > attackRangeR) break;
                disL2.Add(item);
            }
            disL2.Sort((a, b) => b.health.CompareTo(a.health));
            targets.Add(disL2[0].a);
        }
    }
    class C_JNP : ConBase
    {
        public C_JNP(Con_Load con)
        {
            name = "加农炮";
            description = "Ⅰ类建筑\n基础防御建筑";
            position = new Vector2(con.x, con.y);
            maxHealth = 1100f;
            attackPower = 80f;
            attackRangeR = 240f;
            attackInterval = 1.5f;
            targetNum = 1;
            size = 30;
            mPosition = position - new Vector2(size * 0.8f, size * 0.8f);
            this.rank = con.rank;
            for (int i = 0; i < rank; i++)
            {
                maxHealth *= 1.2f;
                attackPower *= 1.2f;
            }
        }
        public override void Attack(GameMG gm)
        {
            Cast_Bullet cast = new Cast_Bullet(this, targets[0], 8f, 160f, attackPower, Color.Black);
            gm.casts.Add(cast);
        }
    }
    class C_ZXPT : ConBase
    {
        public C_ZXPT(Con_Load con)
        {
            name = "重型炮台";
            description = "Ⅲ类建筑\n优先锁定剩余生命值最高的部队\n高额伤害附加百分比伤害\n"
                + "攻击时击退并伤害附近部队\n被摧毁时造成数个小范围爆炸";
            position = new Vector2(con.x, con.y);
            maxHealth = 9000f;
            attackPower = 800f;
            attackRangeR = 250f;
            attackInterval = 3f;
            targetNum = 1;
            size = 40;
            mPosition = position - new Vector2(size * 0.8f, size * 0.8f);
            this.rank = con.rank;
            for (int i = 0; i < rank; i++)
            {
                maxHealth *= 1.2f;
                attackPower *= 1.2f;
            }
        }
        public override void Attack(GameMG gm)
        {
            Cast_Bullet cast1 = new Cast_Bullet(this, targets[0], 12f, 160f, attackPower + targets[0].maxHealth * 0.2f, Color.Black);
            gm.casts.Add(cast1);
            Cast_GroundWave cast2 = new Cast_GroundWave(this.position, 55f, attackPower / 25f, 32f, new Color(130, 130, 130, 155));
            gm.casts.Add(cast2);
        }
        public override void FindTarget(GameMG gm)
        {
            List<(ArmyBase a, float dis, float health)> disL = new List<(ArmyBase, float, float)>();
            foreach (var army in gm.M_a.armyList)
            {
                if (army == null || !army.canBeTrack || army.ShouldDie()) continue;
                disL.Add((army, Vector2.Distance(this.position, army.position), army.maxHealth - army.lostHealth));
            }
            disL.Sort((a, b) => a.dis.CompareTo(b.dis));
            if (disL.Count <= 0 || disL[0].dis > attackRangeR) return;
            List<(ArmyBase a, float dis, float health)> disL2 = new List<(ArmyBase, float, float)>();
            foreach (var item in disL)
            {
                if (item.dis > attackRangeR) break;
                disL2.Add(item);
            }
            disL2.Sort((a, b) => b.health.CompareTo(a.health));
            targets.Add(disL2[0].a);
        }
        public override void WhenDie(GameMG gm)
        {
            Random r = new Random();
            Color c = new Color(230, 41, 55, 55);
            int boomnum = 9;
            for (int i = 0; i < boomnum; i++)
            {
                float radians = (float)(r.NextDouble() - 0.5) * MathF.PI * 2f;
                int dis = r.Next(30, 71);
                Vector2 v = position + new Vector2(dis * MathF.Cos(radians), dis * MathF.Sin(radians));
                Cast_GroundWave cast = new Cast_GroundWave(v, 30f, attackPower * 0.1f, 5f, c);
                gm.casts.Add(cast);
            }
        }
    }
    class C_FST : ConBase
    {
        public C_FST(Con_Load con)
        {
            name = "法师塔";
            description = "Ⅰ类建筑\n造成范围伤害";
            position = new Vector2(con.x, con.y);
            maxHealth = 800f;
            attackPower = 40f;
            attackRangeR = 260f;
            attackInterval = 1.2f;
            targetNum = 1;
            size = 30;
            mPosition = position - new Vector2(size * 0.8f, size * 0.8f);
            this.rank = con.rank;
            for (int i = 0; i < rank; i++)
            {
                maxHealth *= 1.2f;
                attackPower *= 1.2f;
            }
        }
        public override void Attack(GameMG gm)
        {
            Cast_FireBall cast = new Cast_FireBall(this, targets[0], 10f, 120f, attackPower, Color.Red, 16f);
            gm.casts.Add(cast);
        }
    }
    class C_HBFST : ConBase
    {
        public C_HBFST(Con_Load con)
        {
            name = "寒冰法师塔";
            description = "Ⅱ类建筑\n造成范围伤害并施加寒冷效果";
            position = new Vector2(con.x, con.y);
            maxHealth = 2800f;
            attackPower = 50f;
            attackRangeR = 260f;
            attackInterval = 1.2f;
            targetNum = 1;
            size = 30;
            mPosition = position - new Vector2(size * 0.8f, size * 0.8f);
            this.rank = con.rank;
            for (int i = 0; i < rank; i++)
            {
                maxHealth *= 1.2f;
                attackPower *= 1.2f;
            }
        }
        public override void Attack(GameMG gm)
        {
            Cast_ColdBall cast = new Cast_ColdBall(this, targets[0], 10f, 120f, attackPower, Color.Blue, 21f, 1.8f);
            gm.casts.Add(cast);
        }
    }
    class C_XFFST : ConBase
    {
        public C_XFFST(Con_Load con)
        {
            name = "旋风法师塔";
            description = "Ⅲ类建筑\n造成范围伤害和大范围吸附";
            position = new Vector2(con.x, con.y);
            maxHealth = 5500f;
            attackPower = 60f;
            attackRangeR = 260f;
            attackInterval = 1.5f;
            targetNum = 1;
            size = 35;
            mPosition = position - new Vector2(size * 0.8f, size * 0.8f);
            this.rank = con.rank;
            for (int i = 0; i < rank; i++)
            {
                maxHealth *= 1.2f;
                attackPower *= 1.2f;
            }
        }
        public override void Attack(GameMG gm)
        {
            Cast_WindBall cast = new Cast_WindBall(this, targets[0], 10f, 200f, attackPower, new Color(210, 210, 210, 255), 16f);
            gm.casts.Add(cast);
        }
    }
    class C_BFFST : ConBase
    {
        public C_BFFST(Con_Load con)
        {
            name = "冰封法师塔";
            description = "Ⅲ类建筑\n造成范围伤害并施加冰冻和寒冷效果";
            position = new Vector2(con.x, con.y);
            maxHealth = 6000f;
            attackPower = 70f;
            attackRangeR = 260f;
            attackInterval = 1.6f;
            targetNum = 1;
            size = 35;
            mPosition = position - new Vector2(size * 0.8f, size * 0.8f);
            this.rank = con.rank;
            for (int i = 0; i < rank; i++)
            {
                maxHealth *= 1.2f;
                attackPower *= 1.2f;
            }
        }
        public override void Attack(GameMG gm)
        {
            Cast_ColdBall cast = new Cast_ColdBall(this, targets[0], 10f, 120f, attackPower, Color.SkyBlue, 21f, 3f, true);
            gm.casts.Add(cast);
        }
    }
    class C_SZT : ConBase
    {
        public C_SZT(Con_Load con)
        {
            name = "射爪塔";
            description = "Ⅱ类建筑\n发射钩爪，使多个部队远离\n造成些微伤害和些微百分比伤害";
            position = new Vector2(con.x, con.y);
            maxHealth = 2000f;
            attackPower = 15f;
            attackRangeR = 280f;
            attackInterval = 2.5f;
            targetNum = 6;
            size = 25;
            mPosition = position - new Vector2(size * 0.8f, size * 0.8f);
            this.rank = con.rank;
            for (int i = 0; i < rank; i++)
            {
                maxHealth *= 1.2f;
                attackPower *= 1.2f;
            }
        }
        public override void Attack(GameMG gm)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                Vector2 dir = targets[i].position - position;
                float rad = MathF.Atan2(dir.Y, dir.X);
                Cast_Hook cast = new Cast_Hook(targets[i], rad, 100f, 0.5f);
                gm.casts.Add(cast);
                targets[i].BeAttack(attackPower + targets[i].maxHealth * 0.015f);
            }
        }
    }
    class C_XTSZT : ConBase
    {
        public C_XTSZT(Con_Load con)
        {
            name = "玄铁射爪塔";
            description = "Ⅲ类建筑\n攻击时恢复生命值\n发射钩爪，使多个部队远离\n造成少许伤害和一定百分比伤害";
            position = new Vector2(con.x, con.y);
            maxHealth = 9000f;
            attackPower = 50f;
            attackRangeR = 280f;
            attackInterval = 3f;
            targetNum = 12;
            size = 30;
            mPosition = position - new Vector2(size * 0.8f, size * 0.8f);
            this.rank = con.rank;
            for (int i = 0; i < rank; i++)
            {
                maxHealth *= 1.2f;
                attackPower *= 1.2f;
            }
        }
        public override void Attack(GameMG gm)
        {
            float heal = maxHealth * 0.15f;
            if (lostHealth > heal)
                lostHealth -= heal;
            else lostHealth = 0;
            for (int i = 0; i < targets.Count; i++)
            {
                Vector2 dir = targets[i].position - position;
                float rad = MathF.Atan2(dir.Y, dir.X);
                Cast_Hook cast = new Cast_Hook(targets[i], rad, 150f, 1.2f);
                gm.casts.Add(cast);
                targets[i].BeAttack(attackPower + targets[i].maxHealth * 0.08f);
            }
        }
    }

    class C_PZJD : ConBase
    {
        float radians;
        float length;
        public C_PZJD(Con_Load con)
        {
            name = "屏障节点";
            description = "吸引部队仇恨";
            countIn = false;
            position = new Vector2(con.x, con.y);
            maxHealth = 3000f;
            showInterval = false;
            size = 20;
            radians = 0.25f * con.n1 * MathF.PI;
            length = size + con.n2;
            mPosition = position - new Vector2(size * 0.8f, size * 0.8f);
            this.rank = con.rank;
            for (int i = 0; i < rank; i++)
            {
                maxHealth *= 1.2f;
            }
        }
        public override void DrawSelf(GameMG gm)
        {
            if (lostHealth < maxHealth)
            {
                Vector2 mp = new Vector2(length * MathF.Cos(radians), length * MathF.Sin(radians));
                Raylib.DrawLineEx(position, position + mp, 2.5f, Color.Black);
                Raylib.DrawLineEx(position, position - mp, 2.5f, Color.Black);
            }
            Color dcolor = color;
            if (lostHealth >= maxHealth)
            {
                dcolor.R /= 2;
                dcolor.G /= 2;
                dcolor.B /= 2;
            }
            Raylib.DrawRectangleV(mPosition, new Vector2(size * 1.6f, size * 1.6f), dcolor);

            if (nameD == null)
            {
                Color c = new Color(255 - color.R, 255 - color.G, 255 - color.B);
                nameD = new VCenterText(gm.ChineseFont, name, (int)position.X, (int)(position.Y - size * 0.1f), size * 0.4f, 0, c);
            }
            else
                nameD.Draw();
            Raylib.DrawTextEx(gm.ChineseFont, $"LV{rank}",
                new Vector2(position.X - size * 0.2f, position.Y + size * 0.1f), size * 0.4f, 0, nameD.color);
        }
        public override void Behaviors(GameMG gm)
        {
            if (lostHealth >= maxHealth) return;
            Vector2 mp = new Vector2(length * MathF.Cos(radians), length * MathF.Sin(radians));
            Vector2 p1 = position - mp;
            Vector2 p2 = position + mp;
            Vector2 p3 = mp;
            foreach (var army in gm.M_a.armyList)
            {
                if (army == null || army.target == null ||
                    !army.canBeTrack || army.ShouldDie() || army.target is C_PZJD) continue;
                if (Raylib.CheckCollisionLines(p1, p2, army.position, army.target.position, ref p3))
                {
                    Vector2 direction = army.target.position - army.position;
                    float ra = MathF.Atan2(direction.Y, direction.X);
                    float exdis;
                    if (Math.Abs(MathF.Cos(ra)) >= Math.Abs(MathF.Sin(ra)))
                        exdis = Math.Abs(army.target.size * 0.5f / MathF.Cos(ra));
                    else exdis = Math.Abs(army.target.size * 0.5f / MathF.Sin(ra));
                    if (Vector2.Distance(p3, army.target.position) - exdis > army.attackRange)
                        army.target = this;
                }
            }
        }
    }

    class TrapBase : ConBase
    {
        public override void DrawCondition() { }
        public override void DrawSelf(GameMG gm)
        {
#if DEBUG
            DrawS(gm);
            return;
#endif
            if (lostHealth >= maxHealth)
                DrawS(gm);
        }
        public void DrawS(GameMG gm)
        {
            Color dcolor = color;
            dcolor.A = 85;
            if (lostHealth >= maxHealth)
            {
                dcolor.R /= 2;
                dcolor.G /= 2;
                dcolor.B /= 2;
            }
            Raylib.DrawRectangleV(mPosition, new Vector2(size * 1.6f, size * 1.6f), dcolor);

            if (nameD == null)
            {
                Color c = new Color(255 - color.R, 255 - color.G, 255 - color.B);
                nameD = new VCenterText(gm.ChineseFont, name, (int)position.X, (int)(position.Y - size * 0.1f), size * 0.4f, 0, c);
            }
            else
                nameD.Draw();
            Raylib.DrawTextEx(gm.ChineseFont, $"LV{rank}",
                new Vector2(position.X - size * 0.2f, position.Y + size * 0.1f), size * 0.4f, 0, nameD.color);
        }
        public override void Behaviors(GameMG gm)
        {
            if (lostHealth >= maxHealth) return;
            foreach (var army in gm.M_a.armyList)
            {
                if (army == null || !army.canBeTrack || army.ShouldDie()) continue;
                if (Vector2.Distance(position, army.position) <= attackRangeR)
                    lostHealth += 5000f;
            }
        }
    }
    class T_ZD : TrapBase
    {
        public T_ZD(Con_Load con)
        {
            maxHealth = 500f;
            canBeTrack = false;
            countIn = false;
            name = "炸弹";
            description = "部队靠近时小范围爆炸";
            position = new Vector2(con.x, con.y);
            attackPower = 60f;
            attackRangeR = 25f;
            size = 15f;
            mPosition = position - new Vector2(size * 0.8f, size * 0.8f);
            this.rank = con.rank;
            for (int i = 0; i < rank; i++)
            {
                attackPower *= 1.2f;
            }
        }
        public override void WhenDie(GameMG gm)
        {
            Cast_GroundWave cast = new Cast_GroundWave(position, 85f, attackPower, 8f, new Color(230, 41, 55, 95));
            gm.casts.Add(cast);
        }
    }
    class T_QLZD : TrapBase
    {
        public T_QLZD(Con_Load con)
        {
            maxHealth = 500f;
            canBeTrack = false;
            countIn = false;
            name = "强力炸弹";
            description = "部队靠近时范围爆炸";
            position = new Vector2(con.x, con.y);
            attackPower = 110f;
            attackRangeR = 45f;
            size = 15f;
            mPosition = position - new Vector2(size * 0.8f, size * 0.8f);
            this.rank = con.rank;
            for (int i = 0; i < rank; i++)
            {
                attackPower *= 1.2f;
            }
        }
        public override void WhenDie(GameMG gm)
        {
            Cast_GroundWave cast = new Cast_GroundWave(position, 155f, attackPower, 20f, new Color(230, 41, 55, 135));
            gm.casts.Add(cast);
        }
    }
    class T_BDXJ : TrapBase
    {
        float buffT;
        public T_BDXJ(Con_Load con)
        {
            buffT = 1.5f;
            maxHealth = 500f;
            canBeTrack = false;
            countIn = false;
            name = "冰冻陷阱";
            description = "部队靠近时范围施加短时间冻结和寒冷";
            position = new Vector2(con.x, con.y);
            attackPower = 0f;
            attackRangeR = 45f;
            size = 15f;
            mPosition = position - new Vector2(size * 0.8f, size * 0.8f);
            this.rank = con.rank;
            for (int i = 0; i < rank; i++)
            {
                buffT *= 1.2f;
            }
        }
        public override void WhenDie(GameMG gm)
        {
            Cast_BuffCircle cast1 = new Cast_BuffCircle(position, 100f, 0f, Color.SkyBlue, buffT, "frozen");
            gm.casts.Add(cast1);
            Cast_BuffCircle cast2 = new Cast_BuffCircle(position, 100f, 0f, Color.SkyBlue, 3f * buffT, "cold");
            gm.casts.Add(cast2);
        }
    }
}
