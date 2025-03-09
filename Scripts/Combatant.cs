using UnityEngine;
using ARPG.Movement;
using ARPG.Main;
using Unity.VisualScripting;
using System;
using ARPG.Properties;
using ARPG.Saving;
using Newtonsoft.Json.Linq;
using ARPG.Values;
using System.Collections.Generic;

namespace ARPG.Fighting
{
    public class Combatant : MonoBehaviour, InterfaceCancel, IJsonSaveable,InterfaceModProv
    {

        HitPoints currentTarget;
        [SerializeField] float attackInterval = 1f;
        
        [SerializeField] Weapon firstWeapon = null;
        Weapon equippedWeapon = null;
        float timeSinceLastAttack=Mathf.Infinity;
        [SerializeField] Transform rightSlot = null;
        [SerializeField] Transform leftSlot = null;




        private void Start()
        {
            initializeWeapon();
        }

        private void initializeWeapon()
        {
            if (equippedWeapon == null)
            {
                EquipingWeapon(firstWeapon);
            }
        }



        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            
            if(currentTarget == null || currentTarget.IsDead())
            {
                return;
            }
            if (!IsTargetInRange())
            {
                MoveToTarget();
            }
            else
            {
                GetComponent<CharacterMovement>().moveStop();
                ExecuteAttack();
            }
        }

        private void MoveToTarget()
        {
            GetComponent<CharacterMovement>().MoveAction(currentTarget.transform.position, 1f);
        }
        void DamageOnHit()
        {
            if (currentTarget == null) return;
            float damage = GetComponent<CharValues>().GetStat(Stat.Damage);

            if (equippedWeapon.weaponHasProjectile())
            {
                equippedWeapon.ThrowProjectile(rightSlot, leftSlot, currentTarget, gameObject, damage);
            }
            else
            {
                currentTarget.ApplyDamage(gameObject,damage);
            }
        }
        private void OnShot()
        {
            DamageOnHit();
        }



        public HitPoints GetCurrentTarget()
        {
            return currentTarget;
        }
        private void ExecuteAttack()
        {
            transform.LookAt(currentTarget.transform);
            if (timeSinceLastAttack > attackInterval)
            {
                animateAttack();
                timeSinceLastAttack = 0;

            }
        }

        private void animateAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("Attack");
        }


        private bool IsTargetInRange()
        {
            return Vector3.Distance(transform.position, currentTarget.transform.position) < equippedWeapon.GetRangeOfWeapon();
        }

        public bool CanEngage(GameObject targetToEngage)
        {
            if (targetToEngage == null) { return false; }
            HitPoints targetHealth = targetToEngage.GetComponent<HitPoints>();
            return targetHealth != null && !targetHealth.IsDead();
        }
        public void Engage(GameObject combatTarget)
        {
            GetComponent<OrganizerForActions>().NewAct(this);
            currentTarget = combatTarget.GetComponent<HitPoints>();
        }
        public void moveStop()
        {
            endAnimateAttack();
            currentTarget = null;
            GetComponent<CharacterMovement>().moveStop();
        }

        private void endAnimateAttack()
        {
            GetComponent<Animator>().ResetTrigger("Attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        public IEnumerable<float> GetCurrentWeaponDamage(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return equippedWeapon.GetDamageOfWeapon();
            }
        }

        public void EquipingWeapon(Weapon weapon)
        {
            equippedWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            weapon.Create(rightSlot, leftSlot, animator);
        }

        public JToken CaptureAsJToken()
        {
            return equippedWeapon.name;
        }

        public void RestoreFromJToken(JToken state)
        {
            string wepName = (string)state;
            Weapon weapon = Resources.Load<Weapon>(wepName);
            EquipingWeapon(weapon);
        }

        
    }
   

}
