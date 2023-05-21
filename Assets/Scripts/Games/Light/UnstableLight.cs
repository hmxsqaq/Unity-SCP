using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Games.Light
{
    // 用来让手电筒时不时闪一下
    [RequireComponent(typeof(Animation))]
    public class UnstableLight : MonoBehaviour
    {
        public List<AnimationClip> clips;
        private Animation _animation;

        private void Start()
        {
            _animation = GetComponent<Animation>();
            StartCoroutine(Unstable());
        }

        private void Fluctuate()
        {
            _animation.clip = clips[Random.Range(0, clips.Count)];
            _animation.Play();
        }

        IEnumerator Unstable()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(5f,10f));
                Fluctuate();
            }
        }
    }
}