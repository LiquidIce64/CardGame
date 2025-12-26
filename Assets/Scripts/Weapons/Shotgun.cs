using Characters;
using UnityEngine;

namespace Weapons
{
    public class Shotgun : BaseWeapon
    {
        static protected string[] layers = new[]
        {
            "Default",
            "Player",
            "Enemy"
        };

        [SerializeField] protected int numPellets = 12;

        override protected void OnUse()
        {
            Vector3 targetDirection = GetDirectionToPos(targetPos);
            var accuracy = Accuracy;

            for (int i = 0; i < numPellets; i++)
            {
                var direction = Quaternion.Euler(0f, 0f, Random.Range(-accuracy, accuracy)) * targetDirection;

                var hits = Physics2D.RaycastAll(
                    owner.transform.position,
                    direction,
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

                Debug.DrawLine(barrelTransform.position, transform.position + direction * Range, Color.red, 1f);
            }

            owner.ApplyKnockback(-targetDirection * selfKnockback);
        }
    }
}