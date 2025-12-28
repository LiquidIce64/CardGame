using Characters;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] Transform sprite;
    private float speed = 1f;
    private float damage = 0f;
    private float knockback = 0f;
    [SerializeField] private float explosionRange = 2f;
    
    public float Damage { set => damage = value; }
    public float Knockback { set => knockback = value; }

    void Start()
    {
        speed *= sprite.localPosition.y;
    }

    void FixedUpdate()
    {
        sprite.localPosition += Time.fixedDeltaTime * speed * Vector3.down;
        if (sprite.localPosition.y <= 0.5f * sprite.transform.localScale.y)
        {
            List<Collider2D> results = new();
            Physics2D.OverlapCircle(
                transform.position, explosionRange,
                ContactFilter2D.noFilter, results
            );

            foreach (Collider2D c in results)
            {
                if (!c.gameObject.TryGetComponent(out Enemy character)) continue;

                Vector3 characterDir = character.transform.position - transform.position;
                characterDir.z = 0f;
                characterDir.Normalize();

                character.ApplyDamage(damage);
                character.ApplyKnockback(characterDir * knockback);
            }

            Destroy(gameObject);
        }
    }
}
