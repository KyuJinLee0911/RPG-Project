using UnityEngine;
using UnityEngine.InputSystem;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        float isClicked;
        Fighter fighter;
        Health health;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            if(health.IsDead) return;

            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
            // if (!CanInteract()) Debug.Log("Nothing to interact");
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if(target == null) continue;

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
                return true;
            }
            return false;
        }

        void OnMoveToPosition(InputValue value)
        {
            isClicked = value.Get<float>();
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}