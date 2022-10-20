using ECS.Components;
using Scripts.ECS.Components;
using Scripts.SO;
using Unity.Entities;
using UnityEngine;

namespace Scripts.ECS.Systems.InitializationSystemGroup
{
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(InitSystemGroup))]
    public partial class InitializeConfigSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            if (HasSingleton<MainAppConfigTag>()) return;

            var configEntity = EntityManager.CreateEntity();
            EntityManager.AddComponent<MainAppConfigTag>(configEntity);
#if UNITY_EDITOR
            EntityManager.SetName(configEntity, $"MainAppConfig");
#endif
            MainAppConfig.Initialize(() =>
            {
                EntityManager.AddComponentObject(configEntity, MainAppConfig.Instance);
            });
            
            //TO_DO remove from here later
            var spawnAvatarEntity = EntityManager.CreateEntity();
            EntityManager.AddComponentData(spawnAvatarEntity, new SpawnAvatarComponent() {AvatarType = 0});
        }
    }
}