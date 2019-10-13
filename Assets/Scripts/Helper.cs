using System;
using System.Collections.Generic;
using UnityEngine;

namespace Helper
{
    public static class GameHelper
    {
        public static string GetSaveDirectory()
        {
#if UNITY_STANDALONE_WIN
            string baseFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).Replace("\\", "/");
            baseFilePath += "/My Games/Project Bit/";
#else
        string baseFilePath = Application.persistentDataPath + "/";
#endif
            return baseFilePath;
        }
    }

    public static class CircuitHelper
    {
        public static float AngleBetweenVector3(Vector3 vec1, Vector3 vec2)
        {
            Vector3 diference = vec2 - vec1;
            float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
            return Vector3.Angle(Vector3.right, diference) * sign;
        }

        public static Vector3 GetPositionFromNode(string nodeId, int index)
        {
            int x = 0;
            int z = 0;

            string[] nodeValues = nodeId.Split('x');
            int a = int.Parse(nodeValues[0]);
            int b = int.Parse(nodeValues[1]);
            int c = int.Parse(nodeValues[2]);
            int d = int.Parse(nodeValues[3]);

            if (a == 0)
            {
                x = b + index + 1;                
                z = c + d;
            }
            else if (a == 1 || a == 2)
            {
                x = b + d;
                z = c + index + 1;
            }

            return new Vector3(x, 0, z);
        }

        public static void CreateColliderChain(GameObject obj, List<Vector3> wireNodes)
        {
            for (int i = 0; i < wireNodes.Count - 1; i++)
            {
                Vector3 pointA = wireNodes[i];
                Vector3 pointB = wireNodes[i + 1];

                Vector3 rotation = Quaternion.FromToRotation(pointA - pointB, Vector3.down).eulerAngles;
                rotation.x *= -1;
                rotation.z *= -1;

                float height = Vector3.Distance(pointA, pointB) + 0.35f;

                Vector3 point = (pointA + pointB) / 2.0f;

                GameObject collider = new GameObject();
                collider.transform.parent = obj.transform;

                CapsuleCollider capCol = collider.AddComponent<CapsuleCollider>();
                capCol.height = height;
                capCol.radius = 0.2f;

                collider.transform.position = point;
                collider.transform.rotation = Quaternion.Euler(rotation);

                collider.layer = LayerMask.NameToLayer("Component");

            }
        }

    }
}
