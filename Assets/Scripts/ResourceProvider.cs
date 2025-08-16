using System.Collections.Generic;
using UnityEngine;

public class ResourceProvider : MonoBehaviour
{
    private readonly List<Resource> _availableResources = new List<Resource>();
    private readonly HashSet<Resource> _assignedResources = new HashSet<Resource>();

    public void RegisterResource(Resource resource)
    {
        if (resource == null || _assignedResources.Contains(resource))
            return;

        if (_availableResources.Contains(resource) == false)
            _availableResources.Add(resource);
    }

    public bool TryAssignResource(out Resource resource)
    {
        resource = null;

        for (int i = _availableResources.Count - 1; i >= 0; i--)
        {
            Resource current = _availableResources[i];

            if (current == null)
            {
                _availableResources.RemoveAt(i);
                continue;
            }

            if (_assignedResources.Contains(current) == false)
            {
                resource = current;
                _availableResources.RemoveAt(i);
                _assignedResources.Add(resource);

                return true;
            }
        }

        return false;
    }

    public void ReleaseResource(Resource resource)
    {
        if (resource == null)
            return;

        _assignedResources.Remove(resource);
        RegisterResource(resource);
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