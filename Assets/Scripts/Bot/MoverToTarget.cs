using UnityEngine;

public class MoverToTarget : MonoBehaviour
{
    private Transform _target;
    private float _speed = 3f;
    private float _rotationSpeed = 5f;

    public void SetTarget(Transform targetTransform)
    {
        _target = targetTransform;
    }

    private void Update()
    {
        if (_target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);

            Vector3 direction = (_target.position - transform.position).normalized;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            }
        }
    }
}