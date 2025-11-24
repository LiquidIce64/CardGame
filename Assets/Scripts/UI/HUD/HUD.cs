using UnityEngine;

public class HUD : MonoBehaviour
{
    private static HUD instance;

    public static HUD Instance => instance;

    private void Awake()
    {
        instance = this;
    }
}
