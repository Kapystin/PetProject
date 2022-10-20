using Unity.Entities;

namespace Scripts.ECS.Components
{
    public struct JoystickInputComponent : IComponentData
    {
        public float Horizontal;
        public float Vertical;
    }
}