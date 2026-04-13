using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFlash : MonoBehaviour
{
    public Image overlay; 
    public float flashDuration = 0.5f; 

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
}