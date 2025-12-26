using System;

public enum ModifierType
{
    Health,
    Damage,
    Knockback,
    DamageResistance,
    KnockbackResistance,
    Speed,
    ReloadSpeed,
    FireRate,
    Range,
    Accuracy
}

public enum ModifierScaling
{
    Constant,
    Linear,
    Exponential
}

public class Modifier
{
    public readonly ModifierType modifierType;

    private float constantFactor = 0f;
    private float linearFactor = 1f;
    private float exponentialFactor = 1f;

    public float ConstantFactor => constantFactor;
    public float LinearFactor => linearFactor;
    public float ExponentialFactor => exponentialFactor;

    public Modifier(ModifierType modifierType)
    {
        this.modifierType = modifierType;
    }

    public void ApplyScaling(ModifierScaling scaling, float value)
    {
        switch (scaling)
        {
            case ModifierScaling.Constant:
                constantFactor += value;
                break;
            case ModifierScaling.Linear:
                linearFactor += value;
                break;
            case ModifierScaling.Exponential:
                exponentialFactor *= value;
                break;
        }
    }

    public float Apply(float baseValue) => (baseValue + constantFactor) * MathF.Max(0f, linearFactor) * MathF.Max(0f, exponentialFactor);
}
