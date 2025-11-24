using Characters;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class Card : MonoBehaviour
{
    private const float speed = 15f;
    private RectTransform rect;
    private Vector3 targetPos;
    private Vector3 rotationCenter;
    private CardObject cardObject;
    private ICardDeck cardDeck;
    [SerializeField] private Image image;
    [SerializeField] private GameObject outline;
    public bool updateRotation = false;

    public RectTransform Rect => rect;

    public CardObject CardObject
    {
        get { return cardObject; }
        set {
            cardObject = value;
            image.sprite = cardObject.Sprite;
        }
    }

    public bool Selected
    {
        get => outline.activeSelf;
        set => outline.SetActive(value);
    }

    public ICardDeck CardDeck
    {
        get => cardDeck;
        set
        {
            cardDeck?.UnregisterCard(this);
            cardDeck = value;
            if (cardDeck != null)
            {
                transform.SetParent(cardDeck.transform, true);
                cardDeck.RegisterCard(this);
            }
        }
    }

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        targetPos = rect.position;
        rotationCenter = rect.position;
        rotationCenter.y -= 1f;
    }

    private void OnDestroy()
    {
        CardDeck = null;
    }

    public void SetTarget(Vector3 pos)
    {
        targetPos = pos;
        updateRotation = false;
    }

    public void SetTarget(Vector3 pos, Vector3 center)
    {
        targetPos = pos;
        rotationCenter = center;
        updateRotation = true;
    }

    [ContextMenu("Use Card")]
    public void Use()
    {
        cardObject.ApplyEffects(Player.Instance);
        Destroy(gameObject);
    }

    private void Update()
    {
        Vector3 newPos = rect.localPosition;
        newPos += Mathf.Min(1f, speed * Time.unscaledDeltaTime) * (targetPos - newPos);
        rect.localPosition = newPos;
        if (updateRotation)
        {
            Vector3 normal = newPos - rotationCenter;
            normal.z = 0f;
            normal.Normalize();
            rect.rotation = Quaternion.LookRotation(Vector3.forward, normal);
        }
        else rect.rotation = Quaternion.identity;
    }
}
