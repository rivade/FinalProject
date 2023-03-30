using System;
using Raylib_cs;
using System.Numerics;

public class Obstacle
{
    public Obstacle nextObstacle;
    public List<Rectangle> spikes = new();
    public Texture2D spikeTexture = Raylib.LoadTexture("Textures/spike.png");
    public Obstacle(Vector2[] spike) //Vector2 så att den vet vilka koordinater den ska skapa rektangeln för
    {
        for (var i = 0; i < spike.Length; i++)
        {
            spikes.Add(new Rectangle(spike[i].X, spike[i].Y, 50, 65)); //Skapar en spike rektangel för varje vector2 man skapar i program.cs när man skapar klassinstansen (lägger in dem i listan spikes också)
        }
    }

    public void DrawSpikes()
    {
        foreach (var spike in spikes)
        {
            Raylib.DrawTexture(spikeTexture, (int)spike.x, (int)spike.y, Color.WHITE); //Ritar ut spike texturen för varje spike i listan spikes
        }
    }
}
