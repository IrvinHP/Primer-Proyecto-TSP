using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UISelection : MonoBehaviour
{
    public bool gazed;
    public Image button;
    public float fillTime = 3f;
    public UnityEvent onFillComplement;
    public Coroutine fillCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        gazed = false;
        button.fillAmount = 0f;
    }

    public void OnPointerEnter()
    {
        gazed = true;
        
        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
        }

        fillCoroutine = StartCoroutine(FillRadial());
    }

    public void OnPointerExit()
    {
        gazed = false;

        if (fillCoroutine != null)
        {
            StopCoroutine (fillCoroutine);
            fillCoroutine = null;
        }

        button.fillAmount = 0f;
    }

    private IEnumerator FillRadial()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fillTime)
        {
            if (!gazed)
            {
                yield break;
            }

            elapsedTime += Time.deltaTime;

            button.fillAmount = Mathf.Clamp01(elapsedTime/fillTime);

            yield return null;
        }

        onFillComplement?.Invoke();
    }
}
