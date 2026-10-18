using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AnimarotTwin : MonoBehaviour
{
    public RectTransform _transform;
    bool flag = true;
    Image image;
    

    public object Instance { get; internal set; }

    private void Awake()
    {
        _transform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        Color color = image.color;
        color.a = 0; // Устанавливаем alpha-канал
        image.color = color;
    }

   public void End()
    {
        if (flag == true)
        {
            flag = false;
            image.DOFade(1, 1); // поевление за 0.5 секунды

        }
    }
}
