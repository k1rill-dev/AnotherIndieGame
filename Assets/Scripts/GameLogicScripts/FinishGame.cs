using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameLogicScripts
{
    public class FinishGame : MonoBehaviour
    {
        [SerializeField] private Image _loseScreen;

        [SerializeField] private Image _winScreen;
        // [SerializeField] private float _duration;
        // [SerializeField] private float _fadeSpeed;
        // private float _durationTimer;
        //
        // public Image LoseScreen => _loseScreen;
        // public Image WinScreen => _winScreen;
        //
        // public float Duration => _duration;
        // public float FadeSpeed => _fadeSpeed;
        //
        // private void Start()
        // {
        //     _loseScreen.color = new Color(_loseScreen.color.r, _loseScreen.color.g, _loseScreen.color.b, 0);
        //     _winScreen.color = new Color(_winScreen.color.r, _winScreen.color.g, _winScreen.color.b, 0);
        // }

        public void WinGame()
        {
            Time.timeScale = 0;
            _winScreen.gameObject.SetActive(true);
            // if (Math.Abs(_winScreen.color.a - 1f) > 0.00000001)
            // {
            //     _durationTimer += Time.deltaTime;
            //     if (_durationTimer > _duration)
            //     {
            //         float tempAlpha = _winScreen.color.a;
            //         tempAlpha += Time.deltaTime * _fadeSpeed;
            //         _winScreen.color = new Color(_winScreen.color.r, _winScreen.color.g, _winScreen.color.b, tempAlpha);
            //     }
            // }
        }
        
        public void LoseGame()
        {
            Time.timeScale = 0;
            _loseScreen.gameObject.SetActive(true);
        }

        public void LoadMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}