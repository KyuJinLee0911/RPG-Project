using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        Fighter fighter;
        [SerializeField] Health health;
        [SerializeField] Slider healthBar;
        [SerializeField] Text healthText;


        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            if (health == null)
                health = GameObject.FindWithTag("Player").GetComponent<Health>();

            healthBar = GetComponent<Slider>();
            healthText = GetComponentInChildren<Text>();
        }

        private void Update()
        {
            CalculateCurrentHp();     

        }

        private void CalculateCurrentHp()
        {
            healthBar.maxValue = health.maxHp;
            healthBar.value = health.hp;

            if (healthText != null)
                healthText.text = $"{health.hp} / {health.maxHp} ({Mathf.FloorToInt(health.hp / health.maxHp * 100f)}%)";
        }
    }

}