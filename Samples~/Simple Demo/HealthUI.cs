using UnityEngine;
using UnityEngine.UI;

namespace Arugula.SerializedEvents
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField]
        private Text text;

        private void Start()
        {
            if (text == null)
            {
                text = GetComponent<Text>();
            }
        }

        public void OnHealthChanged(float oldVal, float newVal)
        {
            text.text = newVal.ToString();
        }
    }
}
