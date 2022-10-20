using TMPro;
using UnityEngine;

namespace Scripts.Debug
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _displayText;

        private int _lastFrameIndex;
        private float[] _frameDeltaTimeArray;

        private void Start()
        {
            _frameDeltaTimeArray = new float[50];
        }

        private void Update()
        {
            _frameDeltaTimeArray[_lastFrameIndex] = Time.deltaTime;
            _lastFrameIndex = (_lastFrameIndex + 1) % _frameDeltaTimeArray.Length;
            
            _displayText.text = $"{Mathf.RoundToInt(CalculateFPS())} FPS";
        }

        private float CalculateFPS()
        {
            float total = 0f;
            foreach (var deltaTime in _frameDeltaTimeArray)
            {
                total += deltaTime;
            }

            return _frameDeltaTimeArray.Length / total;
        }
    }
}
