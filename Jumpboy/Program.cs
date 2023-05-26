//use of raylib for its ease of access to features useful in game design
global using Raylib_cs;
global using System.Numerics;

//variables declaration
float speed = 5;
int playerHeight = 25;
int playerWidth = 17;
float shootCooldownMax = 0.5f;
float shootCooldownValue = shootCooldownMax;
float gravity = 1f;
float time = 0;
string currentScene = "start";
float spawnTimer=5;

//boolean declaration
Boolean hasDisarmedTrap = false;
Boolean hasTurnedRight = true;
Boolean mayJump = false;
Boolean death = false;
Boolean drawButton = false;
Boolean godmode = true;




//sets the size of screen and frames per second
//Texture2D texture = Raylib.LoadTextureFromImage(image);
Image image = Raylib.LoadImage("Bingbong.png");
Raylib.InitWindow(1380, 1000, "Jump boy");
Raylib.SetTargetFPS(60);
Rectangle button = new Rectangle(100, 900, 40, 40);
Rectangle gun = new Rectangle(600, 600, 20, 5);
Rectangle bullet = new Rectangle();
Player player1 = new Player();
//use of list for infinite possibilities for platforms/traps; otherwise (if array) each platform/trap would have to be declared in array. Lists can be dynamically resized
List<Rectangle> platforms = new List<Rectangle>();
List<Rectangle> traps = new List<Rectangle>();

//Bullet and Enemy both use lists as lists are useful when elements are added and removed frequently

//each new bullet is a unique one, this helps with declaring interractions with specific bullets (eg the right bullet disappears on impact). 
List<Bullet> bullets = new();
//each enemy has its own unique interraction with specific bullets, and each enemy spawns in a specific place, so list allows the amount of enemies per screen/in the game be infinite
List<Enemy> enemies = new();
player1.boundingBox = new Rectangle(0, 700, playerWidth, playerHeight);
player1.velocity = Vector2.Zero;
Vector2 bulletVel = new Vector2();


//logic for game while it runs
while (Raylib.WindowShouldClose() == false)
{
    //rotation variable is decided by the rsulting angle (arctan) of the mouse position and player position
    float rotation = MathF.Atan2
    (player1.boundingBox.y - Raylib.GetMousePosition().Y
    , player1.boundingBox.x - Raylib.GetMousePosition().X) * Raylib.RAD2DEG;

    //säkerhetsställer att rotations värde är ett användbar tal, sedan stoppar rotation av gun om den åker bakom spelaren
    rotation = CheckRotation(hasTurnedRight, rotation);
    rotation += 180;
   
    //begin drawing due to drawing and logic are both under because of method
    Raylib.BeginDrawing();

    
    //defines which screen you are on; draws objects for respective screen
    switch (currentScene)
    {

        case "start":

            Raylib.ClearBackground(Color.WHITE);
            Raylib.DrawText("Jumpboy", 400, 350, 140, Color.BLACK);
            Raylib.DrawText("Press SPACE to start!", 250, 500, 80, Color.BLACK);

            if (Raylib.IsKeyDown(KeyboardKey.KEY_SPACE))
            { currentScene = "intro";
            platforms.Clear();
            traps.Clear();
            
            
            
             }
            break;


            //specifies which screen you are in by having the next scene be within the previous scenes case, so the screen doesnt constantly update the objects every frame and slow the fps
        case "intro":

            
            if (player1.boundingBox.x + playerHeight > 1300)
            {
                currentScene = "screenOne";
                SetupLevel1(player1, platforms, traps, enemies);
            }
            break;



        case "screenOne":


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
                traps.Add(new Rectangle(900, 949, 500, 70));
                traps.Add(new Rectangle(400, 500, 30, 400));
                player1.boundingBox.y = 950;

                
                    enemies.Add(new());
                 
            }
            break;





        //specifies that a button can be drawn within this scene


        case "screenTwo":


            if (player1.boundingBox.y + playerHeight < 0)
            {
                currentScene = "screenThree";
                drawButton = true;
                platforms.Clear();
                traps.Clear();
                player1.boundingBox.y = 950;
                platforms.Add(new Rectangle(100, 880, 100, 30));
                platforms.Add(new Rectangle(250, 650, 100, 30));
                platforms.Add(new Rectangle(150, 500, 100, 30));
                platforms.Add(new Rectangle(700, 180, 100, 30));
                platforms.Add(new Rectangle(685, 400, 100, 30));
                platforms.Add(new Rectangle(770, 600, 100, 30));
                platforms.Add(new Rectangle(820, 740, 100, 30));
                platforms.Add(new Rectangle(900, 860, 100, 30));
                traps.Add(new Rectangle(650, 49, 700, 70));
                traps.Add(new Rectangle(650, 0, 30, 850));
                button.y = 350;
                button.x = 150;
                hasDisarmedTrap = false;
                spawnTimer = 5;
                
            }
            break;

        //has specific definitions for screen 3
        case "screenThree":
                if (spawnTimer <= 0)
                {
                    enemies.Add(new());
                    spawnTimer = 5;
                }
                hasDisarmedTrap = CheckButtonCollision(hasDisarmedTrap, button, player1, traps);

            if (player1.boundingBox.y + playerHeight < 0)
            { currentScene = "vicroy"; }
            break;


        //win screen
        case "vicroy":

            Raylib.DrawText("U win!!!!111!1", 400, 400, 80, Color.BLACK);
            platforms.Clear();
            traps.Clear();
            drawButton=false;
            break;



    //specifies that if case = end then you will be sent to "death screen", uses a method to check if a new scene should be started, and if so case = screenOne and seperately from that level 1 should be drawn (helps correct an error of jumbled scene definition)
        case "end":

            Raylib.ClearBackground(Color.RED);
            Raylib.DrawText("YOU DIED", 270, 350, 170, Color.BLACK);
            Raylib.DrawText("press space to start again", 150, 500, 80, Color.BLACK);
            enemies.Clear();
            time = 0;
            drawButton = false;
            bool l = StartScene(ref currentScene, ref death, player1, enemies);
            if (l == true)
            {
                SetupLevel1(player1, platforms, traps, enemies);
            }

            break;

    }



    //if the current scene is the start or end screen it ends drawing
    if (currentScene == "start" || currentScene == "end")
    {
        Raylib.EndDrawing();
        continue;
    }

    //defines that bool death sends you to death screen
    if (death == true)
    {
        currentScene = "end";

    }

    
    player1.velocity.X = 0;
    gun.x = player1.boundingBox.x + 10;
    gun.y = player1.boundingBox.y + 10;
    time = drawtimer(time);

    //defines D as a move right button and A left move button
    hasTurnedRight = TurnRight(speed, hasTurnedRight, player1);
   
    //if mayjump is true, and you press SPACE you will jump, else you will fall
    JumpIf(gravity, mayJump, player1);
   
    //If player collides with platform player will stand on it, or be pushed away from it (exeption for semi-intentional bug with walljump)
    PlatformStand(player1, platforms);

    
    //players position is affected by velocity variable
    player1.boundingBox.y += player1.velocity.Y;
    
    //You can jump if you are on a platform or on the ground you can jump
    mayJump = CheckGround(player1, platforms);
    
    //draws platforms
    DrawPlatforms(platforms);
    player1.boundingBox.x += player1.velocity.X;
    
    //cooldown for gun and enemies using raylibs time counter
    shootCooldownValue -= Raylib.GetFrameTime();
    spawnTimer -= Raylib.GetFrameTime();
    
    //stops player from moving out of bounds
    Boundries(playerWidth, player1);

    //checks if cooldown is under 0 (0.5s after every shot) and if you pressed mouse1. If true then the bullets velocity is a vector of either the cosinus or sinus of the gun and bullet speed (10)
    //also creates a new bullet by the position of the gun, and resets timer at the end
    shootCooldownValue = ShootBullets(shootCooldownValue, gun, bullets, bulletVel, rotation);






    //Each enemy moves itself towards the player
    //if a bullet in bullets class hits an enemy then the specific bullet and enemies remove bool will be true
    foreach (Enemy e in enemies)
    {
        e.speed.X = player1.boundingBox.x - e.rect.x;
        e.speed.Y = player1.boundingBox.y - e.rect.y;
        foreach (Bullet b in bullets)
        {
            if (Raylib.CheckCollisionRecs(b.circlelol, e.rect))
            {
                b.killme = true;
                e.health -= 1;
                if (e.health == 0)
                {
                    e.killme = true;
                }

            }
        }
    }

    //removes specific enemies/bullets if their respective killme bool is true
    bullets.RemoveAll(b => b.killme);
    enemies.RemoveAll(e => e.killme);


    //updates all bullets in bullets list
    foreach (Bullet b in bullets)
    {
        b.Update();
    }

    //checks if player has touched enemies, and if so returns death as true
    death = NewMethod(death, player1, enemies);

    //draws background, player and bullets
    Raylib.ClearBackground(Color.WHITE);
    Raylib.DrawRectangleRec(player1.boundingBox, Color.BLUE);
    Raylib.DrawRectangleRec(bullet, Color.BLUE);
    //draws gun, using DrawRectanglePro adds a rotation variable to it (called rotation), as to define its position according to mouse
    Raylib.DrawRectanglePro(gun, new Vector2(0, 2.5f), rotation, Color.BLACK);

    //draws enemies/bullets in enemies/bullets list
    foreach (Enemy e in enemies)
    {
        e.Draw();
    }
    foreach (Bullet b in bullets)
    {
        b.Draw();
    }

    //draws button if drawButton bool is true (drawButton is true on screens which need button within it)
    DrawButton(drawButton, button);
    
    //Defines that death will be returned true if player touches trap
    death = TrapDeath(death, player1, traps);

    //draws text for intro screen (uses time variable to draw text according to time in game)
    if(currentScene=="intro"){
       
       if(time < 5){
        Raylib.DrawText("You have been summoned here for a reason...", 100, 305, 20, Color.GREEN);
       
        }
        if(time>7 && time < 20){

        Raylib.DrawText("You stand by the enterance to the Forest of Ethia. You need to free it from the scourge up above...", 100, 350, 20, Color.GREEN);
        Raylib.DrawText("Move forwards to enter, and ascend once you are within. You are Ethia's only hope...", 100, 385, 20, Color.GREEN);
        }
        if(time>22){
            Raylib.DrawText("Move your character with WASD, turn left or right with A and D", 100, 385, 30, Color.BLACK);
            Raylib.DrawText("Aim your gun in front of you with your mouse. Shoot with [LEFT CLICK] ", 100, 435, 30, Color.BLACK);
            Raylib.DrawText("Don't touch anything red, and if enemies approach you shoot them to kill them", 100, 565, 30, Color.BLACK);
        
        
        }
    }

    //godmode command for playtesting
    if (godmode == true)
    {
        if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_RIGHT))
        {
            platforms.Add(new Rectangle(Raylib.GetMousePosition().X, Raylib.GetMousePosition().Y, 10, 10));
        }
    }

    

    Raylib.EndDrawing();
}





static bool CheckGround(Player p, List<Rectangle> platforms)
{
    bool mayJump = false;
    if (p.boundingBox.y >= 950)
    {
        mayJump = true;
        p.boundingBox.y -= p.velocity.Y;
        p.velocity.Y = 0;
    }
    foreach (Rectangle platform in platforms)
    {
        if (Raylib.CheckCollisionRecs(p.boundingBox, platform))
        {
            p.boundingBox.y -= p.velocity.Y;
            p.velocity.Y = 0;


            mayJump = true;
        }
    }
    return mayJump;
}

static bool CheckButtonCollision(bool hasDisarmedTrap, Rectangle button, Player player1, List<Rectangle> traps)
{
    if (Raylib.CheckCollisionRecs(player1.boundingBox, button))
    {
        if (!hasDisarmedTrap)
        {
            traps.RemoveAt(0);
            hasDisarmedTrap = true;
        }
    }

    return hasDisarmedTrap;
}

static float CheckRotation(bool hasTurnedRight, float rotation)
{
    if (hasTurnedRight)
    {
        rotation = ((rotation % 360) + 360) % 360;
        rotation = Math.Clamp(rotation, 90, 270);
    }

    else
    {
        rotation = Math.Clamp(rotation, -90, 90);
    }

    return rotation;


}

static bool StartScene(ref string currentScene, ref bool death, Player player1, List<Enemy> enemies)
{
    if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
    {
        death = false;
        currentScene = "screenOne";
        player1.boundingBox.x = 0;
        player1.boundingBox.y = 700;
        return true;
    }
    return false;
}

static float drawtimer(float time)
{
    time += Raylib.GetFrameTime();
    Raylib.DrawText($"time: {(int)time}", 10, 10, 20, Color.BLUE);
    return time;
}

static bool TurnRight(float speed, bool hasTurnedRight, Player player1)
{
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

    return hasTurnedRight;
}

static void JumpIf(float gravity, bool mayJump, Player player1)
{
    if (mayJump && Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE) && player1.velocity.Y >= 0)
    {
        player1.velocity.Y -= 21;
    }
    else
    {
        player1.velocity.Y += gravity;
    }
}

static void PlatformStand(Player player1, List<Rectangle> platforms)
{
    foreach (Rectangle platform in platforms)
    {
        if (Raylib.CheckCollisionRecs(player1.boundingBox, platform))
        {
            player1.boundingBox.x -= player1.velocity.X;
            

            break;


        }
    }
}


static bool TrapDeath(bool death, Player player1, List<Rectangle> traps)
{
    foreach (Rectangle trap in traps)
    {
        Raylib.DrawRectangleRec(trap, Color.RED);
        if (Raylib.CheckCollisionRecs(player1.boundingBox, trap))
        {
            death = true;
        }


    }

    return death;
}

static float ShootBullets(float shootCooldownValue, Rectangle gun, List<Bullet> bullets, Vector2 bulletVel, float rotation)
{
    if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT) && shootCooldownValue < 0)
    {




        bulletVel.Y = (float)Math.Sin(rotation * Raylib.DEG2RAD) * 10;
        bulletVel.X = (float)Math.Cos(rotation * Raylib.DEG2RAD) * 10;

        bullets.Add(new Bullet(new Vector2(gun.x, gun.y), bulletVel));
        shootCooldownValue = 0.5f;



    }

    return shootCooldownValue;
}

static void Boundries(int playerWidth, Player player1)
{
    if (player1.boundingBox.x < 0)
    {
        player1.boundingBox.x = 0;
    }

    if (player1.boundingBox.x + playerWidth > 1350)
    {
        player1.boundingBox.x = 1350 - playerWidth;
    }

    if (player1.boundingBox.y >= 950)
    {
        player1.boundingBox.y = 950;

    }

}

static void DrawPlatforms(List<Rectangle> platforms)
{
    foreach (Rectangle platform in platforms)
    {
        Raylib.DrawRectangleRec(platform, Color.GREEN);
    }
}

static void DrawButton(bool drawButton, Rectangle button)
{
    if (drawButton)
    {
        Raylib.DrawRectangleRec(button, Color.PINK);
    }
}

static bool NewMethod(bool death, Player player1, List<Enemy> enemies)
{
    foreach (Enemy e in enemies)
    {
        e.Update();
        if (Raylib.CheckCollisionRecs(player1.boundingBox, e.rect))
        {
            death = true;
        }
    }

    return death;
}

static void SetupLevel1(Player player1, List<Rectangle> platforms, List<Rectangle> traps, List<Enemy> enemies)
{
    player1.boundingBox.x = 0;
    player1.boundingBox.y = 880;
    platforms.Clear();
    traps.Clear();
    platforms.Add(new Rectangle(0, 900, 100, 30));
    platforms.Add(new Rectangle(90, 750, 100, 30));
    platforms.Add(new Rectangle(350, 700, 100, 30));
    platforms.Add(new Rectangle(200, 500, 100, 30));
    platforms.Add(new Rectangle(500, 450, 100, 30));
    platforms.Add(new Rectangle(600, 300, 100, 30));
    platforms.Add(new Rectangle(700, 200, 100, 30));
    traps.Add(new Rectangle(0, 949, 1380, 70));
    for (var i = 0; i < 2; i++)
    { enemies.Add(new()); }
}