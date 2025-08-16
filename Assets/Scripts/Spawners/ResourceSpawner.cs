using UnityEngine;
using System.Collections;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private Resource _resourcePrefab;
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private float _spawnRadius = 4f;
    [SerializeField] private Color _gizmoColor = Color.green;

    private Coroutine _spawningCoroutine;
    private bool _isSpawningActive;

    public void StartSpawning()
    {
        if (_spawningCoroutine != null)
            StopCoroutine(_spawningCoroutine);

        _isSpawningActive = true;
        _spawningCoroutine = StartCoroutine(SpawnResources());
    }

    public void StopSpawning()
    {
        _isSpawningActive = false;

        if (_spawningCoroutine != null)
        {
            StopCoroutine(_spawningCoroutine);
            _spawningCoroutine = null;
        }
    }

    private IEnumerator SpawnResources()
    {
        while (_isSpawningActive)
        {
            yield return new WaitForSeconds(_spawnInterval);

            if (_isSpawningActive)
                SpawnResource();
        }
    }

    private void SpawnResource()
    {
        Vector3 spawnPosition = transform.position + new Vector3(
            Random.Range(-_spawnRadius, _spawnRadius),
            0,
            Random.Range(-_spawnRadius, _spawnRadius)
        );
        Resource resource = Instantiate(_resourcePrefab, spawnPosition, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        Gizmos.DrawWireSphere(transform.position, _spawnRadius);
    }

    private void OnDisable() => StopSpawning();
    private void OnDestroy() => StopSpawning();
}