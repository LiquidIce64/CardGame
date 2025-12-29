using Characters;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class CardPicker : MonoBehaviour, ICardDeck
{
    private static CardPicker instance;

    [SerializeField] private float selectedCardOffset = 75f;
    [SerializeField] private int cardCount = 6;
    [SerializeField] private int cardsToSelect = 3;
    [SerializeField] private float cardSpacingMultiplier = 1.5f;
    [SerializeField] private float cardSpaceMultiplier = 0.8f;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private RectTransform hudRect;
    [SerializeField] private Button button;
    [SerializeField] private RectTransform cardInfoRect;
    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private TextMeshProUGUI cardDescription;
    private TextMeshProUGUI buttonText;
    private RectTransform buttonTransform;
    private RectTransform rectTransform;
    public readonly List<Card> cards = new();
    private readonly SortedSet<int> selectedCards = new();

    public static CardPicker Instance => instance;

    private void Awake()
    {
        instance = this;
        buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        buttonTransform = button.GetComponent<RectTransform>();
        rectTransform = GetComponent<RectTransform>();
        cards.Capacity = cardCount;
        gameObject.SetActive(false);
    }

    [ContextMenu("Show")]
    public void Show()
    {
        // Rerolls the cards in case this gets called twice
        foreach (var card in cards)
            Destroy(card.gameObject);

        gameObject.SetActive(true);
        Time.timeScale = 0f;
        UpdateButton();
        AddCards(cardCount);
    }

    [ContextMenu("Take Cards")]
    public void TakeCards()
    {
        // Move selected cards to hand deck
        List<Card> selectedCardsList = new(selectedCards.Count);
        foreach (int idx in selectedCards)
            selectedCardsList.Add(cards[idx]);
        foreach (var card in selectedCardsList)
            card.CardDeck = CardHand.Instance;
        selectedCards.Clear();
        
        // Delete the rest
        foreach (var card in cards)
            Destroy(card.gameObject);

        gameObject.SetActive(false);
        Time.timeScale = 1f;

        GlobalAudio.Play(GlobalAudioClip.CardDeckDraw);
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

    private void UpdateButton()
    {
        buttonText.text = $"Take selected cards ({selectedCards.Count}/{cardsToSelect})";
        button.interactable = selectedCards.Count == cardsToSelect;
    }

    private void UpdateCardInfo(Card card)
    {
        var pos = cardInfoRect.localPosition;
        pos.x = card.Rect.localPosition.x;
        cardInfoRect.localPosition = pos;
        cardName.text = card.CardObject.CardName;
        cardDescription.text = card.CardObject.CardDescription;
        cardInfoRect.gameObject.SetActive(true);
    }

    private void Update()
    {
        cardInfoRect.gameObject.SetActive(false);

        if (cards.Count == 0) return;

        Vector3 center = rectTransform.localPosition;

        if (cards.Count == 1)
        {
            selectedCards.Clear();
            selectedCards.Add(0);
            Vector3 pos = new(0f, rectTransform.rect.height / 2 + selectedCardOffset, 0f);
            var card = cards[0];
            card.SetTarget(pos);
            card.Selected = true;
            UpdateCardInfo(card);
            return;
        }

        float maxOffsetDelta = cards[0].Rect.rect.width * cardSpacingMultiplier;
        float offsetDelta = rectTransform.rect.width * cardSpaceMultiplier / (cards.Count - 1);
        if (offsetDelta > maxOffsetDelta) offsetDelta = maxOffsetDelta;
        float offset = -offsetDelta * (cards.Count - 1) / 2;

        Vector3 mousePos = Player.InputActions.Player.MousePosition.ReadValue<Vector2>();
        mousePos = Camera.main.ScreenToViewportPoint(mousePos) - new Vector3(0.5f, 0.5f, 0f);
        mousePos *= hudRect.rect.size;
        mousePos -= rectTransform.localPosition;
        
        int hoveredCard = Mathf.RoundToInt((mousePos.x - offset) / offsetDelta);
        hoveredCard = Mathf.Clamp(hoveredCard, 0, cards.Count - 1);

        float buttonTop = buttonTransform.localPosition.y + buttonTransform.rect.height;
        if (mousePos.y <= buttonTop) hoveredCard = -1;
        bool clicked = Player.InputActions.Player.Attack.WasPressedThisFrame();
        if (clicked && hoveredCard != -1)
        {
            if (selectedCards.Contains(hoveredCard))
                selectedCards.Remove(hoveredCard);
            else if (selectedCards.Count < cardsToSelect)
                selectedCards.Add(hoveredCard);
            UpdateButton();
        }

        int i = 0;
        foreach (var card in cards)
        {
            Vector3 pos = center;
            pos.x += offset;
            if (i == hoveredCard)
            {
                pos.y += selectedCardOffset;
                UpdateCardInfo(card);
            }

            if (i <= hoveredCard)
                card.transform.SetAsLastSibling();
            else
                card.transform.SetAsFirstSibling();

            card.SetTarget(pos);
            card.Selected = selectedCards.Contains(i);
            offset += offsetDelta;
            i++;
        }
    }
}
