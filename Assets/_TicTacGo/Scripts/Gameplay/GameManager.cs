using UnityEngine;

public enum GameMode
{
    non,
    Carrier,
    Endless,
}

public enum GameState
{
    Prepare,
    Playing,
    Paused,
    LevelCompleted,
    PreGameOver,
    GameOver
}

public enum DifficultyLevel
{
    non,
    Beginner,
    Intermediate,
    Advanced,
}

public class GameManager : Singleton<GameManager>
{
    //// (Optional) Prevent non-singleton constructor use.
    protected GameManager() { }

    [SerializeField] public int targetFrameRate = 60; // Application target frame per second.
    public bool VSYNC = true;

    public static event System.Action<GameState, GameState> GameStateChanged = delegate { };
    public GameMode gameMode { get; set; }
    private GameState _gameState = GameState.Prepare;
    public GameState GameState
    {
        get
        {
            return _gameState;
        }
        private set
        {
            if (value != _gameState)
            {
                GameState oldState = _gameState;
                _gameState = value;

                GameStateChanged(_gameState, oldState);
                
                if(value == GameState.GameOver)
                {
                    GameEnd();
                }
            }
        }
    }


    public ClockController playerClock { get; set; }

    // Prefabs
    [SerializeField] public GameObject Bullet;
    [SerializeField] public GameObject Clock;

    // My Props
    public int bulletSpeed { get ; private set; }
    public int highScore { get; set; }
    public bool  iCanShoot { get; set; }

    private Color _background;
    public Color Background
    {
        get
        {
            return _background;
        }
        private set
        {
            _background = value;
            Camera.main.backgroundColor = Background;
        }
    }

    private void Awake()
    {
        // Set bullet speed
        bulletSpeed = 10;

        // Set background color to Random Color.
        Background = GetRandomColor();

        // VSYNC is enabled.
        if (VSYNC)
        {
            Application.targetFrameRate = targetFrameRate;
        }

    }

    /// <summary>
    /// This function for shoot clock to direction
    /// </summary>
    public void HandleShoot()
    {
        if (playerClock)
        {
            playerClock.isPlayer = false;

            GameObject go = Instantiate(Bullet, playerClock.BulletPosition(), Quaternion.identity);
            go.GetComponent<Rigidbody2D>().velocity = playerClock.ShootDirection() * bulletSpeed;

            ObjectPool.Instance.AddPool(playerClock.gameObject);

            playerClock = null;
        }
    }

    /// <summary>
    /// Spawn new clock on scene
    /// Note: that function called when game mode equals to endless mode.
    /// </summary>
    public void SendNewClock()
    {
        // Get a Clock at ObjectPool
        var newClock = ObjectPool.Instance.GetAtPool();

        // Get a Empty at ObjectGrid
        var clockPosition = ObjectGrid.Instance.EmptyPoint();

        // Apply to scene
        newClock.transform.position = new Vector3(clockPosition.x, clockPosition.y, 0);

        // Set grid id to new clock
        newClock.GetComponent<ClockController>().gridId = clockPosition.z;
    }

    /// <summary>
    /// Retry scene
    /// </summary>
    public void GameEnd()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    Color GetRandomColor()
    {
        var randomColor = Random.ColorHSV(0, 1f, 0, 1f, 0, 1f);
        return randomColor;
    }
    
}
