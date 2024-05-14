using RPG.Combat;
using RPG.Movement;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float weaponRange = 1.5f;

        Fighter fighter;
        GameObject player;
        Health health;
        Mover mover;

        Vector3 guardLocation;
        [SerializeField] Quaternion guardRotation;
        private void Start()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");

            guardLocation = transform.position;
            guardRotation = transform.rotation;
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
            else GetBackToGuardLocation();

            Debug.Log(GetComponent<NavMeshAgent>().velocity.magnitude);
        }

        private void GetBackToGuardLocation()
        {
            mover.StartMovement(guardLocation);
        }


        private bool AttackPlayer()
        {
            if (InAttackRangeOfPlayer() || !fighter.CanAttack(player)) return false;

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

            GetComponent<Mover>().StartMovement(player.transform.position);
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
