using System;
using System.Collections;
using Framework;
using Games.Element;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using AudioType = Framework.AudioType;
using EventType = Framework.EventType;

namespace Games.Protagonist
{
    public class ProtagonistController : MonoBehaviour
    {
        public float speed;
        public GameObject flashLight;
        public GameObject gameOverPanel;
        public GameObject deathBody;

        // 玩家移动朝向相关
        private Vector3 _moveDir; // 移动方向向量
        private Vector3 _mousePos; // 鼠标当前坐标
        private Vector3 _lookDir; // 朝向向量
        private float _angle; // 应朝向角度
        
        // 射线检测相关
        private RaycastHit2D _info;
        private RaycastHit2D _info173;
        private GameObject _selectedObj;
        private GameObject selectedObj
        {
            get => _selectedObj;
            set
            {
                if (_selectedObj == value)
                    return;
                if (value != null)
                    value.GetComponent<InteractableElement>().OnFocus?.Invoke();
                if (_selectedObj != null)
                    _selectedObj.GetComponent<InteractableElement>().OnLeave?.Invoke();
                _selectedObj = value;
            }
        }

        private Rigidbody2D _rigidbody;
        private Transform _rotatablePart;
        public ActionMap ActionMap;

        // 动画相关
        private Animator _animator;
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int UpDown = Animator.StringToHash("UpDown");
        private static readonly int LeftRight = Animator.StringToHash("LeftRight");

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _rotatablePart = transform.GetChild(0);
            _animator = transform.GetChild(1).GetComponent<Animator>();
            ActionMap = new ActionMap();
            ActionMap.Enable();
        }

        private void Start()
        {
            ActionMap.Game.Move.performed += OnMove;
            ActionMap.Game.MouseDrag.performed += OnDrag;
            ActionMap.Game.Interact.performed += OnAct;
            ActionMap.Game.Flashlight.performed += OnLeftClick;
            AudioCenter.Instance.AudioPlaySync(new AudioAsset(AudioType.Effect,"Walk",true));
            AudioCenter.Instance.AudioPause(AudioType.Effect,"Walk");
            EventCenter.Instance.AddEventListener(EventType.GameOver,GameOver);
            EventCenter.Instance.AddEventListener(EventType.Restart,Restart);
        }
        
        private void OnDestroy()
        {
            ActionMap.Disable();
            EventCenter.Instance.RemoveEventListener(EventType.GameOver,GameOver);
            EventCenter.Instance.RemoveEventListener(EventType.Restart,Restart);
        }

        private void Update()
        {
            if (ActionMap.Game.Move.inProgress) { }
            // 动画（写得依托）
            else
            {
                _animator.SetInteger(Idle,0);
                _animator.SetInteger(UpDown,0);
                _animator.SetInteger(LeftRight, 0);
                if (_moveDir.y > 0)
                    _animator.SetInteger(Idle,1);
                else if (_moveDir.y < 0)
                    _animator.SetInteger(Idle, 2);
                else if (_moveDir.x < 0)
                    _animator.SetInteger(LeftRight, 1);
                else if (_moveDir.x > 0)
                    _animator.SetInteger(LeftRight, 2);
                return;
            }
            _animator.SetInteger(Idle,0);
            _animator.SetInteger(UpDown,0);
            _animator.SetInteger(LeftRight, 0);
            if (_moveDir.y > 0)
                _animator.SetInteger(UpDown,1);
            else if (_moveDir.y < 0)
                _animator.SetInteger(UpDown,2);
            else if (_moveDir.x < 0)
                _animator.SetInteger(LeftRight, 1);
            else if (_moveDir.x > 0)
                _animator.SetInteger(LeftRight, 2);
        }

        private void FixedUpdate()
        {
            // 移动
            if (ActionMap.Game.Move.inProgress)
            {
                AudioCenter.Instance.AudioUnPause(AudioType.Effect,"Walk");
                _rigidbody.velocity = _moveDir * speed;
            }
            else
            {
                AudioCenter.Instance.AudioPause(AudioType.Effect,"Walk");
                _rigidbody.velocity = Vector2.zero;
            }
            
            
            // 交互，发射射线检测物体
            _info = Physics2D.Raycast(transform.position, _lookDir, 1.5f, 1 << 6);
            selectedObj = _info.collider ? _info.collider.gameObject : null;
        }

        // 玩家移动
        private void OnMove(InputAction.CallbackContext ctx)
        {
            _moveDir = ctx.ReadValue<Vector2>();
        }

        // 视角跟随鼠标移动
        private void OnDrag(InputAction.CallbackContext ctx)
        {
            if (Camera.main != null) 
                _mousePos = Camera.main.ScreenToWorldPoint(ctx.ReadValue<Vector2>());
            _lookDir = _mousePos - transform.position;
            _angle = Mathf.Atan2(_lookDir.y, _lookDir.x) * Mathf.Rad2Deg - 90;
            _rotatablePart.rotation = Quaternion.Euler(new Vector3(0, 0, _angle));
        }
        
        // 交互物品
        private void OnAct(InputAction.CallbackContext ctx)
        {
            if (selectedObj != null)
            {
                selectedObj.GetComponent<InteractableElement>().OnInteract();
            }
        }
        
        // 开关手电
        private void OnLeftClick(InputAction.CallbackContext ctx)
        {
            if(ProtagonistInfo.HasFlashlight)
            {
                AudioCenter.Instance.AudioPlaySync(new AudioAsset(AudioType.Effect,"Flashlight"));
                if (flashLight.activeSelf)
                    flashLight.SetActive(false);
                else
                    flashLight.SetActive(true);
            }
        }
        
        // 角色死亡
        private void GameOver()
        {
            AudioCenter.Instance.AudioPause(AudioType.Effect,"Walk");
            AudioCenter.Instance.AudioPlaySync(new AudioAsset(AudioType.Effect,"Dead"));
            Instantiate(deathBody, transform.position, Quaternion.identity);
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            StartCoroutine(GameOverPanel());
        }

        private IEnumerator GameOverPanel()
        {
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
            Image[] images = gameOverPanel.GetComponentsInChildren<Image>();
            foreach (var image in images)
            {
                Color color = image.color;
                color.a = 0;
                image.color = color;
            }
            float t = 0f;
            while (true)
            {
                t += Time.fixedDeltaTime * 0.08f;
                foreach (var image in images)
                {
                    Color color = image.color;
                    color.a = t < 0.95 ? t : 1;
                    image.color = color;
                }
                if (t > 0.95)
                    break;
                yield return null;
            }
            yield return new WaitForSecondsRealtime(3f);
            EventCenter.Instance.Trigger(EventType.Restart);
        }

        private void Restart()
        {
            Time.timeScale = 1;
            gameOverPanel.SetActive(false);
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            transform.position = new Vector3(-2.43f, -5.73f, 0);
        }
    }
}