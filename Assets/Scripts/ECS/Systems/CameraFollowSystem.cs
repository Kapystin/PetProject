using Scripts.ECS.Components;
using Scripts.SO;
using Unity.Entities;
using Unity.Transforms;

namespace Scripts.ECS.Systems
{
    public partial class CameraFollowSystem : SystemBase
    {
        private UnityEngine.Camera _mainCamera;

        protected override void OnCreate()
        {
            RequireForUpdate(GetEntityQuery(typeof(AvatarTag)));
        }

        protected override void OnUpdate()
        {
            if (_mainCamera == null)
            {
                _mainCamera = UnityEngine.Camera.main;
            }

            Entities
                .WithoutBurst()
                .WithAny<AvatarTag>()
                .ForEach((Entity e, in Translation translation) =>
                {
                    _mainCamera.gameObject.transform.position = translation.Value;
                    _mainCamera.gameObject.transform.position += MainAppConfig.Instance.CameraOffset;
                }).Run();
        }
    }
}