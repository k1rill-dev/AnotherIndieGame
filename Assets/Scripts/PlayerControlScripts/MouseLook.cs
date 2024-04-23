using UnityEngine;

namespace PlayerControlScripts
{
    public class MouseLook : MonoBehaviour
    {
        private PlayerInput _playerInput;
        [SerializeField] private float _mouseSensitivity;
        private Vector2 _mouseLook;
        private float _xRotation = 0f;
        [SerializeField] private Transform _playerBody;


        private void Awake()
        {
            _mouseSensitivity = PlayerPrefs.GetFloat("sens");
            _playerInput = new PlayerInput();
            _playerBody = transform.parent;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            Look();
        }

        private void Look()
        {
            _mouseLook = _playerInput.Player.Look.ReadValue<Vector2>();

            float mouseX = _mouseLook.x * _mouseSensitivity * Time.deltaTime;
            float mouseY = _mouseLook.y * _mouseSensitivity * Time.deltaTime;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
            _playerBody.Rotate(Vector3.up * mouseX);
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