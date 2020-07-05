using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Manages Pooling instantiation of Objects.
    /// </summary>
    /// <typeparam name="T">Type of the object.</typeparam>
    public class PoolingSystem<T> 
    {
        #region Variables

        /// <summary>
        /// This function is used to instantiate our objects in scene.
        /// </summary>
        public Func<T> CreateFunction;

        /// <summary>
        /// List which stores all of our objects ready to be used in scene.
        /// </summary>
        private List<T> m_Objects = new List<T>();

        #endregion

        #region Public Functions

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PoolingSystem()
        {

        }

        /// <summary>
        /// Constructor with function of instantiation.
        /// </summary>
        /// <param name="createFunction">Function that will instantiate our objects.</param>
        public PoolingSystem(Func<T> createFunction)
        {
            this.CreateFunction = createFunction;
        }

        public int GetSize()
        {
            return m_Objects.Count;
        }

        /// <summary>
        /// Fills our list of objects instantiating them in scene.
        /// </summary>
        /// <param name="size">Amount of objects to be created.</param>
        /// <returns>True if had success or false if an error happened.</returns>
        public bool PopulatePool(int size)
        {
            bool success = true;

            if (CreateFunction != null)
            {
                try
                {
                    for (int i = 0; i < size; i++)
                    {
                        T instance = CreateFunction();
                        PushObject(instance);
                    }

                }
                catch (Exception ex)
                {
                    Debug.LogError("[PoolingSystem.ERROR]: " + ex.Message);
                    success = false;
                }
            }
            else
            {
                Debug.LogError("[PoolingSystem.ERROR]: No create function defined");
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Picks an object from the pool.
        /// </summary>
        /// <returns>Object.</returns>
        public T PopObject()
        {
            T obj = default;

            if (m_Objects.Count > 0)
            {
                obj = m_Objects[0];
                m_Objects.Remove(obj);
            }

            return obj;
        }

        /// <summary>
        /// Stores an object in the pool.
        /// </summary>
        /// <param name="obj">Object to be stored.</param>
        public void PushObject(T obj)
        {
            if (!m_Objects.Contains(obj))
            {
                m_Objects.Add(obj);
            }
        }

        /// <summary>
        /// Removes all references from pool.
        /// </summary>
        public void ClearPoolReferences()
        {
            m_Objects.Clear();
        }

        #endregion
    }
}