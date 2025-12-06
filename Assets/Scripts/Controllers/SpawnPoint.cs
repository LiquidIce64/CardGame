using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SpawnPoint : MonoBehaviour
{
    private RectTransform rectTransform;

    private void OnDrawGizmos()
    {
        rectTransform = GetComponent<RectTransform>();
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, rectTransform.rect.size);
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Spawn(GameObject prefab)
    {
        float xRange = Mathf.Abs(rectTransform.rect.width / 2);
        float yRange = Mathf.Abs(rectTransform.rect.height / 2);
        Vector3 posOffset = new(
            Random.Range(-xRange, xRange),
            Random.Range(-yRange, yRange),
            0f
        );
        Instantiate(prefab, rectTransform.position + posOffset, Quaternion.identity);
    }
}
