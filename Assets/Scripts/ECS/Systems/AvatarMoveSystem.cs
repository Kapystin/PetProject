using ECS.Components;
using Scripts.ECS.Components;
using Scripts.SO;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Scripts.ECS.Systems
{
    public partial class AvatarMoveSystem : SystemBase
    {
        private Entity _joystickInputEntity;
        private JoystickInputComponent _joystickInputComponent;

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
                .ForEach((Entity e, ref Translation translation) =>
                {
                    if (HasSingleton<JoystickInputComponent>() == false) return;
                    
                    _joystickInputEntity = GetSingletonEntity<JoystickInputComponent>();
                    _joystickInputComponent = EntityManager.GetComponentData<JoystickInputComponent>(_joystickInputEntity);
                    
                    var movement = new float3(_joystickInputComponent.Horizontal, 0, _joystickInputComponent.Vertical);
                    translation.Value += movement * MainAppConfig.Instance.AvatarMoveSpeed * deltaTime;
                }).Run();
        }
    }
}