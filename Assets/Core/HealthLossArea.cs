using Orkanoid.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orkanoid.Core
{
    public class HealthLossArea : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Ball ball))
            {
                Debug.Log("YOU LOST A LIFE!");
            }
        }
    }
}
