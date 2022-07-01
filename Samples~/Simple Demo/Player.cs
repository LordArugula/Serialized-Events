using UnityEngine;

namespace Arugula.SerializedEvents.Samples
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private float maxHealth = 100f;

        [SerializeField]
        private float currHealth;

        public float Health
        {
            get => currHealth;
            private set
            {
                onHealthChanged.Invoke(currHealth, value);
                currHealth = value;
            }
        }

        [SerializeField]
        private SerializedAction<float, float> onHealthChanged;

        private void Awake()
        {
            // Adding a listener at runtime
            onHealthChanged.AddListener(KillPlayer);
        }

        private void Start()
        {
            Health = maxHealth;
        }

        public void TakeDamage(float amount)
        {
            Health -= amount;
        }

        private void KillPlayer(float oldVal, float newVal)
        {
            if (newVal <= 0)
            {
                Debug.Log("Player died.");
                Destroy(gameObject);
            }
        }
    }
}
