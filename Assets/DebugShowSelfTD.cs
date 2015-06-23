using UnityEngine;
using System.Collections;

namespace zjjScript
{
    public class DebugShowSelfTD : MonoBehaviour
    {
        public float radius = 0.5f;
        public bool AlwaysOn = false;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        void OnDrawGizmos()
        {
            if (AlwaysOn)
            {
                Gizmos.DrawWireSphere(transform.position, radius);
            }
        }
    }
}

