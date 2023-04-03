using Raylib_cs;
using System.Numerics;

Raylib.InitWindow(Global.screenwidth, Global.screenheight, "Jumpman!");
Raylib.InitAudioDevice();
Raylib.SetTargetFPS(60);
Raylib.SetMasterVolume(1);

Player p = new Player();
UI u = new UI();
Camera c = new Camera(p); //Skapar en instans av kameraklassen med instansen av Player som program.cs använder

Level levelOne = new Level(2, 1938, false, p, new Vector2[] //Skapar en instans av Level klassen med valdra presets för antal golv, etc.
{new Vector2(1030, 550), new Vector2(1130, 550), new Vector2(1230, 550)}); //Coin positioner

Level levelTwo = new Level(3, 1938, true, p, new Vector2[]
{new Vector2(2650, 550), new Vector2(2700, 550), new Vector2(2750, 550), new Vector2(2650, 500), new Vector2(2700, 500), new Vector2(2750, 500)});

Level levelThree = new Level(3, 2962, true, p, new Vector2[]
{ });

Obstacle obsOne = new Obstacle(new Vector2[] { });
Obstacle obsTwo = new Obstacle(new Vector2[] { new Vector2(400, 535), new Vector2(600, 535), new Vector2(800, 535), new Vector2(1200, 535), new Vector2(1250, 535), new Vector2(1300, 535), new Vector2(2200, 535), new Vector2(2250, 535), new Vector2(2300, 535), new Vector2(2450, 535), new Vector2(2500, 535), new Vector2(2550, 535), new Vector2(3022, 535), new Vector2(2972, 535), new Vector2(2922, 535), new Vector2(2872, 535), new Vector2(2822, 535) }); //Positioner för taggar i level 2
Obstacle obsThree = new Obstacle(new Vector2[] { new Vector2() });

levelOne.nextLevel = levelTwo; //Definerar vilken instans av level som ska användas när man klarar en level
levelTwo.nextLevel = levelThree;
obsOne.nextObstacle = obsTwo;
obsTwo.nextObstacle = obsThree;

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
            if (p.hearts > 0)
            {
                if (deathTimer > 0)
                {
                    deathTimer--;
                }
                if (deathTimer == 0)
                {
                    p.rect.x = 0;
                    p.rect.y = 100;
                    p.verticalVelocity = 0f;
                    Global.currentscene = levelDied;
                    deathTimer = 300;
                }
            }
            else
            {
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
                {
                    Global.currentscene = "start";
                    p.rect.x = 0;
                    p.rect.y = 100;
                    p.hearts = 3;
                    currentLevel = levelOne;
                    levelOne.wonLevel = false;
                    levelTwo.wonLevel = false;
                }
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

        case "levelThree":
            levelThree.alphaReset();
            c.CameraBounds();
            if (!levelThree.wonLevel) { p.lastleft = p.Movement(p.lastleft, currentLevel); }
            p.DeathCheck(currentObstacles);
            levelThree.CoinCollection();

            levelThree.NextLevel("levelFour");
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
            if (p.hearts > 0)
            {
                Raylib.DrawText("You died!", 300, 400, 40, Color.RED);
                Raylib.DrawText($"Respawning in: {deathTimer / 60 + 1}", 400, 500, 40, Color.RED);
            }
            else
            {
                Raylib.DrawText("You lost!", 300, 400, 40, Color.RED);
                Raylib.DrawText("Press enter to restart!", 400, 500, 40, Color.RED);
            }
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
            Raylib.DrawText("Level: 1", 25, 10, 30, Color.RED);
            Raylib.DrawTexture(invCoinTexture, 35, 50, Color.WHITE);
            Raylib.DrawText($": {p.coins}", 75, 50, 35, Color.BLACK);
            p.DrawHearts();
            break;

        case "levelTwo":
            Raylib.BeginMode2D(c.c);
            levelTwo.DrawTextures();
            obsTwo.DrawSpikes();
            Raylib.DrawText("Watch out for spikes!", 405, 375, 40, Color.BLACK);
            p.DrawCharacter(ref p.frame, ref p.elapsed, p.rect, p.lastleft);
            levelTwo.DrawCoin(ref levelTwo.frame, ref levelTwo.elapsed);
            Raylib.EndMode2D();
            Raylib.DrawTexture(infoSign, 0, 0, Color.WHITE);
            Raylib.DrawText("Level: 2", 25, 10, 30, Color.RED);
            Raylib.DrawTexture(invCoinTexture, 35, 50, Color.WHITE);
            Raylib.DrawText($": {p.coins}", 75, 50, 35, Color.BLACK);
            p.DrawHearts();
            break;

        case "levelThree":
            Raylib.BeginMode2D(c.c);
            levelThree.DrawTextures();
            obsThree.DrawSpikes();
            p.DrawCharacter(ref p.frame, ref p.elapsed, p.rect, p.lastleft);
            levelThree.DrawCoin(ref levelThree.frame, ref levelThree.elapsed);
            Raylib.EndMode2D();
            Raylib.DrawTexture(infoSign, 0, 0, Color.WHITE);
            Raylib.DrawText("Level: 3", 25, 10, 30, Color.RED);
            Raylib.DrawTexture(invCoinTexture, 35, 50, Color.WHITE);
            Raylib.DrawText($": {p.coins}", 75, 50, 35, Color.BLACK);
            p.DrawHearts();
            break;
    }
    Raylib.EndDrawing();
    Console.WriteLine($"x: {p.rect.x}, y: {p.rect.y}, {levelOne.wonLevel}, {levelTwo.wonLevel}, {levelDied}, {deathTimer}");
}