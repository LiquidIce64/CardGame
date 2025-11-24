using Characters;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardObject", menuName = "Scriptable Objects/CardObject")]
public class CardObject : ScriptableObject
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private string cardName;
    [SerializeField] private string cardDescription;
    [SerializeField] private CardEffect[] effects;
    
    private static CardObject[] cardObjectPool;

    public Sprite Sprite => sprite;
    public CardEffect[] Effects => effects;

    private static void LoadPool() => cardObjectPool ??= Resources.LoadAll<CardObject>("CardObjects");

    public static CardObject GetRandomCard()
    {
        LoadPool();
        return cardObjectPool[Random.Range(0, cardObjectPool.Length)];
    }

    public static IEnumerable<CardObject> GetRandomCards(int count)
    {
        LoadPool();
        foreach (int idx in HelperFuncs.GetRandomIndices(cardObjectPool.Length, count))
            yield return cardObjectPool[idx];
    }

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

