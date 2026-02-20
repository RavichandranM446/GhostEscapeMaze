using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Detection Settings")]
    public float detectionRange = 8f;
    public float attackRange = 1.8f;
    public float losePlayerRange = 12f;

    private NavMeshAgent agent;
    private Animator animator;

    private Vector3 startPosition;

    private enum State { Idle, Chase, Attack, Return }
    private State currentState;

    private bool hasAttacked = false; // prevent multiple GameOver calls

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        startPosition = transform.position;

        currentState = State.Idle;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Idle:

                animator.SetBool("isWalking", false);
                animator.SetBool("isAttacking", false);

                if (distance <= detectionRange)
                {
                    currentState = State.Chase;
                }

                break;

            case State.Chase:

                agent.isStopped = false;
                agent.SetDestination(player.position);

                animator.SetBool("isWalking", true);
                animator.SetBool("isAttacking", false);

                if (distance <= attackRange)
                {
                    currentState = State.Attack;
                }
                else if (distance > losePlayerRange)
                {
                    currentState = State.Return;
                }

                break;

            case State.Attack:

                agent.isStopped = true;

                animator.SetBool("isWalking", false);
                animator.SetBool("isAttacking", true);

                if (distance > attackRange)
                {
                    currentState = State.Chase;
                    hasAttacked = false;
                }

                break;

            case State.Return:

                agent.isStopped = false;
                agent.SetDestination(startPosition);

                animator.SetBool("isWalking", true);
                animator.SetBool("isAttacking", false);

                float returnDistance = Vector3.Distance(transform.position, startPosition);

                if (returnDistance < 0.5f)
                {
                    currentState = State.Idle;
                }

                break;
        }
    }

    // THIS PART handles GameOver when zombie touches player
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasAttacked)
        {
            hasAttacked = true;

            agent.isStopped = true;

            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", true);

            // disable player movement
            PlayerMovement playerMove = other.GetComponent<PlayerMovement>();
            if (playerMove != null)
                playerMove.Die();


            Invoke(nameof(TriggerGameOver), 1.5f);
        }
    }

    void TriggerGameOver()
    {
        GameManager.instance.GameOver();
    }

}
