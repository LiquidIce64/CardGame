using Characters;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class CardHand : MonoBehaviour, ICardDeck
{
    private static CardHand instance;

    [SerializeField] private float selectedCardOffset = 75f;
    [SerializeField] private int startingCardCount = 6;
    [SerializeField] private float cardSpacingMultiplier = 1f;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private RectTransform hudRect;
    private RectTransform rectTransform;
    private int selectedCard = -1;
    public readonly List<Card> cards = new();

    public static CardHand Instance => instance;

    private void Awake()
    {
        instance = this;
        rectTransform = GetComponent<RectTransform>();
        cards.Capacity = startingCardCount;
        AddCards(startingCardCount);
    }

    private void Start()
    {
        Player.InputActions.Player.UseCard.performed += _ => UseCard();
    }

    public void RegisterCard(Card card) => cards.Add(card);
    public void UnregisterCard(Card card) => cards.Remove(card);

    [ContextMenu("Add Card")]
    private void AddCard()
    {
        AddCard(CardObject.GetRandomCard());
    }

    private void AddCard(CardObject cardObject)
    {
        var card = Instantiate(cardPrefab, rectTransform).GetComponent<Card>();
        card.CardObject = cardObject;
        card.CardDeck = this;
    }

    private void AddCards(int count)
    {
        foreach (CardObject cardObj in CardObject.GetRandomCards(count))
            AddCard(cardObj);
    }

    private void UseCard()
    {
        if (selectedCard == -1) return;
        cards[selectedCard].Use();
        selectedCard = -1;
    }

    private float GetRadius()
    {
        float w = rectTransform.rect.width;
        float h = rectTransform.rect.height;
        return h / 2 + (w * w) / (h * 8);
    }

    private void UpdateCardPositions()
    {
        if (cards.Count == 0)
        {
            selectedCard = -1;
            return;
        }

        float radius = GetRadius();
        Vector3 center = new(0f, rectTransform.rect.height / 2 - radius, 0f);

        if (cards.Count == 1)
        {
            selectedCard = 0;
            Vector3 pos = new(0f, rectTransform.rect.height / 2 + selectedCardOffset, 0f);
            var card = cards[0];
            card.SetTarget(pos, center);
            card.Selected = true;
            return;
        }

        float maxAngle = Mathf.Asin(rectTransform.rect.width / (radius * 2));
        float maxAngleDelta = Mathf.Asin(cards[0].Rect.rect.width * cardSpacingMultiplier / radius);
        float angleDelta = Mathf.Min(maxAngle * 2 / (cards.Count - 1), maxAngleDelta);
        float angle = -angleDelta * (cards.Count - 1) / 2;

        Vector3 mousePos = Player.InputActions.Player.MousePosition.ReadValue<Vector2>();
        mousePos = Camera.main.ScreenToViewportPoint(mousePos) - new Vector3(0.5f, 0.5f, 0f);
        mousePos *= hudRect.rect.size;
        mousePos -= rectTransform.localPosition;
        float mouseAngle = Vector2.SignedAngle(rectTransform.up, (mousePos - center).normalized) * Mathf.Deg2Rad;
        selectedCard = Mathf.RoundToInt((mouseAngle - angle) / angleDelta);
        selectedCard = cards.Count - 1 - selectedCard;
        selectedCard = Mathf.Clamp(selectedCard, 0, cards.Count - 1);

        int i = 0;
        foreach (var card in cards)
        {
            Vector3 pos = center;
            pos.x += radius * Mathf.Sin(angle);
            pos.y += radius * Mathf.Cos(angle);
            bool selected = i == selectedCard;
            if (selected)
            {
                Vector3 normal = pos - center;
                normal.z = 0f;
                normal.Normalize();
                pos += normal * selectedCardOffset;
            }
            if (i <= selectedCard)
                card.transform.SetAsLastSibling();
            else
                card.transform.SetAsFirstSibling();
            card.SetTarget(pos, center);
            card.Selected = selected;
            angle += angleDelta;
            i++;
        }
    }

    private void Update()
    {
        UpdateCardPositions();
    }

    private void OnDrawGizmosSelected()
    {
        rectTransform = GetComponent<RectTransform>();
        float scaleFactor = rectTransform.TransformVector(rectTransform.up).magnitude;
        float radius = GetRadius();
        Vector3 center = new(0f, rectTransform.rect.height / 2 - radius, 0f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(rectTransform.position + center * scaleFactor, radius * scaleFactor);
    }
}
