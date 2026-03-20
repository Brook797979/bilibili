global using Raylib_cs;
global using System;
global using System.Numerics;
global using System.Linq;
global using System.IO;
global using System.Text.Json;
global using System.Text.Json.Serialization;

namespace NoBadConflicts
{

    internal class Program
    {
        static double lastFrameRenewal = 0;
        static int frameCount = 0;
        static int frame = 0;
        static void Main()
        {
            GameMG gm = GameMG.Instance;
            gm.currentScene = new Scene_Start(gm);
            
            #region 游戏主循环
            while (!Raylib.WindowShouldClose())
            {
                gm.Fresh();
                //if (Raylib.IsMouseButtonPressed(MouseButton.Left)) Console.WriteLine(gm.mousePos);
                Raylib.BeginDrawing();
                gm.currentScene.Run(gm);

                if(Raylib.GetTime() - lastFrameRenewal >= 1.0)
                {
                    lastFrameRenewal = Raylib.GetTime();
                    frame = frameCount;
                    frameCount = 0;
                }
                frameCount++;
                Raylib.DrawRectangle(0, 0, 30, 12, Color.Black);
                Raylib.DrawText(frame.ToString(), 0, 0, 12, Color.White);
                Raylib.EndDrawing();
            }
            #endregion

            Raylib.CloseAudioDevice();
            Raylib.CloseWindow();
        }
    }
}
