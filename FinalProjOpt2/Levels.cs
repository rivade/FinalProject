using System;
using Raylib_cs;
using System.Numerics;

public class Level
{

    public Level nextLevel; //Används för att den ska veta vilken level som är näst (Defineras när instans av level skapas)
    Player p; //Samma som i kameraklassen
    public Color alpha = new(0, 0, 0, 0); //Används för svart skärm som fadear vid levelbyte

    public List<Rectangle> floors = new();
    public List<Texture2D> backgrounds = new();
    public List<Texture2D> floorTextures = new();
    public List<Texture2D> assetTextures = new();
    public List<Rectangle> coins = new();
    public Rectangle gate;
    public Texture2D coinTexture = Raylib.LoadTexture("Textures/coin.png");
    public int frame = 1;
    public float elapsed = 0;
    public bool wonLevel = false;
    public Level(int count, int gateX, bool resetAlpha, Player pExtern, Vector2[] coinpos)
    {
        Rectangle gatecreator = new(gateX, 490, 110, 110); //Skapar en gate rektangel där man anger vart gaten ska ligga (konstant y, men x bestäms när instansen av klassen skapas)
        for (var i = 0; i < count; i++)
        {
            floors.Add(new Rectangle(i * 1024, 600, Global.screenwidth, 1)); //Lägger till en golvrektangel för antal golv man anger när man skapar instans
        }
        for (var i = 0; i < coinpos.Length; i++)
        {
            coins.Add(new Rectangle(coinpos[i].X, coinpos[i].Y, 30, 30)); //Lägger till en coinrektangel med position man anger för varje coin man vill skapa
        }
        backgrounds.Add(Raylib.LoadTexture("Textures/titlescreen.png"));
        backgrounds.Add(Raylib.LoadTexture("Textures/Wall.png"));
        backgrounds.Add(Raylib.LoadTexture("Textures/black.png"));
        floorTextures.Add(Raylib.LoadTexture("Textures/floor.png"));
        assetTextures.Add(Raylib.LoadTexture("Textures/gate.png"));
        gate = gatecreator; //Gör så att klassens gate rektangel får de värden man anger
        p = pExtern;
        if (resetAlpha) //Gör så att den återställer alphavärdet på svarta skärmen ifall banan har bytts.
        {
            alpha.a = 254;
            resetAlpha = false;
        }
    }
    public void DrawTextures() //Ritar ut bakgrunder och golv
    {
        for (var i = 0; i < 20; i++)
        {
            Raylib.DrawTexture(backgrounds[1], (i * 384), 0, Color.WHITE);
            Raylib.DrawTexture(backgrounds[1], (i * 384), 384, Color.WHITE);
        }
        for (var i = 0; i < floors.Count; i++)
        {
            Raylib.DrawTexture(floorTextures[0], (int)floors[i].x, (int)floors[i].y, Color.WHITE);
        }
        Raylib.DrawTexture(assetTextures[0], (int)gate.x, (int)gate.y, Color.WHITE);
        for (var i = 0; i < 20; i++)
        {
            Raylib.DrawTexture(backgrounds[2], (i * 1024), 0, alpha);
        }
    }

    public void NextLevel(string next) //Ändrar alpha på svarta skärmen och gör så att man går vidare till nästa level när man trycker på enter vid gaten
    {
        if (Raylib.CheckCollisionRecs(p.rect, gate) && Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
        {
            wonLevel = true;
        }
        if (wonLevel)
        {
            if (alpha.a < 255)
            {
                alpha.a += 2;
            }
            if (alpha.a == 254)
            {
                p.rect.x = 265;
                p.rect.y = 526;
                Global.currentscene = next;
            }
        }
    }
    public void alphaReset() //Återställer sakta alphavärdet till 0 på svarta skärmen vilket gör den osynlig igen
    {
        if (alpha.a > 0 && !wonLevel)
        {
            alpha.a -= 2;
        }
    }

    public void CoinCollection() //Kollar ifall spelaren plockar upp en coin
    {
        for (var i = 0; i < coins.Count; i++)
        {
            if (Raylib.CheckCollisionRecs(p.rect, coins[i]))
            {
                p.coins++; //Lägger till en coin till hur många spelaren plockat upp
                coins.Remove(coins[i]); //Tar bort coinen från leveln så att man ej kan plocka upp samma coin flera gånger
            }
        }
    }

    public int CoinFrameLogic(ref int frame, ref float elapsed) //Samma som när spelarens running animation ska ritas bara med längre frame duration
    {
        const float frameDuration = 0.12f;
        elapsed += Raylib.GetFrameTime();
        if (elapsed >= frameDuration)
        {
            frame++;
            elapsed -= frameDuration;
        }
        frame %= 8;
        return frame;
    }

    public void DrawCoin(ref int frame, ref float timer)
    {
        frame = CoinFrameLogic(ref frame, ref timer);
        Rectangle sourcerec = new Rectangle(30 * frame, 0, 30, 30);
        foreach (var coin in coins)
        {
            Raylib.DrawTextureRec(coinTexture, sourcerec, new Vector2(coin.x, coin.y), Color.WHITE);
        }
    }
}