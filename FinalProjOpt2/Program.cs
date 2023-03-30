using Raylib_cs;
using System.Numerics;

Raylib.InitWindow(Global.screenwidth, Global.screenheight, "Jumpman!");
Raylib.SetTargetFPS(60);

Player p = new Player();
UI u = new UI();
Camera c = new Camera(p); //Skapar en instans av kameraklassen med instansen av Player som program.cs använder

Level levelOne = new Level(2, 1938, false, p, new Vector2[] //Skapar en instans av Level klassen med valdra presets för antal golv, etc.
{new Vector2(500, 500), new Vector2(600, 500)}); //Coin positioner

Level levelTwo = new Level(2, 1938, true, p, new Vector2[] 
{ });

Level levelThree = new Level(3, 1938, true, p, new Vector2[] 
{ });

Obstacle obsOne = new Obstacle(new Vector2[] { });
Obstacle obsTwo = new Obstacle(new Vector2[] { new Vector2(400, 535), new Vector2(600, 535), new Vector2(800, 535), new Vector2(1200, 535), new Vector2(1250, 535), new Vector2(1300, 535) }); //Positioner för taggar i level 2

levelOne.nextLevel = levelTwo; //Definerar vilken instans av level som ska användas när man klarar en level
levelTwo.nextLevel = levelThree;
obsOne.nextObstacle = obsTwo;

Level currentLevel = levelOne;
Obstacle currentObstacles = obsOne;

string levelDied = "";
int deathTimer = 300;
Texture2D invCoinTexture = Raylib.LoadTexture("Textures/invcoin.png");
Texture2D infoSign = Raylib.LoadTexture("Textures/infosign.png");

c.InitializeCamera(); //Initierar kamerainställningar i klassen camera

while (!Raylib.WindowShouldClose())
{
    //Logik
    if (Global.currentscene != "death" && Global.currentscene != "start")
    {
        levelDied = Global.currentscene; //Avgör var man ska respawna ifall man dör
    }

    switch (Global.currentscene)
    {
        case "start":
            u.StartButton("levelOne");
            break;

        case "death":
            if (deathTimer > 0)
            {
                deathTimer--;
            }
            if (deathTimer == 0)
            {
                p.rect.x = 0;
                p.rect.y = 0;
                p.verticalVelocity = 0f;
                Global.currentscene = levelDied;
                deathTimer = 300;
            }
            break;

        case "levelOne":
            c.CameraBounds();
            if (!levelOne.wonLevel) { p.lastleft = p.Movement(p.lastleft, currentLevel); } //Gör så att man kan röra sig ifall man inte klarat leveln
            p.DeathCheck(currentObstacles); //Kollar ifall spelaren dött
            levelOne.CoinCollection(); //Kollar ifall spelaren plockar upp en coin

            levelOne.NextLevel("levelTwo"); //Definerar vilken level som currentscene ska uppdateras till när man vinner
            if (levelOne.wonLevel)
            {
                currentLevel = levelOne.nextLevel;
                currentObstacles = obsOne.nextObstacle;
            }
            break;

        case "levelTwo":
            levelTwo.alphaReset(); //Återställer alfavärdet på den svarta skärmen vilket gör att skärmen fadear tillbaka från svart
            c.CameraBounds();
            if (!levelTwo.wonLevel) { p.lastleft = p.Movement(p.lastleft, currentLevel); }
            p.DeathCheck(currentObstacles);
            levelTwo.CoinCollection();

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
            Raylib.DrawText($"Respawning in: {deathTimer / 60 + 1}", 400, 500, 40, Color.RED);
            break;

        case "levelOne":
            Raylib.BeginMode2D(c.c);
            levelOne.DrawTextures(); //Ritar ut golv och bakgrund
            Raylib.DrawText("Welcome to Jumpman!", 30, 375, 40, Color.BLACK);
            Raylib.DrawText("Get to the gate at the end of the level to win", 700, 375, 40, Color.BLACK);
            Raylib.DrawText("Press enter when at", 2100, 400, 40, Color.GREEN);
            Raylib.DrawText("the gate to continue", 2100, 450, 40, Color.GREEN);
            Raylib.DrawText("Don't fall down!", 2100, 600, 40, Color.GREEN);
            p.DrawCharacter(ref p.frame, ref p.elapsed, p.rect, p.lastleft); //Ritar ut spelaren
            levelOne.DrawCoin(ref levelOne.frame, ref levelOne.elapsed); //Ritar ut coins
            Raylib.EndMode2D();
            Raylib.DrawTexture(infoSign, 0, 0, Color.WHITE);
            Raylib.DrawText("Level: 1", 30, 10, 30, Color.RED);
            Raylib.DrawTexture(invCoinTexture, 35, 50, Color.WHITE);
            Raylib.DrawText($": {p.coins}", 75, 50, 35, Color.BLACK);
            Raylib.DrawFPS(950, 5);
            break;

        case "levelTwo":
            Raylib.BeginMode2D(c.c);
            levelTwo.DrawTextures();
            obsTwo.DrawSpikes();
            Raylib.DrawText("Watch out for spikes!", 405, 375, 40, Color.BLACK);
            p.DrawCharacter(ref p.frame, ref p.elapsed, p.rect, p.lastleft);
            levelTwo.DrawCoin(ref levelOne.frame, ref levelOne.elapsed);
            Raylib.EndMode2D();
            Raylib.DrawTexture(infoSign, 0, 0, Color.WHITE);
            Raylib.DrawText("Level: 2", 30, 10, 30, Color.RED);
            Raylib.DrawTexture(invCoinTexture, 35, 50, Color.WHITE);
            Raylib.DrawText($": {p.coins}", 75, 50, 35, Color.BLACK);
            Raylib.DrawFPS(950, 5);
            break;
    }
    Raylib.EndDrawing();
    Console.WriteLine($"x: {p.rect.x}, y: {p.rect.y}, {levelOne.wonLevel}, {levelTwo.wonLevel}, {levelDied}, {deathTimer}");
}