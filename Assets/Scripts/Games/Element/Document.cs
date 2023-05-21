using System;
using Framework;
using Games.Protagonist;
using UnityEngine;
using UnityEngine.Serialization;
using AudioType = Framework.AudioType;

namespace Games.Element
{
    public enum SCP
    {
        SCP173,
        SCP682
    }
    
    public class Document : InteractableElement
    {
        public SCP info;

        public GameObject panel;

        public override void OnInteract()
        {
            AudioCenter.Instance.AudioPlaySync(new AudioAsset(AudioType.Effect,"Document"));
            panel.SetActive(true);
            GameObject.Find("Protagonist").GetComponent<ProtagonistController>().ActionMap.Disable();
        }
    }
}