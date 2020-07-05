using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace NetworkTanks
{
    public class ProjectileManager : Singleton<ProjectileManager>
    {
        #region VARIABLES

        [Header("Pool Config")]

        [Tooltip("GameObject used as projectile.")]
        public ProjectileReflection ProjectilePrefab;

        [Tooltip("Max instances of prefab")]
        public int Size = 20;

        [Tooltip("Container where bullets will be placed.")]
        public Transform ProjectilesContainer;

        // This class manages activation-deactivation of projectiles
        private PoolingSystem<ProjectileReflection> m_PoolingProjectiles;

        private List<ProjectileReflection> m_FiredProjectiles = new List<ProjectileReflection>();

        #endregion

        private void Awake()
        {
            InitPool();
        }

        #region  POOL

        private void InitPool()
        {
            CreatePool();
            PopulatePool();
        }

        private void CreatePool()
        {
            m_PoolingProjectiles = new PoolingSystem<ProjectileReflection>(() =>
            {
                return CreateProjectile();
            });
        }

        private ProjectileReflection CreateProjectile()
        {
            ProjectileReflection projectile = Instantiate(ProjectilePrefab, ProjectilesContainer);
            projectile.gameObject.SetActive(false);

            return projectile;
        }

        private void PopulatePool()
        {
            if (!m_PoolingProjectiles.PopulatePool(Size))
            {
                Debug.LogError("[ProjectileManager.ERROR]: Error while creating pool.");
            }
        }

        public void ClearProjectiles()
        {
            foreach(ProjectileReflection projectile in m_FiredProjectiles)
            {
                DestroyProjectile(projectile);
            }
        }

        public void DestroyProjectile(ProjectileReflection projectile)
        {
            projectile.gameObject.SetActive(false);
            m_FiredProjectiles.Remove(projectile);
            m_PoolingProjectiles.PushObject(projectile);
        }

        public ProjectileReflection SpawnProjectile()
        {
            ProjectileReflection projectile = m_PoolingProjectiles.PopObject();

            if (projectile != null)
            {
                m_FiredProjectiles.Add(projectile);
            }

            return projectile;
        }

        #endregion
    }
}