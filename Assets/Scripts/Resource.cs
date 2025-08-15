using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private int _amount = 1;
    private bool _isAssigned = false;

    public int Amount => _amount;
    public bool IsAssigned => _isAssigned;

    public bool TryAssign()
    {
        if (_isAssigned || gameObject == null)
            return false;

        _isAssigned = true;

        return true;
    }

    public void Unassign()
    {
        _isAssigned = false;
    }

    public bool CanBeCollected()
    {
        return _isAssigned == false && gameObject != null;
    }
}