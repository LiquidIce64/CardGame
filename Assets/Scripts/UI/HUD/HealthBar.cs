using Characters;

namespace UI
{
    public class HealthBar : BaseBar
    {
        protected new void Update()
        {
            maxValue = Player.Instance.MaxHealth;
            value = Player.Instance.Health;
            base.Update();
        }
    }
}
