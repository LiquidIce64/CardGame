using System;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace Characters
{
    abstract public class BaseCharacter : MonoBehaviour
    {
        [SerializeField] protected float baseSpeed = 200f;
        [SerializeField] protected float baseMaxHealth = 100f;
        [SerializeField] protected CardEffect[] startingEffects;
        protected float health;
        protected readonly List<GameObject> weaponHistory = new();
        protected readonly List<GameObject> abilities = new();
        protected BaseWeapon equippedWeapon;
        protected Rigidbody2D rb;
        protected readonly Dictionary<ModifierType, Modifier> modifiers = new();
        protected Vector3 targetPos = new();

        public float Health { get => health; set => health = Mathf.Clamp(value, 0f, MaxHealth); }
        public float MaxHealth => ApplyModifier(ModifierType.Health, baseMaxHealth);
        public BaseWeapon EquippedWeapon => equippedWeapon;
        public Vector3 TargetPos => targetPos;

        protected void Awake()
        {
            foreach (ModifierType modifierType in Enum.GetValues(typeof(ModifierType)))
                modifiers[modifierType] = new Modifier(modifierType);
            rb = GetComponent<Rigidbody2D>();
            health = MaxHealth;
        }

        protected void Start()
        {
            foreach (var effect in startingEffects)
                effect.Apply(this);
        }

        public float ApplyModifier(ModifierType modifierType, float baseValue)
            => modifiers[modifierType].Apply(baseValue);
        public void ApplyModifierScaling(ModifierType modifierType, ModifierScaling scaling, float value)
            => modifiers[modifierType].ApplyScaling(scaling, value);

        public void ApplyKnockback(Vector3 knockback)
        {
            Vector2 _knockback = knockback;
            float knockbackStrength = _knockback.magnitude;
            _knockback.Normalize();
            _knockback *= ApplyModifier(ModifierType.KnockbackResistance, knockbackStrength);
            rb.linearVelocity += _knockback;
        }

        public void ApplyDamage(float damage)
        {
            health -= ApplyModifier(ModifierType.DamageResistance, damage);
            if (health < 0f) health = 0f;
            if (health == 0f)
            {
                OnDeath();
                return;
            }
        }

        protected bool SetWeapon(GameObject weaponPrefab)
        {
            var weaponObj = Instantiate(weaponPrefab, transform);
            if (!weaponObj.TryGetComponent<BaseWeapon>(out var weapon))
            {
                Debug.LogError("Could not get weapon component");
                Destroy(weaponObj);
                return false;
            }
            if (equippedWeapon != null) Destroy(equippedWeapon.gameObject);
            equippedWeapon = weapon;
            return true;
        }

        public void EquipWeapon(GameObject weaponPrefab)
        {
            if (SetWeapon(weaponPrefab))
                weaponHistory.Add(weaponPrefab);
        }

        public void RevertWeapon()
        {
            var weaponPrefab = weaponHistory[^1];
            if (SetWeapon(weaponPrefab))
                weaponHistory.RemoveAt(weaponHistory.Count - 1);
        }

        public void EquipAbility(GameObject abilityPrefab)
        {
            var abilityObj = Instantiate(abilityPrefab, transform);
            abilities.Add(abilityObj);
        }

        public void RevertAbility() => Destroy(abilities[^1]);

        abstract protected void OnDeath();

        protected void Move(Vector3 moveVector)
        {
            float speed = ApplyModifier(ModifierType.Speed, baseSpeed);
            rb.AddForce(moveVector * speed);
        }
    }
}