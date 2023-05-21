using System;
using Framework;
using Games.Protagonist;
using UnityEngine;
using EventType = Framework.EventType;

namespace Games.Cameras
{
    public class CameraFollow : MonoBehaviour
    {
        public float speed;
        
        private Vector2 _xLimit;
        private Vector2 _yLimit;
        private Transform _protagonistPos;
        private Vector3 _pos;

        public void CameraLimitSwitch(float[] limit)
        {
            _xLimit = new Vector2(limit[0], limit[1]);
            _yLimit = new Vector2(limit[2], limit[3]);
        }
        
        private void Start()
        {
            _protagonistPos = GameObject.Find("Protagonist").transform;
            CameraLimitSwitch(ProtagonistInfo.Camera1);
            EventCenter.Instance.AddEventListener(EventType.Restart,ReSetCamera);
        }

        private void OnDestroy()
        {
            EventCenter.Instance.RemoveEventListener(EventType.Restart,ReSetCamera);
        }

        private void Update()
        {
            // 插值跟随
            _pos = transform.position;
            if (_pos != _protagonistPos.position)
            {
                _pos = Vector3.Lerp(_pos, _protagonistPos.position, speed * Time.deltaTime);
            }
            _pos.z = -10;
            _pos.x = Mathf.Clamp(_pos.x, _xLimit.x, _xLimit.y);
            _pos.y = Mathf.Clamp(_pos.y, _yLimit.x, _yLimit.y);
            transform.position = _pos;
        }

        private void ReSetCamera()
        {
            CameraLimitSwitch(ProtagonistInfo.Camera1);
        }
    }
}