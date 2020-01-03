using UnityEngine;

public enum GameMode
{
    non,
    Carrier,
    Endless,
}
    
public enum DifficultyLevel
{
    non,
    Beginner,
    Intermediate,
    Advanced,
}

[RequireComponent(typeof(ObjectGrid))]
public class GameManager : MonoBehaviour
{

    [SerializeField] public int targetFrameRate = 60; // Application target frame per second.

    public GameMode gameMode { get; set; }
    public ClockController playerClock { get; set; }

    // Prefabs
    public GameObject Bullet;
    //public GameObject Clock;

    // My Props
    public int bulletSpeed { get ; private set; }
    public int highScore { get; set; }
    public bool  iCanShoot { get; set; }

    private Color Background;
    public Color _Background 
    {
        get
        {
            return Background;
        }
        private set
        {
            Background = value;
            Camera.main.backgroundColor = Background;
        }
    }

    // Singleton
    private static GameManager _instance;
    public static GameManager Initializing
    {
        get
        {
            if (!_instance)
            {
                GameObject gManager = new GameObject("GameManager");
                gManager.AddComponent<GameManager>();
            }
            return _instance;
        }
    }
    private void Awake()
    {
        // Set bullet speed
        bulletSpeed = 10;

        // Set background color to Random Color.
        _Background = GetRandomColor();

        // VSYNC is enabled.
        Application.targetFrameRate = targetFrameRate;

        // Set _Instance to this
        if(!_instance)
        {
            _instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("GameManager Destroying...");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            SendNewClock();
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

            ObjectPool.Initializing.AddPool(playerClock.gameObject);
            //Eksik yer

            playerClock = null;
        }
    }

    /// <summary>
    /// Spawn new clock on scene
    /// </summary>
    public void SendNewClock()
    {
        // Get a Clock at ObjectPool
        var newClock = ObjectPool.Initializing.GetAtPool();

        // Get a Empty at ObjectGrid
        var clockPosition = ObjectGrid.Initializing.EmptyPoint();

        // Apply on scene
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
