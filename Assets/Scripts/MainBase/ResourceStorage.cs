using UnityEngine;

public class ResourceStorage : MonoBehaviour
{
    public event System.Action<int> ResourceAdded;

    private int _totalCollectedResources = 0;

    public void AddResource()
    {
        _totalCollectedResources++;
        ResourceAdded?.Invoke(_totalCollectedResources);
    }
}