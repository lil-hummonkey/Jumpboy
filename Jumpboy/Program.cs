using Raylib_cs;
using System.Numerics;

float speed = 5;
int groundY = 950;
int playerSpeedY = 0;
int playerHeight = 20;
int playerWidth = 20;


//If jump is false, you can jump
Boolean jump = false;
Boolean death = false;

string currentScene = "start";



//sets the size of screen and frames per second
Raylib.InitWindow(1000, 1000, "Jump boy");
Raylib.SetTargetFPS(60);

//Creates a rectangle called player rect, which width and height is a variable
Rectangle playerRect = new Rectangle(0, 900, playerWidth, playerHeight);

//Creates a list for rectangular platforms
List<Rectangle> platforms = new List<Rectangle>();
List<Rectangle> traps = new List<Rectangle>();


Vector2 movement = Vector2.Zero;

//while the game is running
while (Raylib.WindowShouldClose() == false)
{

    if (currentScene == "start")
    {
        Raylib.ClearBackground(Color.WHITE);
        Raylib.DrawText("Jumpboy", 400, 400, 80, Color.BLACK);
        Raylib.DrawText("Press SPACE to start!", 400, 500, 30, Color.BLACK);
        if (Raylib.IsKeyDown(KeyboardKey.KEY_SPACE))
        {
            currentScene = "screenOne";
            platforms.Clear();
            platforms.Add(new Rectangle(0, 900, 100, 30));
            platforms.Add(new Rectangle(100, 750, 100, 30));
            platforms.Add(new Rectangle(400, 700, 100, 30));
            platforms.Add(new Rectangle(200, 500, 100, 30));
            platforms.Add(new Rectangle(500, 450, 100, 30));
         
            platforms.Add(new Rectangle(600, 300, 100, 30));
            platforms.Add(new Rectangle(700, 200, 100, 30));
            traps.Add(new Rectangle(0, 949, 1000, 70));
          
        }
    }

   


    else if (currentScene == "screenOne")
    {
        if (Raylib.CheckCollisionRecs(playerRect, platforms[0]))
        {
            playerRect.y -= movement.Y;
            playerSpeedY = 0;
            jump = false;
            // playerRect.x -= movement.X;
        }
        if (playerRect.y + playerHeight < 0)
        {
            currentScene = "screenTwo";
            platforms.Clear();
            traps.Clear();
            platforms.Add(new Rectangle(0, 900, 100, 30));
            platforms.Add(new Rectangle(300, 758, 100, 30));
            platforms.Add(new Rectangle(700, 70, 100, 30));
            platforms.Add(new Rectangle(90, 570, 100, 30));
            playerRect.y = 950;
        }
    }

    else if (currentScene == "screenTwo")
    {


        if (Raylib.CheckCollisionRecs(playerRect, platforms[0]))
        {
            playerRect.y -= movement.Y;
            playerSpeedY = 0;
            jump = false;
            // playerRect.x -= movement.X;
        }
    }

    else if (currentScene == "end")
    { 
    Raylib.ClearBackground(Color.RED);
    Raylib.DrawText("YOU DIED", 300, 400, 80, Color.BLACK);
    Raylib.DrawText("Fuck you, you stupid fucking cunt ", 5, 500, 25, Color.BLACK);
     Raylib.DrawText("You will never achieve anything in your pityfull life ", 5, 540, 25, Color.BLACK);
     Raylib.DrawText("you stupid fuckpig faggot whore, looking like a retarded fat cumslut bitch", 5, 580, 25, Color.BLACK);
     Raylib.DrawText("suck my fat knob or i will slaughter your entire bloodline", 5, 620, 25, Color.BLACK);
     Raylib.DrawText("You are more worthless than an dead fetus. I hope you suffer", 5, 660, 25, Color.BLACK);
     
     
     

    }

    if (currentScene != "start" && currentScene != "end")
    {
        //
        movement = Vector2.Zero;
     if (death == true) {
        currentScene = "end";
     }
        Raylib.DrawRectangle(0, groundY, 1000, 60, Color.BLACK);








        if (Raylib.IsKeyDown(KeyboardKey.KEY_D))

        {
            movement.X = speed;
        }
        else if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
        {
            movement.X = -speed;
        }

        if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
        {
            if (jump == false)
            {
                playerSpeedY = -20;

                jump = true;

            }

        }


        // says that the player rectangles X coordinate is being 
        playerRect.x += movement.X;

        foreach (Rectangle platform in platforms)
        {
            if (Raylib.CheckCollisionRecs(playerRect, platform))
            {
                playerRect.x -= movement.X;
            }
            
        }



        movement.Y = playerSpeedY;
        playerRect.y += movement.Y;

        // for (int i = 0; i < 5; i++) { i }
        // foreach

        if (Raylib.IsKeyPressed(KeyboardKey.KEY_ZERO)) {
            death = true;
        }




        if (playerRect.y + playerHeight > groundY)
        {
            playerRect.y = groundY - playerHeight;

            jump = false;
        }
        else
        {
            playerSpeedY++;
        }



        if (playerRect.x < 0) {
        playerRect.x = 0;
        }

      if (playerRect.x + playerWidth > 1000) {
        playerRect.x = 1000 - playerWidth;
        }

        // if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
        // {
        //     playerRect.y += speed;
        // }

        // else if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
        // {
        //     playerRect.y -= speed;
        //}


        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.WHITE);

        Raylib.DrawRectangleRec(playerRect, Color.BLUE);





        foreach (Rectangle trap in traps)
        {
            Raylib.DrawRectangleRec(trap, Color.RED);
             if (Raylib.CheckCollisionRecs(playerRect, trap))
             {
                death = true;
             }
         
            
        }

       








        foreach (Rectangle platform in platforms)
        {
            Raylib.DrawRectangleRec(platform, Color.GREEN);
             if (Raylib.CheckCollisionRecs(playerRect, platform))
            {
                playerRect.y -= movement.Y;
                playerSpeedY = 0;
                jump = false;
                
                // playerRect.x -= movement.X;
            }
            
            
        }

       

    }

    Raylib.EndDrawing();


}






