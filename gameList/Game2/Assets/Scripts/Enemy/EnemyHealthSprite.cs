using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthSprite : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 5;
    private int currentHealth;

    public bool IsDead { get; private set; }

    [Header("References")]
    public Slider healthSlider;

    private EnemyAI enemyAI;
    private Animator anim;

    void Awake()
    {
        currentHealth = maxHealth;

        enemyAI = GetComponent<EnemyAI>();
        anim = GetComponent<Animator>();

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        if (IsDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();

        enemyAI.OnHit();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }

    void Die()
    {
        IsDead = true;

        anim.SetTrigger("isDead");

        if (enemyAI != null && enemyAI.agent != null)
            enemyAI.agent.isStopped = true;

        
        KillCount.AddKill(1);

        if (healthSlider != null)
            healthSlider.gameObject.SetActive(false);

        Destroy(gameObject, 3f);
    }



}
