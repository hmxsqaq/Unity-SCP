using Games.Protagonist;
using Framework;

namespace Games.Element
{
    public class Card : InteractableElement
    {
        public override void OnInteract()
        {
            AudioCenter.Instance.AudioPlaySync(new AudioAsset(AudioType.Effect,"Pickup"));
            ProtagonistInfo.HasCard = true;
            Destroy(gameObject);
        }
    }
}