using Scripts.ECS.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Scripts.ECS.Systems
{
    public partial class MoveAnimationControllerSystem : SystemBase
    {
        private Animator _avatarAnimator;

        protected override void OnCreate()
        {
            RequireForUpdate(GetEntityQuery(typeof(AvatarTag)));
        }

        protected override void OnUpdate()
        {
            if (_avatarAnimator == null)
            {
                _avatarAnimator = GameObject.FindWithTag($"Avatar").GetComponent<Animator>();
            }

            Entities
                .WithoutBurst()
                .WithAny<AvatarTag>()
                .ForEach((Entity e, in PhysicsVelocity physicsVelocity) =>
                {
                    float x = math.abs(physicsVelocity.Linear.x);
                    float z = math.abs(physicsVelocity.Linear.z);
                    
                    float velocity = new Vector2(x, z).magnitude;
                    
                    _avatarAnimator.SetFloat($"Velocity", velocity);
                })
                .Run();
        }
    }
}