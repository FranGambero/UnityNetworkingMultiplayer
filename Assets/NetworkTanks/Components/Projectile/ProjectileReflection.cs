using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkTanks
{
    public class ProjectileReflection : MonoBehaviour
    {
        #region VARIABLES

        public const string CLASS_TAG = "Projectile";
        public const float COLLIDER_OFFTIME_SECONDS = 0.5f;

        public float Speed = 10f;
        public float MaxReflections = 2;
        public float MaxSecondsLifeTime = 5;
        public float ReflectDetectionDistance = 0.5f;

        public LayerMask ReflectionMask;

        private int m_CurrentReflectionsCount = 0;
        private Collider m_Collider;

        #endregion

        private void Awake()
        {
            m_Collider = GetComponent<Collider>();
        }

        public void Init(Transform spawnPosition)
        {
            ResetCurrentReflections();
            SetPosition(spawnPosition);
            gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            ActivateTimers();
        }

        private void Update()
        {
            SimulateReflection();
        }

        #region MOVE

        private void SimulateReflection()
        {
            MoveForward();
            ReflectWhenCollision();
        }

        private void MoveForward()
        {
            transform.Translate(Vector3.forward * Time.deltaTime * Speed);
        }

        private void ReflectWhenCollision()
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, ReflectDetectionDistance, ReflectionMask))
            {
                Reflect(ray.direction, hit.normal);
            }
        }

        private void Reflect(Vector3 direction, Vector3 hitNormal)
        {
            transform.eulerAngles = CalculateReflectionAngle(direction, hitNormal);
            IncrementCurrentReflections();
        }

        private Vector3 CalculateReflectionAngle(Vector3 direction, Vector3 hitNormal)
        {
            // We calculate new direction
            Vector3 reflectionDirection = Vector3.Reflect(direction, hitNormal);

            // Based on new direction, we calculate new rotation
            float reflectionRotation = 90 - Mathf.Atan2(reflectionDirection.z, reflectionDirection.x) * Mathf.Rad2Deg;

            return (new Vector3(0, reflectionRotation, 0));
        }

        private void IncrementCurrentReflections()
        {
            m_CurrentReflectionsCount++;

            DestroyIfReflectionLimitReached();
        }

        private void ResetCurrentReflections()
        {
            m_CurrentReflectionsCount = 0;
        }

        private void SetPosition(Transform spawn)
        {
            transform.position = spawn.position;
            transform.rotation = spawn.rotation;
        }

        #endregion

        #region TRIGGERS

        private void OnTriggerEnter(Collider other)
        {
            HandleTrigger(other);
        }

        private void HandleTrigger(Collider other)
        {
            switch (other.tag)
            {
                case TankController.CLASS_TAG:
                    HandleTankTrigger(other);
                    break;

                case ProjectileReflection.CLASS_TAG:
                    HandleProjectileTrigger(other);
                    break;

                default:
                    break;
            }
        }

        private void HandleTankTrigger(Collider tankCollider)
        {
            TankController tank = tankCollider.gameObject.GetComponent<TankController>();
            tank.DestroyTank(true);

            DestroyProjectile();
        }

        private void HandleProjectileTrigger(Collider projectileCollider)
        {
            ProjectileReflection projectile = projectileCollider.gameObject.GetComponent<ProjectileReflection>();
            projectile.DestroyProjectile();

            DestroyProjectile();
        }

        #endregion

        #region TIMERS

        private void ActivateTimers()
        {
            ActivateColliderTimer();
            ActivateSelfDestroyTimer();
        }

        private void ActivateSelfDestroyTimer()
        {
            StartCoroutine(SelfDestroyTimer());
        }

        private void ActivateColliderTimer()
        {
            StartCoroutine(ColliderTimer());
        }

        private IEnumerator ColliderTimer()
        {
            m_Collider.enabled = false;
            yield return new WaitForSeconds(COLLIDER_OFFTIME_SECONDS);
            m_Collider.enabled = true;
        }

        private IEnumerator SelfDestroyTimer()
        {
            yield return new WaitForSeconds(MaxSecondsLifeTime);
            DestroyProjectile();
        }

        #endregion

        #region DESTROY

        private void DestroyIfReflectionLimitReached()
        {
            if (m_CurrentReflectionsCount >= MaxReflections)
            {
                // TODO: Fx
                DestroyProjectile();
            }
        }

        private void DestroyProjectile()
        {
            ProjectileManager.Instance.DestroyProjectile(this);
        }

        #endregion
    }
}