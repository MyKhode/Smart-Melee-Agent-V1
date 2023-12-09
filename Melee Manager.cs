using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MeleeManager : MonoBehaviour
{
    NavMeshAgent m_Agent;
    IsometricCharacterRenderer isoRenderer;
    GameObject currentTarget;
    [SerializeField] private float StopDistance;
    [SerializeField] private float FindTargetDelay;


    void Awake()
    {
        isoRenderer = GetComponentInChildren<IsometricCharacterRenderer>();
        m_Agent = GetComponent<NavMeshAgent>();
        m_Agent.updateRotation = false;
        m_Agent.updateUpAxis = false;

        m_Agent.stoppingDistance = StopDistance;

        StartCoroutine(FindNewTargetCoroutine());
    }

    IEnumerator FindNewTargetCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(FindTargetDelay); // Adjust this delay as needed

            Debug.Log("Finding new target...");
            FindNewTarget();
        }
    }

    void FindNewTarget()
    {
        GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;

        if (potentialTargets != null && potentialTargets.Length > 0)
        {
            foreach (GameObject enemy in potentialTargets)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    currentTarget = enemy;
                }
            }

            Debug.Log("New target found: " + currentTarget.name);
            MoveToTarget();
        }
        else
        {
            Debug.Log("No targets found.");
        }
    }


    void MoveToTarget()
    {
        if (currentTarget != null)
        {
            m_Agent.SetDestination(currentTarget.transform.position);

            if (m_Agent.remainingDistance >= 0)
            {
                isoRenderer.SetDirection(m_Agent.velocity);
            }
            else
            {
                isoRenderer.SetDirection(Vector3.zero);
                Debug.Log("Reached target or distance less than threshold");
            }
        }
    }

}
