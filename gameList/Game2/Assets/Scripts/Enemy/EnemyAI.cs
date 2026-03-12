using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public Animator anim;
    public Transform player;
    public EnemyHealthSprite health;

    [Header("Detection")]
    public float sightRange = 12f;
    public float attackRange = 2f;

    [Header("Patrol")]
    public Transform[] waypoints;
    private int currentWaypoint;

    private bool playerSpotted;
    private bool isAttacking;
    private bool isHit;

    void Update()
    {
        if (health.IsDead) return;

        if (!playerSpotted)
        {
            LookForPlayer();
            Patrol();
        }
        else
        {
            ChaseAndAttack();
        }
    }

    void LookForPlayer()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist <= sightRange)
        {
            playerSpotted = true;
        }
    }

    void Patrol()
    {
        if (waypoints.Length == 0) return;

        anim.SetBool("isRun", false);
        anim.SetBool("isLookAround", true);

        agent.destination = waypoints[currentWaypoint].position;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }

    void ChaseAndAttack()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        if (isHit) return;

        if (dist > attackRange)
        {
            anim.ResetTrigger("isAtk1");
            anim.ResetTrigger("isAtk2");

            anim.SetBool("isRun", true);
            anim.SetBool("isLookAround", false);

            agent.isStopped = false;
            agent.destination = player.position;
        }
        else
        {
            agent.isStopped = true;
            anim.SetBool("isRun", false);

            if (!isAttacking)
                StartCoroutine(Attack());
        }
    }

    System.Collections.IEnumerator Attack()
    {
        isAttacking = true;

        int atk = Random.Range(0, 2);
        if (atk == 0)
            anim.SetTrigger("isAtk1");
        else
            anim.SetTrigger("isAtk2");

        yield return new WaitForSeconds(1.2f); // match attack animation length
        isAttacking = false;
    }

    // Called by EnemyHealth when hit
    public void OnHit()
    {
        if (health.IsDead) return;

        StopAllCoroutines();
        isHit = true;
        agent.isStopped = true;

        anim.SetTrigger("isHit");

        Invoke(nameof(RecoverFromHit), 0.6f); // hit anim length
    }

    void RecoverFromHit()
    {
        isHit = false;
    }

    public void DealDamage()
    {
        if (player == null) return;

        PlayerHealth pH = player.GetComponent<PlayerHealth>();
        if (pH != null)
        {
            pH.TakeDamage(1);
        }
    }

}
