using System.Collections.Generic;
using ECS.Components;
using Scripts.SO;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Scripts.ECS.Systems
{
    public partial class InitializeGridSystem : SystemBase
    {
        private Entity _gridCellPrefabEntity;
        private bool _isOver;
        private List<float3> _cellsPosition;

        protected override void OnCreate()
        {
            RequireForUpdate(GetEntityQuery(typeof(MainAppConfigTag), typeof(MainAppConfig)));
            _cellsPosition = new List<float3>();
        }

        protected override void OnUpdate()
        {
            if (_isOver) return;
            
            if (_gridCellPrefabEntity == Entity.Null)
            {
                _gridCellPrefabEntity = ConvertToEntity(EntityManager);    
            }

            CalculateCellsPosition(new int3(-5,0, -5), 1f, 0.1f);
            

            foreach (var position in _cellsPosition)
            {
                var gridCell = EntityManager.Instantiate(_gridCellPrefabEntity);
#if UNITY_EDITOR
                EntityManager.SetName(gridCell, $"GridCell {gridCell.Index}");
#endif
                EntityManager.SetComponentData(gridCell, new Translation(){Value = position});
            }

            
            _isOver = true;
        }


        private Entity ConvertToEntity(EntityManager dstManager)
        {
            using (BlobAssetStore blobAssetStore = new BlobAssetStore())
            {
                var prefabEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(
                    MainAppConfig.Instance.GridCellPrefab,
                    GameObjectConversionSettings.FromWorld(dstManager.World, blobAssetStore));
                return prefabEntity;
            }
        }

        private void CalculateCellsPosition(int3 startPosition, float cellSize, float offset)
        {
            for (int row = startPosition.x; row < 5; row++)
            {
                for (int column = startPosition.z; column < 5; column++)
                {
                    _cellsPosition.Add(new float3(row, 0f, column));
                }
            }
        }
    }
}