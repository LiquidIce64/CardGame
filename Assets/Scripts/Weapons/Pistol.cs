using Characters;
using UnityEngine;

namespace Weapons
{
    public class Pistol : BaseWeapon
    {
        static protected string[] layers = new[]
        {
            "Default",
            "Player",
            "Enemy"
        };

        override protected void OnUse()
        {
            Vector3 targetDirection = GetDirectionToPos(targetPos);

            var accuracy = Accuracy;
            targetDirection = Quaternion.Euler(0f, 0f, Random.Range(-accuracy, accuracy)) * targetDirection;

            var hits = Physics2D.RaycastAll(
                owner.transform.position,
                targetDirection,
                Range,
                LayerMask.GetMask(layers)
            );
            
            foreach (var hit in hits)
            {
                if (!hit.collider.gameObject.TryGetComponent(out BaseCharacter character)) continue;
                if (!IsTarget(character)) continue;

                Vector3 characterDir = GetDirectionToPos(character.transform.position);

                character.ApplyDamage(Damage);
                character.ApplyKnockback(characterDir * Knockback);
            }

            Debug.DrawLine(barrelTransform.position, transform.position + targetDirection * Range, Color.red, 1f);
        }
    }
}