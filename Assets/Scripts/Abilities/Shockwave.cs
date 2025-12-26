using Characters;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities
{
    public class Shockwave : BaseAbility
    {
        override protected void OnUse()
        {
            List<Collider2D> results = new();
            Physics2D.OverlapCircle(
                transform.position, Range,
                ContactFilter2D.noFilter, results
            );

            foreach (Collider2D c in results)
            {
                if (!c.gameObject.TryGetComponent(out Enemy character)) continue;

                Vector3 characterDir = character.transform.position - transform.position;
                characterDir.z = 0f;
                characterDir.Normalize();

                character.ApplyDamage(Damage);
                character.ApplyKnockback(characterDir * Knockback);
            }
        }
    }
}
