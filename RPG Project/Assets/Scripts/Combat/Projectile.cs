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

        private void OnTriggerEnter(Collider other) 
        {
            if(other.GetComponent<Health>() != target) return;

            other.GetComponent<Health>().TakeDamage(projectileDamage);

            Destroy(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            MoveToTarget();
        }

        void MoveToTarget()
        {
            if(target == null) return;
            // transform.LookAt(GetAimLocation()); => 유도 기능을 넣고싶을때는 이 라인 주석 해제
            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
        }

        public void SetTarget(Health target, float damage)
        {
            this.target = target;
            projectileDamage = damage;
        }

        // 목표물의 복부를 바라보게 발사
        public Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if(targetCapsule == null) return target.transform.position;

            return target.transform.position + Vector3.up * targetCapsule.height * 0.5f;
        }
    }

}