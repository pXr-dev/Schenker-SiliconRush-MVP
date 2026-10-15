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
        // 1. Visuellen Effekt instanziieren
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, pos, Quaternion.identity);
        }

        // 2. Kollisionsprüfung mit Radius
        Collider2D hitCollider = Physics2D.OverlapCircle(pos, 0.4f);

        if (hitCollider != null)
        {
            // Prüfen, ob es eine zerstörbare Wand ist
            if (hitCollider.CompareTag("DestructibleWall"))
            {
                Destroy(hitCollider.gameObject);
                return true; // Welle stoppen!
            }

            // Falls es der Spieler ist
            if (hitCollider.CompareTag("Player"))
            {
                // Später: Schaden / Leben abziehen
            }
        }

        return false; // Welle breitet sich weiter aus
    }
}
