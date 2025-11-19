using Characters;
using UnityEngine;

[CreateAssetMenu(fileName = "CardObject", menuName = "Scriptable Objects/CardObject")]
public class CardObject : ScriptableObject
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private CardEffect[] effects;

    public void ApplyEffects(Player player)
    {
        foreach (var effect in effects)
            effect.Apply(player);
    }
}

