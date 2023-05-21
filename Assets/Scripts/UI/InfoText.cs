using System;
using Games.Protagonist;
using TMPro;
using UnityEngine;

namespace UI
{
    public class InfoText : MonoBehaviour
    {
        private TextMeshProUGUI _mesh;
        private float _t;

        private void Start()
        {
            _mesh = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            _t += Time.deltaTime * 0.5f;
            float a = Mathf.Lerp(1f, 0f, _t);
            Color color = _mesh.color;
            color.a = a;
            _mesh.color = color;
            if (a < 0.05f)
            {
                _t = 0;
                gameObject.SetActive(false);
            }
        }
    }
}