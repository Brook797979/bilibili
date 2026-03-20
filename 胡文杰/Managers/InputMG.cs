namespace NoBadConflicts
{
    internal class InputMG : ManagerBase<InputMG>
    {
        public Vector2 MousePos_lastClick = new Vector2(0, 0);
        static public bool mulClick = false;
        public double lastClickTime = 0;
        public Vector2 MousePos_lastStay = new Vector2(0, 0);
        public float mouseStayTime = 0;
        public float mouseDownTime = 0;
        public KeyboardKey lastPressKey = 0;
        public float keyDownTime = 0;

        public void Fresh(GameMG gm)
        {
            //鼠标点击
            if( Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                if(Vector2.Distance(gm.mousePos, MousePos_lastClick) <= 5f && Raylib.GetTime() - lastClickTime <= 0.25)
                {
                    mulClick = true;
                }
                else
                {
                    mulClick = false;
                }
                MousePos_lastClick = gm.mousePos;
                lastClickTime = Raylib.GetTime();
            }
            else
            {
                mulClick = false;
            }
            //鼠标按下
            if (Raylib.IsMouseButtonDown(MouseButton.Left))
            {
                mouseDownTime += gm.dt;
            }
            else
            {
                mouseDownTime = 0;
            }
            //鼠标停留时间
            if (Vector2.Distance(gm.mousePos, MousePos_lastStay) <= 5f)
            {
                mouseStayTime += gm.dt;
            }
            else
            {
                mouseStayTime = 0;
                MousePos_lastStay = gm.mousePos;
            }
            //按键按下时间
            if (Raylib.IsKeyDown(lastPressKey) && lastPressKey != 0)
            {
                keyDownTime += gm.dt;
            }
            else
            {
                foreach (KeyboardKey kk in Enum.GetValues<KeyboardKey>())
                {
                    if (Raylib.IsKeyDown(kk))
                    {
                        lastPressKey = kk;
                        keyDownTime = 0;
                        break;
                    }
                }
            }
        }
        public float GetMouseDownTime()
        {
            return mouseDownTime;
        }
        public float GetKeyDownTime(KeyboardKey kk)
        {
            if (kk == lastPressKey)
            {
                return keyDownTime;
            }
            else
            {
                return 0;
            }
        }
    }
}
