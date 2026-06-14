using Unity.VisualScripting;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _maxPositionDistance;
    [SerializeField] private float _maxHeight;
    [SerializeField] private float _minHeight;

    private Vector3 _targetPosition;
    private Vector3 _startingPosition;
    private float _distance;
    private float _startTime;

    private void OnEnable()
    {
        GetNewTargetPosition();    }

    // Destroys object upon collision
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        // Lerps to position
       float distCovered = (Time.time - _startTime) * _moveSpeed;
       float fractionOfTravel = distCovered / _distance;

        transform.position = Vector3.Lerp(_startingPosition, _targetPosition, fractionOfTravel);

        // Made it to position
        if (fractionOfTravel >= 1)
        {
            GetNewTargetPosition();
        }
    }

    // Sets a new position for target to move to
    private void GetNewTargetPosition()
    {
        _startingPosition = transform.position;

        Vector3 position = Random.insideUnitSphere * _maxPositionDistance;
        _targetPosition = transform.position + position;

        // Prevents target from going to far up or down
        if (_targetPosition.y > _maxHeight)
        {
            _targetPosition.y = _maxHeight;
        }
        else if (_targetPosition.y < _minHeight)
        {
            _targetPosition.y = _minHeight;
        }

        _distance = Vector3.Distance(_targetPosition, transform.position);
        _startTime = Time.time;
    }
}
