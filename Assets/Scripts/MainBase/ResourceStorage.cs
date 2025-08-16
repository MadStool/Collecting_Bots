using UnityEngine;

public class ResourceStorage : MonoBehaviour
{
    [SerializeField] private int _maxResourcesCount = 10;

    private int _totalResources = 0;

    public int TotalResources => _totalResources;
    public bool IsFull => _totalResources >= _maxResourcesCount;

    public bool TryAddResource(int amount)
    {
        if (_totalResources + amount > _maxResourcesCount)
        {
            Debug.Log($"I can't add {amount} of resources (╥_╥). The storage is complete: {_totalResources}/{_maxResourcesCount}");

            return false;
        }

        _totalResources += amount;
        Debug.Log($"Resources have been added! Now: {_totalResources}/{_maxResourcesCount}");

        return true;
    }

    public bool CanAddResource(int amount)
    {
        return _totalResources + amount <= _maxResourcesCount;
    }
}