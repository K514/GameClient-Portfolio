using System.Collections.Generic;
using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    public partial class UnitBase
    {
        #region <Fields>

        private Dictionary<UnitTool.AttachPoint, Transform> _AttachPointRecord;

        #endregion

        #region <Indexer>

        public Transform this[UnitTool.AttachPoint p_AttachPoint] => _AttachPointRecord[p_AttachPoint];

        #endregion
        
        #region <Callbacks>
        
        protected void OnCreateAttachPoint()
        {
            _AttachPointRecord = new Dictionary<UnitTool.AttachPoint, Transform>();
   
            if (!ReferenceEquals(null, ModelDataRecord))
            {
                var attachPointNameMap = UnitAttachPointDataTable.GetInstanceUnsafe[ModelDataRecord.AttachPointQueryIndex].AttachPointNameMap;
                if (EnumFlag.TryGetEnumEnumerator<UnitTool.AttachPoint>(EnumFlag.GetEnumeratorType.GetAll, out var o_Enumerator))
                {
                    foreach (var attachPoint in o_Enumerator)
                    {
                        switch (attachPoint)
                        {
                            case UnitTool.AttachPoint.MainTransform:
                                _AttachPointRecord.Add(attachPoint, Affine);
                                break;
                            default:
                                if (attachPointNameMap.TryGetValue(attachPoint, out var o_BoneNameList) && o_BoneNameList.CheckGenericCollectionSafe())
                                {
                                    var (valid, bone) = Affine.FindRecursiveInclude(o_BoneNameList, false);
                                    if (valid)
                                    {
                                        _AttachPointRecord.Add(attachPoint, bone);
                                    }
                                }
                                break;
                        }
                    }
                }
            }
        }

        private void OnActivateAttachPoint()
        {
        }

        private void OnRetrieveAttachPoint()
        {
        }

        #endregion
        
        #region <Methods>
        
        public bool TryGetAttachPoint(UnitTool.AttachPoint p_TargetPoint, out Transform o_Transform)
        {
            return _AttachPointRecord.TryGetValue(p_TargetPoint, out o_Transform);
        }

        public Vector3 GetAttachPosition(UnitTool.AttachPoint p_TargetPoint)
        {
            if (_AttachPointRecord.TryGetValue(p_TargetPoint, out Transform o_Transform))
            {
                return o_Transform.position;
            }
            else
            {
                return GetCenterPosition();
            }
        }

        #endregion
    }
}