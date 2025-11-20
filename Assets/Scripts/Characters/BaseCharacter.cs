using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace Characters
{
    abstract public class BaseCharacter : MonoBehaviour
    {
        [SerializeField] protected float moveSpeed = 500f;
        [SerializeField] protected float maxHealth = 100f;
        [SerializeField] protected CardEffect[] startingEffects;
        protected float health;
        protected readonly List<GameObject> weaponHistory = new();
        protected BaseWeapon equippedWeapon;
        protected Rigidbody2D rb;

        public float Health => health;
        public float MaxHealth => maxHealth;
        public BaseWeapon EquippedWeapon => equippedWeapon;

        protected void OnValidate()
        {
            foreach (var effect in startingEffects)
                effect.OnValidate();
        }

        protected void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            health = maxHealth;
        }

        protected void Start()
        {
            foreach (var effect in startingEffects)
                effect.Apply(this);
        }

        public void ApplyKnockback(Vector3 knockback)
        {
            Vector2 _knockback = knockback;
            rb.linearVelocity += _knockback;
        }

        public void ApplyDamage(float damage)
        {
            health -= damage;
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
            SetWeapon(weaponPrefab);
            weaponHistory.Add(weaponPrefab);
        }

        public void RevertWeapon()
        {
            weaponHistory.RemoveAt(weaponHistory.Count - 1);
            var weaponPrefab = weaponHistory[^1];
            SetWeapon(weaponPrefab);
        }

        abstract protected void OnDeath();

        protected void Move(Vector3 moveVector)
        {
            rb.AddForce(moveVector * moveSpeed);
        }
    }
}