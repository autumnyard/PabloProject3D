using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace Chibig
{
  public class TalkBubble : MonoBehaviour
  {
    [SerializeField] private RectTransform container;
    //[SerializeField] private Text text;
    [SerializeField] private TextMeshPro textTMP;
    [SerializeField] private string texto = "Esto es un texto de prueba.";
    [SerializeField, ReadOnly] private bool isShown = true;
    [SerializeField, ReadOnly] private Vector3 baseScale;

    private void Awake()
    {
      if (container == null) container = GetComponent<RectTransform>();
      if (textTMP == null) textTMP = GetComponent<TextMeshPro>();
    }

    private void Start()
    {
      baseScale = container.localScale;
      container.localScale = Vector3.one * 0.01f;
      container.gameObject.SetActive(false);
      isShown = false;
    }


    public void Show(string forceText = "")
    {
      if (!isShown)
      {
        LeanTween.cancel(container);

        container.gameObject.SetActive(true);
        LeanTween.scaleX(container.gameObject, baseScale.x * 1f, .15f)
          .setEaseOutQuad();

        LeanTween.scaleY(container.gameObject, baseScale.y * 1f, .3f)
          .setEaseOutBack();
        // .setOnComplete(() => container.gameObject.SetActive(true));

        if (string.IsNullOrEmpty(forceText))
        {
          //text.text = texto;
          textTMP.text = texto;
        }
        else
        {
          //text.text = forceText;
          textTMP.text = forceText;
        }

        isShown = true;
      }
    }

    public void Hide()
    {
      if (isShown)
      {
        LeanTween.cancel(container);
        LeanTween.scale(container, baseScale * 0.01f, .15f)
          .setEaseOutQuad()
          .setOnComplete(() => container.gameObject.SetActive(false));
        // container.gameObject.SetActive(false);

        isShown = false;
      }
    }
  }
}
