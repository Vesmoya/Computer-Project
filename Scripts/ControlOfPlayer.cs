using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARPG.Movement;
using ARPG.Fighting;
using ARPG.Properties;

namespace ARPG.CharacterControl
{
    public class ControlOfPlayer : MonoBehaviour
    {
        HitPoints playerHP;
        private void Awake()
        {
            playerHP = GetComponent<HitPoints>();
        }
        void Update()
        {
            if (playerHP.IsDead()) return;
            if( CombatInteraction()) return;
            if( MovementInteraction()) return;
        }

        private bool MovementInteraction()
        {
            RaycastHit hitInfo;
            bool hitDetected = Physics.Raycast(GetMouseRay(), out hitInfo);
            if (hitDetected)
            {
                if (Input.GetMouseButton(0))
                    moveToCursor(hitInfo);
                return true;
            }
            return false;
        }

        private void moveToCursor(RaycastHit hitInfo)
        {
            GetComponent<CharacterMovement>().StartMoveAction(hitInfo.point, 1f);
        }

        private bool CombatInteraction()
        {
            RaycastHit[] allHits = Physics.RaycastAll(GetMouseRay());
            foreach(RaycastHit hit in allHits)
            {
                TargetToFight potantialTarget = hit.transform.GetComponent<TargetToFight>();
                if (potantialTarget == null) continue;

                if (!GetComponent <Combatant>().CanEngage(potantialTarget.gameObject)) continue;

                if (Input.GetMouseButtonDown(0))
                {
                    EngageTarget(potantialTarget);
                }
                return true;
            }
            return false;
        }

        private void EngageTarget(TargetToFight potantialTarget)
        {
            GetComponent<Combatant>().Engage(potantialTarget.gameObject);
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}


