namespace NoBadConflicts
{
    class SpellBase : ArmyBase
    {
        public SpellBase() { }
        public SpellBase(ArmyBase ab, Vector2 pos, Camera2D camera) : base(ab, pos, camera) { }
        public static void AddAllSpell(List<ArmyBase> list)
        {
            list.Add(new RageSpell());
            list.Add(new FastSpell());
            list.Add(new HealSpell());
            list.Add(new HealPSpell());
            list.Add(new FrozenSpell());
            list.Add(new WoundSpell());
            list.Add(new InterruptSpell());
            list.Add(new FireCracker());
        }
        public override void Behaviors(GameMG gm) { }
        public override void DrawCondition() { }
        public override void DrawSelf()
        {
            Color c = tcolor;
            c.A /= 5;
            Raylib.DrawCircleV(position, attackRange, c);
        }
        public override bool ShouldDie()
        {
            return true;
        }
    }
    class RageSpell : SpellBase
    {
        public RageSpell(ArmyBase ab, Vector2 pos, Camera2D camera) : base(ab, pos, camera) { }
        public RageSpell()
        {
            id = "S01";
            name = "狂暴药水";
            description = "范围施加狂暴效果";
            attackRange = 80f;
            num = 0;
            tcolor = new Color(135, 60, 190, 155);
        }
        public override void WhenDie(GameMG gm)
        {
            Cast_SpellCircle2A cast = new Cast_SpellCircle2A(position, attackRange, 8f, "rage");
            gm.casts.Add(cast);
        }
    }
    class FastSpell : SpellBase
    {
        public FastSpell(ArmyBase ab, Vector2 pos, Camera2D camera) : base(ab, pos, camera) { }
        public FastSpell()
        {
            id = "S02";
            name = "急速药水";
            description = "范围施加急速效果";
            attackRange = 150f;
            num = 0;
            tcolor = new Color(255, 0, 255, 155);
        }
        public override void WhenDie(GameMG gm)
        {
            Cast_SpellCircle2A cast = new Cast_SpellCircle2A(position, attackRange, 15f, "fast");
            gm.casts.Add(cast);
        }
    }
    class HealSpell : SpellBase
    {
        public HealSpell(ArmyBase ab, Vector2 pos, Camera2D camera) : base(ab, pos, camera) { }
        public HealSpell()
        {
            id = "S03";
            name = "治疗药水";
            description = "范围施加治疗效果";
            attackRange = 100f;
            num = 0;
            tcolor = new Color(255, 161, 0, 155);
        }
        public override void WhenDie(GameMG gm)
        {
            Cast_SpellCircle2A cast = new Cast_SpellCircle2A(position, attackRange, 8f, "heal", 40f);
            gm.casts.Add(cast);
        }
    }
    class HealPSpell : SpellBase
    {
        public HealPSpell(ArmyBase ab, Vector2 pos, Camera2D camera) : base(ab, pos, camera) { }
        public HealPSpell()
        {
            id = "S04";
            name = "痊愈药水";
            description = "范围施加百分比治疗效果";
            attackRange = 45f;
            num = 0;
            tcolor = new Color(245, 203, 0, 155);
        }
        public override void WhenDie(GameMG gm)
        {
            Cast_SpellCircle2A cast = new Cast_SpellCircle2A(position, attackRange, 5f, "healP", 6f);
            gm.casts.Add(cast);
        }
    }
    class FrozenSpell : SpellBase
    {
        public FrozenSpell(ArmyBase ab, Vector2 pos, Camera2D camera) : base(ab, pos, camera) { }
        public FrozenSpell()
        {
            id = "S05";
            name = "冰冻药水";
            description = "冻结范围内的建筑";
            attackRange = 24f;
            num = 0;
            tcolor = new Color(102, 191, 255, 155);
        }
        public override void WhenDie(GameMG gm)
        {
            Cast_SpellCircle2C cast = new Cast_SpellCircle2C(gm, position, attackRange, 4.5f, "frozen");
            gm.casts.Add(cast);
        }
    }
    class WoundSpell : SpellBase
    {
        public WoundSpell(ArmyBase ab, Vector2 pos, Camera2D camera) : base(ab, pos, camera) { }
        public WoundSpell()
        {
            id = "S06";
            name = "裂伤药水";
            description = "根据损失生命值造成伤害";
            attackRange = 60f;
            num = 0;
            tcolor = new Color(50, 50, 50, 155);
        }
        public override void WhenDie(GameMG gm)
        {
            Cast_SpellCircle2C cast = new Cast_SpellCircle2C(gm, position, attackRange, 0.4f, "wound", 50f);
            gm.casts.Add(cast);
        }
    }
    class InterruptSpell : SpellBase
    {
        public InterruptSpell(ArmyBase ab, Vector2 pos, Camera2D camera) : base(ab, pos, camera) { }
        public InterruptSpell()
        {
            id = "S07";
            name = "干扰药水";
            description = "打断建筑攻击";
            attackRange = 100f;
            num = 0;
            tcolor = new Color(130, 41, 55, 155);
        }
        public override void WhenDie(GameMG gm)
        {
            Cast_SpellCircle2C cast = new Cast_SpellCircle2C(gm, position, attackRange, 0.2f, "interrupt");
            gm.casts.Add(cast);
        }
    }
    class FireCracker : SpellBase
    {
        public FireCracker(ArmyBase ab, Vector2 pos, Camera2D camera) : base(ab, pos, camera) { }
        public FireCracker()
        {
            id = "S66";
            name = "鞭炮";
            description = "对小范围建筑造成伤害\n新年快乐！";
            attackRange = 6.66f;
            num = 6;
            tcolor = Color.Red;
        }
        public override void WhenDie(GameMG gm)
        {
            Cast_SpellCircle2C cast = new Cast_SpellCircle2C(gm, position, attackRange, 0.45f, "firecracker", 36.6f);
            gm.casts.Add(cast);
        }
    }
}
