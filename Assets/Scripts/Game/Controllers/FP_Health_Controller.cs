using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Controllers
{
    public class FP_Health_Controller
    {
        private int maxHealth = 100;
        private int currentHealth;
        
        public int MaxHealth => maxHealth;
        public int CurrentHealth => currentHealth;

        public FP_Health_Controller()
        {
            currentHealth = maxHealth;
        }

        public void Damage(int attackDamage)
        {
            currentHealth -= attackDamage;
            if (currentHealth <= 0)
            {
                SceneManager.LoadScene("main");
            }
        }
    }
}