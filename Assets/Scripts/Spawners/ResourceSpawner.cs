using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private Resource _resourcePrefab;
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private float _spawnRadius = 4f;
    [SerializeField] private Color _gizmoColor = Color.green;

    public System.Action<Resource> OnResourceSpawned;

    public void StartSpawning()
    {
        InvokeRepeating(nameof(SpawnResource), _spawnInterval, _spawnInterval);
    }

    private void SpawnResource()
    {
        Vector3 spawnPosition = transform.position + new Vector3(
            Random.Range(-_spawnRadius, _spawnRadius),
            0,
            Random.Range(-_spawnRadius, _spawnRadius)
        );
        Resource resource = Instantiate(_resourcePrefab, spawnPosition, Quaternion.identity);
        OnResourceSpawned?.Invoke(resource);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        Gizmos.DrawWireSphere(transform.position, _spawnRadius);
    }
}