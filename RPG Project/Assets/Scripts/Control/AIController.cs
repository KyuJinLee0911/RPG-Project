using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Movement;
using RPG.Core;
using UnityEngine;
using System;
using UnityEngine.AI;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [Header("Chase")]
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float chaseSpeed = 5f;
        [SerializeField] float weaponRange = 1.5f;

        [Header("Patrol")]
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float patrolSpeed = 2.5f;
        [SerializeField] float waypointTolerance = 1f;

        Fighter fighter;
        GameObject player;
        Health health;
        Mover mover;

        Vector3 guardLocation;

        float timeSinceLastSawPlayer = Mathf.Infinity;
        float suspicionTime = 5f;
        int currentWaypointIndex = 0;
        float timeSinceLastWaypoint = Mathf.Infinity;
        float visitTime = 4f;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");

            guardLocation = transform.position;
        }

        // called by unity
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

        private void Update()
        {
            if (health.IsDead) return;

            if (AttackPlayer()) return;
            if (ChasePlayer()) return;
            else if (timeSinceLastSawPlayer < suspicionTime) SuspicionBehaviour();
            else PatrolBehaviour();
            UpdateTimers();

        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceLastWaypoint += Time.deltaTime;
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
            transform.LookAt(player.transform.position);
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardLocation;

            if (patrolPath != null)
            {
                GetComponent<NavMeshAgent>().speed = patrolSpeed;
                if (AtWaypoint())
                {
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if (timeSinceLastWaypoint > visitTime)
                mover.StartMovement(nextPosition);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            timeSinceLastWaypoint = 0f;
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());

            return distanceToWaypoint < waypointTolerance;
        }

        private bool AttackPlayer()
        {
            if (InAttackRangeOfPlayer() || !fighter.CanAttack(player)) return false;

            timeSinceLastSawPlayer = 0f;
            fighter.Attack(player);
            return true;
        }

        private bool InAttackRangeOfPlayer()
        {
            return DistanceToPlayer() > weaponRange;
        }

        private bool ChasePlayer()
        {
            if (!InChaseRangeOfPlayer()) return false;

            GetComponent<NavMeshAgent>().speed = chaseSpeed;
            mover.StartMovement(player.transform.position);
            return true;
        }

        private bool InChaseRangeOfPlayer()
        {
            return DistanceToPlayer() <= chaseDistance;
        }

        private float DistanceToPlayer()
        {
            return Vector3.Distance(transform.position, player.transform.position);
        }
    }
}
