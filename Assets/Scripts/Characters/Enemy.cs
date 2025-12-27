using UnityEngine;
using UnityEngine.AI;

namespace Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : BaseCharacter
    {
        protected enum NPCState
        {
            Follow,
            Attack,
            Retreat
        }

        protected NavMeshAgent agent;
        [SerializeField] protected float maxAttackDistance = 7.5f;
        [SerializeField] protected float attackDistance = 5f;
        [SerializeField] protected float minAttackDistance = 2.5f;
        [SerializeField] protected float aimingTime = 1f;
        protected float _remainingAimTime;
        protected NPCState _state = NPCState.Follow;

        private void ResetAimTime() => _remainingAimTime = aimingTime * Random.Range(0.95f, 1.05f);

        new protected void Awake()
        {
            base.Awake();
            agent = GetComponent<NavMeshAgent>();
            agent.updatePosition = false;
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            ResetAimTime();
        }

        new protected void Start()
        {
            base.Start();
            WaveController.Instance.RegisterEnemy(this);
        }

        protected void FollowPlayer()
        {
            ResetAimTime();
            agent.SetDestination(Player.Instance.transform.position);
        }

        protected void Retreat()
        {
            ResetAimTime();

            var playerDirection = Player.Instance.transform.position;
            playerDirection -= agent.transform.position;
            playerDirection.z = 0f;
            playerDirection.Normalize();

            agent.SetDestination(agent.transform.position - playerDirection * maxAttackDistance);
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

        protected void Attack()
        {
            agent.ResetPath();
            if (_remainingAimTime > 0f)
                _remainingAimTime -= Time.fixedDeltaTime;
            else
                equippedWeapon.Use();
        }

        virtual protected void EnemyLogic()
        {
            var distance = GetDistanceToPlayer();
            switch (_state)
            {
                case NPCState.Follow:
                    FollowPlayer();
                    if (distance <= Mathf.Min(equippedWeapon.Range, attackDistance))
                        _state = NPCState.Attack;
                    break;
                case NPCState.Attack:
                    Attack();
                    if (distance > Mathf.Min(equippedWeapon.Range, maxAttackDistance))
                        _state = NPCState.Follow;
                    else if (distance < minAttackDistance)
                        _state = NPCState.Retreat;
                    break;
                case NPCState.Retreat:
                    Retreat();
                    if (distance >= Mathf.Min(equippedWeapon.Range, attackDistance))
                        _state = NPCState.Attack;
                    break;
            }
        }

        protected void FixedUpdate()
        {
            agent.nextPosition = transform.position;

            targetPos = Player.Instance.transform.position;

            EnemyLogic();

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