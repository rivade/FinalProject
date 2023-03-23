using System;
using Raylib_cs;
using System.Numerics;

public class Player
{
    public List<Texture2D> sprites = new();
    public Rectangle rect = new Rectangle(0, 0, 50, 75);
    public bool collidesWithFloor = false;
    public bool lastleft = false;
    public float verticalVelocity = 0f;
    public Player()
    {
        sprites.Add(Raylib.LoadTexture("Sprites/character.png"));
        sprites.Add(Raylib.LoadTexture("Sprites/running.png"));
        sprites.Add(Raylib.LoadTexture("Sprites/air.png"));
    }

    public bool Movement(bool lastleft, Level currentLevel)
    {
        if (Raylib.IsKeyDown(KeyboardKey.KEY_A) && rect.x > 0)
        {
            rect.x -= 5;
            lastleft = true;
        }
        if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
        {
            rect.x += 5;
            lastleft = false;
        }

        collidesWithFloor = false;
        foreach (Rectangle f in currentLevel.floors)
        {
            if (Raylib.CheckCollisionRecs(rect, f))
            {
                collidesWithFloor = true;
                if (rect.y + 75 > 600)
                {
                    rect.y = 526;
                }

                if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
                {
                    verticalVelocity = 10f;
                }
                else
                {
                    verticalVelocity = 0f;
                }
            }
        }
        if (!collidesWithFloor && verticalVelocity > -15)
        {
            verticalVelocity -= 0.3f;
        }
        rect.y -= (int)verticalVelocity;

        return lastleft;
    }

    public int RunningLogic(ref int frame, ref float elapsed)
    {
        const float frameDuration = 0.07f;
        elapsed += Raylib.GetFrameTime();
        if (elapsed >= frameDuration)
        {
            frame++;
            elapsed -= frameDuration;
        }
        frame %= 12;
        return frame;
    }

    public void DrawCharacter(ref int frame, ref float timer, Rectangle player, bool lastLeft)
    {
        Rectangle sourcerec = new Rectangle();
        if (Raylib.IsKeyDown(KeyboardKey.KEY_D) && collidesWithFloor)
        {
            frame = RunningLogic(ref frame, ref timer);
            sourcerec = new Rectangle(50 * frame, 0, 50, 75);
            Raylib.DrawTextureRec(sprites[1], sourcerec, new Vector2(player.x, player.y), Color.WHITE);
            lastLeft = false;
        }
        else if (Raylib.IsKeyDown(KeyboardKey.KEY_A) && collidesWithFloor)
        {
            frame = RunningLogic(ref frame, ref timer);
            sourcerec = new Rectangle(50 * frame, 0, -50, 75);
            Raylib.DrawTextureRec(sprites[1], sourcerec, new Vector2(player.x, player.y), Color.WHITE);
            lastLeft = true;
        }
        else
        {
            if (lastLeft)
            {
                sourcerec = new Rectangle(0, 0, -50, 75);
                if (collidesWithFloor)
                {
                    Raylib.DrawTextureRec(sprites[0], sourcerec, new Vector2(player.x, player.y), Color.WHITE);
                }
                else
                {
                    Raylib.DrawTextureRec(sprites[2], sourcerec, new Vector2(player.x, player.y), Color.WHITE);
                }
            }
            else
            {
                sourcerec = new Rectangle(0, 0, 50, 75);
                if (collidesWithFloor)
                {
                    Raylib.DrawTextureRec(sprites[0], sourcerec, new Vector2(player.x, player.y), Color.WHITE);
                }
                else
                {
                    Raylib.DrawTextureRec(sprites[2], sourcerec, new Vector2(player.x, player.y), Color.WHITE);
                }
            }
        }
    }

    public void DeathCheck(){
        if (rect.y > Global.screenheight){
            Global.currentscene = "death";
        }
    }
}
