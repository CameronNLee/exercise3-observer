using UnityEngine;

namespace Pikmini
{
    public class PikminiSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject MiniPrefab;
        [SerializeField] 
        private GameObject SpawnPoint;

        private void Update()
        {
            if (Input.GetButtonDown("Jump"))
            {
                Instantiate(this.MiniPrefab, this.SpawnPoint.transform.position, Quaternion.identity);
            }
        }

    }
}