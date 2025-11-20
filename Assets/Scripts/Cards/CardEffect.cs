using Characters;
using System;
using Unity.VisualScripting;
using UnityEngine;
using Weapons;

[Serializable]
public struct CardEffect
{
    public enum EffectType
    {
        Modifier,
        Weapon,
        Ability,
        Custom
    }

    public enum ModifierType
    {
        Damage,
        Knockback,
        Resistance,
        Speed,
        ReloadSpeed,
        FiringRate,
    }

    public enum ModifierScaling
    {
        Linear,
        LinearPercent,
        Exponential
    }

    public EffectType effectType;

    // Modifier
    public ModifierType modifierType;
    public ModifierScaling modifierScaling;
    public float modifierValue;

    // Weapon
    public GameObject weapon;

    // Ability


    // Custom


    public void Apply(BaseCharacter character) {
        switch (effectType)
        {
            case EffectType.Modifier:

                break;

            case EffectType.Weapon:
                character.EquipWeapon(weapon);
                break;

            case EffectType.Ability:

                break;

            case EffectType.Custom:

                break;
        }
    }

    public void OnValidate()
    {
        if (weapon != null)
        {
            if (!weapon.IsPrefabDefinition() || !weapon.TryGetComponent<BaseWeapon>(out var _))
            {
                Debug.LogWarning("Weapon effect requires a weapon prefab");
            }
        }
    }
}
