using System;
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

        public event Action OnExperienceGained;

        private void Start()
        {
            OnExperienceGained += CheckExp;
            OnExperienceGained();
        }

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            OnExperienceGained();
        }

        
        private void CheckExp()
        {
            expToNextLevel = GameObject.FindWithTag("Player").GetComponent<BaseStats>().GetStat(Stat.ExpToNextLevel);
        }

        [System.Serializable]
        struct ExpData
        {
            public float exp;
            public float maxExp;
        }

        public object CaptureState()
        {
            ExpData data = new ExpData();
            data.exp = experiencePoints;
            data.maxExp = expToNextLevel;
            return data;
        }

        public void RestoreState(object state)
        {
            ExpData data = (ExpData)state;
            experiencePoints = data.exp;
            expToNextLevel = data.maxExp;
        }
    }

}