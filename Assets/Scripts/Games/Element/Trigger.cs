using Framework;
using UnityEngine;
using AudioType = Framework.AudioType;

namespace Games.Element
{
    public class Trigger : MonoBehaviour
    {
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                AudioCenter.Instance.AudioPause(AudioType.Effect,"Walk");
                ScenesManager.Instance.LoadSceneSync(SceneType.end);
            }
        }
    }
}