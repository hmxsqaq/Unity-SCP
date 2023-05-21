using Framework;
using Games.Protagonist;
using UnityEngine;
using AudioType = Framework.AudioType;

namespace Games.Element
{
    public class Flashlight : InteractableElement
    {
        public GameObject panel;
        public GameObject lighting2;

        public override void OnInteract()
        {
            AudioCenter.Instance.AudioPlaySync(new AudioAsset(AudioType.Effect,"Pickup"));
            panel.SetActive(true);
            lighting2.SetActive(false);
            ProtagonistInfo.HasFlashlight = true;
            Destroy(this.gameObject);
        }
    }
}