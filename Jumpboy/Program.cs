using Raylib_cs;
using System.Numerics;

float speed = 5;
int playerHeight = 20;
int playerWidth = 20;

float gravity = 1f;
float xVel = 2;
float yVel = 0;
int bulPosX = 600;
int bulPosY = 600;



Boolean mayJump = false;
Boolean death = false;


string currentScene = "start";

bool hasDisarmedTrap = false;
bool hasTurnedRight = true;

//sets the size of screen and frames per second
Raylib.InitWindow(1000, 1000, "Jump boy");
Raylib.SetTargetFPS(60);
float time = 0;




Rectangle button = new Rectangle(100, 900, 40, 40);
Player player1 = new Player();
Rectangle gun = new Rectangle(600, 600, 20, 5 );
List<Rectangle> platforms = new List<Rectangle>();
List<Rectangle> traps = new List<Rectangle>();
Rectangle bullet = new Rectangle();




player1.velocity = Vector2.Zero;
player1.boundingBox = new Rectangle(0, 700, playerWidth, playerHeight);
bullet = new Rectangle(gun.x, gun.y, playerWidth, playerHeight);

while (Raylib.WindowShouldClose() == false)
{
    time += Raylib.GetFrameTime();

    float rotation = MathF.Atan2(player1.boundingBox.y - Raylib.GetMousePosition().Y,player1.boundingBox.x-Raylib.GetMousePosition().X)*Raylib.RAD2DEG;
    
     
     if (hasTurnedRight)
    {
    rotation = ((rotation % 360) + 360) % 360; 
    rotation = Math.Clamp(rotation, 90, 270);
     }
 
  else
  {
    rotation = Math.Clamp(rotation, -90, 90); 
  }
rotation += 180;

  
 
    if (currentScene == "start")
    {
        Raylib.ClearBackground(Color.WHITE);
        Raylib.DrawText("Jumpboy", 400, 400, 80, Color.BLACK);
        Raylib.DrawText("Press SPACE to start!", 400, 500, 30, Color.BLACK);
        if (Raylib.IsKeyDown(KeyboardKey.KEY_SPACE))
        {
            currentScene = "screenOne";
            platforms.Clear();
            traps.Clear();
            platforms.Add(new Rectangle(0, 900, 100, 30));
            platforms.Add(new Rectangle(90, 750, 100, 30));
            platforms.Add(new Rectangle(350, 700, 100, 30));
            platforms.Add(new Rectangle(200, 500, 100, 30));
            platforms.Add(new Rectangle(500, 450, 100, 30));
            platforms.Add(new Rectangle(600, 300, 100, 30));
            platforms.Add(new Rectangle(700, 200, 100, 30));
            traps.Add(new Rectangle(0, 949, 1000, 70));

        }
    }
    else if (currentScene == "screenOne")
    {
        if (player1.boundingBox.y + playerHeight < 0)
        {
            currentScene = "screenTwo";
            platforms.Clear();
            traps.Clear();
            platforms.Add(new Rectangle(700, 970, 200, 30));
            platforms.Add(new Rectangle(870, 689, 100, 30));
            platforms.Add(new Rectangle(300, 758, 100, 30));
            platforms.Add(new Rectangle(90, 570, 80, 30));
            platforms.Add(new Rectangle(600, 750, 100, 30));
            platforms.Add(new Rectangle(720, 480, 100, 30));
            platforms.Add(new Rectangle(490, 440, 30, 30));
            platforms.Add(new Rectangle(0, 440, 30, 30));
            platforms.Add(new Rectangle(0, 240, 30, 30));
            platforms.Add(new Rectangle(240, 200, 15, 15));
            traps.Add(new Rectangle(60, 240, 30, 30));
            traps.Add(new Rectangle(750, 450, 50, 30));
            traps.Add(new Rectangle(0, 949, 700, 70));
            traps.Add(new Rectangle(900, 949, 100, 70));
            traps.Add(new Rectangle(400, 500, 30, 400));
            player1.boundingBox.y = 950;
        }
    }
    else if (currentScene == "screenTwo")
    {

        if (player1.boundingBox.y + playerHeight < 0)
        {
            currentScene = "screenThree";
            platforms.Clear();
            traps.Clear();
            player1.boundingBox.y = 950;
            platforms.Add(new Rectangle(0, 970, 1000, 30));
            traps.Add(new Rectangle(900, 949, 100, 70));
            traps.Add(new Rectangle(400, 500, 30, 400));

            hasDisarmedTrap = false;

        }

    }

    else if (currentScene == "screenThree")
    {
        if (player1.boundingBox.y + playerHeight < 0)
        {
            currentScene = "vicroy";
            Raylib.DrawText("U win!!!!111!1", 400, 400, 80, Color.BLACK);
        }

        if (Raylib.CheckCollisionRecs(player1.boundingBox, button))
        {
            if (!hasDisarmedTrap)
            {
                traps.RemoveAt(0);
                hasDisarmedTrap = true;
            }

        }


    }

    else if (currentScene == "end")
    {
        Raylib.ClearBackground(Color.RED);
        Raylib.DrawText("YOU DIED", 250, 400, 80, Color.BLACK);
        Raylib.DrawText("press enter to start again", 250, 500, 25, Color.BLACK);
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
        {
            death = false;
            currentScene = "start";
            player1.boundingBox.x = 0;
            player1.boundingBox.y = 700;
        }

    }

    if (currentScene != "start" && currentScene != "end")
    {
        player1.velocity.X = 0;
        

        gun.x = player1.boundingBox.x+10;
        gun.y = player1.boundingBox.y+10;
       
        
       

        Raylib.DrawText($"time: {time}" , 10, 10, 20, Color.BLUE);

        if (death == true)
        {
            currentScene = "end";
        }

        if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
        {
            player1.velocity.X = speed;
            hasTurnedRight = true;
        }
        else if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
        {
            player1.velocity.X = -speed;
            hasTurnedRight = false;
        }
        if (mayJump && Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE) && player1.velocity.Y >= 0)
        {
            player1.velocity.Y -= 21;
        }
        else
        {
            player1.velocity.Y += gravity;
        }


        mayJump = false;

        foreach (Rectangle platform in platforms)
        {
            if (Raylib.CheckCollisionRecs(player1.boundingBox, platform))
            {
                player1.boundingBox.x -= player1.velocity.X;

                break;


            }
        }

   


        player1.boundingBox.y += player1.velocity.Y;

        foreach (Rectangle platform in platforms)
        {
            if (Raylib.CheckCollisionRecs(player1.boundingBox, platform))
            {
                player1.boundingBox.y -= player1.velocity.Y;
                player1.velocity.Y = 0;


                mayJump = true;
            }
        }

        // gå igenom alla lådor igen
        // ångra y-förflyttning om kollision
        foreach (Rectangle platform in platforms)
        {
            Raylib.DrawRectangleRec(platform, Color.GREEN);
        }

        player1.boundingBox.x += player1.velocity.X;

        if (player1.boundingBox.x < 0)
        {
            player1.boundingBox.x = 0;
        }

        if (player1.boundingBox.x + playerWidth > 1000)
        {
            player1.boundingBox.x = 1000 - playerWidth;
        }

        if (player1.boundingBox.y > 950)
        {
            player1.boundingBox.y = 950;
        }

      
        yVel = player1.boundingBox.y - Raylib.GetMousePosition().Y / player1.boundingBox.x-Raylib.GetMousePosition().X;
     


        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.WHITE);

        Raylib.DrawRectangleRec(player1.boundingBox, Color.BLUE);
        // Raylib.DrawRectangleRec(bullet, Color.BLUE);
        Raylib.DrawRectanglePro(gun, new Vector2(0, 2.5f),rotation, Color.BLACK);
       
  
      
        


        if (currentScene == "screenThree")
        {
            Raylib.DrawRectangleRec(button, Color.PINK);
        }

     

        foreach (Rectangle trap in traps)
        {
            Raylib.DrawRectangleRec(trap, Color.RED);
            if (Raylib.CheckCollisionRecs(player1.boundingBox, trap))
            {
                death = true;
            }


        }


    }

    Raylib.EndDrawing();


}






