using Characters;

namespace CustomEffects
{
    public class HealEffect : ICustomEffect
    {
        public void Apply(BaseCharacter character)
        {
            character.Health = character.MaxHealth;
        }
    }
}