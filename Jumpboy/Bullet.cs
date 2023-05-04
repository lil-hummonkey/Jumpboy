using System;


public class Bullet
{
    public Vector2 pos;
    public Vector2 speed;

    public bool killme = false;

    public Rectangle circlelol;


    public Bullet(Vector2 pos, Vector2 speed)
    {
        this.pos = pos;
        this.speed = speed;

        circlelol = new Rectangle((int)pos.X - 2, (int)pos.Y - 2, 4, 4);
    }

    public void Update()
    {
        pos += speed;
        circlelol.x = pos.X;
        circlelol.y = pos.Y;

    }

    public void Draw()
    {
        Raylib.DrawRectangleRec(circlelol, Color.BLACK);
    }
}
