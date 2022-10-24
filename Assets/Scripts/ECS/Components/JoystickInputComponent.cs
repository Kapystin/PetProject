using Unity.Entities;
using Unity.Mathematics;

namespace Scripts.ECS.Components
{
    public struct JoystickInputComponent : IComponentData
    {
        public float Horizontal;
        public float Vertical;
        public float2 Direction;
    }
}