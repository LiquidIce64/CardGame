using UnityEngine;

namespace Abilities
{
    public class RocketShower : BaseAbility
    {
        [SerializeField] private GameObject rocketPrefab;

        override protected void OnUse()
        {
            var pos = targetPos - owner.transform.position;
            pos = Vector3.ClampMagnitude(pos, Range);
            pos += owner.transform.position;
            pos += (Vector3)(Random.insideUnitCircle * Accuracy);
            var rocketObj = Instantiate(rocketPrefab, pos, Quaternion.identity);
            var rocket = rocketObj.GetComponent<Rocket>();
            rocket.Damage = Damage;
            rocket.Knockback = Knockback;
        }
    }
}
