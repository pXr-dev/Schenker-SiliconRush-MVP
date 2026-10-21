using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int maxLives = 3;
    private int currentLives;

    private Vector2 startPosition;

    private void Start()
    {
        currentLives = maxLives;
        startPosition = transform.position;
    }

    public void TakeDamage()
    {
        currentLives--;
        Debug.Log("Ouch! Captain XMG wurde getroffen. Verbleibende Leben: " + currentLives);

        if (currentLives > 0)
        {
            Respawn();
        }
        else
        {
            Debug.Log("Game Over! Keine Leben mehr.");
        }
    }

    private void Respawn()
    {
        transform.position = startPosition;
        Debug.Log("Spieler wurde respawnt.");
    }
}
