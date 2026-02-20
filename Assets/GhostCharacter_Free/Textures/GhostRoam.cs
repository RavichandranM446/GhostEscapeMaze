using UnityEngine;
using UnityEngine.AI;

public class GhostRoam : MonoBehaviour
{
    public float roamRadius = 5f;
    public float roamDelay = 3f;

    private NavMeshAgent agent;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = roamDelay;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= roamDelay)
        {
            Vector3 newPos = RandomNavSphere(transform.position, roamRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layerMask)
    {
        Vector3 randDirection = Random.insideUnitSphere * distance;
        randDirection += origin;

        UnityEngine.AI.NavMeshHit navHit;

        UnityEngine.AI.NavMesh.SamplePosition(randDirection, out navHit, distance, layerMask);

        return navHit.position;
    }
}
