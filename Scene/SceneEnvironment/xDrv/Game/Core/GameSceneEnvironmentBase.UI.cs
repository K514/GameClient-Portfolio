namespace k514.Mono.Common
{
    public abstract partial class GameSceneEnvironmentBase
    {
        private void OnCreateSceneUI()
        {
            /* UI 활성화 */
            var hpBarTheater = UIxControlRoot.GetInstanceUnsafe.HpBarTheater;
            hpBarTheater.SetHide(false);
            
            var nameTheater = UIxControlRoot.GetInstanceUnsafe.NameTheater;
            nameTheater.SetHide(false);
            
            var numberTheater = UIxControlRoot.GetInstanceUnsafe.NumberTheater;
            numberTheater.SetHide(false);
                        
            var gaugeTheater = UIxControlRoot.GetInstanceUnsafe.GaugeTheater;
            gaugeTheater.SetHide(false);
        }
    }
}