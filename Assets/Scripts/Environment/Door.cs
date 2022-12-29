using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Environment
{
    public class Door : MonoBehaviour
    {
        [SerializeField] Transform _targetPosition;
        [SerializeField] float _openingSpeed = 1f;
        [SerializeField] float _closingSpeed = 1f;

        Vector2 _startingPosition;
        bool _isMoving = false;
        void Awake()
        {
            _startingPosition = transform.position;
        }

        public void Open()
        {
            if (!_isMoving)
            {
                StartCoroutine(OpenAndCloseDoor());
            }
        }

        IEnumerator OpenAndCloseDoor()
        {
            float step = _openingSpeed * Time.deltaTime;

            _isMoving = true;

            while (Vector2.Distance(_targetPosition.position, transform.position) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, _targetPosition.position, step);
                yield return new WaitForEndOfFrame();
            }
            while (Vector2.Distance(_startingPosition, transform.position) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, _startingPosition, step);
                yield return new WaitForEndOfFrame();
            }

            _isMoving = false;
        }
    }
}
