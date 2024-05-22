using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using RPG.SceneManagement;
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
            
            GetComponent<SphereCollider>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
