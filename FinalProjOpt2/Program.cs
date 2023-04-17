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
{ new Vector2(410, 450), new Vector2(460, 450), new Vector2(510, 450), new Vector2(560, 450), new Vector2(710, 450), new Vector2(760, 450), new Vector2(810, 450), new Vector2(860, 450), new Vector2(1010, 450), new Vector2(1060, 450), new Vector2(1110, 450), new Vector2(1160, 450), new Vector2(1310, 450), new Vector2(1360, 450), new Vector2(1410, 450), new Vector2(1460, 450) });

Obstacle obsOne = new Obstacle(new Vector2[] { }, new Vector2[] { });
Obstacle obsTwo = new Obstacle(new Vector2[] { new Vector2(400, 535), new Vector2(600, 535), new Vector2(800, 535), new Vector2(1200, 535), new Vector2(1250, 535), new Vector2(1300, 535), new Vector2(2200, 535), new Vector2(2250, 535), new Vector2(2300, 535), new Vector2(2450, 535), new Vector2(2500, 535), new Vector2(2550, 535), new Vector2(3022, 535), new Vector2(2972, 535), new Vector2(2922, 535), new Vector2(2872, 535), new Vector2(2822, 535) }, new Vector2[] { }); //Positioner för taggar i level 2
Obstacle obsThree = new Obstacle(new Vector2[] { new Vector2(400, 535), new Vector2(450, 535), new Vector2(500, 535), new Vector2(550, 535), new Vector2(700, 535), new Vector2(750, 535), new Vector2(800, 535), new Vector2(850, 535), new Vector2(1000, 535), new Vector2(1050, 535), new Vector2(1100, 535), new Vector2(1150, 535), new Vector2(1300, 535), new Vector2(1350, 535), new Vector2(1400, 535), new Vector2(1450, 535) },
new Vector2[] { new Vector2(1700, 540), new Vector2(1950, 540), new Vector2(2200, 540) }); //Enemy positioner

levelOne.nextLevel = levelTwo; //Definerar vilken instans av level som ska användas när man klarar en level
levelTwo.nextLevel = levelThree;
obsOne.nextObstacle = obsTwo; //Definerar vilken instans av obstacle som ska användas när man klarar en level
obsTwo.nextObstacle = obsThree;

c.InitializeCamera(); //Initierar kamerainställningar i klassen camera
Global.SoundInitialization();
Reset r = new Reset();
Death d = new Death();

while (!Raylib.WindowShouldClose())
{
    //Logik
    Raylib.UpdateMusicStream(Global.music);
    if (Global.currentscene != "death" && Global.currentscene != "start" && Global.currentscene != "win")
    {
        Global.levelDied = Global.currentscene; //Avgör var man ska respawna ifall man dör
    }

    switch (Global.currentscene)
    {
        case "start":
            u.StartButton("levelOne");
            break;

        case "death":
            Raylib.PauseMusicStream(Global.music);
            d.DeathHandler(p, r, levelOne, levelTwo, levelThree);
            break;
        
        case "win":
            Raylib.PauseMusicStream(Global.music);
            r.ResetGame(p, levelOne, levelTwo, levelThree);
            break;

        case "levelOne":
            c.CameraBounds();
            if (!levelOne.wonLevel) { p.lastleft = p.Movement(p.lastleft, levelOne); } //Gör så att man kan röra sig ifall man inte klarat leveln
            p.DeathCheck(obsOne); //Kollar ifall spelaren dött
            levelOne.CoinCollection(); //Kollar ifall spelaren plockar upp en coin

            levelOne.NextLevel("levelTwo"); //Definerar vilken level som currentscene ska uppdateras till när man vinner
            break;

        case "levelTwo":
            levelTwo.alphaReset();
            c.CameraBounds();
            if (!levelTwo.wonLevel && (levelTwo.alpha.a < 20)) { p.lastleft = p.Movement(p.lastleft, levelTwo); }
            p.DeathCheck(obsTwo);
            levelTwo.CoinCollection();

            levelTwo.NextLevel("levelThree");
            break;

        case "levelThree":
            levelThree.alphaReset();
            c.CameraBounds();
            if (!levelThree.wonLevel  && (levelThree.alpha.a < 20)) { p.lastleft = p.Movement(p.lastleft, levelThree); }
            obsThree.MoveEnemy(); //Flyttar på slimesen i bana 3
            p.DeathCheck(obsThree);
            levelThree.CoinCollection();

            levelThree.NextLevel("win");
            break;
    }

    //Grafik
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.WHITE);
    switch (Global.currentscene)
    {
        case "start":
            Raylib.DrawTexture(u.startBG, 0, 0, Color.WHITE);
            Raylib.DrawText("Jumpman!", 350, 250, 75, Color.RED);
            Raylib.DrawRectangleRec(u.button, u.buttoncolor);
            Raylib.DrawText("START", 442, 453, 40, Color.RED);
            break;

        case "death":
            Raylib.DrawTexture(Global.deathScreen, 0, 0, Color.WHITE);
            if (p.hearts > 0)
            {
                Raylib.DrawText("You died!", 600, 400, 40, Color.WHITE);
                Raylib.DrawText($"Respawning in: {d.deathTimer / 60 + 1}", 600, 500, 40, Color.WHITE);
            }
            else
            {
                Raylib.DrawText("You lost!", 600, 400, 40, Color.WHITE);
                Raylib.DrawText("Press enter to restart!", 600, 500, 30, Color.WHITE);
            }
            break;
        
        case "win":
            Raylib.DrawTexture(Global.winScreen, 0, 0, Color.WHITE);
            Raylib.DrawText("Congratulations, you won!", 450, 100, 30, Color.BLACK);
            Raylib.DrawText($"You finished with {p.hearts} lives remaining", 450, 150, 30, Color.BLACK);
            Raylib.DrawText($"You collected {p.coins} coins.", 450, 200, 30, Color.BLACK);
            if (p.coins == 25)
            {
                Raylib.DrawText("Thats all of them, good job!", 450, 250, 30, Color.BLACK);
            }
            Raylib.DrawText("Press enter to restart", 425, 450, 40, Color.BLACK);
            Raylib.DrawText("Press ESC to exit", 425, 500, 40, Color.BLACK);

            break;

        case "levelOne":
            Raylib.BeginMode2D(c.c);
            levelOne.DrawTextures(); //Ritar ut golv och bakgrund
            Raylib.DrawText("Welcome to Jumpman!", 30, 375, 40, Color.BLACK);
            Raylib.DrawText("Move using W/D/SPACE or the Arrow Keys", 30, 200, 40, Color.BLACK);
            Raylib.DrawText("Get to the gate at the end of the level to win", 700, 375, 40, Color.BLACK);
            Raylib.DrawText("Press enter when at", 2100, 400, 40, Color.GREEN);
            Raylib.DrawText("the gate to continue", 2100, 450, 40, Color.GREEN);
            Raylib.DrawText("Don't fall down!", 2100, 600, 40, Color.GREEN);
            p.DrawCharacter(ref p.frame, ref p.elapsed, p.rect, p.lastleft); //Ritar ut spelaren
            levelOne.DrawCoin(ref levelOne.frame, ref levelOne.elapsed); //Ritar ut coins
            Raylib.EndMode2D();
            Raylib.DrawTexture(Global.infoSign, 0, 0, Color.WHITE);
            Raylib.DrawText("Level: 1", 25, 10, 30, Color.RED);
            Raylib.DrawTexture(Global.invCoinTexture, 35, 50, Color.WHITE);
            Raylib.DrawText($": {p.coins}", 75, 50, 35, Color.BLACK);
            p.DrawHearts();
            Raylib.DrawTexture(levelOne.backgrounds[2], 0, 0, levelOne.alpha);
            break;

        case "levelTwo":
            Raylib.BeginMode2D(c.c);
            levelTwo.DrawTextures();
            obsTwo.DrawObstacles();
            Raylib.DrawText("Watch out for spikes!", 405, 375, 40, Color.BLACK);
            p.DrawCharacter(ref p.frame, ref p.elapsed, p.rect, p.lastleft);
            levelTwo.DrawCoin(ref levelTwo.frame, ref levelTwo.elapsed);
            Raylib.EndMode2D();
            Raylib.DrawTexture(Global.infoSign, 0, 0, Color.WHITE);
            Raylib.DrawText("Level: 2", 25, 10, 30, Color.RED);
            Raylib.DrawTexture(Global.invCoinTexture, 35, 50, Color.WHITE);
            Raylib.DrawText($": {p.coins}", 75, 50, 35, Color.BLACK);
            p.DrawHearts();
            Raylib.DrawTexture(levelTwo.backgrounds[2], 0, 0, levelTwo.alpha);
            break;

        case "levelThree":
            Raylib.BeginMode2D(c.c);
            levelThree.DrawTextures();
            obsThree.DrawObstacles();
            Raylib.DrawText("Watch out for slimes!", 1700, 375, 40, Color.BLACK);
            p.DrawCharacter(ref p.frame, ref p.elapsed, p.rect, p.lastleft);
            levelThree.DrawCoin(ref levelThree.frame, ref levelThree.elapsed);
            Raylib.EndMode2D();
            Raylib.DrawTexture(Global.infoSign, 0, 0, Color.WHITE);
            Raylib.DrawText("Level: 3", 25, 10, 30, Color.RED);
            Raylib.DrawTexture(Global.invCoinTexture, 35, 50, Color.WHITE);
            Raylib.DrawText($": {p.coins}", 75, 50, 35, Color.BLACK);
            p.DrawHearts();
            Raylib.DrawTexture(levelThree.backgrounds[2], 0, 0, levelThree.alpha);
            break;
    }
    Raylib.EndDrawing();
}