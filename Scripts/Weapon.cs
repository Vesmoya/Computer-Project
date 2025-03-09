using ARPG.Properties;
using Unity.VisualScripting;
using UnityEngine;

namespace ARPG.Fighting
{
    [CreateAssetMenu(fileName = "Silah", menuName = "Silahlar/Yeni silah yarat", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animOverControl = null;
        [SerializeField] GameObject prefab = null;
        [SerializeField] float rangeOfWeapon = 3f;
        [SerializeField] float damageOfWeapon = 7f;
        [SerializeField] bool isWeaponRightHanded = true;
        [SerializeField] Projectile projectile = null;
        const string wepName = "Silah";

        public void Create(Transform rHand,Transform lHand, Animator anim)
        {
            RemovePreviousWeapon(rHand, lHand);
            ArrangeHands(rHand, lHand);
            ArrangeAnimation(anim);
        }

        private void ArrangeAnimation(Animator anim)
        {
            var overrideController = anim.runtimeAnimatorController as AnimatorOverrideController;
            if (animOverControl != null)
            {
                anim.runtimeAnimatorController = animOverControl;
            }
            else if (overrideController != null)
            {
                anim.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }

        private void ArrangeHands(Transform rHand, Transform lHand)
        {
            if (prefab != null)
            {
                Transform handLocation = GetHands(rHand, lHand);

                GameObject weapon = Instantiate(prefab, handLocation);
                weapon.name = wepName;
            }
        }

        public float GetDamageOfWeapon()
        {
            return damageOfWeapon;
        }
        public float GetRangeOfWeapon()
        {
            return rangeOfWeapon;
        }

        private void RemovePreviousWeapon(Transform rHand, Transform lHand)
        {
            Transform previousWeapon = rHand.Find(wepName);
            if(previousWeapon == null)
            {
                previousWeapon = lHand.Find(wepName);
            }
            if (previousWeapon == null) return;
            previousWeapon.name = "Removing";
            Destroy(previousWeapon.gameObject);
        }
        private Transform GetHands(Transform rightHand, Transform leftHand)
        {
            Transform handsLocation;
            if (isWeaponRightHanded)
            {
                handsLocation = rightHand;
            }
            else handsLocation = leftHand;
            return handsLocation;
        }

        public bool weaponHasProjectile()
        {
            return projectile != null;
        }

        public void ThrowProjectile(Transform rHand, Transform lHand, HitPoints target,GameObject attacker,float damage)
        {
            Projectile projectileInstance = Instantiate(projectile, GetHands(rHand, lHand).position, Quaternion.identity);
            projectileInstance.AimTarget(target,attacker, damage);
        }
       
        
    }
}