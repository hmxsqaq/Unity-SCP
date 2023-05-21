using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;
using EventType = Framework.EventType;
using Random = UnityEngine.Random;

namespace Games.SCP
{
    public class SCP173 : MonoBehaviour
    {
        private enum SCP173States
        {
            Idle,
            BeStared,
            Prepare,
            Attack,
        }

        public float speed;
        public List<Sprite> sprites;

        private SCP173States _currentStates;

        private GameObject _protagonist;
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;
        private float _timer;
        private Vector2 _velocity;

        private void Start()
        {
            _protagonist = GameObject.Find("Protagonist");
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _currentStates = SCP173States.Idle;
        }

        private void FixedUpdate()
        {
            switch (_currentStates)
            {
                case SCP173States.Idle:
                    Idle();
                    break;
                case SCP173States.BeStared:
                    BeStared();
                    break;
                case SCP173States.Prepare:
                    Prepare();
                    break;
                case SCP173States.Attack:
                    Attack();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Idle()
        {
            _timer += Time.fixedDeltaTime;
            if (_timer > Random.Range(5f,10f))
            {
                _timer = 0f;
                if (Random.Range(0,3) == 0) 
                    _velocity = Vector2.zero;
                else
                    _velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                SwitchDirection(_velocity);
            }
            _rigidbody.velocity = _velocity * speed;
        }

        private void BeStared()
        {
            _velocity = Vector2.zero;
            SwitchDirection(((Vector2)(_protagonist.transform.position - transform.position)).normalized);
        }

        private void Prepare()
        {
            _velocity = Vector2.zero;
            _timer += Time.fixedDeltaTime;
            SwitchDirection(((Vector2)(_protagonist.transform.position - transform.position)).normalized);
            if (_timer > 1f)
            {
                _currentStates = SCP173States.Attack;
                _timer = 0;
            }
        }

        private void Attack()
        {
            _velocity = Vector2.zero;
            Vector2 startPos = transform.position;
            Vector2 endPos = _protagonist.transform.position;
            transform.position = Vector2.Lerp(startPos, endPos, _timer);
            _timer += Time.fixedDeltaTime * 2;
            if (_timer > 0.4)
            {
                EventCenter.Instance.Trigger(EventType.GameOver);
                _currentStates = SCP173States.Idle;
                _timer = 0;
            }
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            if (_currentStates == SCP173States.Idle && col.CompareTag("Player"))
            {
                Debug.Log("Preparing");
                _currentStates = SCP173States.Prepare;
                _timer = 0;
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                _currentStates = SCP173States.Idle;
                _timer = 0;
            }
        }

        private void SwitchDirection(Vector2 direction)
        {
            if (direction == Vector2.zero)
            {
                _spriteRenderer.sprite = sprites[0];
                return;
            }
            switch (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg)
            {
                case >= -45 and <= 45:
                    _spriteRenderer.sprite = sprites[2];
                    break;
                case >= 135 or <= -135:
                    _spriteRenderer.sprite = sprites[3];
                    break;
                case > -135 and < -45:
                    _spriteRenderer.sprite = sprites[0];
                    break;
                case > 45 and < 135:
                    _spriteRenderer.sprite = sprites[1];
                    break;
            }
        }

        public void IsStaring()
        {
            Debug.Log("BeStare");
            _currentStates = SCP173States.BeStared;
        }

        public void IsNotStaring()
        {
            Debug.Log("NotStare");
            _currentStates = SCP173States.Idle;
        }
    }
}