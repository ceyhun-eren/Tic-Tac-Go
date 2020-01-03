using UnityEngine;

public class WallController : MonoBehaviour
{
    private PhysicsMaterial2D ballisticMat;
    private bool isBallistic;
    private GameMode gameMode;

    // Start is called before the first frame update
    void Start()
    {
        gameMode = GameManager.Initializing.gameMode;
        if(gameMode == GameMode.Carrier)
        {
            // Enabled ballistic
            isBallistic = true;

            // Set visible wall
            gameObject.SetActive(true);

            // Set physics material
            var myCollider = GetComponent<BoxCollider2D>();
            myCollider.sharedMaterial = ballisticMat;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            GameManager.Initializing.GameEnd();
            //Debug.Log("Game Over...");
        }
    }

}
