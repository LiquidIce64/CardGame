using UnityEngine;
using Weapons;

namespace Characters
{
    abstract public class BaseCharacter : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 500f;
        [SerializeField] protected float maxHealth = 100f;
        protected float health;
        protected BaseWeapon equippedWeapon;
        protected Rigidbody2D rb;

        public float Health => health;
        public float MaxHealth => maxHealth;

        public BaseWeapon EquippedWeapon
        {
            get { return equippedWeapon; }
            set { equippedWeapon = value; }
        }

        protected void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            health = maxHealth;
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

        abstract protected void OnDeath();

        protected void Move(Vector3 moveVector)
        {
            rb.AddForce(moveVector * moveSpeed);
        }
    }
}