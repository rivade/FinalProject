using System;
using Raylib_cs;
using System.Numerics;

public class Camera
{
    Player p;
    public Camera(Player pExtern)
    {
        p = pExtern;
    }
    public Camera2D c = new();
    public void InitializeCamera()
    {
        c.zoom = 1;
        c.rotation = 0;
        c.offset = new Vector2(Global.screenwidth / 2, Global.screenheight / 2);
    }
    public void CameraBounds()
    {
        if (p.rect.x >= 265 && p.rect.x <= 3000)
        {
            c.target = new Vector2((p.rect.x + 250), (Global.screenheight / 2));
        }
        else
        {
            c.target = new Vector2((Global.screenwidth / 2), (Global.screenheight / 2));
        }
    }
}