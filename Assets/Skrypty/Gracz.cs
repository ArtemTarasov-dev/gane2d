using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gracz : Postacie
{
    public PoziomZdrowia PoziomZdrowiaPrefab;
    private PoziomZdrowia PoziomZdrowia;
    private Animator animator;

    public int maxHealthPoints = 100;
    public int healthPoints;
    public int initialHealthPoints = 100;

    public void Start()
    {
        healthPoints = initialHealthPoints;

        if (PoziomZdrowiaPrefab != null)
        {
            PoziomZdrowia = Instantiate(PoziomZdrowiaPrefab);
            PoziomZdrowia.selectedCharacter = this;
            PoziomZdrowia.maxHealthPoints = maxHealthPoints;
            PoziomZdrowia.UpdateHealthPoints(healthPoints);
        }
        else
        {
            Debug.LogError("PoziomZdrowiaPrefab is not assigned in the inspector.");
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component is not found on the GameObject.");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Triggered with " + collision.gameObject.name);

        if (collision.CompareTag("Przeciwnik"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                Debug.Log("Collided with enemy: " + enemy.name);
                TakeDamage(enemy.attackDamage);
            }
            else
            {
                Debug.LogWarning("Collided object does not have an EnemyController component.");
            }
        }
        else if (collision.CompareTag("ObiektyZbieralne"))
        {
            CollectibleItem item = collision.GetComponent<CollectibleItem>();
            if (item != null)
            {
                Debug.Log("Collected item: " + item.name);
                if (item.type == CollectibleType.Health)
                {
                    AddHealthPoints(item.amount);
                }
                Destroy(collision.gameObject);
            }
            else
            {
                Debug.LogWarning("Collided object does not have a CollectibleItem component.");
            }
        }
    }

    public bool AddHealthPoints(int pointsToAdd)
    {
        if (healthPoints < maxHealthPoints)
        {
            healthPoints = Mathf.Min(maxHealthPoints, healthPoints + pointsToAdd);
            if (PoziomZdrowia != null)
            {
                PoziomZdrowia.UpdateHealthPoints(healthPoints);
            }
            return true;
        }
        return false;
    }

    public void TakeDamage(int damage)
    {
        healthPoints -= damage;
        if (PoziomZdrowia != null)
        {
            PoziomZdrowia.UpdateHealthPoints(healthPoints);
        }
        if (healthPoints <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (animator != null)
        {
            Debug.Log("Triggering die animation");
            animator.SetTrigger("die"); // Запуск анимации смерти
            StartCoroutine(DeathCoroutine());
        }
        else
        {
            Debug.LogError("Animator is null, cannot trigger die animation.");
        }
    }

    private IEnumerator DeathCoroutine()
    {
        if (animator != null)
        {
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Перезапуск уровня
    }
}
