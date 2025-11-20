using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using static CardEffect;

[CustomPropertyDrawer(typeof(CardEffect))]
public class CardEffectDrawer : PropertyDrawer
{
    private class CardEffectFoldout : Foldout
    {
        private static readonly Dictionary<string, EffectType> effectMap = new()
        {
            [nameof(CardEffect.modifierType)] = EffectType.Modifier,
            [nameof(CardEffect.modifierScaling)] = EffectType.Modifier,
            [nameof(CardEffect.modifierValue)] = EffectType.Modifier,

            [nameof(CardEffect.weapon)] = EffectType.Weapon,
        };
        private readonly Dictionary<string, PropertyField> fieldMap = new();

        public CardEffectFoldout(SerializedProperty property)
        {
            var effectTypeProp = property.FindPropertyRelative(nameof(CardEffect.effectType));
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
                var show = effectMap[entry.Key] == effectType;
                entry.Value.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }
    }

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        CardEffectFoldout foldout = new(property) { text = property.displayName };
        return foldout;
    }
}
