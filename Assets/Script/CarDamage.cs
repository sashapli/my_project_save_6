using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CarDamage : MonoBehaviour
{
    private float maxHP = 100f;
    public float HP = 100f;
    private float minHP = 2f;
    public Image Hp;
    public SpriteRenderer _spr;
    public List<Sprite> lom = new List<Sprite>();
    public List<Sprite> lom2 = new List<Sprite>();
    public List<Sprite> lom3 = new List<Sprite>();
    public Playercontroller playe;
    public GameObject _im;
    public float late;
    public AnimarotTwin DoTween;

    public static CarDamage Instance { get; private set; } // Статическая ссылка
    private Rigidbody2D _rd;
    public SurvivalTimer SurvivalTimer;
    public WaveSystem WaveSystem;
    public star_policion star_Policion;
    public Minu minu;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Убедимся, что игрок один
        }
    }

    void Start()
    {
        _rd = GetComponent<Rigidbody2D>();
        playe = GetComponent<Playercontroller>();
        _spr = GetComponent<SpriteRenderer>();

        switch (SaveMenager.Instance.GetCar())
        {
            case 1: _spr.sprite = lom[0]; HP = 200; maxHP = 200; break;
            case 2: _spr.sprite = lom2[0]; HP = 1000; maxHP = 1000; break;
            case 3: _spr.sprite = lom3[0]; HP = 700; maxHP = 700; break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        float speed = _rd.velocity.magnitude;

        if (speed < minHP) return;

        Vector2 avageNormal = Vector2.zero;

        foreach(ContactPoint2D contact in collision.contacts)
        {
            avageNormal += contact.normal;
        }
        avageNormal /= collision.contacts.Length;

        Vector2 normal = collision.contacts[0].normal;

        Vector2 normaVelosity = _rd.velocity.normalized;

        float angler = Vector2.Angle(collision.relativeVelocity, avageNormal);//Mathf.Abs(Vector2.Angle(normaVelosity,normal ) * Mathf.Rad2Deg) -90;

        float imaAngle = speed * Mathf.Abs(Mathf.Cos(angler * Mathf.Deg2Rad));

        float damage = imaAngle * 100;

       // texst.text = $"урон: {damage}, угол: {Mathf.Abs(Mathf.Cos(angler * Mathf.Deg2Rad))}, скорость: {speed} здоровье: {HP} ";

        HP -= damage / 20;

        if (HP <= 0)
        {
            switch (SaveMenager.Instance.GetCar())
            {
                case 1: _spr.sprite = lom[4]; break;
                case 2: _spr.sprite = lom2[4]; break;
                case 3: _spr.sprite = lom3[4]; break;
            }
            playe.moveSpeed = 0;
            playe.moveRotation = 0;
            _im.SetActive(true);
            SurvivalTimer.StopTimer();
            WaveSystem.KillAllBots();
            SaveMenager.Instance.AddCoins(SurvivalTimer.Coinpp);
            SurvivalTimer.Coinpp = 0;
            star_Policion.isGame = true;
            WaveSystem.enabled = false;
            minu.isGame = false;
        }
        else if (HP <= 25)
        {
            switch (SaveMenager.Instance.GetCar())
            {
                case 1: _spr.sprite = lom[3]; break;
                case 2: _spr.sprite = lom2[3]; break;
                case 3: _spr.sprite = lom3[3]; break;
            }
        }
        else if (HP <= 50)
        {
            switch (SaveMenager.Instance.GetCar())
            {
                case 1: _spr.sprite = lom[2]; break;
                case 2: _spr.sprite = lom2[2]; break;
                case 3: _spr.sprite = lom3[2]; break;
            }
        }
        else if (HP <= 75)
        {
            switch (SaveMenager.Instance.GetCar())
            {
                case 1: _spr.sprite = lom[1]; break;
                case 2: _spr.sprite = lom2[1]; break;
                case 3: _spr.sprite = lom3[1]; break;
            }
        }
        else if (HP <= 100)
        {
            switch (SaveMenager.Instance.GetCar())
            {
                case 1: _spr.sprite = lom[0]; break;
                case 2: _spr.sprite = lom2[0]; break;
                case 3: _spr.sprite = lom3[0]; break;
            }
        }
    }
     void OnCollisionStay2D(Collision2D collision)
    { 
        if (collision.gameObject.CompareTag("bot1"))
        {
           HP -= 0.1f;
            
        }
        if (collision.gameObject.CompareTag("bot2"))
        {
            HP -= 0.3f;
        }
        if(HP <= 0)
        {
            Resets();
        }
        if (HP <= 0)
        {
            switch (SaveMenager.Instance.GetCar())
            {
                case 1: _spr.sprite = lom[4]; break;
                case 2: _spr.sprite = lom2[4]; break;
                case 3: _spr.sprite = lom3[4]; break;
            }
        }
        else if (HP <= 25)
        {
            switch (SaveMenager.Instance.GetCar())
            {
                case 1: _spr.sprite = lom[3]; break;
                case 2: _spr.sprite = lom2[3]; break;
                case 3: _spr.sprite = lom3[3]; break;
            }
        }
        else if (HP <= 50)
        {
            switch (SaveMenager.Instance.GetCar())
            {
                case 1: _spr.sprite = lom[2]; break;
                case 2: _spr.sprite = lom2[2]; break;
                case 3: _spr.sprite = lom3[2]; break;
            }
        }
        else if (HP <= 75)
        {
            switch (SaveMenager.Instance.GetCar())
            {
                case 1: _spr.sprite = lom[1]; break;
                case 2: _spr.sprite = lom2[1]; break;
                case 3: _spr.sprite = lom3[1]; break;
            }
        }
        else if (HP <= 100)
        {
            switch (SaveMenager.Instance.GetCar())
            {
                case 1: _spr.sprite = lom[0]; break;
                case 2: _spr.sprite = lom2[0]; break;
                case 3: _spr.sprite = lom3[0]; break;
            }
        }
      
    }

    private void Update()
    {      
        Hp.fillAmount = HP / maxHP;
    }

    void Resets()
    {
        if (HP <= 0 && !_im.activeSelf)
        {
            switch (SaveMenager.Instance.GetCar())
            {
                case 1: _spr.sprite = lom[4]; break;
                case 2: _spr.sprite = lom2[4]; break;
                case 3: _spr.sprite = lom3[4]; break;
            }
            playe.moveSpeed = 0;
            playe.moveRotation = 0;
            _im.SetActive(true);
            DoTween.End();
            star_Policion.isGame = false;
            // Просто останавливаем таймер без проверок
            if (SurvivalTimer.Instance != null)
            {
                SurvivalTimer.Instance.StopTimer();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
    }

    public void ReastartLevel()
    {
        SceneManager.LoadScene(0);
    }
}
