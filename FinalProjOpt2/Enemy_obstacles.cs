using System;
using Raylib_cs;
using System.Numerics;

public class Obstacle
{
    public Obstacle nextObstacle;
    public List<Rectangle> spikes = new();
    public Texture2D spikeTexture = Raylib.LoadTexture("Textures/spike.png");
    public Obstacle(int obs1x, int obs2x, int obs3x, int obs1y, int obs2y, int obs3y)
    {
        spikes.Add(new Rectangle(obs1x, obs1y, 50, 65));
        spikes.Add(new Rectangle(obs2x, obs2y, 50, 65));
        spikes.Add(new Rectangle(obs3x, obs3y, 50, 65));
    }

    public void DrawSpikes(){
        foreach (var spike in spikes)
        {
            Raylib.DrawTexture(spikeTexture, (int)spike.x, (int)spike.y, Color.WHITE);
        }
    }
}
