using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soluciones
{
    public class TankAnimatorFunctions : MonoBehaviour
    {
        private TankController m_Tank;

        private void Awake()
        {
            m_Tank = GetComponentInParent<TankController>();
        }

        public void FinishReloading()
        {
            m_Tank.FinishReloading();
        }
    }
}