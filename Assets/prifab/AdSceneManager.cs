using UnityEngine;
using YG;
using System.Collections;
using UnityEngine.SceneManagement;

public class AdSceneManager : MonoBehaviour
{

    [Header("Настройки рекламы")]
    public bool enableAdOnClick = true;

    [Header("События рекламы")]
    public UnityEngine.Events.UnityEvent onAdStarted;    // Когда реклама началась
    public UnityEngine.Events.UnityEvent onAdFinished;   // Когда реклама закончилась
    public UnityEngine.Events.UnityEvent onAdError;      // Когда ошибка рекламы
    public GameObject G1, G2, G3, G4;
    public Playercontroller playercontroller;
    public WaveSystem WaveSystem1;
    public star_policion Star_Policion;
    public SurvivalTimer SurvivalTimer;
    public int idClik;
    private string firstLaunchKey = "FirstLaunch";
    public WBGL_derect wBGL_Derect;
    private void OnEnable()
    {
        // Подписываемся на события полноэкранной рекламы
        YandexGame.OpenFullAdEvent += OnAdStarted;
        YandexGame.CloseFullAdEvent += OnAdFinished;
        YandexGame.ErrorFullAdEvent += OnAdError;
    }

    private void OnDisable()
    {
        // Отписываемся от событий при выключении скрипта
        YandexGame.OpenFullAdEvent -= OnAdStarted;
        YandexGame.CloseFullAdEvent -= OnAdFinished;
        YandexGame.ErrorFullAdEvent -= OnAdError;
    }

    // Публичный метод для кнопки - показать полноэкранную рекламу
    public void ShowInterstitialAd(int id)
    {
        if (!enableAdOnClick)
            return;
        idClik = id;
        if (!YandexGame.nowAdsShow)
        {
            YandexGame.FullscreenShow();
        }
        else
        {
            Debug.LogWarning("Реклама уже показывается! Текущий статус: " + GetAdStatus());
        }
    }

    // Вызывается когда начинается полноэкранная реклама
    private void OnAdStarted()
    {
        Debug.Log("Полноэкранная реклама началась - останавливаем время игры");

        // Останавливаем игровое время
        Time.timeScale = 0f;

        // Вызываем событие
        onAdStarted?.Invoke();
    }

    private void OnAdFinished()
    {
        Debug.Log("Полноэкранная реклама закончилась - возобновляем время игры");

        // Возобновляем игровое время
        Time.timeScale = 1f;
        if (idClik == 0)
        {
            SceneManager.LoadScene(0);
        }
        if (idClik == 1)
        {
            SceneManager.LoadScene(1);
        }
        if (idClik == 2)
        {
            if (PlayerPrefs.GetInt(firstLaunchKey, 0) == 0)
            {
                Time.timeScale = 0f; // Останавливаем игру
                PlayerPrefs.SetInt(firstLaunchKey, 1);
                PlayerPrefs.Save();
            }
            else
            {
                // Не первый запуск - время нормальное, показываем только контроллер
                Time.timeScale = 1f; // Нормальная скорость игры
                if (G1 != null) G1.SetActive(true);
                if (G2 != null) G2.SetActive(true);
                if (G3 != null) G3.SetActive(true);
                if (G4 != null) G4.SetActive(false);

                // Вызываем методы с проверкой на null
                playercontroller?.play2();
                Star_Policion?.startStar();
                WaveSystem1?.StartSpawning();
                SurvivalTimer?.StartGame();
                wBGL_Derect.CheckFirstLaunch();
            }
        }
        // Вызываем событие
        onAdFinished?.Invoke();
    }

    public void PlayTutot()
    {
                        // Не первый запуск - время нормальное, показываем только контроллер
                Time.timeScale = 1f; // Нормальная скорость игры
                if (G1 != null) G1.SetActive(true);
                if (G2 != null) G2.SetActive(true);
                if (G3 != null) G3.SetActive(true);
                if (G4 != null) G4.SetActive(false);

                // Вызываем методы с проверкой на null
                playercontroller?.play2();
                Star_Policion?.startStar();
                WaveSystem1?.StartSpawning();
                SurvivalTimer?.StartGame();
                wBGL_Derect.CheckFirstLaunch();
    }
    // Вызывается при ошибке показа рекламы
    private void OnAdError()
    {
        Debug.LogError("Ошибка показа полноэкранной рекламы");

        // Все равно возобновляем время на случай ошибки
        Time.timeScale = 1f;

        onAdError?.Invoke();
    }

    // Метод для проверки, можно ли показывать рекламу
    public bool CanShowAd()
    {
        return !YandexGame.nowAdsShow;
    }

    // Метод для получения текущего статуса рекламы
    public string GetAdStatus()
    {
        if (YandexGame.nowFullAd) return "Показывается полноэкранная реклама";
        if (YandexGame.nowVideoAd) return "Показывается реклама за вознаграждение";
        return "Реклама не показывается";
    }

    // Метод для принудительного показа рекламы (игнорируя проверки)
    public void ForceShowAd()
    {
        YandexGame.FullscreenShow();
    }

    // Метод для проверки таймера рекламы
    public float GetAdTimer()
    {
        // Возвращает сколько времени прошло с последнего показа
        return YandexGame.timerShowAd;
    }

    // Метод для проверки, готов ли таймер рекламы
    public bool IsAdReady()
    {
        // Проверяем, прошло ли достаточно времени с последнего показа
        return YandexGame.timerShowAd >= YandexGame.Instance.infoYG.fullscreenAdInterval;
    }
}
