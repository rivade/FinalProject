using System;
using Raylib_cs;
using System.Numerics;

public class Global //Globala variabler som ska kunna användas i flera klasser utan att skapa en instans av klassen
{
    public static string currentscene = "start";
    public const int screenwidth = 1024;
    public const int screenheight = 768;
    public static string levelDied = "";
    public static Music music = Raylib.LoadMusicStream("Sounds/bgmusic.mp3");
    public static Sound jumpSound = Raylib.LoadSound("Sounds/jump.mp3");
    public static Sound deathSound = Raylib.LoadSound("Sounds/death.mp3");
    public static Sound coinSound = Raylib.LoadSound("Sounds/coin.mp3");
    public static Sound winSound = Raylib.LoadSound("Sounds/win.mp3");
    public static Sound cheer = Raylib.LoadSound("Sounds/cheer.mp3");
    public static Texture2D invCoinTexture = Raylib.LoadTexture("Textures/invcoin.png");
    public static Texture2D infoSign = Raylib.LoadTexture("Textures/infosign.png");
    public static Texture2D deathScreen = Raylib.LoadTexture("Textures/deathscreen.png"); 
    public static Texture2D winScreen = Raylib.LoadTexture("Textures/winscreen.png"); 
    public static void SoundInitialization()
    {
        Raylib.SetSoundVolume(jumpSound, 0.5f);
        Raylib.SetSoundVolume(coinSound, 0.5f);
        Raylib.SetSoundVolume(cheer, 0.6f);
    }
}

public class UI //Klass för startskärm som man interagerar med
{
    public Rectangle button = new((Global.screenwidth / 2) - 100, (Global.screenheight / 2) + 50, 200, 75); 
    public Color buttoncolor = new(64, 64, 64, 255);
    public Texture2D startBG = Raylib.LoadTexture("Textures/titlescreen.png");
    public void StartButton(string level) //Gör så att man kommer till level one när man trycker på knappen
    {
        Vector2 mouse = Raylib.GetMousePosition(); //Skapar en vektor med musens position
        if (Raylib.CheckCollisionPointRec(mouse, button))
        {
            buttoncolor.r = (byte)40; buttoncolor.g = (byte)40; buttoncolor.b = (byte)40; // Gör så att knappens färg blir mörkare när man hoverar med musen på knappen.
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            {
                Global.currentscene = level;
                Raylib.PlayMusicStream(Global.music);
            }
        }
        else
        {
            buttoncolor.r = (byte)64; buttoncolor.g = (byte)64; buttoncolor.b = (byte)64; //Återställer knappfärgen när musen slutar hovera på knappen
        }
    }
}

public class Reset
{
    public void ResetGame(Player p, Level levelOne, Level levelTwo, Level levelThree)
{
    if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
    {
        Raylib.StopMusicStream(Global.music);
        Global.currentscene = "start";
        p.rect.x = 0;
        p.rect.y = 100;
        p.hearts = 3;
        p.coins = 0;
        p.verticalVelocity = 0;
        levelOne.wonLevel = false;
        levelTwo.wonLevel = false;
        levelThree.wonLevel = false;
        levelOne.ResetCoins();
        levelTwo.ResetCoins();
        levelThree.ResetCoins();
        levelOne.alpha.a = 0;
    }
}
}