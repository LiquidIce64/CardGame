using Characters;
using UnityEngine;

namespace Weapons
{
    abstract public class BaseWeapon : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer sprite;
        [SerializeField] protected Transform barrelTransform;
        [SerializeField] protected GameObject bulletTrace;
        [SerializeField] protected float baseDamage = 1f;
        [SerializeField] protected float baseKnockback = 1f;
        [SerializeField] protected float selfKnockback = 0f;
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
            targetPos = owner.TargetPos;
            OnUse();
        }

        protected void TraceBullet(Vector3 direction)
        {
            if (bulletTrace == null) return;

            var endPoint = owner.transform.position + transform.localPosition + direction * Range;
            var midPoint = (barrelTransform.position + endPoint) / 2;
            direction = endPoint - barrelTransform.position;

            var trace = Instantiate(bulletTrace, midPoint, Quaternion.FromToRotation(Vector3.right, direction));
            trace.transform.localScale = new Vector3(direction.magnitude, trace.transform.localScale.y, trace.transform.localScale.z);
        }

        abstract protected void OnUse();

        protected void Awake()
        {
            owner = GetComponentInParent<BaseCharacter>();
        }

        protected void FixedUpdate()
        {
            if (_cooldown > 0f) _cooldown -= Time.fixedDeltaTime;

            targetPos = owner.TargetPos;

            if (targetPos != null)
            {
                var posOffset = Vector3.Scale(transform.localPosition, owner.transform.localScale - Vector3.one);
                var dir = GetDirectionToPos(targetPos - posOffset);
                transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
                transform.localScale = dir.x < 0f ? new Vector3(1f, -1f, 1f) : Vector3.one;
            }
        }
    }
}