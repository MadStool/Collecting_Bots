using System.Collections.Generic;
using UnityEngine;

public class ResourceProvider : MonoBehaviour
{
    private readonly List<Resource> _availableResources = new List<Resource>();
    private readonly HashSet<Resource> _assignedResources = new HashSet<Resource>();

    public void RegisterResource(Resource resource)
    {
        if (resource == null || _assignedResources.Contains(resource) || _availableResources.Contains(resource))
            return;

        _availableResources.Add(resource);
    }

    public bool TryGetAvailableResource(out Resource resource)
    {
        _availableResources.RemoveAll(item => item == null);
        _assignedResources.RemoveWhere(item => item == null);

        foreach (Resource availableResource in _availableResources)
        {
            if (_assignedResources.Contains(availableResource) == false)
            {
                resource = availableResource;
                _availableResources.Remove(resource);
                _assignedResources.Add(resource);

                return true;
            }
        }

        resource = null;
        return false;
    }

    public void ReleaseResource(Resource resource)
    {
        if (resource == null)
            return;

        _assignedResources.Remove(resource);

        if (resource.gameObject != null)
            _availableResources.Add(resource);
    }

    public void RemoveResource(Resource resource)
    {
        if (resource == null)
            return;

        _availableResources.Remove(resource);
        _assignedResources.Remove(resource);
        Destroy(resource.gameObject);
    }
}