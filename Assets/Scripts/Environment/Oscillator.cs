using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Environment
{
    public class Oscillator : MonoBehaviour
    {
        [SerializeField] Transform _destination;
        [SerializeField] float _stopDuration = 3f;
        [SerializeField] float _stopTolerance = .2f;
        [SerializeField] float _movementSpeed = 1f;

        Vector3 _startingPosition;
        bool _isMovingToDestination = true;
        bool _isStopped = false;

        void Awake()
        {
            _startingPosition = transform.position;    
        }

        void Update() {

            if (_isStopped) return;

            if (_isMovingToDestination)
            {
                MoveToPosition(_destination.position);
                StopAtDestination(_destination.position);
            }
            else
            {
                MoveToPosition(_startingPosition);
                StopAtDestination(_startingPosition);
            }
            
        }

        void MoveToPosition(Vector3 destinationPosition)
        {
            Vector3 movementDirection = destinationPosition - transform.position;
            transform.Translate(movementDirection.normalized * Time.deltaTime * _movementSpeed);
            
        }

        void StopAtDestination(Vector3 destination)
        {
            if (IsInStoppingDistance(destination))
            {
                _isStopped = true;
                StartCoroutine(WaitAtDestination());
            }
        }

        bool IsInStoppingDistance(Vector3 destination)
        {
            if (Vector3.Distance(transform.position, destination) <= _stopTolerance)
            {
                return true;
            }

            return false;
        }

        IEnumerator WaitAtDestination()
        {
            yield return new WaitForSeconds(_stopDuration);
            _isMovingToDestination = !_isMovingToDestination;
            _isStopped = false;
        }
    }

}