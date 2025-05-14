public static class Constants
{
    public const int PLAYER_MOVEMENT_SPEED = 7; // Stores the player movement speed, before multiplication by deltaTime.
    public const int CUSTOMER_MOVEMENT_SPEED = 4; // Stores the customer movement speed in the kitchen queue, before multiplication by deltaTime.
    public const int LEVEL_ONE = 0;
    public const int LEVEL_TWO = 1;

    public const int LEVEL_ONE_DURATION = 5 * 60; // 5 minutes in seconds
    public const int LEVEL_TWO_DURATION = 6 * 60; // 6 minutes in seconds

    public static readonly float[] CUSTOMER_MIN_SPAWN_RATE = new float[] { 5, 4 }; // first = level one, second = level two (time in seconds)
    public static readonly float[] CUSTOMER_MAX_SPAWN_RATE = new float[] { 15, 13 };

    // Stores the min/max values for customer patience in each level (in seconds).
    public static readonly float[] CUSTOMER_MIN_PATIENCE = new float[] { 40, 35 };

    public static readonly float[] CUSTOMER_MAX_PATIENCE = new float[] { 50, 50 };

    public const string FACE_SIDE = "side";
    public const string FACE_UP = "up";
    public const string FACE_DOWN = "down";

    public const string LEFT = "Left";
    public const string RIGHT = "Right";

    // Store the scene names
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
    public const string END_MENU = "EndMenu";

    // Store the meals the player can hold.
    public const string PHO = "pho";

    public const string SUSHI = "sushi";
    public const string BAO_BUNS = "bao buns";
    public const string MANGO_STICKY_RICE = "mango sticky rice";
    public const string NOT_HOLDING_MEAL = "not holding meal";

    // Constants for the sushi mini-game, are the same as names in scene.
    public const string SUSHI_MG_RICE = "Ingredient_RiceBall";

    public const string SUSHI_MG_SEAWEED = "Ingredient_SeaWeed";
    public const string SUSHI_MG_TUNA = "Ingredient_Tuna";
    public const string SUSHI_MG_WASABI = "Ingredient_Wasabi";

    // Constants for the pho mini-game, are the same as names in the scene.
    public const string PHO_MG_POT_NOODLES = "Pot_Noodles";

    public const string PHO_MG_POT_BEEF = "Pot_Beef";
    public const string PHO_MG_BOWL_CILLANTRO = "Bowl_Cillantro";
    public const string PHO_MG_BOWL_PARSLEY = "Bowl_Parsley";

    // Stores the names of the coroutines the customer can have applied to them.
    public const string NO_COROUTINE = "no coroutine";

    public const string MOVE_IN_QUEUE = "move in queue";
    public const string MOVE_INTO_RESTAURANT = "move into restaurant";

    // Stores the customer animation states (per customer colour)
    public const int CUSTOMER_WALK_SIDE_ANIMATION = 0;

    public const int CUSTOMER_IDLE_UP_ANIMATION = 1;
    public const int CUSTOMER_WALK_DOWN_ANIMATION = 2;
    public const int CUSTOMER_IDLE_SIDE_ANIMATION = 3;
    public const int CUSTOMER_IDLE_SIT_ANIMATION = 4;

    // Stores the player animation states.
    public const int PLAYER_IDLE_ANIMATION = 0;

    public const int PLAYER_WALK_UP_ANIMATION = 1;
    public const int PLAYER_WALK_DOWN_ANIMATION = 2;
    public const int PLAYER_WALK_SIDE_ANIMATION = 3;

    // Stores max scores for each level.
    public const int LEVEL_ONE_MAX_SCORE = 1000;

    public const int LEVEL_TWO_MAX_SCORE = 1500;

    // Stores the positions for each customer in the restaurant scene.
    public static readonly float[,] CUSTOMER_SEATS_IN_RESTAURANT = new float[,] {
        { -4.15f,  1.68f }, { -1.87f,  1.68f }, { 0.86f,  1.68f }, { 3.14f,  1.68f }, // top row
        { -4.15f, -2.34f }, { -1.87f, -2.34f }, { 0.86f, -2.34f }, { 3.14f, -2.34f } }; // bottom row

    // Defines the range (min/max) pho mini-game items can move in the x-axis, before multiplication by deltaTime
    public static readonly float[] PHO_FOOD_ITEMS_SPEED = new float[] { 3.0f, 6.0f };

    // Defines the range (min/max) delay in the buns mini-game (in seconds)
    public static readonly float[] BUNS_TIME_DELAY = new float[] { 5.0f, 10.0f };

    public static readonly float BUNS_REACTION_THRESHOLD = 0.8f; // How long (in seconds) the player has to react
    public static readonly float ACTIVE_EGG_TIMER_DISPLACEMENT = 0.5f; // How far the egg timer should be displayed when goes off

    public static readonly int[] CUSTOMER_LEVEL_TWO_SECOND_MEAL_RANGE = new int[] { 0, 2 }; // Probability of customer ordering a second meal in level two, P(second order) = 0.5

    public static readonly int LEVEL_TWO_CUSTOMER_MAX_ORDERS = 2; // Max number of times customers can order meals in level two.

    public const float MINI_GAME_SPLASH_ART_DURATION = 2.0f; // The time the splash art is displayed for (in seconds)
    public const float SUSHI_MG_MINI_GAME_TIME = 15.0f; // The time the player has to complete the sushi mini-game (in seconds)
}