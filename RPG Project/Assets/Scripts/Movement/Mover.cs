using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField]
        Transform target;

        NavMeshAgent nmAgent;

        [SerializeField]
        Animator animator;
        Health health;

        float isClicked;

        void Start()
        {
            nmAgent = gameObject.GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
            animator = transform.GetComponent<Animator>();
        }

        void Update()
        {
            if(health.IsDead) nmAgent.enabled = false;
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            animator.SetFloat("Speed", nmAgent.velocity.magnitude);
        }

        public void Cancel()
        {
            nmAgent.isStopped = true;
        }

        public void StartMovement(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            // GetComponent<Fighter>().Cancel();
            MoveTo(destination);
        }

        public void MoveTo(Vector3 targetPosition)
        {
            nmAgent.destination = targetPosition;
            nmAgent.isStopped = false;
        }

        

    }
}
