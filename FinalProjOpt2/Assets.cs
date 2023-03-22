using System;
using Raylib_cs;
using System.Numerics;

public class Global
{
    public static string currentscene = "start";
    public const int screenwidth = 1024;
    public const int screenheight = 768;
}

public class UI
{
    public Rectangle button = new((1024 / 2) - 100, (768 / 2) + 50, 200, 75);
    public Color buttoncolor = new(64, 64, 64, 255);
    public void StartButton(string level)
    {
        Vector2 mouse = Raylib.GetMousePosition();
        if (Raylib.CheckCollisionPointRec(mouse, button))
        {
            buttoncolor.r = (byte)40; buttoncolor.g = (byte)40; buttoncolor.b = (byte)40;
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            {
                Global.currentscene = level;
            }
        }
        else
        {
            buttoncolor.r = (byte)64; buttoncolor.g = (byte)64; buttoncolor.b = (byte)64;
        }
    }
}
