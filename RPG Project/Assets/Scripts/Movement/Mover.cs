using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Core;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField]
        Transform target;

        NavMeshAgent nmAgent;

        [SerializeField]
        Animator animator;
        Health health;

        float isClicked;

        void Start()
        {
            nmAgent = gameObject.GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
            animator = transform.GetComponent<Animator>();
        }

        void Update()
        {
            if(health.IsDead) nmAgent.enabled = false;
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            animator.SetFloat("Speed", nmAgent.velocity.magnitude);
        }

        public void Cancel()
        {
            nmAgent.isStopped = true;
        }

        public void StartMovement(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            // GetComponent<Fighter>().Cancel();
            MoveTo(destination);
        }

        public void MoveTo(Vector3 targetPosition)
        {
            nmAgent.destination = targetPosition;
            nmAgent.isStopped = false;
        }

        [System.Serializable]
        struct MoverSaveData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }
        public object CaptureState()
        {
            // using dictionary
            // Dictionary<string, object> data = new Dictionary<string, object>();
            // data["position"] = new SerializableVector3(transform.position);
            // data["rotation"] = new SerializableVector3(transform.rotation.eulerAngles);
            // return data;

            MoverSaveData data = new MoverSaveData();
            data.position = new SerializableVector3(transform.position);
            data.rotation = new SerializableVector3(transform.eulerAngles);
            return data;
        }

        public void RestoreState(object state)
        {
            // using dictionary
            // Dictionary<string, object> data = (Dictionary<string, object>)state;
            // SerializableVector3 position = (SerializableVector3)data["position"];
            // SerializableVector3 rotation = (SerializableVector3)data["rotation"];

            MoverSaveData data = (MoverSaveData)state;
            GetComponent<NavMeshAgent>().enabled = false;
            // transform.position = position.ToVector();
            // transform.eulerAngles = rotation.ToVector();
            transform.position = data.position.ToVector();
            transform.eulerAngles = data.rotation.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
