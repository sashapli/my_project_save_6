using UnityEngine;
using TMPro;
using YG;

public class SurvivalTimer : MonoBehaviour
{
    public static SurvivalTimer Instance;

    public TMP_Text survivalTimeText; // Текст времени во время игры
    public TMP_Text finalTimeText;    // Текст финального времени после смерти
    public TMP_Text finalCoinText;  

    private float survivalTime;
    private bool isTimerRunning;
    public int Coinpp;
    public float late;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (isTimerRunning)
        {
            survivalTime += Time.deltaTime;
            survivalTimeText.text = FormatTime(survivalTime);
            if (Time.time - late > 1)
            {
                late = Time.time;
                Coinpp++;
            }

        }
    }

    public void StartGame()
    {
        isTimerRunning = true;
        survivalTime = 0f;

        // Запускаем волны
        WaveSystem waveSystem = FindObjectOfType<WaveSystem>();
        if (waveSystem != null)
            waveSystem.StartSpawning();
    }

    public void StopTimer()
    {
        if (!isTimerRunning) return;

        isTimerRunning = false;
        int totalSeconds = Mathf.FloorToInt(survivalTime);

        // Показываем финальное время
        finalTimeText.text = $"Время: {FormatTimeForDisplay(survivalTime)}";
        finalCoinText.text =$"Вы получили{Coinpp}";

        // Отправляем в лидерборд
        SendTimeToLeaderboard(totalSeconds);
    }

    private void SendTimeToLeaderboard(int totalSeconds)
    {
        // Просто отправляем текущий результат в лидерборд
        YandexGame.NewLeaderboardScores("survivalTime", totalSeconds);
        Debug.Log($"Отправлено в лидерборд: {totalSeconds} сек");
    }

    string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        return $"{minutes:00}:{seconds:00}";
    }

    string FormatTimeForDisplay(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        return minutes > 0 ? $"{minutes} мин {seconds} сек" : $"{seconds} сек";
    }
}