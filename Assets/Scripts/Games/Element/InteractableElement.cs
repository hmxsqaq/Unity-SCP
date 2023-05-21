using System;
using HighlightPlus2D;
using UnityEngine;

namespace Games.Element
{
    public class InteractableElement : MonoBehaviour
    {
        public Action OnFocus;
        public Action OnLeave;

        private HighlightEffect2D _effect;
        private void Awake()
        {
            _effect = GetComponentInParent<HighlightEffect2D>();
            OnFocus += HighlightOn;
            OnLeave += HighlightOff;
        }

        private void HighlightOn()
        {
            _effect.SetTarget(transform);
            _effect.SetHighlighted(true);
        }

        private void HighlightOff()
        {
            _effect.SetTarget(transform);
            _effect.SetHighlighted(false);
        }

        public virtual void OnInteract(){}
    }
}