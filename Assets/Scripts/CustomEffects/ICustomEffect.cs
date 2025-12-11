using Characters;
using System;
using UnityEngine;

namespace CustomEffects
{
    public interface ICustomEffect
    {
        public enum CustomEffect
        {
            Nuke,
        }

        public static ICustomEffect GetCustomEffect(CustomEffect type)
        {
            return type switch
            {
                CustomEffect.Nuke => new NukeEffect(),
                _ => throw new Exception("Invalid custom effect"),
            };
        }

        public void Apply(BaseCharacter character)
        {
            Debug.LogException(new NotImplementedException());
        }

        public void Revert(BaseCharacter character)
        {
            Debug.LogException(new NotImplementedException());
        }
    }
}
