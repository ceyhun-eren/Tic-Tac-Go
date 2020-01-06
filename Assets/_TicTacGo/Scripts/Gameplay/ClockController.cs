using UnityEngine;

public class ClockController : MonoBehaviour
{
    public bool isPlayer = false;
    public float gridId = 0;

    public GameObject Circle;
    public GameObject Arrow;
    public GameObject Clockwise;

    [Range(100f,500f)]
    public float clockSpeed;

    private void Awake()
    {
        // Set clock size ClockType.Big : ClockType.Medium : ClockType.Small
        SetClockSize();

        // ClockWise's color equals to background color
        var spriteClockwise = Clockwise.GetComponent<SpriteRenderer>();
        spriteClockwise.color = GameManager.Initializing._Background;

        // Set child is disabled.
        Arrow.SetActive(false);
        Circle.SetActive(false);

        // Get random value for clockSpeed
        clockSpeed = Random.Range(100f, 500f);
    }

    private void Update()
    {
        // Move Clockwise when is not player.
        MoveClockwise();
    }

    void SetClockSize()
    {
        float scale = Random.Range(0.5f, 0.72f);
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
    public void MakePlayer()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();

        if (isPlayer)
        {
            spriteRenderer.color = Color.black;

            var gManager = GameManager.Initializing;
            gManager.playerClock = this;
            gManager.iCanShoot = true;
            gManager.SendNewClock();
        }
        else
        {
            spriteRenderer.color = Color.white;
        }

        Arrow.SetActive(isPlayer);
        Circle.SetActive(isPlayer);
        Clockwise.SetActive(!isPlayer);

        Debug.Log("NewPlayer is Me");
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

            MakePlayer();
        }
    }

}
