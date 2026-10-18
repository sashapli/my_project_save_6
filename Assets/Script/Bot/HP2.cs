using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP2 : MonoBehaviour
{
    public BotController Bot1;
    public float health = 200;
    public WaveSystem waveSystem; // Ссылка на систему волн

    void Start()
    {
        // Автоматически находим систему волн
        waveSystem = FindObjectOfType<WaveSystem>();

        // Регистрируем врага в системе волн
        if (waveSystem != null)
        {
            waveSystem.RegisterEnemy(gameObject);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            health -= 5;
            CheckDeath();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float damag = Bot1.rb.velocity.magnitude * 100;
        health -= damag;
        CheckDeath();
    }

    void CheckDeath()
    {
        if (health <= 0 && Bot1.isagent == false)
        {
            Bot1.rotationSpeed = 0;
            Bot1._rend.sprite = Bot1._srite;
            Bot1.isagent = true;
            Bot1.system.Play();
            Bot1.agent.speed = 0;

            // Уведомляем систему волн о смерти
            if (waveSystem != null)
            {
                waveSystem.RemoveEnemy(gameObject);
            }

            Invoke("Des", 4);
        }
    }

    private void Des()
    {
        // ui.Instance.moneyPlus(20);
        Destroy(gameObject);
    }

    // Вызывается при уничтожении объекта
    void OnDestroy()
    {
        // На всякий случай удаляем из системы волн
        if (waveSystem != null)
        {
            waveSystem.RemoveEnemy(gameObject);
        }
    }
}