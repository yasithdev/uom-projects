public static class Constants
{
    #region "C2S - Client To Server"

    public const string C2S_JOIN = "JOIN#";
    public const string UP = "UP#";
    public const string DOWN = "DOWN#";
    public const string LEFT = "LEFT#";
    public const string RIGHT = "RIGHT#";
    public const string SHOOT = "SHOOT#";

    #endregion

    #region "S2C - Server To Client"

    public const string S2C_DEL = "#";

    public const string S2C_GAMESTARTED = "GAME_ALREADY_STARTED";
    public const string S2C_NOTSTARTED = "GAME_NOT_STARTED_YET";
    public const string S2C_GAMEOVER = "GAME_HAS_FINISHED";
    public const string S2C_GAMEJUSTFINISHED = "GAME_FINISHED";

    public const string S2C_CONTESTANTSFULL = "PLAYERS_FULL";
    public const string S2C_ALREADYADDED = "ALREADY_ADDED";

    public const string S2C_INVALIDCELL = "INVALID_CELL";
    public const string S2C_NOTACONTESTANT = "NOT_A_VALID_CONTESTANT";
    public const string S2C_TOOEARLY = "TOO_QUICK";
    public const string S2C_CELLOCCUPIED = "CELL_OCCUPIED";

    // Penalty should be added for hitting obstacle or pitfall
    public const string S2C_HITONOBSTACLE = "OBSTACLE";
    public const string S2C_FALLENTOPIT = "PITFALL";

    public const string S2C_NOTALIVE = "DEAD";

    public const string S2C_REQUESTERROR = "REQUEST_ERROR";
    public const string S2C_SERVERERROR = "SERVER_ERROR";

    #endregion

    #region "Server Configurations"

    // IP of the server
    public const string SERVER_IP = "127.0.0.1";
	public const string CLIENT_IP = "127.0.0.1";
	// Port that the server listens to (C2S sent through this)
    public const int SERVER_PORT = 6000;
	// Port that the client listens to (S2C received through this)
    public const int CLIENT_PORT = 7000;
    // Period where the game is played
    public const int LIFE_TIME = 900000;
    // Period where a player has to wait before sending another command
    public const double PLAYER_DELAY = 1000;
    // Speed of a Bullet / Tank
    public const int BULLET_MULTI = 4;
    // Update Period
    public const int UPDATE_PERIOD = (int) (PLAYER_DELAY/BULLET_MULTI);
    // Player initial health
    public const int PLAYER_HEALTH = 100;
    // Points earned when a brick is shot
    public const int BRICK_POINTS = 10;
    // Life time of a plunder coin pile
    public const int PLUNDER_TREASURE_LIFE = 5000;
    // Map size
    public const int MAP_SIZE = 10;
    // Maximum number of pits. Have to be >=5
    public const int MAX_BRICKS = 10;
    // Maximum number of obstacles. Have to be >= 8
    public const int MAX_OBSTACLES = 10;
    // Number of coin piles per minute
    public const int COINPILE_RATE = 10;
    // Number of life packs per minute
    public const int LIFEPACK_RATE = 5;
    // Artificial intelligence level. 1 to 10
    public const int AI_FACTOR = 10;

    #endregion
}