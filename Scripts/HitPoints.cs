using UnityEngine;
using ARPG.Saving;
using ARPG.Values;
using Newtonsoft.Json.Linq;
using ARPG.Main;

namespace ARPG.Properties
{
    public class HitPoints : MonoBehaviour, IJsonSaveable
    {
        [SerializeField] private float currentHp = -1f;
        [SerializeField] private bool isDead = false;
        [SerializeField] private GameObject healItem = null;
        [SerializeField] private GameObject weaponToDrop = null;
        [SerializeField] private bool shouldDropWeapon = false;
        [SerializeField] private bool shouldDropHeal = true;

        private void Start()
        {
            InitializeHealth();
            
        }

        private void InitializeHealth()
        {
            CharValues stats = GetComponent<CharValues>();
            stats.onLevelUp += FullyRestoreHealth;

            if (currentHp < 0)
            {
                currentHp = stats.GetStat(Stat.Health);
            }
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void ApplyDamage(GameObject attacker, float damageValue)
        {
            Debug.Log($"{gameObject.name} took damage: {damageValue}");
            currentHp = Mathf.Max(currentHp - damageValue, 0);

            if (currentHp == 0 && !isDead)
            {
                DeathAction();
                GrandExperienceToAttacker(attacker);
            }
        }

        public float GetCurrentHp()
        {
            return currentHp;
        }

        public float GetMaxHP()
        {
            return GetComponent<CharValues>().GetStat(Stat.Health);
        }

        private void DeathAction()
        {
            var animator = GetComponent<Animator>();
            animator.SetTrigger("Die");
            isDead = true;

            var scheduler = GetComponent<OrganizerForActions>();
            scheduler.CancelAct();

            var spawnPosition = transform.position + Vector3.up * 0.5f;

            if (shouldDropHeal)
            {
                Instantiate(healItem, spawnPosition, Quaternion.identity);
            }

            if (shouldDropWeapon)
            {
                Instantiate(weaponToDrop, spawnPosition, Quaternion.identity);
            }
        }

        private void GrandExperienceToAttacker(GameObject attacker)
        {
            var exp = attacker.GetComponent<Experience>();
            if (exp == null) return;

            var expPoint = GetComponent<CharValues>().GetStat(Stat.ExperienceReward);
            exp.AwardExp(expPoint);
        }

        public void FullyRestoreHealth()
        {
            currentHp = GetComponent<CharValues>().GetStat(Stat.Health);
        }

        public void HealOnPickUp(float healAmount)
        {
            currentHp = Mathf.Min(currentHp + healAmount, GetComponent<CharValues>().GetStat(Stat.Health));
        }

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(currentHp);
        }

        public void RestoreFromJToken(JToken state)
        {
            currentHp = state.ToObject<float>();

            if (currentHp > 0)
            {
                isDead = false;
                GetComponent<Animator>().Rebind();
            }

            if (currentHp == 0 && !isDead)
            {
                DeathAction();
            }
        }
    }
}
