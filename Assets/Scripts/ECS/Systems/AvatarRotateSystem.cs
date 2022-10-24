using Scripts.ECS.Components;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Scripts.ECS.Systems
{
    public partial class AvatarRotateSystem : SystemBase
    {
        private GameObject _avatar;
        
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

            if (_avatar == null)
            {
                _avatar = GameObject.FindWithTag($"Avatar");
            }
            
            var deltaTime = UnityEngine.Time.deltaTime;
            
            Entities
                .WithoutBurst()
                .WithAll<AvatarTag>()
                .ForEach((Entity e, ref LocalToWorld transform) =>
                {
                    if (HasSingleton<JoystickInputComponent>() == false) return;
                    
                    _joystickInputEntity = GetSingletonEntity<JoystickInputComponent>();
                    _joystickInputComponent = EntityManager.GetComponentData<JoystickInputComponent>(_joystickInputEntity);

                    var targetPosition = new Vector3(_joystickInputComponent.Horizontal , 0, _joystickInputComponent.Vertical);
                    Vector3 currentPos = transform.Position;
                    var facePos = currentPos + targetPosition;
         
                    _avatar.transform.LookAt(facePos);
                }).Run();
        }
    }
}