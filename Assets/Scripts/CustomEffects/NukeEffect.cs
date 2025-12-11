using Characters;
using UnityEngine;

namespace CustomEffects
{
    public class NukeEffect : ICustomEffect
    {
        private const float damage = 500f;

        public void Apply(BaseCharacter character)
        {
            var enemies = GameObject.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            foreach (Enemy enemy in enemies)
            {
                enemy.ApplyDamage(damage);
            }
        }
    }
}