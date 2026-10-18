using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using YG;

public class MadificGarash : MonoBehaviour
{
    public int Stranica = 1;
    public Animator Animation;
    public Animator AnimationJ;
    public Animator AnimationS;
    public Scrollbar[] ScrollRil;
    public Scrollbar[] ScrollFake;
    public float[] normParam;
    public float[] jeepParam;
    public float[] SportParam;
    public int ClikGarash;

    [Header("Текстовые элементы")]
    public TMP_Text defaultCarStatusText; // Стартовый автомобиль (всегда куплен)
    public TMP_Text jeepStatusText;       // Jeep
    public TMP_Text sportcarStatusText;   // Sportcar
    public TMP_Text MoneyText;

    [Header("Локализация - Русский")]
    public string ru_equipped = "Экипирован";
    public string ru_equip = "Экипировать";
    public string ru_buy = "Купить за";

    [Header("Локализация - Английский")]
    public string en_equipped = "Equipped";
    public string en_equip = "Equip";
    public string en_buy = "Buy for";

    [Header("Цены предметов")]
    public int jeepPrice = 1000;
    public int sportcarPrice = 5000;
    public RectTransform garash;

    private void Awake()
    {
        // Подписываемся на событие смены языка
        YandexGame.SwitchLangEvent += SwitchLanguage;
    }

    private void Start()
    {
        UpdateAllItemStatuses();
        // Применяем текущий язык при старте
        SwitchLanguage(YandexGame.savesData.language);
    }

    private void OnEnable() => YandexGame.SwitchLangEvent += SwitchLanguage;
    private void OnDisable() => YandexGame.SwitchLangEvent -= SwitchLanguage;

    public void SwitchLanguage(string lang)
    {
        // Обновляем все тексты при смене языка
        UpdateAllItemStatuses();
    }

    public void UpdateAllItemStatuses()
    {
        UpdateDefaultCarStatus();
        UpdateJeepStatus();
        UpdateSportcarStatus();
    }

    private string GetLocalizedText(string russianText, string englishText)
    {
        string currentLang = YandexGame.savesData.language;
        return currentLang == "ru" ? russianText : englishText;
    }

    private void UpdateDefaultCarStatus()
    {
        if (defaultCarStatusText == null) return;

        int currentCar = SaveMenager.Instance.GetCar();

        // Стартовый автомобиль всегда куплен
        if (currentCar == 1) // Стартовый автомобиль экипирован
        {
            defaultCarStatusText.text = GetLocalizedText(ru_equipped, en_equipped);
            defaultCarStatusText.color = Color.green;
        }
        else // Стартовый автомобиль не экипирован
        {
            defaultCarStatusText.text = GetLocalizedText(ru_equip, en_equip);
            defaultCarStatusText.color = Color.white;
        }
    }

    private void UpdateJeepStatus()
    {
        if (jeepStatusText == null) return;

        bool isJeepPurchased = SaveMenager.Instance.GetISjeep();
        int currentCar = SaveMenager.Instance.GetCar();

        if (isJeepPurchased)
        {
            if (currentCar == 2) // Jeep экипирован
            {
                jeepStatusText.text = GetLocalizedText(ru_equipped, en_equipped);
                jeepStatusText.color = Color.green;
            }
            else // Jeep куплен, но не экипирован
            {
                jeepStatusText.text = GetLocalizedText(ru_equip, en_equip);
                jeepStatusText.color = Color.white;
            }
        }
        else // Jeep не куплен
        {
            string buyText = GetLocalizedText(ru_buy, en_buy);
            jeepStatusText.text = $"{buyText} {jeepPrice}";
            jeepStatusText.color = Color.yellow;
        }
    }

    private void UpdateSportcarStatus()
    {
        if (sportcarStatusText == null) return;

        bool isSportcarPurchased = SaveMenager.Instance.GetISSprortcar();
        int currentCar = SaveMenager.Instance.GetCar();

        if (isSportcarPurchased)
        {
            if (currentCar == 3) // Sportcar экипирован
            {
                sportcarStatusText.text = GetLocalizedText(ru_equipped, en_equipped);
                sportcarStatusText.color = Color.green;
            }
            else // Sportcar куплен, но не экипирован
            {
                sportcarStatusText.text = GetLocalizedText(ru_equip, en_equip);
                sportcarStatusText.color = Color.white;
            }
        }
        else // Sportcar не куплен
        {
            string buyText = GetLocalizedText(ru_buy, en_buy);
            sportcarStatusText.text = $"{buyText} {sportcarPrice}";
            sportcarStatusText.color = Color.yellow;
        }
    }

    public void UPBut()
    {
        if (Stranica == 2)
        {
            Animation.SetTrigger("Up2");
            Stranica++;
            ScrollFake[0].size = SportParam[0];
            ScrollFake[1].size = SportParam[1];
        }
        if (Stranica == 1)
        {
            Animation.SetTrigger("Up");
            Stranica++;
            ScrollFake[0].size = jeepParam[0];
            ScrollFake[1].size = jeepParam[1];
        }
        SaveMenager.Instance.GetISjeep();
        SaveMenager.Instance.GetISSprortcar();
        SaveMenager.Instance.GetCar();
    }
    public void DowBut()
    {
        if (Stranica == 2)
        {
            Animation.SetTrigger("Dowen");
            Stranica--;
            ScrollFake[0].size = normParam[0];
            ScrollFake[1].size = normParam[1];
        }
        if (Stranica == 3)
        {
            Animation.SetTrigger("Dowen2");
            Stranica--;
            ScrollFake[0].size = jeepParam[0];
            ScrollFake[1].size = jeepParam[1];
        }
    }

    public void buttEcpir()
    {
        switch (Stranica)
        {
            case 1:
                SaveMenager.Instance.AddCar(1);
                ClikGarash = 1;
                UpdateAllItemStatuses();
                StartCoroutine(Coritiin1());
                break;
            case 2:
                if (SaveMenager.Instance.GetISjeep())
                {
                    SaveMenager.Instance.AddCar(2);
                    ClikGarash = 2;
                    UpdateAllItemStatuses();
                    StartCoroutine(Coritiin1());
                }
                else if (SaveMenager.Instance.GetCoins() >= 500)
                {
                    SaveMenager.Instance.AddIsjeep(true);
                    SaveMenager.Instance.AddCar(2);
                    SaveMenager.Instance.AddCoins(-500);
                    ClikGarash = 2;
                    UpdateAllItemStatuses();
                    StartCoroutine(Coritiin1());
                }
                else
                {
                    AnimationJ.SetTrigger("NoCoinA");
                }
                break;
            case 3:
                if (SaveMenager.Instance.GetISSprortcar())
                {
                    SaveMenager.Instance.AddCar(3);
                    ClikGarash = 3;
                    UpdateAllItemStatuses();
                    StartCoroutine(Coritiin1());
                }
                else if (SaveMenager.Instance.GetCoins() >= 1000)
                {
                    SaveMenager.Instance.AddIsSportcar(true);
                    SaveMenager.Instance.AddCar(3);
                    SaveMenager.Instance.AddCoins(-1000);
                    ClikGarash = 3;
                    UpdateAllItemStatuses();
                    StartCoroutine(Coritiin1());
                }
                else
                {
                    AnimationS.SetTrigger("NoCoinA");
                }
                break;
        }
    }

    IEnumerator Coritiin1()
    {
        float speed = 2f; // Скорость изменения
        while (ClikGarash == Stranica)
        {
            ScrollRil[0].size = Mathf.MoveTowards(ScrollRil[0].size, ScrollFake[0].size, speed * Time.deltaTime);
            ScrollRil[1].size = Mathf.MoveTowards(ScrollRil[1].size, ScrollFake[1].size, speed * Time.deltaTime);
            yield return null; // Ждем следующий кадр вместо фиксированного интервала
        }
    }
}