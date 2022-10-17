using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Scripts.Debug
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _displayText;

        private int _avgFrameRate;
        
        public void Update()
        {
            float current = 0;
            current = Time.frameCount / Time.time;
            _avgFrameRate = (int) current;
            _displayText.text = $"{_avgFrameRate} FPS";
        }
    }
}
