using UnityEngine;

namespace RPG.Core
{    
    public class Health : MonoBehaviour 
    {
        bool isDead = false;

        public bool IsDead { get => isDead; }
        [SerializeField] float health = 100f;

        public void TakeDamage(float damage)
        {    
            if(isDead) return;

            health = Mathf.Max(health - damage, 0);
            Debug.Log(health); 
            if(health <= 0)
            {
                Die();
                return;
            }
        }

        public void Die()
        {
            if(health > 0) return;
            if(isDead) return;

            health = 0;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            isDead = true;
        }
    }
}