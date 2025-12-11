using Characters;
using UnityEngine;

namespace Abilities
{
    abstract public class BaseAbility : MonoBehaviour
    {
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

        protected bool IsTarget(BaseCharacter character)
        {
            if (owner is Player) return character != owner;

            return character is Player;
        }

        abstract protected void OnUse();

        protected void Awake()
        {
            owner = GetComponentInParent<BaseCharacter>();
        }

        protected void FixedUpdate()
        {
            if (_cooldown > 0f) _cooldown -= Time.fixedDeltaTime;

            if (_cooldown > 0f) return;
            _cooldown = 1f / FireRate;
            OnUse();
        }
    }
}