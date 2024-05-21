using System;
using System.Runtime.InteropServices;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make new Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] bool isLeftHand;
        public bool IsLeftHand { get => isLeftHand; }

        [SerializeField] float weaponRange;
        public float range { get => weaponRange; }

        [SerializeField] float weaponDamage;
        public float damage { get => weaponDamage; }

        [SerializeField] float timeBetweenAttack;
        public float duration { get => timeBetweenAttack; }

        [SerializeField] AnimatorOverrideController weaponOverrideController;
        public AnimatorOverrideController getAnimCtrl { get => weaponOverrideController; }

        [SerializeField] GameObject weaponPrefab;
        public GameObject prefab { get => weaponPrefab; }

        [SerializeField] Projectile projectile = null;

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            Transform handTransform = FindHandTransform(rightHand, leftHand);

            if (weaponPrefab != null)
                Instantiate(weaponPrefab, handTransform);

            if (weaponOverrideController != null)
                animator.runtimeAnimatorController = weaponOverrideController;
        }

        private Transform FindHandTransform(Transform rightHand, Transform leftHand)
        {
            return isLeftHand ? leftHand : rightHand;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Transform hand = FindHandTransform(rightHand, leftHand);
            Projectile projectileInstance = Instantiate(projectile, hand.position, Quaternion.identity);
            
            // 투사체의 목표를 설정 후 발사할 때 목표를 바라보고 화살이 일직선으로 나가도록
            projectileInstance.SetTarget(target, weaponDamage);
            projectileInstance.transform.LookAt(projectileInstance.GetAimLocation());
        }
    }
}