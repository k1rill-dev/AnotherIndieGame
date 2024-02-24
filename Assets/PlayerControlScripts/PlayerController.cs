using System;
using GameLogicScripts;
using UnityEngine;

namespace PlayerControlScripts
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private float _moveSpeed = 6f;
        private Vector3 _velocity;
        private float _gravity = -9.81f;
        private Vector2 _move;
        private CharacterController _characterController;
        private Flashlight _flashlight;
        private bool _isGrounded;
        private bool _isActive;
        // private static event Action<> 
        
        [SerializeField]private Transform _ground;
        [SerializeField]private float _distanceToGround = 0.4f;
        [SerializeField]private LayerMask _groundMask;
        [SerializeField] private float _activateDistance = 3f;
        [SerializeField] private GameObject _flashLightObject;
        [SerializeField] private int _noteCount = 0;
        private static event Action _allNotesCollected; 
        

        private void Awake()
        {
            _allNotesCollected += OnAllNoteCollected;
            _playerInput = new PlayerInput();
            _characterController = GetComponent<CharacterController>();
            _flashlight = new Flashlight();
            // Interacter _interacter = new Interacter();
            _playerInput.Player.Interact.performed += ctx => PickUp();
            _playerInput.Player.Flashlight.performed += ctx => _flashlight.turnOnFlashlight(_flashLightObject);
        }

        private void Update()
        {
            Grav();
            PlayerMovement();
        }
        
        private void PickUp()
        {
            RaycastHit hit;
            _isActive = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, _activateDistance);
            if (hit.transform.CompareTag("Note"))
            {
                _noteCount += 1;
                hit.transform.gameObject.SetActive(false);
                if (_noteCount == 3)
                {
                    _allNotesCollected?.Invoke();
                }
            }
        }

        private void OnAllNoteCollected()
        {
            Debug.Log("все записки собраны");
        }

        private void Grav()
        {
            _isGrounded = Physics.CheckSphere(_ground.position, _distanceToGround, _groundMask);

            if (_isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }

            _velocity.y += _gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);
        }

        private void PlayerMovement()
        {
            _move = _playerInput.Player.Movement.ReadValue<Vector2>();

            Vector3 movement = (_move.y * transform.forward) + (_move.x * transform.right);

            _characterController.Move(movement * (_moveSpeed * Time.deltaTime));
        }

        private void OnEnable()
        {

            _playerInput.Enable();
        }

        private void OnDestroy()
        {
            _playerInput.Disable();
        }
    }
}
