using Characters;
using CustomEffects;
using System;
using UnityEngine;

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

    public EffectType effectType;

    // Modifier
    public ModifierType modifierType;
    [Tooltip(
        "Constant - increase base value\n" +
        "Linear - increase the linear multiplier\n" +
        "Exponential - multiply the exponential multiplier"
    )]
    public ModifierScaling modifierScaling;
    public float modifierValue;

    // Weapon
    public GameObject weapon;

    // Ability
    public GameObject ability;

    // Custom
    public ICustomEffect.CustomEffect customEffect;

    public void Apply(BaseCharacter character) {
        switch (effectType)
        {
            case EffectType.Modifier:
                character.ApplyModifierScaling(modifierType, modifierScaling, modifierValue);
                break;

            case EffectType.Weapon:
                character.EquipWeapon(weapon);
                break;

            case EffectType.Ability:
                character.EquipAbility(ability);
                break;

            case EffectType.Custom:
                var effect = ICustomEffect.GetCustomEffect(customEffect);
                effect.Apply(character);
                break;
        }
    }

    public void Revert(BaseCharacter character)
    {
        switch (effectType)
        {
            case EffectType.Modifier:
                float value = (modifierScaling == ModifierScaling.Exponential) ? 1f / modifierValue : -modifierValue;
                character.ApplyModifierScaling(modifierType, modifierScaling, value);
                break;
            case EffectType.Weapon:
                // I'll just assume the player didn't get a new weapon before the current one got reverted
                character.RevertWeapon();
                break;
            case EffectType.Ability:
                // Same here
                character.RevertAbility();
                break;

            case EffectType.Custom:
                var effect = ICustomEffect.GetCustomEffect(customEffect);
                effect.Revert(character);
                break;
        }
    }
}
