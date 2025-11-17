using Characters;
using UnityEngine;

namespace Weapons
{
    abstract public class BaseWeapon : MonoBehaviour
    {
        [SerializeField] protected float damage = 1f;
        [SerializeField] protected float knockback = 1f;
        [SerializeField] protected float range = 1f;
        [SerializeField] protected float cooldown = 1f;
        protected float _cooldown = 0f;
        protected BaseCharacter owner;

        public float Damage => damage;
        public float Knockback => knockback;
        public float Range => range;
        public float Cooldown => cooldown;
        public float RemainingCooldown => _cooldown;

        protected Vector3 GetDirectionToPos(Vector3 pos)
        {
            Vector3 vec = pos - owner.transform.position;
            vec.z = 0f;
            return vec.normalized;
        }

        protected bool IsTarget(BaseCharacter character)
        {
            if (owner is Player) return character != owner;

            return character is Player;
        }

        public void Use(Vector3 targetPos)
        {
            if (_cooldown > 0f) return;
            _cooldown = cooldown;
            OnUse(targetPos);
        }

        abstract protected void OnUse(Vector3 targetPos);

        protected void Awake()
        {
            owner = GetComponentInParent<BaseCharacter>();
            owner.EquippedWeapon = this;
        }

        protected void FixedUpdate()
        {
            if (_cooldown > 0f) _cooldown -= Time.fixedDeltaTime;
        }
    }
}