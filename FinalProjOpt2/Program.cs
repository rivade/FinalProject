using Raylib_cs;
using System.Numerics;

Raylib.InitWindow(Global.screenwidth, Global.screenheight, "Jumpman!");
Raylib.SetTargetFPS(60);

Player p = new Player();
UI u = new UI();
Camera c = new Camera(p);

Level levelOne = new Level(2, 1938, false, p);
Level levelTwo = new Level(5, 1938, true, p);
Level levelThree = new Level(2, 1938, true, p);

Obstacle obsOne = new Obstacle(400, 500, 600 , 5000, 5000, 5000);
Obstacle obsTwo = new Obstacle(400, 600, 800, 535, 535, 535);

levelOne.nextLevel = levelTwo;
levelTwo.nextLevel = levelThree;
obsOne.nextObstacle = obsTwo;

Level currentLevel = levelOne;
Obstacle currentObstacles = obsOne;

string levelDied = "";
int deathTimer = 300;

c.InitializeCamera();

while (!Raylib.WindowShouldClose())
{
    //Logik
    if (Global.currentscene != "death" && Global.currentscene != "start")
    {
        levelDied = Global.currentscene;
    }

    switch (Global.currentscene)
    {
        case "start":
            u.StartButton("levelOne");
            break;

        case "death":
            if (deathTimer > 0){
                deathTimer--;
            }
            if (deathTimer == 0){
                p.rect.x = 0;
                p.rect.y = 0;
                p.verticalVelocity = 0f;
                Global.currentscene = levelDied;
                deathTimer = 300;
            }
            break;

        case "levelOne":
            c.CameraBounds();
            if (!levelOne.wonLevel) { p.lastleft = p.Movement(p.lastleft, currentLevel); }
            p.DeathCheck(currentObstacles);

            levelOne.NextLevel("levelTwo");
            if (levelOne.wonLevel)
            {
                currentLevel = levelOne.nextLevel;
                currentObstacles = obsOne.nextObstacle;
            }
            break;

        case "levelTwo":
            levelTwo.alphaReset();
            c.CameraBounds();
            if (!levelTwo.wonLevel) { p.lastleft = p.Movement(p.lastleft, currentLevel); }
            p.DeathCheck(currentObstacles);

            levelTwo.NextLevel("levelThree");
            if (levelTwo.wonLevel)
            {
                currentLevel = levelTwo.nextLevel;
                currentObstacles = obsTwo.nextObstacle;
            }
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

        case "death":
            Raylib.DrawText("You died!", 300, 400, 40, Color.RED);
            Raylib.DrawText($"Respawning in: {deathTimer/60 + 1}", 400, 500, 40, Color.RED);
            break;

        case "levelOne":
            Raylib.BeginMode2D(c.c);
            levelOne.DrawTextures();
            obsOne.DrawSpikes();
            Raylib.DrawText("Welcome to Jumpman!", 30, 375, 40, Color.BLACK);
            Raylib.DrawText("Get to the gate at the end of the level to win", 700, 375, 40, Color.BLACK);
            Raylib.DrawText("Press enter when at", 2100, 400, 40, Color.GREEN);
            Raylib.DrawText("the gate to continue", 2100, 450, 40, Color.GREEN);
            Raylib.DrawText("Don't fall down!", 2100, 600, 40, Color.GREEN);
            p.DrawCharacter(ref p.frame, ref p.elapsed, p.rect, p.lastleft);
            Raylib.EndMode2D();
            Raylib.DrawText("Level: 1", 5, 5, 30, Color.RED);
            Raylib.DrawFPS(950, 5);
            break;

        case "levelTwo":
            Raylib.BeginMode2D(c.c);
            levelTwo.DrawTextures();
            obsTwo.DrawSpikes();
            Raylib.DrawText("Watch out for spikes!", 405, 375, 40, Color.BLACK);
            p.DrawCharacter(ref p.frame, ref p.elapsed, p.rect, p.lastleft);
            Raylib.EndMode2D();
            Raylib.DrawText("Level: 2", 5, 5, 30, Color.RED);
            Raylib.DrawFPS(950, 5);
            break;
    }
    Raylib.EndDrawing();
    Console.WriteLine($"x: {p.rect.x}, y: {p.rect.y}, {levelOne.wonLevel}, {levelTwo.wonLevel}, {levelDied}, {deathTimer}");
}