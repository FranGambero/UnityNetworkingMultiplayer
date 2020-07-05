using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Soluciones
{
    public class TankShootMultiplayer : NetworkBehaviour
    {
        public bool Reloading { get; private set; }

        private TankController m_Tank;

        private void Awake()
        {
            m_Tank = GetComponent<TankController>();    
        }

        public void Shoot()
        {
            if (!Reloading)
            {
                CmdShoot();
                m_Tank.LaunchShootAnim();
                Reloading = true;
            }
        }

        public void FinishReloading()
        {
            Reloading = false;
        }

        public void ResetState()
        {
            Reloading = false;
        }

        [Command]
        private void CmdShoot()
        {
            // I'm on the server
            ProjectileReflection projectile = ProjectileManager.Instance.SpawnProjectile();
            projectile.Init(m_Tank.ProjectileSpawn);

            NetworkServer.Spawn(projectile.gameObject);
        }
    }
}