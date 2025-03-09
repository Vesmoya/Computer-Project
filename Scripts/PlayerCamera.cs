using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARPG.Main
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] Transform playerPosCoordinates;

        void LateUpdate()
        {
            transform.position = playerPosCoordinates.position;
        }
    }
}


