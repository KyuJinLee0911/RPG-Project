using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using UnityEngine.AI;
using UnityEditor;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        float isClicked;
        Fighter fighter;
        Health health;

        [Header("Dodge")]
        [SerializeField] float dodgeDistance = 3f;

        [SerializeField] bool isDodging = false;
        [SerializeField] float dodgeSpeed = 40f;
        Vector3 dodgeDest = Vector3.positiveInfinity;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }

        void Update()
        {

            if (health.IsDead) return;

            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
            // if (!CanInteract()) Debug.Log("Nothing to interact");

            if (isDodging) StopDodging();
            
        }

        private void StopDodging()
        {
            // isClicked = 0;
            GetComponent<NavMeshAgent>().destination = dodgeDest;

            bool hasArrived = Vector3.Distance(transform.position, GetComponent<NavMeshAgent>().destination) <= 0.01f ? true : false;

            if (hasArrived)
            {
                GetComponent<NavMeshAgent>().speed = 7;
                isDodging = false;
            }
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                GameObject targetGameObject = target.gameObject;

                if (!fighter.CanAttack(targetGameObject)) continue;

                if (isClicked == 1)
                {
                    fighter.Attack(targetGameObject);

                }
                return true;
            }

            return false;
        }

        // private bool CanInteract()
        // {
        //     RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

        //     if (hits.Length == 0) return false;
        //     else return true;
        // }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            if (Physics.Raycast(GetMouseRay(), out hit, 100f))
            {
                if (isClicked == 1)
                {
                    GetComponent<Mover>().StartMovement(hit.point);
                }
                if (isDodging) return false;
                return true;
            }
            return false;
        }

        void OnMoveToPosition(InputValue value)
        {
            isClicked = value.Get<float>();
            Debug.Log($"Moving, isClicked? {isClicked}");
        }

        void OnDash()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();

            NavMeshAgent nmAgent = GetComponent<NavMeshAgent>();

            RaycastHit hit;
            if (Physics.Raycast(GetMouseRay(), out hit, 100f))
            {
                if (isDodging) return;
                // if(!canDash) return;
                isDodging = true;
                
                transform.LookAt(hit.point);
                nmAgent.velocity = Vector3.zero;
                
                Vector3 directionVec = (hit.point - transform.position).normalized;
                dodgeDest = transform.position + directionVec * dodgeDistance;
               
                nmAgent.speed = dodgeSpeed;
                GetComponent<Mover>().StartMovement(dodgeDest);
            }
        }

        void OnDance()
        {
            GetComponent<Animator>().Play("Dance");
        }



        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}