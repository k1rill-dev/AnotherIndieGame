using UnityEngine;

namespace GameLogicScripts
{
    public class Flashlight : MonoBehaviour
    {
        public void turnOnFlashlight(GameObject flashLight) => flashLight.SetActive(!flashLight.activeSelf);
    }
}
