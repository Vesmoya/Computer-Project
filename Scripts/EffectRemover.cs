using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARPG.Main
{
    public class EffectRemover : MonoBehaviour
    {
        [SerializeField] GameObject removeTarget = null;
        
        void Update()
        {
            if (!GetComponent<ParticleSystem>().IsAlive())
            {
                if(removeTarget != null)
                {
                    Destroy(removeTarget);
                }
                else
                {
                    Destroy(gameObject);
                }
                
            }
        }
    }
}
