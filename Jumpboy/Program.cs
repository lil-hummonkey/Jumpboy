using Raylib_cs;
using System.Numerics;

float speed = 5;
int playerHeight = 20;
int playerWidth = 20;

float gravity = 1f;



Boolean mayJump = false;
Boolean death = false;


string currentScene = "start";

bool hasDisarmedTrap = false;
bool onPlat = false;

//sets the size of screen and frames per second
Raylib.InitWindow(1000, 1000, "Jump boy");
Raylib.SetTargetFPS(60);

//Creates a rectangle called player rect, which width and height is a variable
Rectangle button = new Rectangle(100, 900, 40, 40);

//Creates a list for rectangular platforms
List<Rectangle> platforms = new List<Rectangle>();
List<Rectangle> traps = new List<Rectangle>();

Player player1 = new Player();
player1.velocity = Vector2.Zero;
player1.boundingBox = new Rectangle(0, 830, playerWidth, playerHeight);




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
            platforms.Add(new Rectangle(700, 950, 200, 30));
            platforms.Add(new Rectangle(870, 600, 100, 30));
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
            player1.boundingBox.y = 890;
        }

    }

    if (currentScene != "start" && currentScene != "end")
    {
        player1.velocity.X = 0;

        if (death == true)
        {
            currentScene = "end";
        }

        if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
        {
            player1.velocity.X = speed;
        }
        else if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
        {
            player1.velocity.X = -speed;
        }

        if (mayJump && Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE) && player1.velocity.Y >= 0)
        {
            player1.velocity.Y -= 20;
        }
        else
        {
            player1.velocity.Y += gravity;
        }

        player1.boundingBox.x += player1.velocity.X;
        player1.boundingBox.y += player1.velocity.Y;

        onPlat = false;
        mayJump = false;
        foreach (Rectangle platform in platforms)
        {
            Raylib.DrawRectangleRec(platform, Color.GREEN);

            if (Raylib.CheckCollisionRecs(player1.boundingBox, platform))
            {
                onPlat = true;
                mayJump = true;
            }
        }

    
        
        if (onPlat)
        {
            
            player1.boundingBox.y -= player1.velocity.Y;
            player1.velocity.Y = 0;
        }

        if (player1.boundingBox.x < 0)
        {
            player1.boundingBox.x = 0;
        }

        if (player1.boundingBox.x + playerWidth > 1000)
        {
            player1.boundingBox.x = 1000 - playerWidth;
        }


        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.WHITE);

        Raylib.DrawRectangleRec(player1.boundingBox, Color.BLUE);

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






