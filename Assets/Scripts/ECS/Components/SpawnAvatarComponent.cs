using Unity.Entities;
using Unity.Mathematics;

namespace Scripts.ECS.Components
{
    public struct SpawnAvatarComponent : IComponentData
    {
        public int AvatarType;
    }
}