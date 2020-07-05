using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NetworkTanks
{
    [RequireComponent(typeof(LineRenderer))]
    public class ProjectileReflectionLauncher : MonoBehaviour
    {
        public int MaxReflectionCount = 5;
        public float MaxStepDistance = 100;

        public bool DebugMode = true;

        private LineRenderer m_LineRenderer;
        private List<Vector3> m_ReflectionPositions = new List<Vector3>();

        private void Awake()
        {
            m_LineRenderer = GetComponent<LineRenderer>();
        }

        private void OnEnable()
        {
            StartCoroutine(LaserCoroutine());
        }

        private IEnumerator LaserCoroutine()
        {
            while (true)
            {
                DrawLaser();
                yield return new WaitForSeconds(0.01f);
            }
        }

        private void DrawLaser()
        {
            // Limpiamos los puntos calculados previamente
            CleanStoredReflections();

            // Calculamos los nuevos
            CalculateReflections(transform.position, transform.forward);

            // Dibujamos la nueva línea
            DrawLine();
        }

        private void CalculateReflections(Vector3 position, Vector3 direction)
        {
            // Pintamos la siguiente reflexión que se producirá
            CalculateNextReflection(position, direction, MaxReflectionCount);
        }

        private void CalculateNextReflection(Vector3 position, Vector3 direction, int reflectionsRemaining)
        {
            // Guardamos la nueva posición calculada en el paso anterior
            StoreReflection(position);

            if (reflectionsRemaining > 0)
            {
                // Calculamos los datos de la posición de reflexión
                ReflectionData reflectionData = CalculateReflectionPosition(position, direction);

                // Si la dirección reflejada es la misma, significa que no ha colisionado
                if (direction != reflectionData.Direction)
                {
                    // Calculamos la siguiente
                    CalculateNextReflection(reflectionData.Position, reflectionData.Direction, reflectionsRemaining - 1);
                }
                else
                {
                    // Guardamos la última posición calculada
                    StoreReflection(reflectionData.Position);
                }
            }
        }

        private ReflectionData CalculateReflectionPosition(Vector3 currentPosition, Vector3 currentDirection)
        {
            Ray ray = new Ray(currentPosition, currentDirection);
            RaycastHit hit;

            // Comprobamos si colisiona con algo
            if (Physics.Raycast(ray, out hit, MaxStepDistance))
            {
                // Si colisiona, calculamos la reflexión
                currentDirection = Vector3.Reflect(currentDirection, hit.normal);
                currentPosition = hit.point;
            }
            else
                // Si no colisiona, pintamos una línea recta en la misma dirección
                currentPosition += currentDirection * MaxStepDistance;

            return (new ReflectionData(currentPosition, currentDirection));
        }

        private void DrawLine()
        {
            m_LineRenderer.positionCount = m_ReflectionPositions.Count;

            m_LineRenderer.SetPositions(m_ReflectionPositions.ToArray());
        }

        private void CleanStoredReflections()
        {
            m_ReflectionPositions.Clear();
        }

        private void StoreReflection(Vector3 position)
        {
            // Almacenamos la posición si es diferente de la última guardada
            if (m_ReflectionPositions.Count == 0 || m_ReflectionPositions[m_ReflectionPositions.Count - 1] != position)
            {
                m_ReflectionPositions.Add(position);
            }
        }

        private class ReflectionData
        {
            public Vector3 Position;
            public Vector3 Direction;

            public ReflectionData(Vector3 pos, Vector3 dir)
            {
                this.Position = pos;
                this.Direction = dir;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            if (m_ReflectionPositions.Count > 0)
            {
                foreach(Vector3 pos in m_ReflectionPositions)
                {
                    Gizmos.DrawWireSphere(pos, 0.25f);
                }
            }
        }
    }
}