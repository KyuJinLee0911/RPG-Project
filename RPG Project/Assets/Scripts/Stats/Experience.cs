using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;
        public float exp { get => experiencePoints; }
        [SerializeField] float expToNextLevel = 0;
        public float maxExp { get => expToNextLevel; }

        public object CaptureState()
        {
            return experiencePoints;
        }

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
        }

        private void Start()
        {

        }

        private void Update()
        {
            expToNextLevel = GameObject.FindWithTag("Player").GetComponent<BaseStats>().GetStat(Stat.ExpToNextLevel);
        }

        public void RestoreState(object state)
        {
            float exp = (float)state;
            experiencePoints = exp;
        }
    }

}