using Games.SCP;
using UnityEngine;

namespace Games.Protagonist
{
    public class Check173 : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("173"))
            {
                other.GetComponentInParent<SCP173>().IsStaring();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("173"))
            {
                other.GetComponentInParent<SCP173>().IsNotStaring();
            }
        }
    }
}