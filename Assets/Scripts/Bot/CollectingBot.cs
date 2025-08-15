using UnityEngine;

[RequireComponent(typeof(MoverToTarget))]
public class CollectingBot : MonoBehaviour
{
    private Transform _basePosition;
    private Resource _assignedResource;   
    private MoverToTarget _mover;

    public bool IsFree { get; private set; } = true;
    private bool _carryingResource = false;

    private System.Action<int> _onResourceDelivered;

    public void Initialize(Transform basePosition, System.Action<int> onResourceDelivered)
    {
        _basePosition = basePosition;
        _onResourceDelivered = onResourceDelivered;
        _mover = GetComponent<MoverToTarget>();
        ResetBot();
    }

    public void AssignResource(Resource resource)
    {
        if (resource == null)
            return;

        _assignedResource = resource;
        IsFree = false;
        _carryingResource = false;
        _mover.SetTarget(resource.transform);
    }

    private void Update()
    {
        if (_assignedResource == null)
        {
            if (IsFree == false)
                ResetBot();

            return;
        }

        if (_carryingResource == false)
        {
            if (_assignedResource == null || _assignedResource.gameObject == null)
            {
                ResetBot();

                return;
            }

            float distanceToResource = Vector3.Distance(transform.position, _assignedResource.transform.position);

            if (distanceToResource <= 1f)
            {
                PickUp();
            }
        }
        else
        {
            if (_basePosition == null)
                return;

            float distToBase = Vector3.Distance(transform.position, _basePosition.transform.position);

            if (distToBase <= 1f)
                Deliver();
        }
    }

    private void ResetBot()
    {
        if (_assignedResource != null)
            _assignedResource.Unassign();

        _assignedResource = null;
        _carryingResource = false;
        IsFree = true;

        if (_basePosition != null && _mover != null)
            _mover.SetTarget(_basePosition.transform);
    }

    private void PickUp()
    {
        if (_assignedResource == null)
            return;

        _assignedResource.transform.SetParent(transform);
        _assignedResource.transform.localPosition = Vector3.up * 0.5f;
        _mover.SetTarget(_basePosition.transform);
        _carryingResource = true;
    }

    private void Deliver()
    {
        if (_assignedResource == null || _onResourceDelivered == null)
        {
            ResetBot();
            return;
        }

        Resource resourceToDeliver = _assignedResource;
        int amount = resourceToDeliver.Amount;

        _assignedResource = null;
        _carryingResource = false;
        IsFree = true;

        if (resourceToDeliver.gameObject != null)
            Destroy(resourceToDeliver.gameObject);

        _onResourceDelivered.Invoke(amount);
    }
}