using ECS.Components;
using Scripts.ECS.Components;
using Scripts.SO;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace Scripts.ECS.Systems
{
    public partial class AvatarMoveSystem : SystemBase
    {
        private Entity _joystickInputEntity;
        private JoystickInputComponent _joystickInputComponent;
        private float _clampValue = 3f;

        protected override void OnCreate()
        {
            RequireForUpdate(GetEntityQuery(typeof(AvatarTag)));
        }

        protected override void OnUpdate()
        {
            if (HasSingleton<JoystickInputComponent>() == false)
                return;

            var deltaTime = UnityEngine.Time.deltaTime;
            
            Entities
                .WithoutBurst()
                .WithAll<AvatarTag>()
                .ForEach((Entity e, ref PhysicsVelocity physicsVelocity, ref Rotation rotation) =>
                {
                    if (HasSingleton<JoystickInputComponent>() == false) return;
                    
                    _joystickInputEntity = GetSingletonEntity<JoystickInputComponent>();
                    _joystickInputComponent = EntityManager.GetComponentData<JoystickInputComponent>(_joystickInputEntity);
                    
                    var movement = new float3(_joystickInputComponent.Horizontal, 0, _joystickInputComponent.Vertical);
                    physicsVelocity.Linear.xyz += movement * MainAppConfig.Instance.AvatarMoveSpeed * deltaTime;
                    physicsVelocity.Linear.xyz = new float3
                    (
                        Mathf.Clamp(physicsVelocity.Linear.x, -_clampValue, _clampValue),
                        physicsVelocity.Linear.y,
                        Mathf.Clamp(physicsVelocity.Linear.z, -_clampValue, _clampValue)
                    );
                }).Run();
        }
    }
}