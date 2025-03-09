using ARPG.Main;
using UnityEngine;
using UnityEngine.AI;
using ARPG.Saving;
using Newtonsoft.Json.Linq;
using ARPG.Properties;

namespace ARPG.MoveActions
{
    public class CharacterMovement : MonoBehaviour, InterfaceCancel, IJsonSaveable
    {
        [SerializeField] float maxSpeed = 6f;
        
        NavMeshAgent navMesh;
        HitPoints HP;
        private void Awake()
        {
            getNavmMesh();
        }

        

        void Start()
        {

            HP = GetComponent<HitPoints>();
        }

        void Update()
        {
            navMesh.enabled = !HP.IsDead();
            MovementAnimation();

        }

        public void StartMoveAction(Vector3 dest, float speedRatio)
        {
            GetComponent<OrganizerForActions>().NewAct(this);
            MoveAction(dest, speedRatio);
        }

        private void getNavmMesh()
        {
            navMesh = GetComponent<NavMeshAgent>();
        }
        public void MoveAction(Vector3 dest, float speedRatio)
        {
            navMesh.destination = dest;
            navMesh.speed = maxSpeed * Mathf.Clamp01(speedRatio);
            navMesh.isStopped = false;
        }

        
        private void MovementAnimation()
        {
            Vector3 moveSpeed = navMesh.velocity;
            Vector3 localMoveSpeed = transform.InverseTransformDirection(moveSpeed);
            float finalSpeed = localMoveSpeed.z;
            GetComponent<Animator>().SetFloat("ForwardSpeed", finalSpeed);
        }

        public JToken CaptureAsJToken()
        {
            return transform.position.ToToken();
        }

        public void RestoreFromJToken(JToken state)
        {
            navMesh.enabled = false;
            transform.position = state.ToVector3();
            navMesh.enabled = true;
            GetComponent<OrganizerForActions>().CancelAct();
        }

        public void moveStop()
        {
            navMesh.isStopped = true;
        }
    }

}