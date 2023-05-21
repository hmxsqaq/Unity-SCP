using Framework;
using Games.Protagonist;
using UnityEngine;
using AudioType = Framework.AudioType;

namespace Games.Element
{
    public class Door : InteractableElement
    {
        public Sprite openSprite;
        public Sprite colseSprite;
        public GameObject infoUI;

        private bool _isOpen;

        public override void OnInteract()
        {
            if (ProtagonistInfo.HasCard)
            {
                AudioCenter.Instance.AudioPlaySync(new AudioAsset(AudioType.Effect,"Open"));
                if (_isOpen)
                {
                    GetComponent<SpriteRenderer>().sprite = colseSprite;
                    transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
                    _isOpen = false;
                }
                else
                {
                    GetComponent<SpriteRenderer>().sprite = openSprite;
                    transform.GetChild(0).GetComponent<Collider2D>().enabled = true;
                    _isOpen = true;
                }
            }
            else
            {
                infoUI.SetActive(true);
            }
        }
    }
}