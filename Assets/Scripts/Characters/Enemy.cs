using UnityEngine;
using UnityEngine.AI;

namespace Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : BaseCharacter
    {
        private NavMeshAgent agent;

        new protected void Awake()
        {
            base.Awake();
            agent = GetComponent<NavMeshAgent>();
            agent.updatePosition = false;
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }

        new protected void Start()
        {
            base.Start();
            WaveController.Instance.RegisterEnemy(this);
        }

        protected void FollowPlayer()
        {
            agent.SetDestination(Player.Instance.transform.position);
        }

        protected float GetDistanceToPlayer()
        {
            Vector3 vec = Player.Instance.transform.position;
            vec -= agent.transform.position;
            vec.z = 0f;
            return vec.magnitude;
        }

        override protected void OnDeath()
        {
            Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            agent.nextPosition = transform.position;

            targetPos = Player.Instance.transform.position;

            if (GetDistanceToPlayer() > equippedWeapon.Range)
            {
                FollowPlayer();
            }
            else
            {
                equippedWeapon.Use();
            }

            Move(agent.desiredVelocity.normalized);
        }

        protected void OnDestroy()
        {
            var controller = WaveController.Instance;
            if (controller != null)
                controller.UnregisterEnemy(this);
        }
    }
}