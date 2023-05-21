using Games.Protagonist;
using UnityEngine;

namespace UI
{
    public class GameContinue : MonoBehaviour
    {
        public void Continue()
        {
            GameObject.Find("Protagonist").GetComponent<ProtagonistController>().ActionMap.Enable();
        }
    }
}