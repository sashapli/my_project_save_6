using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using YG;

public class ui : MonoBehaviour  
{
    public int monye;
    private float late;
    public TextMeshProUGUI tmp;
    public Image image;

    private bool isPaused = false;
    private string targetSceneName = "";
    private bool isTransitioning = false;
    private bool isAdShowingOnStart = false;
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
     //   Color color = image.color;
      //  color.a = 0; // Устанавливаем alpha-канал
      //  image.color = color;
    }
    public void garag()
    {
        LoadSceneWithAd("SampleScene"); 
    }
    public void garag2()
    {
        LoadSceneWithAd("garag");
    }
    public void moneyPlus(int moneyy)
    {
        monye += moneyy;
    }

    private void Update()
    {
        if (Time.time - late > 1)
        {
            monye++;
            late = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SaveMenager.Instance.AddCoins(1000);
        }
       //x tmp.text = $"монеты{monye}";
    }

    // ОСНОВНОЙ МЕТОД ДЛЯ ПЕРЕХОДА МЕЖДУ СЦЕНАМИ
    public void LoadSceneWithAd(string sceneName)
    {
        if (isTransitioning || isAdShowingOnStart) return;

        isTransitioning = true;
        targetSceneName = sceneName;
;

        // Показываем рекламу перед переходом
        ShowFullscreenAd();
    }

    // Метод для показа полноэкранной рекламы
    private void ShowFullscreenAd()
    {
        if (YandexGame.nowAdsShow || YandexGame.nowVideoAd)
        {
            // Если уже показывается реклама, ждем её завершения
            StartCoroutine(WaitForAdToComplete());
            return;
        }

        // Показываем полноэкранную рекламу
        YandexGame.FullscreenShow();

        // Запускаем таймаут на случай, если реклама не покажется
        StartCoroutine(AdTimeoutCheck());
    }

    // Корутина для проверки таймаута рекламы
    private IEnumerator AdTimeoutCheck()
    {
        yield return new WaitForSeconds(2f);

        // Если через 2 секунды реклама так и не началась, переходим без рекламы
        if (!YandexGame.nowAdsShow && !YandexGame.nowVideoAd)
        {
            CompleteSceneTransition();
        }
    }

    // Ожидание завершения уже идущей рекламы
    private IEnumerator WaitForAdToComplete()
    {
        while (YandexGame.nowAdsShow || YandexGame.nowVideoAd)
        {
            yield return new WaitForSeconds(0.1f);
        }

        // После завершения рекламы показываем новую
        ShowFullscreenAd();
    }

    // Обработчик закрытия полноэкранной рекламы
    private void OnFullscreenAdClosed()
    {
        CompleteSceneTransition();
    }

    // Обработчик закрытия rewarded рекламы
    private void OnRewardedAdClosed()
    {
        // Для rewarded рекламы тоже убираем черный экран
        if (isAdShowingOnStart)
        {

            isAdShowingOnStart = false;
        }
    }

    // Завершение перехода на сцену
    private void CompleteSceneTransition()
    {

        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }

        isTransitioning = false;
        targetSceneName = "";
    }
}
