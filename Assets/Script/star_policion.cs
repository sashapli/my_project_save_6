using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class star_policion : MonoBehaviour
{
    public Image star;
    public Sprite _starImeg;
    public Image star1;
    public Sprite _starImeg1;
    public Image star2;
    public Sprite _starImeg2;
    public Image star3;
    public Sprite _starImeg3;
    public Image star4;
    public Sprite _starImeg4;

    public int intstar = 1;
    public bool isGame = true;

    public void startStar()
    {
        Invoke("Star", 40); 
    }
    public void Star()
    {
        if (isGame) return;
        star1.sprite = _starImeg1;
            Invoke("Star2", 40);
            intstar++;
    }

    public void Star2()
    {
        if (isGame) return;
        star2.sprite = _starImeg2;
        Invoke("Star3", 40);
        intstar++;

    }
    public void Star3()
    {
        if (isGame) return;
        star3.sprite = _starImeg3;
        Invoke("Star4", 40);
        intstar++;

    }
    public void Star4()
    {
        if (isGame) return;
        star4.sprite = _starImeg4;
        intstar++;
    }
}
