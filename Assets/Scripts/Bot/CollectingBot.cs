using UnityEngine;

[RequireComponent(typeof(MoverToTarget))]
public class CollectingBot : MonoBehaviour
{
    private Transform _basePosition;
    private Resource _currentResource;
    private MoverToTarget _mover;
    private bool _hasPickedUpResource;

    public bool IsFree => _currentResource == null;
    public bool HasResource => _currentResource != null && _hasPickedUpResource;

    public void Initialize(Transform basePosition)
    {
        _basePosition = basePosition;
        _mover = GetComponent<MoverToTarget>();
        _hasPickedUpResource = false;
    }

    public void AssignResource(Resource resource)
    {
        if (resource == null || resource.gameObject == null)
            return;

        _currentResource = resource;
        _hasPickedUpResource = false;
        _mover.SetTarget(resource.transform);
    }

    public Resource TakeResource()
    {
        Resource resource = _currentResource;
        _currentResource = null;
        _hasPickedUpResource = false;

        return resource;
    }

    public void ReturnToBase()
    {
        _mover.SetTarget(_basePosition);
    }

    private void Update()
    {
        if (_currentResource != null)
        {
            if (_currentResource.gameObject == null)
            {
                _currentResource = null;
                ReturnToBase();

                return;
            }

            if (_hasPickedUpResource == false)
            {
                float distance = Vector3.Distance(transform.position, _currentResource.transform.position);

                if (distance <= 1.5f)
                    PickUpResource();
            }
        }
    }

    private void PickUpResource()
    {
        if (_currentResource == null)
            return;

        _currentResource.transform.SetParent(transform);
        _currentResource.transform.localPosition = Vector3.up * 0.5f;
        _hasPickedUpResource = true;
        _mover.SetTarget(_basePosition);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == _basePosition && HasResource)
        {
            BotRetriever retriever = _basePosition.GetComponent<BotRetriever>();
            retriever?.HandleBotArrival(this);
        }
    }
}