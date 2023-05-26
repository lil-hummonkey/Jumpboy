using System;



//use of bullet class allows easier sorting of code

//bullet variables are declared
public class Bullet
{
    public Vector2 pos;
    public Vector2 speed;

    public bool killme = false;

    public Rectangle circlelol;

    //specific instance in bullet class of position/speed is the position/speed of all bullets.
    //Bullets (circlelol) position is a variable of pos vector 
    public Bullet(Vector2 pos, Vector2 speed)
    {
        this.pos = pos;
        this.speed = speed;

        circlelol = new Rectangle((int)pos.X - 2, (int)pos.Y - 2, 4, 4);
    }

    //updates position according to pos vector

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
