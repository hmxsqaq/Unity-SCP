using Framework;
using Games.Protagonist;

namespace Games.Element
{
    public class DeathHand : InteractableElement
    {
        public override void OnInteract()
        {
            AudioCenter.Instance.AudioPlaySync(new AudioAsset(AudioType.Effect,"Pickup"));
            ProtagonistInfo.HasHand = true;
            Destroy(gameObject);
        }
    }
}