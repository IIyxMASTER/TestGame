using UnityEngine;

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

    }
}