using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using UnityEngine.InputSystem;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] Transform rightHandTransform;
        [SerializeField] Transform leftHandTransform;
        [SerializeField] Weapon defaultWeapon = null;
        [SerializeField] string defaultWeaponName = "Fist";
        Weapon currentWeapon;
        public Weapon getCurrentWeapon { get => currentWeapon; }
        Weapon prevWeapon = null;
        Health target;
        float timeSinceLastAttack = Mathf.Infinity;

        private void Start()
        {
            if (currentWeapon == null)
            {
                Weapon weapon = Resources.Load<Weapon>(defaultWeaponName);
                EquipWeapon(weapon);
            }

        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;
            if (target.IsDead)
            {
                target = null;
                return;
            }

            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }

        }

        public void EquipWeapon(Weapon weapon)
        {
            // current weapon이 있으면 해당 무기를 장착 해제(후 이전 무기에 등록)
            if (currentWeapon != null)
                UnequipCurrentWeapon(currentWeapon);
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();

            Transform hand = FindHand(weapon);

            // 무기를 착용하고자 하는 손에 해당 무기가 있는지 확인 (자식 오브젝트의 이름과 weapon에 등록된 프리팹의 이름 비교)
            for (int i = 0; i < hand.childCount; i++)
            {
                // 착용하고자 하는 무기가 주먹이라면 반복문을 바로 빠져나옴 (무기가 있는지 확인 할 필요가 없음)
                if (weapon.prefab == null) break;

                //있다면 활성화
                if (hand.GetChild(i).name.Contains(weapon.prefab.name))
                {
                    hand.GetChild(i).gameObject.SetActive(true);
                    animator.runtimeAnimatorController = weapon.getAnimCtrl;
                    return;
                }
            }

            //없다면 무기를 새로 만듬
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        Transform FindHand(Weapon weapon)
        {
            return weapon.IsLeftHand ? leftHandTransform : rightHandTransform;
        }

        public void UnequipCurrentWeapon(Weapon _currentWeapon)
        {
            // 현재 끼고 있는 웨폰을 어느 손에 착용했는지에 따라 장착 해제 할 웨폰이 있는 손 트랜스폼 결정
            Transform hand = FindHand(_currentWeapon);

            for (int i = 0; i < hand.childCount; i++)
            {
                // 자식 오브젝트가 활성화 되어있으면
                if (hand.GetChild(i).gameObject.activeInHierarchy)
                {
                    // 해당 무기를 이전 무기로 하고 해당 오브젝트 비활성화
                    prevWeapon = _currentWeapon;
                    hand.GetChild(i).gameObject.SetActive(false);
                    currentWeapon = null;
                    return;
                }
            }
            // for문을 다 돌았는데도 활성화 된 오브젝트가 없음
            // = 이전 무기가 주먹
            // 이전 무기를 주먹으로 바꾸고 현재 무기를 null로 한다음 리턴
            prevWeapon = defaultWeapon;
            currentWeapon = null;
            return;
        }

        public Health GetTarget()
        {
            return target;
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack < currentWeapon.duration) return;
            if (gameObject.CompareTag("Player"))
            {
                if (isClicked == 0) return;
            }
            TriggerAttack();
            timeSinceLastAttack = 0f;

        }

        float isClicked = 0f;
        void OnMoveToPosition(InputValue input)
        {
            isClicked = input.Get<float>();
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            // This will trigger the Hit() event;
            GetComponent<Animator>().SetTrigger("attack");
        }

        // Animation Event
        void Hit()
        {
            if (target == null)
                return;

            if (currentWeapon.HasProjectile())
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject);
            else
                target.TakeDamage(gameObject, currentWeapon.damage);
        }

        // Animation Event -> Bow
        void Shoot()
        {
            Hit();
        }
        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.range;
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }
        public void Cancel()
        {
            target = null;
            StopAttack();
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().SetTrigger("stopAttack");
            GetComponent<Animator>().ResetTrigger("attack");
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;

            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead;

        }

        void OnSwapToPrevWeapon()
        {
            if (prevWeapon == null)
            {
                Debug.LogWarning("There is no previous weapon");
                return;
            }
            EquipWeapon(prevWeapon);
        }


        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string currentWeaponName = (string)state;
            Weapon _currentWeapon = Resources.Load<Weapon>(currentWeaponName);
            EquipWeapon(_currentWeapon);
        }
    }
}
