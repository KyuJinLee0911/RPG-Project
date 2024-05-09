using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class mover : MonoBehaviour
{
    [SerializeField]
    Transform target;

    NavMeshAgent nmAgent;

    [SerializeField]
    Animator animator;

    void Start()
    {
        nmAgent = gameObject.GetComponent<NavMeshAgent>();
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetFloat("Speed", nmAgent.velocity.magnitude / nmAgent.speed);
    }

    void OnMoveToPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 mousePosition;
        if(Physics.Raycast(ray, out hit, 100f))
        {
            mousePosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            nmAgent.destination = mousePosition;
        }
    }
}
