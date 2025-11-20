using Characters;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(Image))]
public class Card : MonoBehaviour
{
    private const float speed = 15f;
    private RectTransform rect;
    private Vector3 targetPos;
    private Vector3 rotationCenter;
    private CardObject cardObject;

    public RectTransform Rect => rect;

    public CardObject CardObject
    {
        get { return cardObject; }
        set {
            cardObject = value;
            GetComponent<Image>().sprite = cardObject.Sprite;
        }
    }

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        targetPos = rect.position;
        rotationCenter = rect.position;
        rotationCenter.y -= 1f;
        CardHand hand = GetComponentInParent<CardHand>();
        hand.cards.Add(this);
    }

    private void OnDestroy()
    {
        CardHand hand = GetComponentInParent<CardHand>();
        if (hand != null) hand.cards.Remove(this);
    }

    public void SetTarget(Vector3 pos, Vector3 center)
    {
        targetPos = pos;
        rotationCenter = center;
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
        newPos += Mathf.Min(1f, speed * Time.deltaTime) * (targetPos - newPos);
        rect.localPosition = newPos;
        Vector3 normal = newPos - rotationCenter;
        normal.z = 0f;
        normal.Normalize();
        rect.rotation = Quaternion.LookRotation(Vector3.forward, normal);
    }
}
