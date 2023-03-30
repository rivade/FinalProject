using System;
using Raylib_cs;
using System.Numerics;

public class Global //Globala variabler som ska kunna användas i flera klasser utan att skapa en instans av klassen
{
    public static string currentscene = "start";
    public const int screenwidth = 1024;
    public const int screenheight = 768;
}

public class UI //Klass för startskärm som man interagerar med
{
    public Rectangle button = new((Global.screenwidth / 2) - 100, (Global.screenheight / 2) + 50, 200, 75); 
    public Color buttoncolor = new(64, 64, 64, 255);
    public void StartButton(string level) //Gör så att man kommer till level one när man trycker på knappen
    {
        Vector2 mouse = Raylib.GetMousePosition(); //Skapar en vektor med musens position
        if (Raylib.CheckCollisionPointRec(mouse, button))
        {
            buttoncolor.r = (byte)40; buttoncolor.g = (byte)40; buttoncolor.b = (byte)40; // Gör så att knappens färg blir mörkare när man hoverar med musen på knappen.
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            {
                Global.currentscene = level;
            }
        }
        else
        {
            buttoncolor.r = (byte)64; buttoncolor.g = (byte)64; buttoncolor.b = (byte)64; //Återställer knappfärgen när musen slutar hovera på knappen
        }
    }
}
