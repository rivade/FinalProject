using System;
using Raylib_cs;
using System.Numerics;

public class Level
{

    public Level nextLevel;
    Player p;
    public Color alpha = new(0, 0, 0, 0);

    public List<Rectangle> floors = new();
    public List<Texture2D> backgrounds = new();
    public List<Texture2D> floorTextures = new();
    public List<Texture2D> assetTextures = new();
    public Rectangle gate;
    public bool wonLevel = false;
    public Level(int count, int gateX, bool resetAlpha, Player pExtern)
    {
        Rectangle gatecreator = new(gateX, 490, 110, 110);
        for (var i = 0; i < count; i++)
        {
            floors.Add(new Rectangle(i * 1024, 600, Global.screenwidth, 1));
        }
        backgrounds.Add(Raylib.LoadTexture("Textures/titlescreen.png"));
        backgrounds.Add(Raylib.LoadTexture("Textures/Wall.png"));
        backgrounds.Add(Raylib.LoadTexture("Textures/black.png"));
        floorTextures.Add(Raylib.LoadTexture("Textures/floor.png"));
        assetTextures.Add(Raylib.LoadTexture("Textures/gate.png"));
        gate = gatecreator;
        p = pExtern;
        if (resetAlpha)
        {
            alpha.a = 254;
            resetAlpha = false;
        }
    }
    public void DrawTextures()
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

    public void NextLevel(string next)
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
    public void alphaReset()
    {
        if (alpha.a > 0 && !wonLevel)
        {
            alpha.a -= 2;
        }
    }
}