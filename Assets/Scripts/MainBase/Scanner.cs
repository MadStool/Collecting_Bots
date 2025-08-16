using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private Vector3 _scanAreaSize = new Vector3(20, 2, 20);
    [SerializeField] private LayerMask _resourceLayer;

    private Collider[] _collidersBuffer = new Collider[20];

    public void Scan(ResourceProvider provider)
    {
        int count = Physics.OverlapBoxNonAlloc(
            transform.position,
            _scanAreaSize / 2f,
            _collidersBuffer,
            Quaternion.identity,
            _resourceLayer
        );

        for (int i = 0; i < count; i++)
        {
            if (_collidersBuffer[i].TryGetComponent<Resource>(out var resource))
            {
                provider.RegisterResource(resource);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, _scanAreaSize);
    }
}