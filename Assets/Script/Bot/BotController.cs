using UnityEngine;
using UnityEngine.AI;

public class BotController : MonoBehaviour
{
    [Header("References")]
    public GameObject target; // Цель (игрок)
    public SpriteRenderer _rend;
    public ParticleSystem system;

    [Header("Movement Settings")]
    public float moveForce = 3f; // Сила движения
    public float rotationSpeed = 5f; // Скорость поворота
    public float driftFactor = 0.95f; // Коэффициент дрифта
    public float maxSpeed = 8f; // Максимальная скорость
    public float lookAheadDistance = 1.5f;

    [Header("Behavior Settings")]
    public bool isstatic = false;
    public bool isagent = false;

    // Public variables for other scripts
    public NavMeshAgent agent;
    public Rigidbody2D rb;
    public float targetAngle;
    public Sprite _srite;

    // Private variables
    private Vector2 targetDirection;
    private float late;
    private const float PATH_UPDATE_INTERVAL = 0.2f;

    // Оптимизация: кэширование для уменьшения аллокаций
    private Vector2[] pathCornersCache = new Vector2[20];
    private int cachedCornerCount = 0;
    private float lastPathCheckTime = 0f;
    private const float PATH_CACHE_UPDATE_INTERVAL = 0.1f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody2D>();

        if (target == null)
        {
            target = GameObject.Find("playerCar");
        }

        if (agent != null)
        {
            agent.updateUpAxis = false;
            agent.updateRotation = false;
        }

        if (rb != null)
        {
            rb.drag = 0.5f;
            rb.angularDrag = 2f;
        }
    }

    void Update()
    {
        if (isstatic || target == null) return;

        // Обновляем путь с задержкой
        if (Time.time - late > PATH_UPDATE_INTERVAL && isagent == false && agent != null)
        {
            agent.SetDestination(target.transform.position);
            late = Time.time;
        }

        // Оптимизация: обновляем кэш пути реже
        if (Time.time - lastPathCheckTime > PATH_CACHE_UPDATE_INTERVAL)
        {
            UpdatePathCache();
            lastPathCheckTime = Time.time;
        }

        // Получаем направление к следующей точке пути
        CalculateTargetDirection();
    }

    void UpdatePathCache()
    {
        if (agent == null || !agent.hasPath)
        {
            cachedCornerCount = 0;
            return;
        }

        // Кэшируем углы пути для быстрого доступа
        cachedCornerCount = Mathf.Min(agent.path.corners.Length, pathCornersCache.Length);
        for (int i = 0; i < cachedCornerCount; i++)
        {
            pathCornersCache[i] = agent.path.corners[i];
        }
    }

    void CalculateTargetDirection()
    {
        if (agent == null || !agent.hasPath || cachedCornerCount <= 1)
        {
            // Прямое направление к цели
            targetDirection = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
            return;
        }

        // Используем кэшированные данные для вычисления точки
        Vector2 nextPoint = GetNextPathPointOptimized(lookAheadDistance);
        targetDirection = (nextPoint - (Vector2)transform.position).normalized;
    }

    Vector2 GetNextPathPointOptimized(float distance)
    {
        if (cachedCornerCount <= 1)
            return pathCornersCache[0];

        float remainingDistance = distance;
        Vector2 futurePoint = pathCornersCache[cachedCornerCount - 1];

        // Быстрый поиск точки с использованием кэша
        for (int i = 0; i < cachedCornerCount - 1; i++)
        {
            Vector2 segmentStart = pathCornersCache[i];
            Vector2 segmentEnd = pathCornersCache[i + 1];

            // Оптимизация: используем sqrMagnitude для быстрых проверок
            float segmentLength = Vector2.Distance(segmentStart, segmentEnd);

            if (segmentLength < 0.01f) continue; // Пропускаем очень короткие сегменты

            if (remainingDistance <= segmentLength)
            {
                // Интерполируем точку внутри сегмента пути
                float t = remainingDistance / segmentLength;
                futurePoint.x = segmentStart.x + (segmentEnd.x - segmentStart.x) * t;
                futurePoint.y = segmentStart.y + (segmentEnd.y - segmentStart.y) * t;
                return futurePoint;
            }
            remainingDistance -= segmentLength;
        }

        return futurePoint;
    }

    // Старый метод для обратной совместимости (если где-то используется)
    Vector2 GetNextPathPoint(float distance)
    {
        return GetNextPathPointOptimized(distance);
    }

    void FixedUpdate()
    {
        if (isstatic || target == null) return;

        ApplyDrift();
        ApplyRotation();
    }

    void ApplyRotation()
    {
        targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        float currentAngle = rb.rotation;
        float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, rotationSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(newAngle);
    }

    void ApplyDrift()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rb.velocity, transform.right);
        rb.velocity = forwardVelocity + rightVelocity * driftFactor;

        // Limit maximum speed
        if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    // Оптимизация: отключаем Gizmos в релизной сборке
#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (agent != null && agent.hasPath && cachedCornerCount > 0)
        {
            Gizmos.color = Color.red;
            Vector2 nextPoint = GetNextPathPointOptimized(lookAheadDistance);
            Gizmos.DrawWireSphere(nextPoint, 0.3f);
            Gizmos.DrawLine(transform.position, nextPoint);
        }
    }
#endif
}