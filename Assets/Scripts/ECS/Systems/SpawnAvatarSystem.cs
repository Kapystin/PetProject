using ECS.Components;
using Scripts.ECS.Components;
using Scripts.SO;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Authoring;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using CapsuleCollider = Unity.Physics.CapsuleCollider;
using Collider = Unity.Physics.Collider;

namespace Scripts.ECS.Systems
{
    public partial class SpawnAvatarSystem : SystemBase
    {
        private Entity _avatarPrefabEntity;
        
        protected override void OnCreate()
        {
            RequireForUpdate(GetEntityQuery(typeof(MainAppConfigTag), typeof(MainAppConfig)));
        }

        protected override void OnUpdate()
        {
            if (_avatarPrefabEntity == Entity.Null)
            {
                _avatarPrefabEntity = ConvertToEntity(EntityManager);
            }

            Entities
                .WithoutBurst()
                .WithStructuralChanges()
                .ForEach((Entity e, in SpawnAvatarComponent spawnAvatarComponent) =>
                {
                    var colliderPrefabTransform = MainAppConfig.Instance.AvatarEntityColliderPrefab.transform;
                    var spawnPoint = MainAppConfig.Instance.SpawnPosition;
                    var avatarModel = GameObject.Instantiate(MainAppConfig.Instance.AvatarModelPrefab);
                    var avatarColliderEntity = EntityManager.Instantiate(_avatarPrefabEntity);

                    avatarModel.transform.position = spawnPoint;

                    var translation = new Translation() {Value = spawnPoint};
                    EntityManager.AddComponentData(avatarColliderEntity, translation);

                    var boxCollider = Unity.Physics.BoxCollider.Create(new BoxGeometry()
                            {
                                Center = colliderPrefabTransform.position,
                                Size = colliderPrefabTransform.localScale, 
                                Orientation = quaternion.identity,
                                BevelRadius = 0.05f
                            },
                        CollisionFilter.Default);


                    var physicsCollider = new PhysicsCollider()
                    {
                        Value = boxCollider
                    };

                    
                    var physicsVelocity = new PhysicsVelocity(){};
                    var physicsMass = PhysicsMass.CreateDynamic(physicsCollider.MassProperties, 0.5f);
                    physicsMass.InverseInertia = new float3(0f, 0f, 0f);
                    var physicsWorldIndex = new PhysicsWorldIndex(){ Value = 0};

                    EntityManager.AddComponentData(avatarColliderEntity, physicsCollider);
                    EntityManager.AddComponentData(avatarColliderEntity, physicsVelocity);
                    EntityManager.AddComponentData(avatarColliderEntity, physicsMass);
                    EntityManager.AddSharedComponentData(avatarColliderEntity, physicsWorldIndex);
                    
                    
#if UNITY_EDITOR
                    EntityManager.SetName(avatarColliderEntity, $"Avatar Collider");
#endif
                    EntityManager.AddComponentData(avatarColliderEntity, new AvatarTag());
                    EntityManager.DestroyEntity(e);
            }).Run();
        }

        private Entity ConvertToEntity(EntityManager dstManager)
        {
            using (BlobAssetStore blobAssetStore = new BlobAssetStore())
            {
                var prefabEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(
                    MainAppConfig.Instance.AvatarEntityColliderPrefab,
                    GameObjectConversionSettings.FromWorld(dstManager.World, blobAssetStore));
                return prefabEntity;
            }
        }
    }
}