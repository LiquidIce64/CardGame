using Characters;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons
{
    public class Fists : BaseWeapon
    {
        [SerializeField] protected float sweepAngle = 45f;

        override protected void OnUse(Vector3 targetPos)
        {
            Vector3 targetDirection = GetDirectionToPos(targetPos);

            List<Collider2D> results = new();
            Physics2D.OverlapCircle(
                owner.transform.position, Range,
                ContactFilter2D.noFilter, results
            );

            foreach (Collider2D c in results)
            {
                if (!c.gameObject.TryGetComponent(out BaseCharacter character)) continue;
                if (!IsTarget(character)) continue;

                Vector3 characterDir = GetDirectionToPos(character.transform.position);
                if (Vector3.Angle(targetDirection, characterDir) > sweepAngle) continue;

                character.ApplyDamage(Damage);
                character.ApplyKnockback(characterDir * Knockback);
            }
        }
    }
}