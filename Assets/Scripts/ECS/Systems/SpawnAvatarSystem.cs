using ECS.Components;
using Scripts.ECS.Components;
using Scripts.SO;
using Unity.Entities;
using UnityEngine;

namespace Scripts.ECS.Systems
{
    public partial class SpawnAvatarSystem : SystemBase
    {
        private Entity _avatarPrefabEntity;
        
        protected override void OnCreate()
        {
            RequireForUpdate(GetEntityQuery(typeof(MainAppConfigTag), typeof(MainAppConfig)));
            // RequireForUpdate(GetEntityQuery(typeof(SpawnAvatarComponent)));
        }

        protected override void OnUpdate()
        {
            if (_avatarPrefabEntity == Entity.Null)
            {
                _avatarPrefabEntity = ConvertToEntity(EntityManager);    
            }

            Entities
                .WithStructuralChanges()
                .ForEach((Entity e, in SpawnAvatarComponent spawnAvatarComponent) =>
            {
                var avatarEntity = EntityManager.Instantiate(_avatarPrefabEntity);
#if UNITY_EDITOR
                EntityManager.SetName(avatarEntity, $"Avatar");
#endif
                EntityManager.DestroyEntity(e);
                
            }).Run();
        }

        private Entity ConvertToEntity(EntityManager dstManager)
        {
            using (BlobAssetStore blobAssetStore = new BlobAssetStore())
            {
                var prefabEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(
                    MainAppConfig.Instance.AvatarPrefab,
                    GameObjectConversionSettings.FromWorld(dstManager.World, blobAssetStore));
                return prefabEntity;
            }
        }
    }
}