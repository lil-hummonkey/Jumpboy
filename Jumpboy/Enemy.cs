using Raylib_cs;
using System.Numerics;

//Specific class for Enemies so enemy variables are seperated from program.cs/so potential future unique enemies can be added

//variables specific to enemy class
public class Enemy {
    
    
    
    public Vector2 speed;

    public Vector2 Velocity;

    

    public Vector2 pos;

    
    Random generator = new Random();

    public bool killme = false;

    public int maxEnemySpeed = 4;

    public int health = 2;

    int damage;

    //generates Enemy as a rectangle
    public Rectangle rect;

    public Enemy() {
        int x = generator.Next(500, 1350);
        int y = generator.Next(1, 600);
        rect = new Rectangle(x, y, 20, 20);
        
    }

    
    //declares that enemy rectangles position is changed using the speed vector to move towards player and said movement has the speed of maxEnemySpeed
    //declares that enemy health is lowered by damage (for potential future use in a dynamic weapon system)
    public void Update()
    {
         
        speed = Vector2.Normalize(speed);
        rect.x += speed.X*maxEnemySpeed;
        rect.y += speed.Y*maxEnemySpeed;
        health -= damage;   
         
        
        
    
        }

    

    public void Draw()
    {
        Raylib.DrawRectangleRec(rect, Color.RED);
    }
}