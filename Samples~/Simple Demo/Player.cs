using UnityEngine;

namespace Arugula.SerializedEvents
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
            onHealthChanged.AddListener(LogHealthChange);
        }

        private void Start()
        {
            Health = maxHealth;
        }

        public void TakeDamage(float amount)
        {
            Health -= amount;
        }

        public void LogHealthChange(float oldVal, float newVal)
        {
            Debug.Log($"{gameObject.name} health changed from {oldVal} -> {newVal}.");
        }
    }
}
