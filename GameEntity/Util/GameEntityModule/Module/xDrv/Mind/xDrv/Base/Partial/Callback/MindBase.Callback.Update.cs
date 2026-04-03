using UnityEngine;

namespace k514.Mono.Common
{
    public partial class MindBase
    {
        public sealed override void OnModule_Update(float p_DeltaTime)
        {
            base.OnModule_Update(p_DeltaTime);

            OnUpdateQueue(p_DeltaTime);
            
            if (Entity.HasState_Only(GameEntityTool.EntityStateType.AIUpdatePassMask))
            {
                OnUpdateAIState(p_DeltaTime);
            }
        }

        private void OnUpdateQueue(float p_DeltaTime)
        {
            if (Entity.IsDrivingOrder)
            {
                if (ReferenceEquals(null, _CurrentOrder))
                {
                    DequeueNextOrder(MindTool.MindOrderPhase.None);
                }
                else
                {
                    var phase = _CurrentOrder.OnUpdate(p_DeltaTime);
                    switch (phase)
                    {
                        case MindTool.MindOrderPhase.Fail:
                            ReleaseCurrentOrder();
                            DequeueNextOrder(MindTool.MindOrderPhase.Fail);
                            break;
                        case MindTool.MindOrderPhase.Success:
                            ReleaseCurrentOrder();
                            DequeueNextOrder(MindTool.MindOrderPhase.Success);
                            break;
                    }
                }
            }
        }
        
        protected abstract void OnUpdateAIState(float p_DeltaTime);

        public sealed override void OnModule_Update_TimeBlock()
        {
            base.OnModule_Update_TimeBlock();

            if (Entity.HasState_Only(GameEntityTool.EntityStateType.AIUpdatePassMask))
            {
                OnUpdateAIState_TimeBlock();
            }
        }

        protected abstract void OnUpdateAIState_TimeBlock();
    }
}