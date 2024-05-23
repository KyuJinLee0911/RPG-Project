using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class ExperienceDisplay : MonoBehaviour
    {
        [SerializeField] Experience experience;
        [SerializeField] BaseStats stats;
        [SerializeField] Slider expBar;
        [SerializeField] Text expText;
        [SerializeField] Text levelText;

        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
            stats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            CalculateCurrentExp();     

        }

        private void CalculateCurrentExp()
        {
            expBar.value = experience.exp - stats.maxExp;
            expBar.maxValue = experience.maxExp - stats.maxExp;
            expText.text = $"{Mathf.FloorToInt((experience.exp - stats.maxExp) / (experience.maxExp - stats.maxExp) * 100)}% ({experience.exp} / {experience.maxExp})";
            levelText.text = $"Lv.{stats.GetLevel()}";
        }
    }

}