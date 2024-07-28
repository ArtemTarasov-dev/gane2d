using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform pointA; // Точка А патрулирования
    public Transform pointB; // Точка B патрулирования
    public float speed = 2f; // Скорость перемещения
    public float attackRange = 1.5f; // Дистанция атаки
    public float chaseSpeed = 3.5f; // Скорость погони за игроком
    public int attackDamage = 10; // Урон от атаки
    private Transform targetPoint; // Текущая цель патрулирования
    private Transform player; // Ссылка на игрока
    private Animator animator; // Ссылка на компонент Animator

    private void Start()
    {
        targetPoint = pointA; // Начинаем с точки A
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform; // Находим игрока по тегу
        }
        animator = GetComponent<Animator>(); // Получаем компонент Animator
    }

    private void Update()
    {
        Patrol();
        CheckForPlayer();
    }

    private void Patrol()
    {
        // Перемещение к текущей точке патрулирования
        Vector2 direction = (targetPoint.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // Обновляем анимацию на основе направления движения
        UpdateAnimation(direction);

        // Если достигли текущей точки, меняем цель на противоположную
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            targetPoint = targetPoint == pointA ? pointB : pointA;
        }
    }

    private void CheckForPlayer()
    {
        if (player == null) return; // Проверка на наличие игрока

        // Если игрок находится в зоне атаки
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            AttackPlayer();
        }
        // Если игрок находится рядом, но вне зоны атаки, начинаем погоню
        else if (Vector2.Distance(transform.position, player.position) < attackRange * 1.5f)
        {
            ChasePlayer();
        }
    }

    private void AttackPlayer()
    {
        // Логика атаки (например, уменьшение здоровья игрока)
        Debug.Log("Атакуем игрока!");
        Gracz playerScript = player.GetComponent<Gracz>();
        if (playerScript != null)
        {
            playerScript.TakeDamage(attackDamage);
        }
    }

    private void ChasePlayer()
    {
        // Перемещение к игроку с большей скоростью
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);

        // Обновляем анимацию на основе направления движения
        UpdateAnimation(direction);
    }

    private void UpdateAnimation(Vector2 direction)
    {
        if (animator != null)
        {
            animator.SetBool("isWalking", direction != Vector2.zero);
            if (direction.x > 0)
            {
                animator.SetTrigger("right");
            }
            else if (direction.x < 0)
            {
                animator.SetTrigger("left");
            }
            else if (direction.y > 0)
            {
                animator.SetTrigger("up");
            }
            else if (direction.y < 0)
            {
                animator.SetTrigger("down");
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Рисуем зоны патрулирования и атаки для удобства в редакторе
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange * 1.5f);
    }
}
