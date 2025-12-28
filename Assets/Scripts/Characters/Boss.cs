using UnityEngine;

namespace Characters
{
    public class Boss : Enemy
    {
        new protected void Start()
        {
            base.Start();
            var controller = WaveController.Instance;
            controller.RegisterEnemy(this);
            controller.UnregisterEnemy(this);
        }

        override protected void OnDeath()
        {
            Destroy(gameObject);
            HUD.Instance.GameOver(isVictory: true);
        }

        new protected void OnDestroy() { return; }
    }
}