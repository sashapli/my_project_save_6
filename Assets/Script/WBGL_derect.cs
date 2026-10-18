using UnityEngine;
using TMPro;
using System.Runtime.InteropServices;
using YG;

public class WBGL_derect : MonoBehaviour
{
    public GameObject mobileController;  // 1. Контроллер для мобилок (кнопки управления)
    public GameObject mobileTutorial;    // 2. Тутор для мобилок
   public GameObject pcTutorial;        // 3. Тутор для ПК
    public bool testInIspector = false;

    public bool IsMobileDevice;
    private string firstLaunchKey = "FirstLaunch";
    private bool isFirstLaunch = false;

    private void Start()
    {
        // Изначально отключаем всё
        if (mobileController != null) mobileController.SetActive(false);
      if (mobileTutorial != null) mobileTutorial.SetActive(false);
     if (pcTutorial != null) pcTutorial.SetActive(false);

    }

    public void CheckFirstLaunch()
    {
        // Проверяем, первый ли это запуск
        if (PlayerPrefs.GetInt(firstLaunchKey, 0) == 0)
        {
            // Первый запуск - останавливаем время и показываем тутор
            Time.timeScale = 0f; // Останавливаем игру
            isFirstLaunch = true;
            DetermineDeviceType();
            PlayerPrefs.SetInt(firstLaunchKey, 1);
            PlayerPrefs.Save();
        }
        else
        {
            // Не первый запуск - время нормальное, показываем только контроллер
            Time.timeScale = 1f; // Нормальная скорость игры
            isFirstLaunch = false;
            ShowControllerOnly();
        }
    }

    private void DetermineDeviceType()
    {
        // Определяем тип устройства
#if UNITY_EDITOR
        IsMobileDevice = SystemInfo.deviceType == DeviceType.Handheld;
#else
        IsMobileDevice = YandexGame.EnvironmentData.isMobile || YandexGame.EnvironmentData.isTablet;
#endif

        if (testInIspector)
        {
            IsMobileDevice = !IsMobileDevice;
        }

        if (IsMobileDevice)
        {
            // Мобильное устройство: показываем мобильный тутор и контроллер
           if (mobileTutorial != null) mobileTutorial.SetActive(true);
            if (mobileController != null) mobileController.SetActive(true);
            if (pcTutorial != null) pcTutorial.SetActive(false);
            Debug.Log("Мобильное устройство - показан мобильный туториал и контроллер (время остановлено)");
        }
        else
        {
            // ПК устройство: показываем только ПК тутор
           if (mobileTutorial != null) mobileTutorial.SetActive(false);
            if (mobileController != null) mobileController.SetActive(false);
           if (pcTutorial != null) pcTutorial.SetActive(true);
            Debug.Log("ПК устройство - показан ПК туториал (время остановлено)");
        }
    }

    private void ShowControllerOnly()
    {
        // Не первый запуск: показываем только контроллер для мобилок
#if UNITY_EDITOR
        IsMobileDevice = SystemInfo.deviceType == DeviceType.Handheld;
#else
        IsMobileDevice = YandexGame.EnvironmentData.isMobile || YandexGame.EnvironmentData.isTablet;
#endif

        if (testInIspector)
        {
            IsMobileDevice = !IsMobileDevice;
        }

        if (IsMobileDevice)
        {
            if (mobileController != null) mobileController.SetActive(true);
        }

        // Всегда отключаем туторы при повторных запусках
       if (mobileTutorial != null) mobileTutorial.SetActive(false);
        if (pcTutorial != null) pcTutorial.SetActive(false);

        Debug.Log("Не первый запуск - показан только контроллер для мобилок");
    }

    // Публичный метод для кнопки - размораживает игру
    public void ResumeGame()
    {
        if (isFirstLaunch)
        {
            Time.timeScale = 1f; // Возобновляем игру
            isFirstLaunch = false;

            // Скрываем туторы после нажатия кнопки
          if (mobileTutorial != null) mobileTutorial.SetActive(false);
          if (pcTutorial != null) pcTutorial.SetActive(false);

            Debug.Log("Игра разморожена, туториал скрыт");
        }
    }

    // Метод для сброса первого запуска
    public void ResetFirstLaunch()
    {
        PlayerPrefs.DeleteKey(firstLaunchKey);
        PlayerPrefs.Save();
        Time.timeScale = 1f; // Восстанавливаем время
        isFirstLaunch = false;
        Debug.Log("Первый запуск сброшен, время восстановлено");
    }
}