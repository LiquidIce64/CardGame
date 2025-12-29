using Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardObject", menuName = "Scriptable Objects/CardObject")]
public class CardObject : ScriptableObject
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private string cardName;
    [SerializeField] private string cardDescription;
    [SerializeField] private float effectDuration = 0f;
    [SerializeField] private CardEffect[] effects;
    
    private static CardObject[] cardObjectPool;

    public Sprite Sprite => sprite;
    public string CardName => cardName;
    public string CardDescription => cardDescription;
    public float EffectDuration => effectDuration;
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

    private IEnumerator RevertEffects(BaseCharacter character)
    {
        yield return new WaitForSeconds(effectDuration);
        foreach (var effect in effects)
            effect.Revert(character);
    }

    public void ApplyEffects(BaseCharacter character)
    {
        foreach (var effect in effects)
            effect.Apply(character);
        if (effectDuration > 0f)
            character.StartCoroutine(RevertEffects(character));
    }
}

