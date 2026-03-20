namespace NoBadConflicts
{
    internal class ButtonBase
    {
        public Vector2 pos;
        public Vector2 size;
        public Rectangle rect;
        public Color color;
        public virtual bool IsHangOn(GameMG gm)
        {
            if(gm.mousePos.X >= pos.X && gm.mousePos.X <= pos.X + size.X &&
               gm.mousePos.Y >= pos.Y && gm.mousePos.Y <= pos.Y + size.Y)
            {
                return true;
            }
            return false;
        }
        public virtual bool IsClick(GameMG gm)
        {
            if (IsHangOn(gm) && Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                return true;
            }
            return false;
        }
        public virtual void Draw(GameMG gm) { }
    }
}
