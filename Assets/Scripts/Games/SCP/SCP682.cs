using System;
using Framework;
using UnityEngine;
using EventType = Framework.EventType;

namespace Games.SCP
{
    public class SCP682 : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                EventCenter.Instance.Trigger(EventType.GameOver);
            }
        }
    }
}