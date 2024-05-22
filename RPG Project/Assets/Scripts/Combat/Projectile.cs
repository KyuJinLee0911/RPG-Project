using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using Unity.VisualScripting;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        Health target;
        [SerializeField] float projectileSpeed = 2f;
        [SerializeField] float projectileDamage = 0f;
        [SerializeField] bool isHoaming = false;
        [SerializeField] bool isExplode = false;
        [SerializeField] GameObject explosionEffect;

        float maxLifeTime = 10f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            if (other.GetComponent<Health>() == null)
            {
                Destroy(gameObject);
                return;
            }
            if (target.IsDead) return;

            other.GetComponent<Health>().TakeDamage(projectileDamage);
            if (isExplode)
                Instantiate(explosionEffect, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            MoveToTarget();
        }

        void MoveToTarget()
        {
            if (target == null) return;
            if (isHoaming && !target.IsDead)
                transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
        }

        public void SetTarget(Health target, float damage)
        {
            this.target = target;
            projectileDamage = damage;

            // 추후 오브젝트 풀링 방식으로 바꿀것
            Destroy(gameObject, maxLifeTime);
        }

        // 목표물의 복부를 바라보게 발사
        public Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null) return target.transform.position;

            return target.transform.position + Vector3.up * targetCapsule.height * 0.5f;
        }
    }

}