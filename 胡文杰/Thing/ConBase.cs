namespace NoBadConflicts
{
    struct Con_Load
    {
        public string name { get; set; }
        public int rank { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public int n1 { get; set; }
        public int n2 { get; set; }
    }
    class ConBase
    {
        public string name;
        public VCenterText? nameD;
        public string description;
        public float maxHealth;
        public float attackPower;
        public float attackRangeR;
        public float attackRanger = 0;
        public float attackInterval;
        public float size;
        public int rank;

        public float lostHealth = 0;
        public float intervalCounter = 0;
        public Color color = Color.LightGray;
        public Vector2 position;
        public Vector2 mPosition;

        public List<ArmyBase> targets = new List<ArmyBase>();
        public int targetNum;
        public bool canBeTrack = true;
        public bool countIn = true;
        public bool isMain = false;
        public bool showInterval = true;
        public bool hasDeadOnce = false;

        public float frozenTime = 0f;
        public float coldTime = 0f;

        public ConBase() { }
        public ConBase(Con_Load con) { }
        public virtual void Behaviors(GameMG gm)
        {
            float dt = gm.dt * RateCalculate(gm.dt);
            if (lostHealth >= maxHealth) return;
            LostTarget(gm);
            if (ShouldFindTarget(gm))
            {
                FindTarget(gm);
            }
            if (targets.Count == 0) intervalCounter = 0;
            else
            {
                intervalCounter += dt;
                if(intervalCounter > attackInterval)
                {
                    intervalCounter -= attackInterval;
                    Attack(gm);
                }
            }
        }
        public bool MouseOn(GameMG gm)
        {
            float size = this.size * 0.4f;
            if (gm.worldPos.X > position.X - size && gm.worldPos.X < position.X + size &&
                gm.worldPos.Y > position.Y - size && gm.worldPos.Y < position.Y + size) return true;
            return false;
        }
        public virtual void DrawSelf(GameMG gm)
        {
            Color dcolor = color;
            if(lostHealth < maxHealth)
            {
                if (frozenTime > 0)
                {
                    dcolor.R = (byte)(dcolor.R * 0.4f + Color.SkyBlue.R * 0.6f);
                    dcolor.G = (byte)(dcolor.G * 0.4f + Color.SkyBlue.G * 0.6f);
                    dcolor.B = (byte)(dcolor.B * 0.4f + Color.SkyBlue.B * 0.6f);
                }
                else if (coldTime > 0)
                {
                    dcolor.R = (byte)(dcolor.R * 0.8f + Color.SkyBlue.R * 0.2f);
                    dcolor.G = (byte)(dcolor.G * 0.8f + Color.SkyBlue.G * 0.2f);
                    dcolor.B = (byte)(dcolor.B * 0.8f + Color.SkyBlue.B * 0.2f);
                }
            }
            else
            {
                dcolor.R /= 2;
                dcolor.G /= 2;
                dcolor.B /= 2;
            }
            Raylib.DrawRectangleV(mPosition, new Vector2(size * 1.6f, size * 1.6f), dcolor);

            if(nameD == null)
            {
                Color c = new Color(255 - color.R, 255 - color.G, 255 - color.B);
                nameD = new VCenterText(gm.ChineseFont, name, (int)position.X, (int)(position.Y - size * 0.1f), size * 0.4f, 0, c);
            }
            else
                nameD.Draw();
            Raylib.DrawTextEx(gm.ChineseFont, $"LV{rank}", 
                new Vector2(position.X - size * 0.2f, position.Y + size * 0.1f), size * 0.4f, 0, nameD.color);
        }
        public virtual void DrawCondition()
        {
            float w = size * 1.2f;
            float h = size * 0.25f;
            Rectangle rect = new Rectangle(position.X - w / 2, mPosition.Y - h / 2, w, h);
            Raylib.DrawRectangleRec(rect, Color.Gray);
            rect.Width *= (1f - lostHealth /  maxHealth);
            Raylib.DrawRectangleRec(rect, Color.Purple);
            rect.Width = w;
            Raylib.DrawRectangleLinesEx(rect, size * 0.05f, Color.DarkGray);

            if (!showInterval) return;
            rect = new Rectangle(mPosition.X + size * 1.5f, mPosition.Y, h / 2, w);
            Raylib.DrawRectangleRec(rect, Color.Red);
            rect.Height *= (1f - intervalCounter / attackInterval);
            Raylib.DrawRectangleRec(rect, Color.Gray);
            rect.Height = w;
            Raylib.DrawRectangleLinesEx(rect, size * 0.03f, Color.DarkGray);
        }
        public virtual void LostTarget(GameMG gm)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                ArmyBase target = targets[i];
                if (target == null || !gm.M_a.armyList.Contains(target) || !target.canBeTrack || target.ShouldDie() ||
                    Vector2.Distance(position, targets[i].position) - targets[i].size > attackRangeR ||
                    Vector2.Distance(position, targets[i].position) + targets[i].size < attackRanger)
                {
                    targets.Remove(targets[i]);
                    i--;
                }
            }
        }
        public virtual bool ShouldFindTarget(GameMG gm)
        {
            if (targets.Count < targetNum) return true;
            return false;
        }
        public virtual void FindTarget(GameMG gm)
        {
            List<(ArmyBase a, float dis)> disL = new List<(ArmyBase, float)>();
            foreach(var army in gm.M_a.armyList)
            {
                if (army == null || !army.canBeTrack || army.ShouldDie()) continue;
                disL.Add((army, Vector2.Distance(this.position, army.position)));
            }
            disL.Sort((a, b) => a.dis.CompareTo(b.dis));
            for(int i = 0;i < disL.Count; i++)
            {
                if (disL[i].dis + disL[i].a.size < attackRanger) continue;
                if (disL[i].dis - disL[i].a.size > attackRangeR) break;
                if (!targets.Contains(disL[i].a)) targets.Add(disL[i].a);
                if (targets.Count >= targetNum) break;
            }
        }
        public float RateCalculate(float dt)
        {
            float baseRate = 1f;
            if (frozenTime > 0)
            {
                frozenTime -= dt;
                baseRate = 0;
            }
            if (coldTime > 0)
            {
                coldTime -= dt;
                baseRate *= 0.67f;
            }
            return baseRate;
        }
        public virtual void Attack(GameMG gm) { }
        public virtual void WhenDie(GameMG gm) { }
    }
}
