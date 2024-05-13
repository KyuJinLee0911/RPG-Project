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

        float isClicked;

        void Start()
        {
            nmAgent = gameObject.GetComponent<NavMeshAgent>();
            animator = transform.GetChild(0).GetComponent<Animator>();
        }

        void Update()
        {
            UpdateAnimator();
            Debug.Log($"{nmAgent.isStopped}");
        }

        private void UpdateAnimator()
        {
            animator.SetFloat("Speed", nmAgent.velocity.magnitude / nmAgent.speed);
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
