using UnityEngine;

namespace Arugula.SerializedEvents.Samples
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private Player player;

        private void Awake()
        {
            if (player == null)
            {
                player = FindObjectOfType<Player>();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)
                && player != null)
            {
                player.TakeDamage(10);
            }
        }
    }
}
