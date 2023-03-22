using System;
using Raylib_cs;
using System.Numerics;

public class Level
{
    public List<Rectangle> floors = new();
    public List<Texture2D> backgrounds = new();
    public List<Texture2D> floorTextures = new();
    public int floorCount;
    public Level(int count)
    {
        for (var i = 0; i < count; i++)
        {
            floors.Add(new Rectangle(i * 1100, 600, Global.screenwidth, 1));
        }
        backgrounds.Add(Raylib.LoadTexture("Textures/titlescreen.png"));
        backgrounds.Add(Raylib.LoadTexture("Textures/Wall.png"));
        floorTextures.Add(Raylib.LoadTexture("Textures/floor.png"));
    }
    public void DrawTextures()
    {
        for (var i = 0; i < (floors.Count + 1); i++)
        {
            Raylib.DrawTexture(backgrounds[1], (i * 1024), 0, Color.WHITE);
        }
        for (var i = 0; i < floors.Count; i++)
        {
            Raylib.DrawTexture(floorTextures[0], (int)floors[i].x, (int)floors[i].y, Color.WHITE);
        }
    }
}