using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon pickupWeapon;
        private void OnTriggerEnter(Collider other)
        {
            if(!other.CompareTag("Player")) return;

            Fighter fighter = other.transform.GetComponent<Fighter>();

            fighter.EquipWeapon(pickupWeapon);

            Destroy(gameObject);
        }
    }
}
