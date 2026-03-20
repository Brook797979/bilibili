namespace NoBadConflicts
{
    enum E_CastType
    {
        Ground,
        Air
    }
    internal class CastBase
    {
        public E_CastType type;
        public float maxTime;
        public float pastTime;
        public virtual void Behavior(GameMG gm) { }
        public virtual void Draw() { }
        public virtual void OnDestroy(GameMG gm) { }
    }
    class Cast_Kick : CastBase
    {
        ArmyBase from;
        Vector2 position;
        float radians;
        Color color;
        public Cast_Kick(ArmyBase from, ConBase to, float damage)
        {
            type = E_CastType.Air;
            this.maxTime = 0.3f;
            this.pastTime = 0f;
            this.from = from;
            to.lostHealth += damage;

            Vector2 direction = to.position - from.position;
            float rad = MathF.Atan2(direction.Y, direction.X);
            this.radians = rad * (180f / MathF.PI);

            this.position = from.position;
            this.color = from.tcolor;
            color.R /= 2;
            color.G /= 2;
            color.B /= 2;
        }
        public override void Behavior(GameMG gm) 
        {
            if (pastTime < maxTime)
            {
                pastTime += gm.dt;
            }
        }
        public override void Draw()
        {
            Raylib.DrawRing(position, from.size * 0.8f, from.size * 1.35f, radians - 30, radians + 30, 9, color);
        }
    }
    class Cast_Arrow : CastBase
    {
        Vector2 position;
        ArmyBase toA;
        ConBase toC;
        float length;
        float width;
        float radians;
        float speed;
        Color color;
        float damage;
        public Cast_Arrow(ArmyBase from, ConBase to, float len, float wid, float speed, float damage)
        {
            type = E_CastType.Air;
            this.maxTime = 0.1f;
            this.position = from.position;
            this.toC = to;
            this.length = len;
            this.width = wid;
            Vector2 direction = to.position - from.position;
            this.radians = MathF.Atan2(direction.Y, direction.X);
            this.speed = speed;
            this.color = from.tcolor;
            color.R /= 2;
            color.G /= 2;
            color.B /= 2;
            this.damage = damage;
        }
        public Cast_Arrow(ConBase from, ArmyBase to, float len, float wid, float speed, float damage, Color color)
        {
            type = E_CastType.Air;
            this.maxTime = 0.1f;
            this.position = from.position;
            this.toA = to;
            this.length = len;
            this.width = wid;
            Vector2 direction = to.position - from.position;
            this.radians = MathF.Atan2(direction.Y, direction.X);
            this.speed = speed;
            this.color = color;
            this.damage = damage;
        }
        public override void Behavior(GameMG gm)
        {
            position += new Vector2(speed * gm.dt * MathF.Cos(radians), speed * gm.dt * MathF.Sin(radians));
            if (toA != null)
            {
                float dis = Vector2.Distance(position, toA.position);
                if (dis <= toA.size)
                {
                    toA.BeAttack(damage);
                    pastTime = maxTime + 0.1f;
                }
            }
            else if (toC != null)
            {
                float dis;
                if (Math.Abs(MathF.Cos(radians)) >= Math.Abs(MathF.Sin(radians)))
                    dis = Math.Abs(this.toC.size * 0.8f / MathF.Cos(radians));
                else dis = Math.Abs(this.toC.size * 0.8f / MathF.Sin(radians));
                if(Vector2.Distance(position, toC.position) <= dis)
                {
                    toC.lostHealth += damage;
                    pastTime = maxTime + 0.1f;
                }
            }
            else pastTime = maxTime + 1f;
        }
        public override void Draw()
        {
            Vector2 direction = new Vector2();
            if (toA != null)
                direction = toA.position - this.position;
            else if (toC != null)
                direction = toC.position - this.position;
            else return;
            this.radians = MathF.Atan2(direction.Y, direction.X);
            float dx = length / 2 * MathF.Cos(radians);
            float dy = length / 2 * MathF.Sin(radians);
            Raylib.DrawLineEx(new Vector2(position.X + dx, position.Y + dy), 
                new Vector2(position.X - dx, position.Y - dy), width, color);
        }
    }
    class Cast_FireBall : CastBase
    {
        Vector2 position;
        ArmyBase toA;
        ConBase toC;
        float size;
        float radians;
        float speed;
        float range;
        Color color;
        Color lcolor;
        float damage;
        public Cast_FireBall(ArmyBase from, ConBase to, float size, float speed, float damage, Color color)
        {
            type = E_CastType.Air;
            this.maxTime = 0.1f;
            this.position = from.position;
            this.toC = to;
            this.size = size;
            Vector2 direction = to.position - from.position;
            this.radians = MathF.Atan2(direction.Y, direction.X);
            this.speed = speed;
            this.color = color;
            lcolor = new Color(color.R / 2 + 128, color.G / 2 + 128, color.B / 2 + 128, 255);
            this.damage = damage;
        }
        public Cast_FireBall(ConBase from, ArmyBase to, float size, float speed, float damage, Color color, float range)
        {
            type = E_CastType.Air;
            this.maxTime = 0.1f;
            this.position = from.position;
            this.range = range;
            this.toA = to;
            this.size = size;
            Vector2 direction = to.position - from.position;
            this.radians = MathF.Atan2(direction.Y, direction.X);
            this.speed = speed;
            this.color = color;
            lcolor = new Color(color.R / 2 + 128, color.G / 2 + 128, color.B / 2 + 128, 255);
            this.damage = damage;
        }
        public override void Behavior(GameMG gm)
        {
            position += new Vector2(speed * gm.dt * MathF.Cos(radians), speed * gm.dt * MathF.Sin(radians));
            if (toA != null)
            {
                float dis = Vector2.Distance(position, toA.position);
                if(dis < toA.size)
                {
                    List<(ArmyBase a, float dis)> disL = new List<(ArmyBase, float)>();
                    foreach (var army in gm.M_a.armyList)
                    {
                        disL.Add((army, Vector2.Distance(this.position, army.position) - army.size));
                    }
                    disL.Sort((a, b) => a.dis.CompareTo(b.dis));
                    foreach(var dl in disL)
                    {
                        if (dl.dis > range) break;
                        dl.a.BeAttack(damage);
                    }
                    pastTime = maxTime + 0.1f;
                }
            }
            else if (toC != null)
            {
                float dis;
                if (Math.Abs(MathF.Cos(radians)) >= Math.Abs(MathF.Sin(radians)))
                    dis = Math.Abs(this.toC.size * 0.8f / MathF.Cos(radians));
                else dis = Math.Abs(this.toC.size * 0.8f / MathF.Sin(radians));
                if (Vector2.Distance(position, toC.position) <= dis + size)
                {
                    toC.lostHealth += damage;
                    pastTime = maxTime + 0.1f;
                }
            }
            else pastTime = maxTime + 1f;
        }
        public override void Draw()
        {
            Vector2 direction = new Vector2();
            if (toA != null)
                direction = toA.position - this.position;
            else if (toC != null)
                direction = toC.position - this.position;
            else return;
            Raylib.DrawCircleV(position, size, color);
            this.radians = MathF.Atan2(direction.Y, direction.X);
            Vector2 op = new Vector2(position.X - size * 0.8f * MathF.Cos(radians), position.Y - size * 0.8f * MathF.Sin(radians));
            Raylib.DrawCircleV(op, size * 0.7f, color);
            Raylib.DrawCircleV(position, size * 0.6f, lcolor);
        }
    }
    class Cast_Bullet : CastBase
    {
        Vector2 position;
        ArmyBase to;
        float radians;
        float size;
        float speed;
        Color color;
        float damage;
        public Cast_Bullet(ConBase from, ArmyBase to, float size, float speed, float damage, Color color)
        {
            type = E_CastType.Air;
            this.maxTime = 0.1f;
            this.position = from.position;
            this.to = to;
            this.size = size;
            Vector2 direction = to.position - from.position;
            this.radians = MathF.Atan2(direction.Y, direction.X);
            this.speed = speed;
            this.color = color;
            this.damage = damage;
        }
        public override void Behavior(GameMG gm)
        {
            position += new Vector2(speed * gm.dt * MathF.Cos(radians), speed * gm.dt * MathF.Sin(radians));
            if (to != null)
            {
                float dis = Vector2.Distance(position, to.position);
                if (dis <= to.size)
                {
                    to.BeAttack(damage);
                    pastTime = maxTime + 0.1f;
                }
            }
            else pastTime = maxTime + 1f;
        }
        public override void Draw()
        {
            Vector2 direction = new Vector2();
            if (to != null)
                direction = to.position - this.position;
            else return;
            this.radians = MathF.Atan2(direction.Y, direction.X);
            Raylib.DrawCircleV(position, size, color);
        }
    }
    class Cast_GroundWave : CastBase
    {
        Vector2 position;
        float size;
        Color color;
        float damage;
        float force;
        bool done = false;
        public Cast_GroundWave(Vector2 position, float size, float damage, float force, Color color)
        {
            type = E_CastType.Ground;
            this.position = position;
            this.size = size;
            this.damage = damage;
            this.force = force;
            this.color = color;
            maxTime = 0.15f;
        }
        public override void Behavior(GameMG gm)
        {
            pastTime += gm.dt;
            if (done) return;
            List<(ArmyBase a, float dis)> disL = new List<(ArmyBase, float)>();
            foreach (var army in gm.M_a.armyList)
            {
                disL.Add((army, Vector2.Distance(this.position, army.position)));
            }
            disL.Sort((a, b) => a.dis.CompareTo(b.dis));
            foreach (var dl in disL)
            {
                if (dl.dis > this.size) break;
                float force = this.force;
                if(dl.a.weight > 0.01f) force /= dl.a.weight;
                if (force <= -1f || force >= 1f)
                {
                    if (force > 0)
                    {
                        Vector2 dir = Vector2.Normalize(dl.a.position - this.position);
                        dl.a.position += dir * force;
                    }
                    else if (force < 0)
                    {
                        float dis = Vector2.Distance(this.position, dl.a.position);
                        float f = force < -dis * 0.9f ? -dis * 0.9f : force;
                        Vector2 dir = Vector2.Normalize(dl.a.position - this.position);
                        dl.a.position += dir * f;
                    }
                }
                dl.a.BeAttack(damage);
            }
            done = true;
        }
        public override void Draw()
        {
            Raylib.DrawCircleV(position, size, color);
        }
    }
    class Cast_ColdBall : CastBase
    {
        Vector2 position;
        ArmyBase toA;
        ConBase toC;
        float size;
        float radians;
        float speed;
        float range;
        Color color;
        Color lcolor;
        float damage;
        float coldtime;
        bool dofrozen;
        public Cast_ColdBall(ArmyBase from, ConBase to, float size, float speed, float damage, Color color, float coldtime)
        {
            type = E_CastType.Air;
            this.maxTime = 0.1f;
            this.position = from.position;
            this.toC = to;
            this.size = size;
            Vector2 direction = to.position - from.position;
            this.radians = MathF.Atan2(direction.Y, direction.X);
            this.speed = speed;
            this.color = color;
            lcolor = new Color(color.R / 2 + 128, color.G / 2 + 128, color.B / 2 + 128, 255);
            this.damage = damage;
            this.coldtime = coldtime;
        }
        public Cast_ColdBall(ConBase from, ArmyBase to, 
            float size, float speed, float damage, Color color, float range, float coldtime, bool frozen = false)
        {
            type = E_CastType.Air;
            this.maxTime = 0.1f;
            this.position = from.position;
            this.range = range;
            this.toA = to;
            this.size = size;
            Vector2 direction = to.position - from.position;
            this.radians = MathF.Atan2(direction.Y, direction.X);
            this.speed = speed;
            this.color = color;
            lcolor = new Color(color.R / 2 + 128, color.G / 2 + 128, color.B / 2 + 128, 255);
            this.damage = damage;
            this.coldtime = coldtime;
            this.dofrozen = frozen;
        }
        public override void Behavior(GameMG gm)
        {
            position += new Vector2(speed * gm.dt * MathF.Cos(radians), speed * gm.dt * MathF.Sin(radians));
            if (toA != null)
            {
                float dis = Vector2.Distance(position, toA.position);
                if (dis < toA.size)
                {
                    List<(ArmyBase a, float dis)> disL = new List<(ArmyBase, float)>();
                    foreach (var army in gm.M_a.armyList)
                    {
                        disL.Add((army, Vector2.Distance(this.position, army.position) - army.size));
                    }
                    disL.Sort((a, b) => a.dis.CompareTo(b.dis));
                    foreach (var dl in disL)
                    {
                        if (dl.dis > range) break;
                        dl.a.BeAttack(damage);
                        dl.a.coldTime = dl.a.coldTime <= coldtime ? coldtime : dl.a.coldTime;
                    }
                    pastTime = maxTime + 0.1f;
                }
            }
            else if (toC != null)
            {
                float dis;
                if (Math.Abs(MathF.Cos(radians)) >= Math.Abs(MathF.Sin(radians)))
                    dis = Math.Abs(this.toC.size * 0.8f / MathF.Cos(radians));
                else dis = Math.Abs(this.toC.size * 0.8f / MathF.Sin(radians));
                if (Vector2.Distance(position, toC.position) <= dis + size)
                {
                    toC.lostHealth += damage;
                    pastTime = maxTime + 0.1f;
                    toC.coldTime = toC.coldTime <= coldtime ? coldtime : toC.coldTime;
                }
            }
            else pastTime = maxTime + 1f;
        }
        public override void Draw()
        {
            Vector2 direction = new Vector2();
            if (toA != null)
                direction = toA.position - this.position;
            else if (toC != null)
                direction = toC.position - this.position;
            else return;
            Raylib.DrawCircleV(position, size, color);
            this.radians = MathF.Atan2(direction.Y, direction.X);
            Vector2 op = new Vector2(position.X - size * 0.8f * MathF.Cos(radians), position.Y - size * 0.8f * MathF.Sin(radians));
            Raylib.DrawCircleV(op, size * 0.7f, color);
            Raylib.DrawCircleV(position, size * 0.6f, lcolor);
        }
        public override void OnDestroy(GameMG gm)
        {
            if (dofrozen)
            {
                Cast_BuffCircle cast = new Cast_BuffCircle(position, range, 0, new Color(102, 191, 255, 55), coldtime / 2, "frozen");
                gm.casts.Add(cast);
            }
        }
    }
    class Cast_WindBall : CastBase
    {
        Vector2 position;
        ArmyBase to;
        float size;
        float radians;
        float speed;
        float range;
        Color color;
        Color lcolor;
        float damage;
        Cast_GroundWave cast;
        public Cast_WindBall(ConBase from, ArmyBase to, float size, float speed, float damage, Color color, float range)
        {
            type = E_CastType.Air;
            this.maxTime = 0.1f;
            this.position = from.position;
            this.range = range;
            this.to = to;
            this.size = size;
            Vector2 direction = to.position - from.position;
            this.radians = MathF.Atan2(direction.Y, direction.X);
            this.speed = speed;
            this.color = color;
            lcolor = new Color(color.R / 2 + 128, color.G / 2 + 128, color.B / 2 + 128, 255);
            this.damage = damage;
        }
        public override void Behavior(GameMG gm)
        {
            position += new Vector2(speed * gm.dt * MathF.Cos(radians), speed * gm.dt * MathF.Sin(radians));
            float dis = Vector2.Distance(position, to.position);
            if (dis < to.size)
            {
                List<(ArmyBase a, float dis)> disL = new List<(ArmyBase, float)>();
                foreach (var army in gm.M_a.armyList)
                {
                    disL.Add((army, Vector2.Distance(this.position, army.position) - army.size));
                }
                disL.Sort((a, b) => a.dis.CompareTo(b.dis));
                foreach (var dl in disL)
                {
                    if (dl.dis > range) break;
                    dl.a.BeAttack(damage);
                }
                Vector2 p = position + new Vector2(20f * MathF.Cos(radians), 20f * MathF.Sin(radians));
                Color c = lcolor;
                c.A = 155;
                cast = new Cast_GroundWave(p, 125f, damage / 3, -35f, c);
                pastTime = maxTime + 0.1f;
            }
        }
        public override void Draw()
        {
            Vector2 direction = to.position - this.position;
            Raylib.DrawCircleV(position, size, color);
            this.radians = MathF.Atan2(direction.Y, direction.X);
            Vector2 op = new Vector2(position.X - size * 0.8f * MathF.Cos(radians), position.Y - size * 0.8f * MathF.Sin(radians));
            Raylib.DrawCircleV(op, size * 0.7f, color);
            Raylib.DrawCircleV(position, size * 0.6f, lcolor);
        }
        public override void OnDestroy(GameMG gm)
        {
            gm.casts.Add(cast);
        }
    }
    class Cast_BuffCircle : CastBase
    {
        Vector2 position;
        float size;
        Color color;
        float damage;
        float btime;
        string bname;
        bool done = false;
        public Cast_BuffCircle(Vector2 position, float size, float damage, Color color, float btime, string bname)
        {
            type = E_CastType.Ground;
            this.position = position;
            this.size = size;
            this.damage = damage;
            this.color = color;
            maxTime = 0.15f;
            this.btime = btime;
            this.bname = bname;
        }
        public override void Behavior(GameMG gm)
        {
            pastTime += gm.dt;
            if (done) return;
            List<(ArmyBase a, float dis)> disL = new List<(ArmyBase, float)>();
            foreach (var army in gm.M_a.armyList)
            {
                disL.Add((army, Vector2.Distance(this.position, army.position)));
            }
            disL.Sort((a, b) => a.dis.CompareTo(b.dis));
            foreach (var dl in disL)
            {
                if (dl.dis > this.size) break;
                switch (bname)
                {
                    case "cold":
                        dl.a.coldTime = dl.a.coldTime <= btime ? btime : dl.a.coldTime;
                        break;
                    case "frozen":
                        dl.a.frozenTime = dl.a.frozenTime <= btime ? btime : dl.a.frozenTime;
                        break;
                    default:
                        break;
                }
                dl.a.BeAttack(damage);
            }
            done = true;
        }
        public override void Draw()
        {
            Raylib.DrawCircleV(position, size, color);
        }
    }
    class Cast_SpellCircle2A : CastBase
    {
        Vector2 position;
        float range;
        Color color;
        string bname;
        bool firstdone = true;
        float num;
        public Cast_SpellCircle2A(Vector2 position, float range, float time, string bname, float num = 0)
        {
            type = E_CastType.Ground;
            this.position = position;
            this.range = range;
            maxTime = time;
            this.bname = bname;
            this.num = num;
            switch (bname)
            {
                case "rage":
                    color = new Color(135, 60, 190, 55);
                    break;
                case "fast":
                    color = new Color(255, 0, 255, 55);
                    break;
                case "heal":
                    color = new Color(255, 161, 0, 55); 
                    break;
                case "healP":
                    color = new Color(245, 203, 0, 55);
                    break;
                default:
                    ErroR.Report("未录入的效果Cast_SpellCircle2A");
                    break;
            }
        }
        public override void Behavior(GameMG gm)
        {
            switch (bname)
            {
                case "rage":
                case "fast":
                    if (firstdone)
                    {
                        firstdone = false;
                        BuffOn(gm);
                    }
                    pastTime += gm.dt;
                    if(pastTime >= 0.2f)
                    {
                        pastTime -= 0.2f;
                        BuffOn(gm);
                        maxTime -= 0.2f;
                    }
                    break;
                case "heal":
                case "healP":
                    pastTime += gm.dt;
                    BuffOn(gm);
                    break;
            }
        }
        public override void OnDestroy(GameMG gm)
        {
            if(bname == "rage" || bname == "fast")
                BuffOn(gm);
        }
        public void BuffOn(GameMG gm)
        {
            List<(ArmyBase a, float dis)> disL = new List<(ArmyBase, float)>();
            foreach (var army in gm.M_a.armyList)
            {
                disL.Add((army, Vector2.Distance(this.position, army.position) - army.size / 2));
            }
            disL.Sort((a, b) => a.dis.CompareTo(b.dis));
            foreach (var dl in disL)
            {
                if (dl.dis > this.range) break;
                switch (bname)
                {
                    case "rage":
                        dl.a.rageTime = dl.a.rageTime <= 0.3f ? 0.3f : dl.a.rageTime;
                        break;
                    case "fast":
                        dl.a.fastTime = dl.a.fastTime <= 0.3f ? 0.3f : dl.a.fastTime;
                        break;
                    case "heal":
                        dl.a.BeHealed(gm.dt * num);
                        break;
                    case "healP":
                        dl.a.BeHealed(gm.dt * num / 100f * dl.a.maxHealth);
                        break;
                    default:
                        break;
                }
            }
        }
        public override void Draw()
        {
            Raylib.DrawCircleV(position, range, color);
        }
    }
    class Cast_SpellCircle2C : CastBase
    {
        Vector2 position;
        float range;
        Color color;
        string bname;
        bool firstdone = true;
        float damage;
        List<(ConBase c, float dis)> disL = new List<(ConBase, float)>();
        public Cast_SpellCircle2C(GameMG gm, Vector2 position, float range, float time, string bname, float num = 0)
        {
            type = E_CastType.Ground;
            this.position = position;
            this.range = range;
            maxTime = time;
            this.bname = bname;
            this.damage = num;
            switch (bname)
            {
                case "frozen":
                    color = new Color(102, 191, 255, 55);
                    break;
                case "firecracker":
                    type = E_CastType.Air;
                    color = new Color(230, 41, 55, 95);
                    break;
                case "wound":
                    color = new Color(0, 0, 0, 55);
                    break;
                case "interrupt":
                    color = new Color(130, 41, 55, 55);
                    break;
                default :
                    ErroR.Report("未录入的效果Cast_SpellCircle2C");
                    break;
            }

            foreach (var con in gm.M_c.conList)
            {
                float distance = Vector2.Distance(this.position, con.position);
                Vector2 direction = con.position - this.position;
                float radians = MathF.Atan2(direction.Y, direction.X);
                float truedis;
                if (Math.Abs(MathF.Cos(radians)) >= Math.Abs(MathF.Sin(radians)))
                    truedis = distance - Math.Abs(con.size * 0.8f / MathF.Cos(radians));
                else truedis = distance - Math.Abs(con.size * 0.8f / MathF.Sin(radians));
                disL.Add((con, truedis));
            }
            disL.Sort((a, b) => a.dis.CompareTo(b.dis));
        }
        public override void Behavior(GameMG gm)
        {
            switch (bname)
            {
                case "frozen":
                case "wound":
                case "interrupt":
                    pastTime += gm.dt;
                    if (firstdone)
                    {
                        BuffOn(gm); 
                        firstdone = false;
                    }
                    break;
                case "firecracker":
                    pastTime += gm.dt;
                    if(pastTime >= 0.2f)
                    {
                        BuffOn(gm);
                        if (Raylib.IsSoundPlaying(SoundMG.boomSound))
                        {
                            Raylib.StopSound(SoundMG.boomSound);
                        }
                        Raylib.PlaySound(SoundMG.boomSound);    
                        pastTime -= 0.2f;
                        maxTime -= 0.2f;
                    }
                    break;
            }
        }
        public void BuffOn(GameMG gm)
        {
            foreach (var dl in disL)
            {
                if (dl.dis > this.range) break;
                switch (bname)
                {
                    case "frozen":
                        dl.c.frozenTime = dl.c.frozenTime <= maxTime ? maxTime : dl.c.frozenTime;
                        break;
                    case "firecracker":
                        dl.c.lostHealth += damage;
                        break;
                    case "wound":
                        dl.c.lostHealth += dl.c.lostHealth * damage / 100f;
                        break;
                    case "interrupt":
                        dl.c.intervalCounter = 0;
                        dl.c.targets.Clear();
                        break;
                    default:
                        break;
                }
            }
        }
        public override void Draw()
        {
            Raylib.DrawCircleV(position, range, color);
        }
    }
    class Cast_Hook : CastBase
    {
        float radians;
        float force;
        float totald;
        float maxd;
        ArmyBase target;
        Vector2 mpos;
        public Cast_Hook(ArmyBase target, float r, float f, float t, float maxd = 500f)
        {
            type = E_CastType.Air;
            this.target = target;
            this.radians = r;
            this.force = f;
            maxTime = t;
            mpos = new Vector2(target.size * MathF.Cos(radians), target.size * MathF.Sin(radians));
            totald = 0f;
            this.maxd = maxd;
        }
        public override void Behavior(GameMG gm)
        {
            if(target == null) return;
            pastTime += gm.dt;
            float force = this.force / target.weight * gm.dt;
            totald += Math.Abs(force);
            target.position += new Vector2(force * MathF.Cos(radians), force * MathF.Sin(radians));
            if (totald >= maxd)
                pastTime += maxTime;
        }
        public override void Draw()
        {
            if (target == null) return;
            Vector2 posc = target.position + mpos;
            Vector2 mp = new Vector2(target.size / 5 * MathF.Cos(radians), target.size / 5 * MathF.Sin(radians));
            Raylib.DrawLineEx(posc + mp, posc - mp, 2f, Color.Black);
        }
    }
}
