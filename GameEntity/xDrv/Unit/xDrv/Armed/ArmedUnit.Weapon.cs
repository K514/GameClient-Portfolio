using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    public partial class ArmedUnit
    {
        #region <Fields>

        private DefaultObject _LeftWeapon;
        private DefaultObject _RightWeapon;

        #endregion

        #region <Callbacks>

        private void OnCreateWeapon()
        {
            GrabLeftWeapon(ComponentDataRecord.LeftWeaponIndex);
            GrabRightWeapon(ComponentDataRecord.RightWeaponIndex);
        }

        private void OnDisposeWeapon()
        {
            if (_LeftWeapon.IsContentValid())
            {
                _LeftWeapon.Pooling();
                _LeftWeapon = null;
            }
            
            if (_RightWeapon.IsContentValid())
            {
                _RightWeapon.Pooling();
                _RightWeapon = null;
            }
        }
        
        #endregion

        #region <Methods>

        private void GrabLeftWeapon(int p_Index)
        {
            if (GameEntityModelDataTableQuery.GetInstanceUnsafe.TryGetRecordBridge(p_Index, out var o_LeftWeaponModelRecord))
            {
                var createParams = DefaultObjectPoolManager.GetInstanceUnsafe.GetCreateParams(o_LeftWeaponModelRecord.PrefabName, ResourceLifeCycleType.ManualUnload);
                if (TryGetAttachPoint(UnitTool.AttachPoint.LeftArm, out var o_Affine))
                {
                    _LeftWeapon 
                        = DefaultObjectPoolManager.GetInstanceUnsafe
                            .Pop
                            (
                                createParams,
                                new DefaultObjectPoolManager.ActivateParams(o_Affine, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, o_Affine))
                            );
                }
            }
        }
        
        private void GrabRightWeapon(int p_Index)
        {
            if (GameEntityModelDataTableQuery.GetInstanceUnsafe.TryGetRecordBridge(p_Index, out var o_RightWeaponModelRecord))
            {
                var createParams = DefaultObjectPoolManager.GetInstanceUnsafe.GetCreateParams(o_RightWeaponModelRecord.PrefabName, ResourceLifeCycleType.ManualUnload);
                if (TryGetAttachPoint(UnitTool.AttachPoint.RightArm, out var o_Affine))
                {
                    _RightWeapon
                        = DefaultObjectPoolManager.GetInstanceUnsafe
                            .Pop
                            (
                                createParams,
                                new DefaultObjectPoolManager.ActivateParams(o_Affine, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, o_Affine))
                            );
                }
            }
        }

        #endregion
    }
}