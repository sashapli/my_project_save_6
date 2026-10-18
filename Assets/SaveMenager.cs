using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class SaveMenager : MonoBehaviour
{
    public static SaveMenager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Подписываемся на событие загрузки данных
        YandexGame.GetDataEvent += OnDataLoaded;
    }

    private void OnDestroy()
    {
        // Отписываемся от события при уничтожении объекта
        YandexGame.GetDataEvent -= OnDataLoaded;
    }

    public void AddCoins(int amount)
    {
        YandexGame.savesData.coins += amount;
        SaveData();
        Debug.Log($"Добавлено {amount} монет. Всего: {YandexGame.savesData.coins}");
    }

    public void AddCar(int amount)
    {
        YandexGame.savesData.Car = amount;
        SaveData();
        Debug.Log($"Добавлено {amount} монет. Всего: {YandexGame.savesData.coins}");
    }

    public void AddIsjeep(bool amount)
    {
        YandexGame.savesData.ISjeep = amount;
        SaveData();
        Debug.Log($"Добавлено {amount} монет. Всего: {YandexGame.savesData.coins}");
    }

    public void AddIsSportcar(bool amount)
    {
        YandexGame.savesData.ISsprortCar = amount;
        SaveData();
        Debug.Log($"Добавлено {amount} монет. Всего: {YandexGame.savesData.coins}");
    }

    public int GetCoins()
    {
        if (YandexGame.SDKEnabled)
        {
            return YandexGame.savesData.coins;
        }
        return 0;
    }

    public int GetCar()
    {
        if (YandexGame.SDKEnabled)
        {
            return YandexGame.savesData.Car;
        }
        return 0;
    }
    public bool GetISjeep()
    {
        if (YandexGame.SDKEnabled)
        {
            return YandexGame.savesData.ISjeep;
        }
        return false;
    }

    public bool GetISSprortcar()
    {
        if (YandexGame.SDKEnabled)
        {
            return YandexGame.savesData.ISsprortCar;
        }
        return false;
    }


    public void SaveData()
    {
        YandexGame.SaveProgress();
    }


    private void OnDataLoaded()
    {
        Debug.Log($"Данные загружены. Монеты: {YandexGame.savesData.coins}");
    }
}
