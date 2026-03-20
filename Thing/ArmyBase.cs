namespace NoBadConflicts
{
    class ArmyBase
    {
        #region 通用属性
        #region 读取自Json文件
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public float maxHealth{ get; set; }
        public float attackPower { get; set; }
        public float attackRange { get; set; }
        public float attackInterval { get; set; }
        public float moveSpeed { get; set; }
        public float size { get; set; }
        public float weight { get; set; }
        public string color { get; set; }
        #endregion

        public Vector2 position;
        public float lostHealth;
        public float intervalCounter;
        public ConBase target;
        public Color tcolor;
        public float conditionSize;

        public int rank = 0;
        public int num = 0;
        public int EX_mode = -1;

        public bool canBeTrack = true;
        public float rageTime = 0f;
        public float frozenTime = 0f;
        public float coldTime = 0f;
        public float fastTime = 0f;
        #endregion
        public ArmyBase() { }

        #region 战斗中创建副本
        public ArmyBase(ArmyBase ab, Vector2 pos, Camera2D camera)
        {
            this.id = ab.id;
            this.name = ab.name;
            this.EX_mode = ab.EX_mode;
            this.maxHealth = ab.maxHealth;
            this.attackPower = ab.attackPower;
            for(int i = 0;i < ab.rank; i++) { this.maxHealth *= 1.2f; this.attackPower *= 1.2f; }
            this.attackRange = ab.attackRange;
            this.attackInterval = ab.attackInterval;
            this.moveSpeed = ab.moveSpeed;
            this.size = ab.size;
            this.weight = ab.weight;
            this.tcolor = ab.tcolor;
            this.position = Raylib.GetScreenToWorld2D(pos, camera);
            this.lostHealth = 0;
            this.intervalCounter = 0;
            conditionSize = 1.0f;
            WhenCreate(this);
        }
        public void Create(GameMG gm, int index)
        {
            if (gm.M_a.armyDataInF[index] <= 0) return;
            ArmyBase army = null;
            switch (this.id)
            {
                case "A01":
                    army = new YMR(this, Raylib.GetMousePosition(), gm.camera);
                    break;
                case "A02":
                    army = new GJS(this, Raylib.GetMousePosition(), gm.camera);
                    break;
                case "A03":
                    army = new JR(this, Raylib.GetMousePosition(), gm.camera);
                    break;
                case "A04":
                    army = new PKCR(this, Raylib.GetMousePosition(), gm.camera);
                    break;
                case "A05":
                    army = new GLSR(this, Raylib.GetMousePosition(), gm.camera);
                    break;
                case "A06":
                    army = new ZDR(this, Raylib.GetMousePosition(), gm.camera);
                    break;
                case "A07":
                    army = new FS(this, Raylib.GetMousePosition(), gm.camera);
                    break;

                case "S01":
                    army = new RageSpell(this, Raylib.GetMousePosition(), gm.camera);
                    break;
                case "S02":
                    army = new FastSpell(this, Raylib.GetMousePosition(), gm.camera);
                    break;
                case "S03":
                    army = new HealSpell(this, Raylib.GetMousePosition(), gm.camera);
                    break;
                case "S04":
                    army = new HealPSpell(this, Raylib.GetMousePosition(), gm.camera);
                    break; 
                case "S05":
                    army = new FrozenSpell(this, Raylib.GetMousePosition(), gm.camera);
                    break;
                case "S66":
                    army = new FireCracker(this, Raylib.GetMousePosition(), gm.camera);
                    break;
                case "S06":
                    army = new WoundSpell(this, Raylib.GetMousePosition(), gm.camera);
                    break;
                case "S07":
                    army = new InterruptSpell(this, Raylib.GetMousePosition(), gm.camera);
                    break;
                default:
                    army = new ArmyBase(this, Raylib.GetMousePosition(), gm.camera);
                    break;
            }
            gm.M_a.armyList.AddLast(army);
            gm.M_a.armyDataInF[index] -= 1;
        }
        public virtual void WhenCreate(ArmyBase army) { }
        #endregion

        #region 战斗中行为
        public virtual void Behaviors(GameMG gm)
        {
            bool faster;
            if(fastTime > 0)
            {
                faster = true;
                fastTime -= gm.dt;
            }
            else 
                faster = false;
            float dt = gm.dt * RateCalculate(gm.dt);

            if(ShouldFindTarget())
            {
                FindTarget(gm);
                intervalCounter = 0;
            }
            else
            {
                float distance = Vector2.Distance(this.position, this.target.position);
                Vector2 direction = this.target.position - this.position;
                float radians = MathF.Atan2(direction.Y, direction.X);
                float truedis;
                if (Math.Abs(MathF.Cos(radians)) >= Math.Abs(MathF.Sin(radians)))
                    truedis = distance - Math.Abs(this.target.size * 0.5f / MathF.Cos(radians));
                else truedis = distance - Math.Abs(this.target.size * 0.5f / MathF.Sin(radians));

                if (truedis <= this.attackRange + this.size)
                {
                    intervalCounter += dt;
                    if (intervalCounter >= attackInterval)
                    {
                        intervalCounter -= attackInterval;
                        Attack(gm);
                    }
                }
                else
                {
                    intervalCounter = 0;
                    Vector2 dir = Vector2.Normalize(target.position - this.position);
                    if(faster)
                        this.position += dir * (this.moveSpeed + 25f) * dt;
                    else
                        this.position += dir * this.moveSpeed * dt;
                }
            }
        }
        public virtual void DrawCondition()
        {
            float w = 30f * conditionSize;
            float h = 4f * conditionSize;
            Rectangle rect = new Rectangle(position.X - w / 2, position.Y - (size * 2), w, h);
            Raylib.DrawRectangleRec(rect, Color.Gray);
            if (this.intervalCounter < this.attackInterval)
            {
                if (this.intervalCounter < 0) rect.Width = 0;
                else rect.Width *= (this.intervalCounter / this.attackInterval);
            }
            Raylib.DrawRectangleRec(rect, Color.LightGray);
            rect.Width = w;
            Raylib.DrawRectangleLinesEx(rect, 1.0f, Color.DarkGray);
            Color color;
            if (lostHealth > maxHealth * 0.75f) color = Color.Red;
            else if (lostHealth > maxHealth * 0.25f) color = Color.Yellow;
            else color = Color.Green;
            float hh = 5f * conditionSize;
            rect = new Rectangle(position.X - w / 2, position.Y - (size * 2) + h, w, hh);
            Raylib.DrawRectangleRec(rect, Color.Gray);
            rect.Width *= (1.0f - (float)lostHealth / maxHealth);
            Raylib.DrawRectangleRec(rect, color);
            rect.Width = w;
            Raylib.DrawRectangleLinesEx(rect, 1.0f, Color.DarkGray);
        }
        public virtual void DrawSelf()
        {
            Color dcolor = tcolor;
            float sizeMul = rageTime > 0 ? 1.2f : 1f;
            if (rageTime > 0)
            {
                dcolor.R = (byte)(dcolor.R * 0.75f + Color.Purple.R * 0.25f);
                dcolor.G = (byte)(dcolor.G * 0.75f + Color.Purple.G * 0.25f);
                dcolor.B = (byte)(dcolor.B * 0.75f + Color.Purple.B * 0.25f);
            }
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
            Raylib.DrawCircleV(position, size * sizeMul, dcolor);
            Raylib.DrawCircleLinesV(position, size * sizeMul, Color.Black);
        }
        public virtual bool ShouldFindTarget() 
        { 
            if (target == null || target.lostHealth >= target.maxHealth) return true;
            return false;
        }
        public virtual void FindTarget(GameMG gm) 
        { 
            float minDis = float.MaxValue;
            foreach (ConBase con in gm.M_c.conList)
            {
                if (con is C_PZJD || con.lostHealth >= con.maxHealth || !con.canBeTrack) continue;
                float dis = Vector2.Distance(this.position, con.position);
                if (dis < minDis)
                {
                    minDis = dis;
                    this.target = con;
                }
            }
        }
        public virtual void Attack(GameMG gm) { }
        public virtual void BeAttack(float num)
        {
            this.lostHealth += num;
        }
        public virtual float RateCalculate(float dt)
        {
            float baseRate = 1f;
            if(frozenTime > 0)
            {
                frozenTime -= dt;
                baseRate = 0;
            }
            if (coldTime > 0)
            {
                coldTime -= dt;
                baseRate *= 0.67f;
            }
            if (rageTime > 0)
            {
                rageTime -= dt;
                baseRate *= 1.5f;
            }
            return baseRate;
        }
        public virtual void BeHealed(float healAmount) 
        { 
            lostHealth -= healAmount;
            if (lostHealth < 0) lostHealth = 0;
        }
        public virtual bool ShouldDie() 
        { 
            if(lostHealth >= maxHealth)
                return true;
            return false;
        }
        public virtual void WhenDie(GameMG gm) { }
        #endregion
    }
    
}
