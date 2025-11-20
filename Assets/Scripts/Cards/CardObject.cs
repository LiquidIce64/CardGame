using Characters;
using UnityEngine;

[CreateAssetMenu(fileName = "CardObject", menuName = "Scriptable Objects/CardObject")]
public class CardObject : ScriptableObject
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private CardEffect[] effects;

    public Sprite Sprite => sprite;
    public CardEffect[] Effects => effects;
    
    public void ApplyEffects(BaseCharacter character)
    {
        foreach (var effect in effects)
            effect.Apply(character);
    }

    private void OnValidate()
    {
        foreach (var effect in effects)
            effect.OnValidate();
    }
}

