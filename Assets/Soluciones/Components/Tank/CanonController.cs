using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soluciones
{
    public class CanonController
    {
        public Transform Head;
        public Transform ProjectileSpawn;
        public float RotationSpeed = 1f;
        public bool Reloading { get; private set; }

        private Camera m_Camera;
        private TankController m_Tank;

        public CanonController(TankController tank, Transform canonTransform, Transform projectileSpawn, float rotationSpeed)
        {
            this.m_Tank = tank;
            this.m_Camera = Camera.main;
            this.Head = canonTransform;
            this.RotationSpeed = rotationSpeed;
            this.ProjectileSpawn = projectileSpawn;
        }

        public void RotateCanon(float axisValueX, float axisValueY)
        {
            // Rotación X-Z del input
            Vector3 rotation = new Vector3(axisValueX, 0f, axisValueY);

            // Rotamos el vector para que se ajuste a la rotación de la cámara
            rotation = Quaternion.Euler(0, m_Camera.transform.eulerAngles.y, 0) * rotation;

            if (rotation != Vector3.zero)
            {
                // Obtenemos la rotación final
                Quaternion quatR = Quaternion.LookRotation(rotation);

                // Interpolación para que la rotación se realice de forma suave
                Head.rotation = Quaternion.Lerp(Head.rotation, quatR, Time.deltaTime * RotationSpeed);
            }
        }

        public void Shoot()
        {
            if (!Reloading)
            {
                ProjectileReflection projectile = ProjectileManager.Instance.SpawnProjectile();
                projectile.Init(ProjectileSpawn);
                StartReloading();
            }
        }

        private void StartReloading()
        {
            Reloading = true;
            m_Tank.LaunchShootAnim();
        }

        public void FinishReloading()
        {
            Reloading = false;
        }

        public void ResetState()
        {
            Reloading = false;
            Head.rotation = Quaternion.Euler(Vector3.zero);
        }
    }
}