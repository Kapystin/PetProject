using Scripts.ECS.Components;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Scripts.ECS.Systems
{
    public partial class GameObjectAvatarFollowSystem : SystemBase
    {
        private GameObject _avatar;

        protected override void OnCreate()
        {
            RequireForUpdate(GetEntityQuery(typeof(AvatarTag)));
        }

        protected override void OnUpdate()
        {
            if (_avatar == null)
            {
                _avatar = GameObject.FindWithTag($"Avatar");
            }

            Entities
                .WithoutBurst()
                .WithAny<AvatarTag>()
                .ForEach((Entity e, in Translation translation) => { _avatar.transform.position = translation.Value; })
                .Run();
        }
    }
}