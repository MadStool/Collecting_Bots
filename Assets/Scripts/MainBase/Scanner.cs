using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private Vector3 _scanAreaSize = new Vector3(20, 2, 20);
    [SerializeField] private LayerMask _resourceLayer;

    public List<Resource> FindResourcesInCollectionArea()
    {
        Vector3 scanCenter = transform.position;
        Vector3 halfScanSize = _scanAreaSize / 2f;

        Collider[] potentialResourceColliders = Physics.OverlapBox(
            scanCenter,
            halfScanSize,
            Quaternion.identity,
            _resourceLayer
        );

        List<Resource> foundResources = new List<Resource>(potentialResourceColliders.Length);

        foreach (var collider in potentialResourceColliders)
        {
            Resource resource = collider.GetComponent<Resource>();

            if (resource != null)
                foundResources.Add(resource);
        }

        return foundResources;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, _scanAreaSize);
    }
}