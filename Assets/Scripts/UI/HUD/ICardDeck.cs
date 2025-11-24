using UnityEngine;

public interface ICardDeck
{
    public void RegisterCard(Card card);
    public void UnregisterCard(Card card);
    public Transform transform { get; }
}
