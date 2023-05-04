using Raylib_cs;
using System.Numerics;


public class Enemy {
    
    
    
    public Vector2 speed;

    public Vector2 Velocity;

    

    public Vector2 pos;

    
    Random generator = new Random();

    public bool killme = false;

    public int maxEnemySpeed = 4;

    public int health = 2;

    int damage;

    public Rectangle rect;

    public Enemy() {
        int x = generator.Next(500, 1350);
        int y = generator.Next(1, 600);
        rect = new Rectangle(x, y, 20, 20);
        
    }

    

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