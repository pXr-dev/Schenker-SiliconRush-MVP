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
        Vector2 currentPos = transform.position;
        Vector3 snapPosition = new Vector3(
            Mathf.Round(currentPos.x),
            Mathf.Round(currentPos.y),
            0f
        );

        if (bombPrefab != null)
        {
            GameObject bomb = Instantiate(bombPrefab, snapPosition, Quaternion.identity);

            BombController controller = bomb.GetComponent<BombController>();
            if (controller != null)
            {
                controller.SetPlayerReference(this);
            }
                      
            currentBombCount++;
        }
        else
        {
            Debug.LogWarning("Kein Bomb Prefab im Inspector von BombDropper zugewiesen!");
        }
    }

    public void OnBombExploded()
    {
        currentBombCount = Mathf.Max(0, currentBombCount - 1);
    }
}