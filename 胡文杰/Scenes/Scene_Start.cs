namespace NoBadConflicts
{
    internal class Scene_Start : Scene
    {
        VCenterText tap2Next;
        public Scene_Start(GameMG gm) 
        {
            tap2Next = new VCenterText(gm.ChineseFont,"点击任意处开始游戏", 
                Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 7 * 6, 40, 0,Color.Black);
        }
        public override void Run(GameMG gm)
        {
            if(Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                gm.currentScene = new Scene_Explanation(gm);
            }

            Raylib.ClearBackground(Color.White);
            tap2Next.Draw();
        }
    }
}
