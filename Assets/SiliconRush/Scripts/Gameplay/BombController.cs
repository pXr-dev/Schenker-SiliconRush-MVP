using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class BombController : MonoBehaviour
{
    [Header("Bomb Timing & Range")]
    [SerializeField] private float fuseTime = 2.5f;
    [SerializeField] private int explosionRange = 2;

    [Header("Visual Effects")]
    [SerializeField] private GameObject explosionEffectPrefab;

    [Header("Audio")]
    [SerializeField] private AudioClip explosionSound;
    
    private BombDropper playerBombDropper;

    public void SetPlayerReference(BombDropper dropper)
    {
        playerBombDropper = dropper;
    }

    private void Start()
    {
        StartCoroutine(ExplosionCountdown());
    }

    private IEnumerator ExplosionCountdown()
    {
        yield return new WaitForSeconds(fuseTime);
        Explode();
    }

    private void Explode()
    {
        Debug.Log("BOOM! Kühl-Bombe detoniert!");

        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        }

        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        ProcessExplosionAtPosition(transform.position);

        foreach (Vector2 dir in directions)
        {
            for (int i = 1; i <= explosionRange; i++)
            {
                Vector2 checkPos = (Vector2)transform.position + dir * i;

                bool hitSomething = ProcessExplosionAtPosition(checkPos);

                if (hitSomething)
                {
                    break;
                }
            }
        }

        if (playerBombDropper != null)
        {
            playerBombDropper.OnBombExploded();
        }

        Destroy(gameObject);
    }

    private bool ProcessExplosionAtPosition(Vector2 pos)
    {
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, pos, Quaternion.identity);
        }

        Collider2D hitCollider = Physics2D.OverlapCircle(pos, 0.3f);

        if (hitCollider != null)
        {
            if (hitCollider.CompareTag("DestructibleWall"))
            {
                Debug.Log("Zerstörbare Wand getroffen und zerstört!");
                Destroy(hitCollider.gameObject);
                return true;
            }

            if (hitCollider.CompareTag("Enemy"))
            {
                Debug.Log("Gegner wurde durch Explosion vernichtet!");
                Destroy(hitCollider.gameObject);
                return true;
            }

            if (hitCollider.CompareTag("Player"))
            {
                Debug.Log("Captain XMG wurde getroffen!");

                PlayerHealth playerHealth = hitCollider.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage();
                }
                
                return true;
            }
        }

        return false;
    }
}
