using UnityEngine;
using System.Collections.Generic;

public class Creature4 : MonoBehaviour
{
    public float speed = 2f;
    [SerializeField]private float neighborRadius;
    [SerializeField]private float separationDistance ;

    private Transform target;
    private List<Creature4> flockMates;

    public void Init(Transform target, List<Creature4> flock)
    {
        this.target = target;
        this.flockMates = flock;
    }

    void FixedUpdate()
    {
        if (!PlayerBehaviorLv2.PlayerTracked||(target.transform.position.y>11.5f) || target == null) return;

        Vector3 moveDir = (target.position - transform.position).normalized;

        Vector3 separation = Vector3.zero;
        int count = 0;

        foreach (var mate in flockMates)
        {
            if (mate != this)
            {
                float dist = Vector3.Distance(transform.position, mate.transform.position);
                if (dist < separationDistance)
                {
                    separation += (transform.position - mate.transform.position).normalized / dist;
                    count++;
                }
            }
        }

        if (count > 0)
            separation /= count;

        Vector3 totalDir = (moveDir + separation).normalized;
        transform.position += totalDir * speed * Time.deltaTime;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerBehavior.TriggerPlayerDied();
        }
    }
}