using UnityEngine;

namespace Pikmini
{
    public class PikminiSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject MiniPrefab;
        [SerializeField] 
        private GameObject SpawnPoint;

        private void Start()
        {
            
        }

        // todo: maybe use pubsub instead of update().
        private void Update()
        {
            if (Input.GetButtonDown("Jump"))
            {
                MiniPrefab = Instantiate(this.MiniPrefab, this.SpawnPoint.transform.position, Quaternion.identity);
            }
        }

    }
}