using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;

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
            Debug.Log("You win");
        }

        new protected void OnDestroy() { return; }
    }
}