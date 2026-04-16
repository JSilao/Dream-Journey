using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFlash : MonoBehaviour
{
    public Image overlay; 
    public float flashDuration = 0.5f; 
    public float damageDuration = 0.3f; 

    private Coroutine currentFlash;

    public void Flash(bool bright)
    {
        if(currentFlash != null)
            StopCoroutine(currentFlash);

        currentFlash = StartCoroutine(DoFlash(bright));
    }

    private IEnumerator DoFlash(bool bright)
    {
        Color targetColor = bright ? new Color(1,1,1,0.5f) : new Color(0,0,0,0.5f);
        overlay.color = targetColor;

        float elapsed = 0f;
        while(elapsed < flashDuration)
        {
            float alpha = Mathf.Lerp(targetColor.a, 0f, elapsed / flashDuration);
            overlay.color = new Color(targetColor.r, targetColor.g, targetColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        overlay.color = new Color(targetColor.r, targetColor.g, targetColor.b, 0f);
    }

    public void FlashDamage()
    {
        if (currentFlash != null)
            StopCoroutine(currentFlash);

        currentFlash = StartCoroutine(DamageFlash());
    }

    private IEnumerator DamageFlash()
    {
        Color targetColor = new Color(1f, 0f, 0f, 0.2f); 

        overlay.color = targetColor;

        float elapsed = 0f;

        while (elapsed < damageDuration)
        {
            float alpha = Mathf.Lerp(targetColor.a, 0f, elapsed / damageDuration);
            overlay.color = new Color(targetColor.r, targetColor.g, targetColor.b, alpha);

            elapsed += Time.deltaTime;
            yield return null;
        }

        overlay.color = new Color(1f, 0f, 0f, 0f);
    }
}