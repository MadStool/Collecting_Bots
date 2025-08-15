using System.Collections.Generic;
using UnityEngine;

public class ResourceStorage : MonoBehaviour
{
    [SerializeField] private int _maxResourcesCount = 10;

    private int _totalResources = 0;
    private List<Resource> _availableResources = new List<Resource>();

    public int TotalResources => _totalResources;
    public bool IsFull => _totalResources >= _maxResourcesCount;
    public bool HasAvailableResources => IsFull == false && GetUnassignedResource() != null;

    public void UpdateResources(List<Resource> foundResources)
    {
        if (IsFull)
        {
            Debug.Log($"The storage is full! {_totalResources}/{_maxResourcesCount}");

            return;
        }

        foreach (Resource resource in foundResources)
        {
            if (_availableResources.Contains(resource) == false)
                _availableResources.Add(resource);
        }
        _availableResources.RemoveAll(item => item == null || item.IsAssigned);
    }

    public bool TryAddResource(int amount)
    {
        if (_totalResources + amount > _maxResourcesCount)
        {
            Debug.Log($"I can't add {amount} of resources (╥_╥). The storage is almost full: {_totalResources}/{_maxResourcesCount}");

            return false;
        }

        _totalResources += amount;
        Debug.Log($"Resources collected: {_totalResources}/{_maxResourcesCount}");

        return true;
    }

    public Resource GetUnassignedResource() => _availableResources.Find(item => item != null && item.IsAssigned == false);
    public void MarkResourceAsAssigned(Resource resource)
    {
        if (resource != null && resource.TryAssign() == false)
            Debug.LogWarning("Failed to assign resource - already assigned or destroyed");
    }
}