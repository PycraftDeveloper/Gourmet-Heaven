public static class Constants
{
    public const int PLAYER_MOVEMENT_SPEED = 7;
    public const int CUSTOMER_MOVEMENT_SPEED = 1;
    public const int LEVEL_ONE = 0;
    public const int LEVEL_TWO = 1;

    public static readonly float[] CUSTOMER_MIN_SPAWN_RATE = new float[] { 5, 4 }; // first = level one, second = level two, third = ...
    public static readonly float[] CUSTOMER_MAX_SPAWN_RATE = new float[] { 15, 13 };

    public static readonly float[] CUSTOMER_MIN_PATIENCE = new float[] { 17, 15 };
    public static readonly float[] CUSTOMER_MAX_PATIENCE = new float[] { 25, 22 };

    public const string FACE_SIDE = "side";
    public const string FACE_UP = "up";
    public const string FACE_DOWN = "down";

    public const string LEFT = "Left";
    public const string RIGHT = "Right";

    public const string MAIN_MENU = "MainMenu";
    public const string OPTIONS_MENU = "OptionsMenu";
    public const string CREDITS_MENU = "CreditsMenu";
    public const string KITCHEN = "Kitchen";
    public const string SHOP_MENU = "ShopMenu";
    public const string LEVEL_SELECTION_MENU = "LevelSelectionMenu";
    public const string PAUSE_MENU = "PauseMenu";
    public const string BUNS_MG = "BunsMG";
    public const string PHO_MG = "PhoMG";
    public const string RESTAURANT = "Restaurant";
    public const string RICE_MG = "RiceMG";
    public const string SUSHI_MG = "SushiMG";

    public const string PHO = "pho";
    public const string SUSHI = "sushi";
    public const string BAO_BUNS = "bao buns";
    public const string MANGO_STICKY_RICE = "mango sticky rice";
    public const string NOT_HOLDING_MEAL = "not holding meal";

    public const string RICE = "Rice";
    public const string SEAWEED = "Seaweed";
    public const string SOMETHING_ELSE = "SomethingElse";

    public const string NO_COROUTINE = "no coroutine";
    public const string MOVE_IN_QUEUE = "move in queue";
    public const string MOVE_INTO_RESTAURANT = "move into restaurant";

    public const int CUSTOMER_WALK_SIDE_ANIMATION = 0;
    public const int CUSTOMER_IDLE_UP_ANIMATION = 1;
    public const int CUSTOMER_WALK_DOWN_ANIMATION = 2;
    public const int CUSTOMER_IDLE_SIDE_ANIMATION = 3;

    public const int PLAYER_IDLE_ANIMATION = 0;
    public const int PLAYER_WALK_UP_ANIMATION = 1;
    public const int PLAYER_WALK_DOWN_ANIMATION = 2;
    public const int PLAYER_WALK_SIDE_ANIMATION = 3;

    public static readonly float[,] CUSTOMER_SEATS_IN_RESTAURANT = new float[,] {
        { -4.15f,  1.68f }, { -1.87f,  1.68f }, { 0.86f,  1.68f }, { 3.14f,  1.68f }, // top row
        { -4.15f, -2.34f }, { -1.87f, -2.34f }, { 0.86f, -2.34f }, { 3.14f, -2.34f } }; // bottom row
}