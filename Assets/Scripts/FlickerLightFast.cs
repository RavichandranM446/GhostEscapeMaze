using UnityEngine;
using System.Collections;

public class FlickerLightFast : MonoBehaviour
{
    private Light lightSource;

    [Header("Flicker Speed Settings")]
    public float minDelay = 0.2f;   // slower
    public float maxDelay = 0.8f;   // slower

    [Header("Random Full Off Chance")]
    public float fullOffChance = 0.2f;

    void Start()
    {
        lightSource = GetComponent<Light>();
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            // Sometimes fully OFF for horror effect
            if (Random.value < fullOffChance)
            {
                lightSource.enabled = false;
                yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
            }

            // Normal flicker ON/OFF
            lightSource.enabled = !lightSource.enabled;

            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        }
    }
}
