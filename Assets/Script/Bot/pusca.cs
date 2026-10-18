using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class pusca : MonoBehaviour
{
    public GameObject batR;// Объект, за которым следует пушка (враг
    public GameObject target;
    public Transform dulo;
    public Transform rotatiPoint;
    public BotController3 batRS3;
    public float rotationSpeed = 10;
    private Vector3 _localOffset;
    public Sprite sprite;
    private SpriteRenderer SpriteRenderer;
    public TextMeshProUGUI texst;
    public LayerMask mask;
    public ParticleSystem ps;
    public ParticleSystem pointfier;
    private bool flag = false; 

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        target = GameObject.Find("playerCar");  
        _localOffset = transform.position - rotatiPoint.position;
    }
    void Start()
    {
        StartCoroutine(MyCoroutine()); // Вызов
    }

    
    void Destroy()
    {
        Destroy(gameObject);
    }

    void FixedUpdate()
    {

        // if (batR == null || target == null) return;
       
        if (batRS3.health >= 0)
        {
            // Вычисляем направление к цели от точки вращения
            Vector2 direction = target.transform.position - rotatiPoint.position;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

            // Плавное изменение угла
            float currentAngle = transform.rotation.eulerAngles.z;
            float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, rotationSpeed * Time.fixedDeltaTime);

            // Поворачиваем смещение и обновляем позицию
            Vector3 rotatedOffset = Quaternion.Euler(0, 0, newAngle) * _localOffset;
            transform.position = rotatiPoint.position + rotatedOffset;

            // Применяем поворот
            transform.rotation = Quaternion.Euler(0, 0, newAngle);
        }
        else if (flag == false)
        {
            flag = true;
            SpriteRenderer.sprite = sprite;
        }
       

    }
       
    IEnumerator MyCoroutine() // Объявление
    {
        while (batRS3 != null && batRS3.health >= 0)
        {
            if (target == null) yield break; // Если цель пропала, выходим

            Vector2 pau = (target.transform.position - dulo.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(dulo.position, pau, Mathf.Infinity, mask);

            Debug.DrawRay(dulo.position, pau * 100, Color.red);

            ps.Play();

            if (hit.collider != null)
            {
                float angler = Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg;
                pointfier.transform.rotation = Quaternion.Euler(0, 0, angler - 90);
                pointfier.transform.position = new Vector3(hit.point.x, hit.point.y, -5);
                pointfier.Play();

                if (hit.collider.name == "playerCar")
                {
                    CarDamage.Instance.TakeDamage(1f);
                }
            }

            yield return new WaitForSeconds(0.4f);
        }
    }
}