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
                // if (HasSingleton<MainAppConfigTag>())
                // {
                //     configEntity = GetSingletonEntity<MainAppConfigTag>();
                // }
                // else
                // {
                //     configEntity = EntityManager.CreateEntity();
                //     EntityManager.AddComponent<MainAppConfigTag>(configEntity);
                // }
                
                EntityManager.AddComponentObject(configEntity, MainAppConfig.Instance);
            });
        }
    }
}