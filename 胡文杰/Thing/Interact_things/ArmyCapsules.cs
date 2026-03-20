namespace NoBadConflicts
{
    class ArmyCapsule
    {
        public Rectangle rec;
        ArmyBase army;
        VCenterText text;
        public ArmyCapsule(GameMG gm, Rectangle rec, ArmyBase army)
        {
            this.rec = rec;
            this.army = army;
            text = new VCenterText(gm.ChineseFont, army.name,
                (int)(rec.Position.X + rec.Size.X / 2), (int)(rec.Position.Y + rec.Size.Y / 2), 36, 0, Color.White);
        }
        public void Act(GameMG gm, bool bechoosed)
        {
            if (bechoosed)
            {
                Vector2 pos = new Vector2(rec.Position.X + rec.Size.X / 2, rec.Position.Y + rec.Size.Y / 2);
                float r = rec.Size.Y * 1.5f;
                Raylib.DrawCircleV(pos, r, army.tcolor);
            }
            Raylib.DrawRectangleRec(rec, army.tcolor);
            Raylib.DrawRectangleLinesEx(rec, 1f, Color.Black);
            text.Draw();
        }
    }
}
