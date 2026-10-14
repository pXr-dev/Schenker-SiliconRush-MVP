using UnityEngine;

public class BombDropper : MonoBehaviour
{
    [Header("Bomb Settings")]
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private int maxBombs = 1;

    private int currentBombCount = 0;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentBombCount < maxBombs)
        {
            DropBomb();
        }
    }

    private void DropBomb()
    {
        if (bombPrefab == null)
        {
            Debug.LogWarning("Kein Bomb Prefab im Inspector von BombDropper");
            return;
        }

        // 1. Position auf das Raster runden
        Vector2 spawnPos = new Vector2(
            Mathf.Round(transform.position.x),
            Mathf.Round(transform.position.y)
        );

        // 2. Bombe genau einmal an der gerundeten Position erzeugen
        GameObject spawnedBomb = Instantiate(bombPrefab, spawnPos, Quaternion.identity);
        
        // 3. Spieler-Referenz übergeben
        BombController controller = spawnedBomb.GetComponent<BombController>();
        if (controller != null)
        {
            controller.SetPlayerReference(this);
        }

        currentBombCount++;
    }

    public void OnBombExploded()
    {
        currentBombCount = Mathf.Max(0, currentBombCount - 1);
    }
}