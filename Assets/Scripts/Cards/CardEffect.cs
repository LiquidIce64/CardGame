using Characters;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Weapons;

[Serializable]
public struct CardEffect
{
    public enum EffectType
    {
        Modifier,
        Weapon,
        Ability
    }

    public EffectType effectType;

    // Modifier
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

    public ModifierType modifierType;
    public ModifierScaling modifierScaling;
    public float modifierValue;

    // Weapon
    public BaseWeapon weapon;

    // Ability


    public void Apply(Player player) { }


#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(CardEffect))]
    public class CardEffectDrawer : PropertyDrawer
    {
        private class CardEffectFoldout : Foldout
        {
            private readonly SerializedProperty property;
            private static readonly Dictionary<string, EffectType> effectMap = new()
            {
                ["modifierType"] = EffectType.Modifier,
                ["modifierScaling"] = EffectType.Modifier,
                ["modifierValue"] = EffectType.Modifier,

                ["weapon"] = EffectType.Weapon,
            };
            private readonly Dictionary<string, PropertyField> fieldMap = new();

            public CardEffectFoldout(SerializedProperty property)
            {
                this.property = property;

                var effectTypeProp = property.FindPropertyRelative(nameof(effectType));
                PropertyField effectTypeField = new(effectTypeProp);
                effectTypeField.TrackPropertyValue(effectTypeProp, UpdatePropertyFields);
                Add(effectTypeField);

                foreach (var key in effectMap.Keys)
                {
                    PropertyField field = new(property.FindPropertyRelative(key));
                    fieldMap[key] = field;
                    Add(field);
                }

                UpdatePropertyFields(effectTypeProp);
            }

            private void UpdatePropertyFields(SerializedProperty effectTypeProp)
            {
                var effectType = (EffectType)effectTypeProp.enumValueIndex;
                foreach (KeyValuePair<string, PropertyField> entry in fieldMap)
                {
                    entry.Value.visible = effectMap[entry.Key] == effectType;
                }
            }
        }

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            CardEffectFoldout foldout = new(property) { text = property.displayName };
            return foldout;
        }
    }

#endif
}
