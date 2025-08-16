using UnityEngine;

public class ResourceStorage : MonoBehaviour
{
    public event System.Action ResourceAdded;
    private int _totalCollectedResources = 0;

    public bool TryAddResource()
    {
        _totalCollectedResources++;
        ResourceAdded?.Invoke();

        return true;
    }
}