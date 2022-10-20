using System.Collections;
using System.Collections.Generic;
using Scripts.ECS.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Scripts.ECS.Systems
{
    public partial class AvatarMoveSystem : SystemBase
    {
        private Entity _joystickInputEntity;
        private JoystickInputComponent _joystickInputComponent;
        private float _moveSpeed = 5;
        
        protected override void OnCreate()
        {
            RequireForUpdate(GetEntityQuery(typeof(AvatarTag)));
        }

        protected override void OnUpdate()
        {
            if (HasSingleton<JoystickInputComponent>() == false)
                return;
            
            if (_joystickInputEntity == Entity.Null)
            {
                _joystickInputEntity = GetSingletonEntity<JoystickInputComponent>();
                _joystickInputComponent = EntityManager.GetComponentData<JoystickInputComponent>(_joystickInputEntity);
            }

            var deltaTime = UnityEngine.Time.deltaTime;
            
            Entities
                .WithoutBurst()
                .WithAll<AvatarTag>()
                .ForEach((Entity e, ref Translation translation) =>
                {
                    var movement = new float3(_joystickInputComponent.Horizontal, 0, _joystickInputComponent.Vertical);
                    // transform.position += movement * _moveSpeed * Time.deltaTime;
                    translation.Value.xyz +=  movement * _moveSpeed * deltaTime;
                }).Run();
        }
    }
}