using DD.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Environment
{
    public class Boundary : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent<IDemolishable>(out IDemolishable demolisable))
            {
                demolisable.Demolish();
            }
        }
    }
}
