using Characters;
using UnityEngine;

namespace Weapons
{
    abstract public class BaseWeapon : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer sprite;
        [SerializeField] protected Transform barrelTransform;
        [SerializeField] protected float baseDamage = 1f;
        [SerializeField] protected float baseKnockback = 1f;
        [SerializeField] protected float baseRange = 1f;
        [SerializeField] protected float baseFireRate = 1f;
        [SerializeField] protected float baseAccuracy = 1f;
        protected Vector3 targetPos;
        protected float _cooldown = 0f;
        protected BaseCharacter owner;

        public float Damage => owner.ApplyModifier(ModifierType.Damage, baseDamage);
        public float Knockback => owner.ApplyModifier(ModifierType.Knockback, baseKnockback);
        public float Range => owner.ApplyModifier(ModifierType.Range, baseRange);
        public float FireRate => owner.ApplyModifier(ModifierType.FireRate, baseFireRate);
        public float Accuracy => owner.ApplyModifier(ModifierType.Accuracy, baseAccuracy);
        public float RemainingCooldown => _cooldown;

        public Vector3 TargetPos
        {
            get => targetPos;
            set => targetPos = value - transform.localPosition;
        }

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

        public void Use()
        {
            if (_cooldown > 0f) return;
            _cooldown = 1f / FireRate;
            OnUse();
        }

        abstract protected void OnUse();

        protected void Awake()
        {
            owner = GetComponentInParent<BaseCharacter>();
        }

        protected void FixedUpdate()
        {
            if (_cooldown > 0f) _cooldown -= Time.fixedDeltaTime;

            if (targetPos != null)
            {
                var dir = GetDirectionToPos(targetPos);
                transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
                if (sprite != null) sprite.flipY = dir.x < 0f;
            }
        }
    }
}