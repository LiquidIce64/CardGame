using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class Card : MonoBehaviour
{
    private const float speed = 15f;
    private RectTransform rect;
    private Vector3 targetPos;
    private Vector3 rotationCenter;

    public RectTransform Rect => rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        targetPos = rect.position;
        rotationCenter = rect.position;
        rotationCenter.y -= 1f;
        CardHand hand = GetComponentInParent<CardHand>();
        hand.AddCard(this);
        GetComponent<Image>().color = Random.ColorHSV(0, 1);
    }

    private void OnDestroy()
    {
        CardHand hand = GetComponentInParent<CardHand>();
        if (hand != null) hand.RemoveCard(this);
    }

    public void SetTarget(Vector3 pos, Vector3 center)
    {
        targetPos = pos;
        rotationCenter = center;
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
