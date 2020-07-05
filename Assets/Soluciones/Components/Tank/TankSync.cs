using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Soluciones
{
    public class TankSync : NetworkBehaviour
    {
        public float SmoothFactor = 10f;
        public float Threshold = 0.1f;

        private Vector3 m_LastPosition;
        private Quaternion m_LastBaseRotation;
        private Quaternion m_LastCanonRotation;
        private bool m_Reloading = false;
        private bool m_LastSyncReloading = false;

        private TankController m_Tank;

        private void Awake()
        {
            m_Tank = GetComponent<TankController>();
        }

        private void Update()
        {
            SyncTank();
        }

        private void SyncTank()
        {
            if (m_Tank.HasAuthority())
            {
                SendData();
            }
            else
            {
                SyncOthersTank();
            }
        }

        private void SyncOthersTank()
        {
            m_Tank.LerpPosition(m_LastPosition, SmoothFactor);
            m_Tank.LerpBaseRotation(m_LastBaseRotation, SmoothFactor);
            m_Tank.LerpCanonRotation(m_LastCanonRotation, SmoothFactor);
            SyncReloading();
        }

        private void SyncReloading()
        {
            if (m_Reloading && m_LastSyncReloading != m_Reloading)
            {
                m_Tank.LaunchShootAnim();
            }

            m_LastSyncReloading = m_Reloading;
        }

        private void SendData()
        {
            CmdSendTransformData(m_Tank.transform.position, m_Tank.BaseTransform.rotation, m_Tank.CanonTransform.rotation, m_Tank.IsReloading());
        }

        [Command]
        private void CmdSendTransformData(Vector3 position, Quaternion baseRotation, Quaternion canonRotation, bool reloading)
        {
            // I'm on the Server

            m_LastPosition = position;
            m_LastBaseRotation = baseRotation;
            m_LastCanonRotation = canonRotation;
            m_Reloading = reloading;

            RpcReceiveTransformData(m_LastPosition, m_LastBaseRotation, m_LastCanonRotation, m_Reloading);
        }

        [ClientRpc]
        private void RpcReceiveTransformData(Vector3 position, Quaternion baseRotation, Quaternion canonRotation, bool reloading)
        {
            // I'm on the Client

            if (!m_Tank.HasAuthority())
            {
                // We don't need to update or predict local player data
                m_LastPosition = position;
                m_LastBaseRotation = baseRotation;
                m_LastCanonRotation = canonRotation;
                m_Reloading = reloading;
            }
        }
    }
}