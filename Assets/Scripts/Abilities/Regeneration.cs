using UnityEngine;

namespace Abilities
{
    public class Regeneration : BaseAbility
    {
        [SerializeField] private float regenAmount = 10f;

        override protected void OnUse()
        {
            owner.Health += regenAmount;
        }
    }
}
