using Characters;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CharacterHealthBar : MonoBehaviour
    {
        [SerializeField] RectTransform bar;
        [SerializeField] Color barColor;
        protected float maxValue = 0f;
        protected RectTransform rectTransform;
        protected BaseCharacter character;

        protected void OnValidate()
        {
            bar.GetComponent<Image>().color = barColor;
        }

        protected void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            character = GetComponentInParent<BaseCharacter>();
            maxValue = character.MaxHealth;
        }

        protected void FixedUpdate()
        {
            float value = character.Health;
            bar.offsetMax = new Vector2
            (
                (value / maxValue - 1f) * rectTransform.rect.width,
                bar.offsetMax.y
            );
        }
    }
}
