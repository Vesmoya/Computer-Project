using ARPG.Main;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using ARPG.Fighting;
using ARPG.Movement;
using System;
using ARPG.Properties;

namespace ARPG.CharacterControl
{
    public class ControlOfAI : MonoBehaviour
    {
        [SerializeField] float AggroRange = 10f;
        [SerializeField] float susTime = 5f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float wpError = 2f;
        [SerializeField] float wpWaitTime = 3f;
        [Range(0,1)] [SerializeField] float speedOnPatrol = 0.5f;

        Combatant combatant;
        GameObject player;
        HitPoints charHP;
        Vector3 defPos;
        CharacterMovement Mover;
        
        float timeSinceLastSeen = Mathf.Infinity;
        float currentWPWaitingTime = Mathf.Infinity;
        int currentWPNo = 0;
        private void Start()
        {
            InitializeData();
        }

        

        private void Update()
        {
            if (charHP.IsDead()) return;

            GameObject player = GameObject.FindWithTag("Player");
            if (inRangeToAttackToPlayer() && combatant.CanEngage(player))
            {
                timeSinceLastSeen = 0;
                EngageAction(player);
            }
            else if (timeSinceLastSeen<susTime)
            {
                SusAction();
            }
            else
            {
                PatrolAction();
            }
            timeSinceLastSeen += Time.deltaTime;
            currentWPWaitingTime += Time.deltaTime;
        }

        private void PatrolAction()
        {
            Vector3 nextPosition = defPos;
            if(patrolPath != null)
            {
                if (AtWP())
                {
                    currentWPWaitingTime = 0;
                    GetNextWP();
                }
                nextPosition = GetCurrentWP();
                if (currentWPWaitingTime > wpWaitTime)
                {
                    Mover.StartMoveAction(nextPosition,speedOnPatrol);
                }               
            }
            
        }

        private void InitializeData()
        {
            combatant = GetComponent<Combatant>();
            player = GameObject.FindWithTag("Player");
            charHP = GetComponent<HitPoints>();
            Mover = GetComponent<CharacterMovement>();
            defPos = transform.position;
        }

        private Vector3 GetCurrentWP()
        {
            return patrolPath.GetWaypoint(currentWPNo);
        }

        private void GetNextWP()
        {
            currentWPNo = patrolPath.ArranceAndGetNextWPNo(currentWPNo);
        }

        private bool AtWP()
        {
            float WPDistance = Vector3.Distance(transform.position, GetCurrentWP());
            return WPDistance < wpError;
        }

        private void SusAction()
        {
            GetComponent<OrganizerForActions>().CancelAct();
        }

        private bool inRangeToAttackToPlayer()
        {

            float PlayerDistance = Vector3.Distance(transform.position, player.transform.position);
            return PlayerDistance < AggroRange;
        }

        private void EngageAction(GameObject player)
        {
            combatant.Engage(player);
        }

        

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, AggroRange);
        }
    }
}
