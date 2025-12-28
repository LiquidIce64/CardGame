using System.Collections;
using UnityEngine;

public class BulletTrace : MonoBehaviour
{
    [SerializeField] private float duration = 0.25f;

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
    
    void Start()
    {
        StartCoroutine(Timer());
    }
}
