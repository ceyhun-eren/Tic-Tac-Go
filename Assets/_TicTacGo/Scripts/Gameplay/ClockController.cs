using UnityEngine;

public enum ClockType
{
    non,
    Big,
    Medium,
    Small
}

public class ClockController : MonoBehaviour
{
    // Get Game Manager
    private GameManager gameManager;
    
    
    public float gridId = 0;
    public bool isPlayer = false;
    private bool onChange = false;
    
    // for change of clock type
    public GameObject Circle;
    public GameObject Arrow;
    public SpriteRenderer Clockwise;
    public SpriteRenderer ClockBackground;

    [Range(100f, 500f)]
    public float clockSpeed;

    private void Awake()
    {
        gameManager = GameManager.Instance;

        // Set clock size ClockType.Big : ClockType.Medium : ClockType.Small
        SetClockSize();

        OnChangeClockType();

        // Get random value for clockSpeed
        clockSpeed = Random.Range(100f, 500f);
    }

    private void Update()
    {
        
        if(!onChange)
        {
            // Move Clockwise.
            MoveClockwise();
        }

    }

    void SetClockSize()
    {
        float scale = Random.Range(0.45f, 0.72f);
        transform.localScale = new Vector3(scale, scale, scale);
    }

    /// <summary>
    /// This function make loop for Clockwise object
    /// </summary>
    void MoveClockwise()
    {
        if (!isPlayer)
        {
            Clockwise.transform.eulerAngles += new Vector3(0, 0, -1) * clockSpeed * Time.deltaTime;
        }
        else
        {
            Arrow.transform.eulerAngles += new Vector3(0, 0, -1) * clockSpeed * Time.deltaTime;
        }
    }

    /// <summary>
    /// This function make player this
    /// </summary>
    public void RefreshStatus()
    {
        OnChangeClockType();

        ClockBackground.color = Color.black;

        gameManager.playerClock = this;
        gameManager.iCanShoot = true;

        gameManager.SendNewClock();
    }

    /// <summary>
    /// Get Bullet position.
    /// </summary>
    /// <returns></returns>
    public Vector2 BulletPosition()
    {
        return Arrow.transform.GetChild(0).transform.position;
    }

    /// <summary>
    /// Get Shoot direction.
    /// </summary>
    /// <returns></returns>
    public Vector2 ShootDirection()
    {
        return (Arrow.transform.GetChild(0).transform.position - Arrow.transform.position).normalized;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") && !isPlayer)
        {
            Destroy(collision.gameObject);

            isPlayer = true;

            RefreshStatus();
        }
    }

    private void OnEnable()
    {
        GameManager.GameStateChanged += OnGameStateChanged;
    }

    /// <summary>
    /// I will use that when adding to objects pool.
    /// Note : Only endless mode.
    /// </summary>
    private void OnDisable()
    {
        OnChangeClockType();
        GameManager.GameStateChanged -= OnGameStateChanged;

        ClockBackground.color = Color.white;

        /*Arrow.SetActive(isPlayer);
        Circle.SetActive(isPlayer);
        Clockwise.gameObject.SetActive(!isPlayer);*/
    }

    private void OnGameStateChanged(GameState newState, GameState oldState)
    {
        if(newState == GameState.Paused || newState == GameState.PreGameOver || newState == GameState.GameOver)
        {
            onChange = true;
        }
        else
        {
            onChange = false;
        }
    }

    void OnChangeClockType()
    {
        Arrow.SetActive(!Arrow.activeSelf);
        Circle.SetActive(!Circle.activeSelf);
        Clockwise.gameObject.SetActive(!Clockwise.gameObject.activeSelf);
    }

}
