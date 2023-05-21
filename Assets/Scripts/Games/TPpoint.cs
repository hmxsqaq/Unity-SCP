using System;
using Games.Cameras;
using Games.Protagonist;
using UnityEngine;

namespace Games
{
    public class TPpoint : MonoBehaviour
    {
        public enum Room
        {
            Room0,
            Room173,
            Room682
        }
        public Vector2 pos;
        public Room room;

        private float[] _limit;
        private GameObject _protagonist;
        private GameObject _camera;

        private void Start()
        {
            _protagonist = GameObject.Find("Protagonist");
            _camera = GameObject.Find("Camera");
            switch (room)
            {
                case Room.Room0:
                    _limit = ProtagonistInfo.Camera1;
                    break;
                case Room.Room173:
                    _limit = ProtagonistInfo.Camera2;
                    break;
                case Room.Room682:
                    _limit = ProtagonistInfo.Camera3;
                    break;
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                _protagonist.transform.position = pos;
                _camera.GetComponent<CameraFollow>().CameraLimitSwitch(_limit);
            }
        }
    }
}