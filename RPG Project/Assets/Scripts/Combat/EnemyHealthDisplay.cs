using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        [SerializeField] Fighter fighter;
        [SerializeField] Health health;
        [SerializeField] Slider healthBar;
        [SerializeField] Text healthText;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            ShowAndHideHpBar();
            CalculateCurrentHp();
        }

        private void CalculateCurrentHp()
        {
            if (fighter.GetTarget() == null)
                return;

            if (health == null)
                health = fighter.GetTarget();

            healthBar.maxValue = health.maxHp;
            healthBar.value = health.hp;

            if (healthText != null)
                healthText.text = $"{health.hp} / {health.maxHp} ({Mathf.FloorToInt(health.hp / health.maxHp * 100f)}%)";


        }

        private void ShowAndHideHpBar()
        {
            if (fighter.GetTarget() == null)
            {
                health = null;
                healthBar.gameObject.SetActive(false);
                return;
            }
            else
            {
                healthBar.gameObject.SetActive(true);
                return;
            }
        }
    }

}