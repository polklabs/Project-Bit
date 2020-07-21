using System;
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

            if (IsNodeRotated(nodeId))
            {
                if (a == 0)
                {
                    z = c - index - 1;
                    x = b + d;
                }
                else if (a == 1 || a == 2)
                {
                    z = c - d;
                    x = b + index + 1;
                }
            }
            else
            {
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
            }

            return new Vector3(x, 0, z);
        }

        public static int GetIndexFromNode(string nodeId, Vector3 point)
        {
            int x = Mathf.RoundToInt(point.x);
            int z = Mathf.RoundToInt(point.z);

            string[] s = nodeId.Split('x');
            int index = 0;

            if (IsNodeRotated(nodeId))
            {
                if (s[0].Equals("1") || s[0].Equals("2"))
                {
                    index = x - int.Parse(s[1]) - 1;
                }
                else if (s[0].Equals("0"))
                {
                    index = -z + int.Parse(s[2]) - 1;
                }
            }
            else
            {
                if (s[0].Equals("1") || s[0].Equals("2"))
                {
                    index = z - int.Parse(s[2]) - 1;
                }
                else if (s[0].Equals("0"))
                {
                    index = x - int.Parse(s[1]) - 1;
                }
            }

            return index;
        }

        public static bool IsNodeRotated(string nodeId)
        {
            return nodeId[nodeId.Length-1] == '1';
        }

        public static bool breadboardsOverlap(float x1,  float z1, float x2, float z2)
        {
            Vector2 l1 = new Vector2(x1 - 0.5f, z1 - 0.5f);
            Vector2 r1 = new Vector2(x1 + 64.5f, z1 + 21.5f);

            Vector2 l2 = new Vector2(x2 - 0.5f, z2 - 0.5f);
            Vector2 r2 = new Vector2(x2 + 64.5f, z2 + 21.5f);

            if (l1.x >= r2.x || l2.x >= r1.x)
            {
                return false;
            }

            if (l1.y >= r2.y || l2.y >= r1.y)
            {
                return false;
            }

            return true;
        }

        public static void CreateColliderChain(GameObject obj, Vector3[] wireNodes)
        {
            for (int i = 1; i < wireNodes.Length; i++)
            {
                Vector3 pointA = wireNodes[i - 1];
                Vector3 pointB = wireNodes[i];

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
                collider.transform.tag = "Wire";

                collider.layer = LayerMask.NameToLayer("Component");

            }
        }

    }

    public static class WireHelper
    {
        public static int MaxDistance = 1415;
        public static int MinDistance = 0;
        public static int MaxHeight = 3;
        public static float MinHeight = 0.5f;

        public static int NumberOfWirePoints(Vector3 A, Vector3 B)
        {
            return Mathf.FloorToInt(Vector3.Distance(A, B) * 1.75f);
        }

        public static Vector3[] CurvedPoints(Vector3 A, Vector3 B)
        {
            int points = NumberOfWirePoints(A, B);            
            Vector3[] newPoints = new Vector3[points];
            if (points == 0)
            {
                return newPoints;
            }

            float k = (Mathf.Log10(Vector3.Distance(A, B) - MinDistance) / Mathf.Log10(MaxDistance - MinDistance)) * (MaxHeight - MinHeight);
            float a = (-k) / 1;

            float step = 2.0f / (points+1.0f);
            for(int i = 0; i < points; i++)
            {
                float x = -1.0f + (step * (i + 1));
                float y = (a * Mathf.Pow(x, 2)) + k;

                float xDist = Mathf.Abs(A.x - B.x);
                float xStep = (xDist / (points + 1.0f)) * Mathf.Sign(B.x - A.x);

                float zDist = Mathf.Abs(A.z - B.z);
                float zStep = (zDist / (points + 1.0f)) *Mathf.Sign(B.z - A.z);

                newPoints[i] = new Vector3(A.x + (xStep * (i+ 1)), y, A.z + (zStep * (i + 1)));
            }

            return newPoints;
        }

    }
}
