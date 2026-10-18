using UnityEngine;
using UnityEngine.AI;

public class BotController3 : MonoBehaviour
{
    public GameObject target; // Цель (игрок)
    public float moveForce = 3; // Сила движения
    public float rotationSpeed = 5; // Скорость поворота
    public float driftFactor = 0.95f; // Коэффициент дрифта
    public float maxSpeed = 8f; // Максимальная скорость
    public bool isstatic = false;
    public bool isagent = false;
    private Vector2 targetDirection;
    public float lookAheadDistance = 1.5f;
    // public Playercontroller player;

    private NavMeshAgent agent;
    public Rigidbody2D rb;
    private float late;
    public float targetAngle;
    public SpriteRenderer _rend;
    public Sprite _srite;
    public ParticleSystem system;

    public float health = 300;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.Find("playerCar");

        agent.updateUpAxis = false;
        agent.updateRotation = false;

        rb.drag = 0.5f;
        rb.angularDrag = 2f;

       health = 300;
    }

    void Update()
    {
        // Обновляем путь с задержкой
        if (Time.time - late > 0.2f && isagent == false)
        {
            agent.SetDestination(target.transform.position);
            late = Time.time;
        }
        // Получаем направление к следующей точке пути
        if (agent.hasPath && agent.path.corners.Length > 1)
        {
            // Берём точку на пути впереди (для плавности)
            Vector2 nextPoint = GetNextPathPoint(lookAheadDistance);
            targetDirection = (nextPoint - (Vector2)transform.position).normalized;
        }
        else
        {
            // Если пути нет, едем прямо к цели
            targetDirection = (target.transform.position - transform.position).normalized;
        }
    }

    void FixedUpdate()
    {
        ApplyDrift();
        ApplyRotation(ref targetDirection);
    }

    void ApplyRotation(ref Vector2 move)
    {
        targetAngle = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg - 90f;

        float currentAngle = rb.rotation;
        float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, rotationSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(newAngle);
    }

    void ApplyDrift()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rb.velocity, transform.right);
        rb.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    Vector2 GetNextPathPoint(float distance)
    {
        Vector2[] pathCorners = System.Array.ConvertAll(agent.path.corners, v => (Vector2)v);
        float remainingDistance = distance;
        Vector2 futurePoint = pathCorners[pathCorners.Length - 1]; // По умолчанию - последняя точка

        for (int i = 0; i < pathCorners.Length - 1; i++)
        {
            Vector2 segmentStart = pathCorners[i];
            Vector2 segmentEnd = pathCorners[i + 1];
            float segmentLength = Vector2.Distance(segmentStart, segmentEnd);

            if (remainingDistance <= segmentLength)
            {
                // Интерполируем точку внутри сегмента пути
                futurePoint = Vector2.Lerp(segmentStart, segmentEnd, remainingDistance / segmentLength);
                break;
            }
            remainingDistance -= segmentLength;
        }

        return futurePoint;
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            health -= 5;
            if (health <= 0 && isagent == false)
            {
                rotationSpeed = 0;
                isagent = true;
                _rend.sprite = _srite;
                agent.speed = 0;
                system.Play();
                Invoke("Des", 4);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        float damag = rb.velocity.magnitude * 100;
        health -= damag;
    }

    private void Des()
    {
        Destroy(gameObject);
    }
}