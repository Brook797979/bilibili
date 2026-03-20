namespace NoBadConflicts
{
    class YMR : ArmyBase
    {
        float undieT;
        bool tStart = false;
        public YMR(ArmyBase ab, Vector2 pos, Camera2D camera) : base(ab, pos, camera) { }
        public override void WhenCreate(ArmyBase army)
        {
            conditionSize = 0.9f;
            if (EX_mode == 0)
            {
                maxHealth *= 1.2f;
                attackPower *= 1.2f;
                rageTime = 18f;
                undieT = 3.5f;
            }
            else if (EX_mode == 1)
            {
                maxHealth *= 1.8f;
                attackPower *= 0.8f;
                moveSpeed *= 0.9f;
            }
        }
        public override void Attack(GameMG gm)
        {
            Cast_Kick cast = null;
            if (rageTime > 0)
                cast = new Cast_Kick(this, this.target, this.attackPower * 1.33f);
            else
                cast = new Cast_Kick(this, this.target, this.attackPower);
            gm.casts.Add(cast);
        }
        public override bool ShouldDie()
        {
            if (EX_mode == 0)
            {
                if (lostHealth >= maxHealth && undieT <= 0)
                    return true;
                else if (lostHealth >= maxHealth && !tStart)
                {
                    tStart = true;
                    canBeTrack = false;
                }
                return false;
            }
            else
                return base.ShouldDie();
        }
        public override void BeHealed(float healAmount)
        {
            if (!tStart)
                base.BeHealed(healAmount);
        }
        public override void Behaviors(GameMG gm)
        {
            if (tStart)
                undieT -= gm.dt;
            base.Behaviors(gm);
        }
        public override void BeAttack(float num)
        {
            if (EX_mode == 1 && intervalCounter <= 0)
                base.BeAttack(num * 0.33f);
            else 
                base.BeAttack(num);
        }
    }
    class GJS : ArmyBase
    {
        float hideT;
        public GJS(ArmyBase ab, Vector2 pos, Camera2D camera) : base(ab, pos, camera) { }
        public override void WhenCreate(ArmyBase army)
        {
            conditionSize = 0.8f;
            if(EX_mode == 0)
            {
                attackRange *= 1.1f;
                canBeTrack = false;
                hideT = 7.5f;
            }
        }
        public override void Attack(GameMG gm)
        {
            Cast_Arrow cast = null;
            if (rageTime > 0)
                cast = new Cast_Arrow(this, this.target, size * 0.7f, 1.8f, 115f, this.attackPower * 1.33f);
            else
                cast = new Cast_Arrow(this, this.target, size * 0.6f, 1.5f, 100f, this.attackPower);
            gm.casts.Add(cast);
        }
        public override void Behaviors(GameMG gm)
        {
            if(hideT > 0)
            {
                hideT -= gm.dt;
                if(hideT <= 0)
                    canBeTrack = true;
            }
            base.Behaviors(gm);
        }
    }
    class JR : ArmyBase
    {
        float attackM = 1f;
        public JR(ArmyBase ab, Vector2 pos, Camera2D camera) : base(ab, pos, camera) { }
        public override void WhenCreate(ArmyBase army)
        {
            conditionSize = 1.05f;
            if (EX_mode == 0)
            {
                maxHealth *= 1.1f;
                attackPower *= 0.35f;
                attackInterval *= 0.35f;
            }
            else if (EX_mode == 1)
            {
                maxHealth *= 1.8f;
            }
        }
        public override void Attack(GameMG gm)
        {
            if(EX_mode == 1)
            {
                this.BeHealed(0.08f * maxHealth);
            }
            else
            {
                Cast_Kick cast = null;
                if (rageTime > 0)
                    cast = new Cast_Kick(this, this.target, this.attackPower * 1.33f * attackM);
                else
                    cast = new Cast_Kick(this, this.target, this.attackPower * attackM);
                gm.casts.Add(cast);
                if (EX_mode == 0)
                    attackM *= 1.33f;
            }
        }
        public override void BeHealed(float healAmount)
        {
            if (EX_mode == 1)
            {
                lostHealth -= healAmount;
                if (lostHealth < -maxHealth) lostHealth = -maxHealth;
            }
            else
                base.BeHealed(healAmount);
        }
        public override void Behaviors(GameMG gm)
        {
            if (lostHealth < 0)
                lostHealth -= lostHealth * 0.1f * gm.dt;
            if(ShouldFindTarget())
                attackM = 1f;
            base.Behaviors(gm);
        }
    }
    class PKCR : ArmyBase
    {
        float undieT;
        bool tStart = false;
        public PKCR(ArmyBase ab, Vector2 pos, Camera2D camera) : base(ab, pos, camera) { }
        public override void WhenCreate(ArmyBase army)
        {
            conditionSize = 1.05f;
            if (EX_mode == 0)
            {
                maxHealth *= 1.15f;
                attackPower *= 1.15f;
                rageTime = 15f;
                undieT = 6f;
            }
            else if(EX_mode == 1)
            {
                maxHealth *= 0.9f;
                attackPower *= 0.9f;
                attackInterval *= 0.9f;
                attackRange = 65f;
                moveSpeed *= 2f;
            }
        }
        public override void Attack(GameMG gm)
        {
            if(EX_mode == 1)
            {
                Cast_Arrow cast = null;
                if (rageTime > 0)
                    cast = new Cast_Arrow(this, this.target, size * 0.7f, 12f, 240f, this.attackPower * 1.33f);
                else
                    cast = new Cast_Arrow(this, this.target, size * 0.6f, 10f, 200f, this.attackPower);
                gm.casts.Add(cast);
            }
            else
            {
                Cast_Kick cast = null;
                if (rageTime > 0)
                    cast = new Cast_Kick(this, this.target, this.attackPower * 1.33f);
                else
                    cast = new Cast_Kick(this, this.target, this.attackPower);
                gm.casts.Add(cast);
            } 
        }
        public override bool ShouldDie()
        {
            if (EX_mode == 0)
            {
                if (lostHealth >= maxHealth && undieT <= 0)
                    return true;
                else if (lostHealth >= maxHealth && !tStart)
                {
                    tStart = true;
                    canBeTrack = false;
                }
                return false;
            }
            else
                return base.ShouldDie();
        }
        public override void BeHealed(float healAmount)
        {
            if (!tStart)
                base.BeHealed(healAmount);
        }
        public override void Behaviors(GameMG gm)
        {
            if (tStart)
                undieT -= gm.dt;
            base.Behaviors(gm);
        }
    }
    class GLSR : ArmyBase
    {
        float damageN = 0f;
        public GLSR(ArmyBase ab, Vector2 pos, Camera2D camera) : base(ab, pos, camera) { }
        public override void WhenCreate(ArmyBase army)
        {
            conditionSize = 1.25f;
            if (EX_mode == 0)
            {
                maxHealth *= 0.8f;
                attackPower *= 0.02f;
                attackInterval *= 0.3f;
                moveSpeed *= 3.5f;
                size *= 0.8f;
            }
            else if (EX_mode == 1)
            {
                conditionSize = 1.35f;
                maxHealth *= 2f;
                attackPower *= 1.2f;
                moveSpeed *= 0.8f;
                size *= 1.3f;
            }
        }
        public override void Attack(GameMG gm)
        {
            Cast_Kick cast = null;
            float power = this.attackPower + damageN;
            damageN = 0f;
            if (rageTime > 0)
                cast = new Cast_Kick(this, this.target, power * 1.33f);
            else
                cast = new Cast_Kick(this, this.target, power);
            gm.casts.Add(cast);
        }
        public override void BeAttack(float num)
        {
            if (EX_mode == 0)
                damageN += num;
            base.BeAttack(num);
        }
        public override void BeHealed(float healAmount)
        {
            if (EX_mode == 0)
                base.BeHealed(healAmount * 2f);
            else
                base.BeHealed(healAmount);
        }
        public override void Behaviors(GameMG gm)
        {
            if (EX_mode == 1)
                BeHealed(0.005f * maxHealth * gm.dt);
            base.Behaviors(gm);
        }
        public override float RateCalculate(float dt)
        {
            if (EX_mode == 1)
            {
                float baseRate = 1f;
                if (frozenTime > 0)
                {
                    frozenTime -= dt;
                    baseRate *= 0.67f;
                }
                if (coldTime > 0)
                {
                    coldTime -= dt;
                    baseRate *= 0.9f;
                }
                if (rageTime > 0)
                {
                    rageTime -= dt;
                    baseRate *= 1.5f;
                }
                return baseRate;
            }
            return base.RateCalculate(dt);
        }
    }
    class ZDR : ArmyBase
    {
        bool statusDown = false;
        public ZDR(ArmyBase ab, Vector2 pos, Camera2D camera) : base(ab, pos, camera) { }
        public override void WhenCreate(ArmyBase army)
        {
            conditionSize = 0.65f;
            if(EX_mode == 0)
            {
                maxHealth *= 1.8f;
                size *= 1.2f;
                statusDown = true;
            }
            else if (EX_mode == 1)
            {
                attackPower *= 1.2f;
            }
        }
        public override void Attack(GameMG gm)
        {
            Cast_Kick cast = null;
            if (rageTime > 0)
                cast = new Cast_Kick(this, this.target, this.attackPower * 1.33f);
            else
                cast = new Cast_Kick(this, this.target, this.attackPower);
            gm.casts.Add(cast);
            if (statusDown)
            {
                statusDown = false;
                attackInterval = 0.75f;
                attackPower *= 0.5f;
                moveSpeed *= 0.5f;
            }
            if (EX_mode == 0)
                lostHealth += maxHealth * 0.05f;
            else
                lostHealth += maxHealth;
        }
        public override void BeHealed(float healAmount)
        {
            if(EX_mode == 0) return;
            base.BeHealed(healAmount);
        }
        public override void WhenDie(GameMG gm)
        {
            if(EX_mode == 1)
            {
                Cast_SpellCircle2A cast1 = new Cast_SpellCircle2A(position, 45f, 4f, "heal", 20f);
                Cast_SpellCircle2A cast2 = new Cast_SpellCircle2A(position, 30f, 4f, "healP", 2.5f);
                gm.casts.Add(cast1);
                gm.casts.Add(cast2);
            }
        }
    }
    class FS : ArmyBase
    {
        float IntervalM;
        float SpeedM;
        public FS(ArmyBase ab, Vector2 pos, Camera2D camera) : base(ab, pos, camera) { }
        public override void WhenCreate(ArmyBase army)
        {
            conditionSize = 1f;
            if(EX_mode == 0)
            {
                maxHealth *= 1.2f;
                attackPower *= 0.95f;
            }
            else if(EX_mode == 1)
            {
                attackPower *= 1.2f;
                IntervalM = attackInterval;
                SpeedM = moveSpeed;
            }
        }
        public override void Attack(GameMG gm)
        {
            if(EX_mode == 0)
            {
                Cast_ColdBall cast;
                if (rageTime > 0)
                    cast = new Cast_ColdBall(this, this.target, 7.5f, 75, this.attackPower * 1.33f, Color.Blue, 2f);
                else
                    cast = new Cast_ColdBall(this, this.target, 6f, 60, this.attackPower, Color.Blue, 2f);
                gm.casts.Add(cast);
            }
            else if(EX_mode == 1)
            {
                float rate = lostHealth <= maxHealth ? lostHealth / maxHealth : 1f;
                float powerM = 0.5f + 3f * rate;
                float speed = 30f + 90f * rate;
                float csize = 3f + 9f * rate;
                Cast_FireBall cast;
                if (rageTime > 0)
                    cast = new Cast_FireBall(this, this.target, csize * 1.2f, speed * 1.2f, this.attackPower * powerM * 1.33f, Color.Orange);
                else
                    cast = new Cast_FireBall(this, this.target, csize, speed, this.attackPower * powerM, Color.Orange);
                gm.casts.Add(cast);
            }
            else
            {
                Cast_FireBall cast = null;
                if (rageTime > 0)
                    cast = new Cast_FireBall(this, this.target, 7.5f, 75, this.attackPower * 1.33f, Color.Orange);
                else
                    cast = new Cast_FireBall(this, this.target, 6f, 60, this.attackPower, Color.Orange);
                gm.casts.Add(cast);
            }
        }
        public override void WhenDie(GameMG gm)
        {
            if(EX_mode == 0)
            {
                Cast_SpellCircle2C cast = new Cast_SpellCircle2C(gm, position, attackRange * 1.5f, 1f, "frozen");
                gm.casts.Add(cast);
            }
        }
        public override void Behaviors(GameMG gm)
        {
            if(EX_mode == 1)
            {
                float rate = lostHealth <= maxHealth ? lostHealth / maxHealth : 1f;
                if (lostHealth <= maxHealth / 2)
                    lostHealth += 0.1f * gm.dt * maxHealth;
                attackInterval = IntervalM * (0.33f + 0.67f * (1f - rate));
                moveSpeed = SpeedM * (1f + 3f * rate);
            }
            base.Behaviors(gm);
        }
    }
}
