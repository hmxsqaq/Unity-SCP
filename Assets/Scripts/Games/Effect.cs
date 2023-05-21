using System;
using Framework;
using UnityEngine;
using AudioType = Framework.AudioType;

namespace Games
{
    public class Effect : MonoBehaviour
    {
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                AudioCenter.Instance.AudioPlaySync(new AudioAsset(AudioType.Effect,"monster-breath"));
            }
        }
    }
}