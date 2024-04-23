using UnityEngine;
using UnityEngine.UI;

namespace GameLogicScripts
{
    public class DamageScreen: MonoBehaviour
    {
        [SerializeField] private Image _overlay;
        [SerializeField] private float _duration;
        [SerializeField] private float _fadeSpeed;
        private float _durationTimer;
        
        public Image Overlay
        {
            get => _overlay;
            set => _overlay = value;
        }

        public float DurationTimer
        {
            get => _durationTimer;
            set => _durationTimer = value;
        }

        private void Start()
        {
            _overlay.color = new Color(_overlay.color.r, _overlay.color.g, _overlay.color.b, 0);
        }

        private void Update()
        {
            if (_overlay.color.a > 0)
            {
                _durationTimer += Time.deltaTime;
                if (_durationTimer > _duration)
                {
                    float tempAlpha = _overlay.color.a;
                    tempAlpha -= Time.deltaTime * _fadeSpeed;
                    _overlay.color = new Color(_overlay.color.r, _overlay.color.g, _overlay.color.b, tempAlpha);
                }
            }
            
        }
    }
}