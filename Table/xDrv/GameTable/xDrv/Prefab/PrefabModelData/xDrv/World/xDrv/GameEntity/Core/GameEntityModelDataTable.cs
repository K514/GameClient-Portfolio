using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Common
{
    public interface IGameEntityModelDataTableBridge<out RecordBridge> : IWorldObjectModelDataTableBridge<RecordBridge>, ITableBridgeLabel<GameEntityModelDataTableQuery.TableLabel>
    {
    }

    public interface IGameEntityModelDataTableRecordBridge : IWorldObjectModelDataTableRecordBridge
    {
        /// <summary>
        /// 해당 모델의 애니메이터 이름 테이블 키
        /// </summary>
        int AnimatorIndex { get; }
        
        /// <summary>
        /// 프리팹 충돌 반경
        /// </summary>
        float PrefabColliderRadius { get; }
                    
        /// <summary>
        /// 프리팹 충돌 높이
        /// </summary>
        float PrefabColliderHeight { get; }
                    
        /// <summary>
        /// 프리팹 질량
        /// </summary>
        float PrefabMass { get; }
        
        /// <summary>
        /// 프리팹 충돌 오프셋
        /// </summary>
        float PrefabSkinOffset { get; }
        
        /// <summary>
        /// 프리팹 충돌 범위 중심 오프셋
        /// </summary>
        TableTool.SerializableVector3 PrefabColliderCenterOffset { get; }
        
        /// <summary>
        /// 개체 모델 속성 마스크
        /// </summary>
        GameEntityModelDataTableQuery.GameEntityModelAttribute AttributeMask { get; }
    }
    
    public abstract class GameEntityModelDataTable<Table, Record, RecordBridge> : WorldObjectModelDataTable<Table, Record, RecordBridge>, IGameEntityModelDataTableBridge<RecordBridge>
        where Table : GameEntityModelDataTable<Table, Record, RecordBridge>, new() 
        where Record : GameEntityModelDataTable<Table, Record, RecordBridge>.GameEntityModelDataTableRecord, RecordBridge, new()
        where RecordBridge : class, IGameEntityModelDataTableRecordBridge
    {
        #region <Fields>

        protected GameEntityModelDataTableQuery.TableLabel _GameEntityModelLabel;
        GameEntityModelDataTableQuery.TableLabel ITableBridgeLabel<GameEntityModelDataTableQuery.TableLabel>.TableLabel => _GameEntityModelLabel;

        #endregion
        
        #region <Record>

        [Serializable]
        public class GameEntityModelDataTableRecord : WorldObjectModelDataTableRecord, IGameEntityModelDataTableRecordBridge
        {
            #region <Consts>

            private const float __Default_SkinWidth = 0.001f;
            private const float __Default_Radius = 0.5f;

            #endregion
            
            #region <Fields>

            public int AnimatorIndex { get; protected set; }
            public float PrefabColliderRadius { get; protected set; }
            public float PrefabColliderHeight { get; protected set; }
            public float PrefabMass { get; protected set; }
            public float PrefabSkinOffset { get; protected set; }
            public TableTool.SerializableVector3 PrefabColliderCenterOffset { get; protected set; }
            public int SoundIndex { get; protected set; }
            public GameEntityModelDataTableQuery.GameEntityModelAttribute AttributeMask { get; protected set; }
            
            #endregion

            #region <Callbacks>

            public override async UniTask OnRecordAdded(Table p_Table, CancellationToken p_CancellationToken)
            {
                await base.OnRecordAdded(p_Table, p_CancellationToken); 
                
                if (PrefabColliderRadius.IsReachedZero())
                {
                    PrefabColliderRadius = __Default_Radius;
                }
                
                PrefabColliderHeight = Mathf.Max(PrefabColliderHeight, 2f * PrefabColliderRadius);
                PrefabSkinOffset = Mathf.Max(__Default_SkinWidth, PrefabSkinOffset);
                
                if (AttributeMask.HasAnyFlagExceptNone(GameEntityModelDataTableQuery.GameEntityModelAttribute.BottomPivot))
                {
                    if (CustomMath.IsReachedZero(PrefabColliderCenterOffset))
                    {
                        PrefabColliderCenterOffset += 0.5f * PrefabColliderHeight * Vector3.up;
                    }
                }
            }

            #endregion

            #region <Methods>

            public override async UniTask SetRecord(int p_Key, object[] p_RecordField, CancellationToken p_CancellationToken)
            {
                await base.SetRecord(p_Key, p_RecordField, p_CancellationToken);
                
                AnimatorIndex = (int)p_RecordField.GetElementSafe(2);
                PrefabColliderRadius = (float)p_RecordField.GetElementSafe(3);
                PrefabColliderHeight = (float)p_RecordField.GetElementSafe(4);
                PrefabMass = (float)p_RecordField.GetElementSafe(5);
                PrefabSkinOffset = (float)p_RecordField.GetElementSafe(6);
                PrefabColliderCenterOffset = (Vector3)p_RecordField.GetElementSafe(7);
                AttributeMask = (GameEntityModelDataTableQuery.GameEntityModelAttribute)p_RecordField.GetElementSafe(8);
            }

            #endregion
        }
        
        #endregion
        
        #region <Callbacks>

        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();
            
            _WorldObjectModelLabel = WorldObjectModelDataTableQuery.TableLabel.GameEntity;
        }

        #endregion
    }
}