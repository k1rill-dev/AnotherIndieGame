using UnityEngine;

namespace GameLogicScripts
{
    public class Flashlight
    {
        public void turnOnFlashlight(GameObject flashLight) => flashLight.SetActive(!flashLight.activeSelf);
    }
}
