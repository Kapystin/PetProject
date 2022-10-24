using System.Numerics;
using Scripts.ECS.Components;
using Scripts.SO;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

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
                .ForEach((Entity e, ref PhysicsVelocity physicsVelocity, ref Rotation rotation, ref Translation translation, ref LocalToWorld transform) =>
                {
                    if (HasSingleton<JoystickInputComponent>() == false) return;
                    
                    _joystickInputEntity = GetSingletonEntity<JoystickInputComponent>();
                    _joystickInputComponent = EntityManager.GetComponentData<JoystickInputComponent>(_joystickInputEntity);

                    var movement = new Vector3(0f, physicsVelocity.Linear.y, 0f) 
                                   + (Vector3.right * _joystickInputComponent.Horizontal * MainAppConfig.Instance.AvatarMoveSpeed)
                                   + (Vector3.forward * _joystickInputComponent.Vertical * MainAppConfig.Instance.AvatarMoveSpeed);
                    physicsVelocity.Linear.xyz = movement;
                    
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