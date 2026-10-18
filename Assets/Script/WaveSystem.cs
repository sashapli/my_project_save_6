using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaveSystem : MonoBehaviour
{
    public star_policion starScript;
    public Transform[] spawnPoints;
    public GameObject[] enemyPrefabs;
    public float baseSpawnTime = 5f;
    public float minSpawnTime = 1f;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool isInfiniteWave = false;
    private float waveStartTime;
    private float spawnRateMultiplier = 1f;

    private Coroutine spawnCoroutine;
    private bool isWaveActive = false;
    private int currentSpawnedCount = 0;
    private bool isSpawningStarted = false;

    // Время для каждой волны в секундах
    private float[] waveDurations = { 60f, 60f, 60f, 60f }; // 1, 1.5, 2, 2.5 минуты

    public void StartSpawning()
    {
        if (!isSpawningStarted)
        {
            isSpawningStarted = true;
            StartNextWave();
        }
    }

    void Start()
    {

    }

    void Update()
    {
        // Постоянно чистим null элементы
        activeEnemies.RemoveAll(item => item == null);

        // Автоматически начинаем следующую волну когда star_policion изменил номер волны
        // и текущая волна не активна
        if (!isWaveActive && !isInfiniteWave && isSpawningStarted)
        {
            StartNextWave();
        }

        if (isInfiniteWave){ }
        else if (isWaveActive)
        {
            float waveTimeLeft = GetCurrentWaveDuration() - (Time.time - waveStartTime);
        }
    }

    void StartNextWave()
    {
        // Останавливаем предыдущую волну если была
        StopAllSpawning();

        // НЕ очищаем врагов с прошлой волны - они остаются на карте!
        // ClearAllEnemies(); // ЗАКОММЕНТИРОВАНО

        // Запускаем волну в соответствии с star_policion
        switch (starScript.intstar)
        {
            case 1:
                StartWave1();
                break;
            case 2:
                StartWave2();
                break;
            case 3:
                StartWave3();
                break;
            case 4:
                StartWave4();
                break;
            case 5:
                StartInfiniteWave();
                break;
            default:
                // Если волна больше 5, продолжаем бесконечную
                if (!isInfiniteWave)
                {
                    StartInfiniteWave();
                }
                break;
        }
    }

    void StartWave1()
    {
        isWaveActive = true;
        currentSpawnedCount = 0;
        waveStartTime = Time.time;
        spawnCoroutine = StartCoroutine(SpawnWave(new GameObject[] { enemyPrefabs[0] }, baseSpawnTime));

    }

    void StartWave2()
    {
        isWaveActive = true;
        currentSpawnedCount = 0;
        waveStartTime = Time.time;
        spawnCoroutine = StartCoroutine(SpawnWave(new GameObject[] { enemyPrefabs[1] }, baseSpawnTime * 0.8f));
    }

    void StartWave3()
    {
        isWaveActive = true;
        currentSpawnedCount = 0;
        waveStartTime = Time.time;
        spawnCoroutine = StartCoroutine(SpawnWave(new GameObject[] { enemyPrefabs[2] }, baseSpawnTime * 0.6f));
    }

    void StartWave4()
    {
        isWaveActive = true;
        currentSpawnedCount = 0;
        waveStartTime = Time.time;
        spawnCoroutine = StartCoroutine(SpawnWave(new GameObject[] { enemyPrefabs[2] }, baseSpawnTime * 0.5f));
    }

    void StartInfiniteWave()
    {
        isInfiniteWave = true;
        isWaveActive = false;
        waveStartTime = Time.time;
        spawnRateMultiplier = 1f;

        StopAllSpawning();
        spawnCoroutine = StartCoroutine(SpawnInfiniteWave());
    }

    IEnumerator SpawnWave(GameObject[] enemyTypes, float spawnInterval)
    {
        float waveDuration = GetCurrentWaveDuration();

        while (Time.time - waveStartTime < waveDuration)
        {
            GameObject randomEnemy = enemyTypes[Random.Range(0, enemyTypes.Length)];
            SpawnEnemy(randomEnemy);
            currentSpawnedCount++;
            yield return new WaitForSeconds(spawnInterval);
        }

        // Волна завершена по времени
        isWaveActive = false;

        // Враги остаются на карте и будут сражаться в следующей волне
    }

    IEnumerator SpawnInfiniteWave()
    {
        while (isInfiniteWave)
        {
            float waveDuration = Time.time - waveStartTime;
            spawnRateMultiplier = 1f + (waveDuration / 60f);

            GameObject randomEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            SpawnEnemy(randomEnemy);

            float currentSpawnInterval = Mathf.Max(minSpawnTime, baseSpawnTime / spawnRateMultiplier);
            yield return new WaitForSeconds(currentSpawnInterval);
        }
    }

    float GetCurrentWaveDuration()
    {
        int waveIndex = starScript.intstar - 1;
        if (waveIndex >= 0 && waveIndex < waveDurations.Length)
        {
            return waveDurations[waveIndex];
        }
        return 60f; // значение по умолчанию
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        if (enemyPrefab == null)
        {
            return;
        }

        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        // Настраиваем ссылку на WaveSystem
        HP2 enemyHealth = newEnemy.GetComponent<HP2>();
        if (enemyHealth != null)
        {
            enemyHealth.waveSystem = this;
        }

        activeEnemies.Add(newEnemy);
    }

    public void RegisterEnemy(GameObject enemy)
    {
        if (!activeEnemies.Contains(enemy))
        {
            activeEnemies.Add(enemy);
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }
    }

    void StopAllSpawning()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);
    }

    // Этот метод теперь используется только для принудительной очистки
    public void ClearAllEnemies()
    {
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null)
                Destroy(enemy);
        }
        activeEnemies.Clear();
    }

    public void KillAllBots()
    {
        // Создаем временный список чтобы избежать ошибок при удалении
        List<GameObject> botsToKill = new List<GameObject>(activeEnemies);

        foreach (GameObject bot in botsToKill)
        {
            if (bot != null)
            {
                KillBot(bot);
            }
        }

        activeEnemies.Clear();
    }

    private void KillBot(GameObject bot)
    {
        // Проверяем все возможные скрипты здоровья
        HP1 hp1 = bot.GetComponent<HP1>();
        HP2 hp2 = bot.GetComponent<HP2>();
        BotController3 botController3 = bot.GetComponent<BotController3>();

        if (hp1 != null)
        {
            BotController botController = bot.GetComponent<BotController>();
            if (botController != null)
            {
                botController.rotationSpeed = 0;
                botController._rend.sprite = botController._srite;
                botController.isagent = true;
                botController.system.Play();
                botController.agent.speed = 0;
            }
            Destroy(bot, 4f);
        }
        else if (hp2 != null)
        {
            BotController botController = bot.GetComponent<BotController>();
            if (botController != null)
            {
                botController.rotationSpeed = 0;
                botController._rend.sprite = botController._srite;
                botController.isagent = true;
                botController.system.Play();
                botController.agent.speed = 0;
            }
            Destroy(bot, 4f);
        }
        else if (botController3 != null)
        {
            botController3.rotationSpeed = 0;
            botController3.isagent = true;
            botController3._rend.sprite = botController3._srite;

            NavMeshAgent agent = bot.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.speed = 0;
            }

            botController3.system.Play();
            Destroy(bot, 4f);
        }
        else
        {
            Destroy(bot);
        }
    }
}