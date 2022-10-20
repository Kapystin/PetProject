using Scripts.ECS.Components;
using Unity.Entities;
using UnityEngine;

namespace Scripts.ECS.Systems
{
    public partial class JoystickInputSystem : SystemBase
    {
        private VariableJoystick _variableJoystick;
        private Entity _joystickInputEntity;
        
        protected override void OnUpdate()
        {
            if (_variableJoystick == null || _joystickInputEntity == Entity.Null)
            {
                _variableJoystick = GameObject.FindWithTag("VariableJoystick").GetComponent<VariableJoystick>();
                _joystickInputEntity = EntityManager.CreateEntity();
#if UNITY_EDITOR
                EntityManager.SetName(_joystickInputEntity, $"joystickInput");
#endif
                EntityManager.AddComponentData(_joystickInputEntity, new JoystickInputComponent());
            }

            var joystickInputComponent = new JoystickInputComponent()
            {
                Horizontal = _variableJoystick.Horizontal,
                Vertical = _variableJoystick.Vertical
            };
            
            EntityManager.SetComponentData(_joystickInputEntity, joystickInputComponent);
        }
    }
}