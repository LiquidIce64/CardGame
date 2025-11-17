using Characters;

namespace UI
{
    public class HealthBar : BaseBar
    {
        protected new void Start()
        {
            base.Start();
            maxValue = Player.Instance.MaxHealth;
        }

        protected new void Update()
        {
            value = Player.Instance.Health;
            base.Update();
        }
    }
}
