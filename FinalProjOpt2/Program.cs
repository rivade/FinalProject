using Raylib_cs;
using System.Numerics;

Raylib.InitWindow(Global.screenwidth, Global.screenheight, "Jumpman!");
Raylib.SetTargetFPS(60);

Level levelOne = new Level(2);
Player p = new Player();
UI u = new UI();
Camera c = new Camera(p);

int frame = 1;
float elapsed = 0;

c.InitializeCamera();

while (!Raylib.WindowShouldClose())
{
    //Logik
    switch (Global.currentscene)
    {
        case "start":
            u.StartButton("levelOne");
            break;

        case "levelOne":
            c.CameraBounds();
            p.lastleft = p.Movement(p.lastleft);
            break;
    }

    //Grafik
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.WHITE);
    switch (Global.currentscene)
    {
        case "start":
            Raylib.DrawText("Jumpman!", 350, 320, 75, Color.RED);
            Raylib.DrawRectangleRec(u.button, u.buttoncolor);
            Raylib.DrawText("START", 442, 453, 40, Color.RED);
            break;

        case "levelOne":
            Raylib.BeginMode2D(c.c);
            levelOne.DrawTextures();
            p.DrawCharacter(ref frame, ref elapsed, p.rect, p.lastleft);
            Raylib.EndMode2D();
            break;
    }
    Raylib.EndDrawing();
    Console.WriteLine($"{p.collidesWithFloor}, {p.verticalVelocity}");
}