using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private VariableJoystick _variableJoystick;
        
        private void Update()
        {
            Move();
        }

        private void Move()
        {
            var movement = new Vector3 (_variableJoystick.Horizontal, 0, _variableJoystick.Vertical);
            transform.position += movement * _moveSpeed * Time.deltaTime;
        }
    }
}