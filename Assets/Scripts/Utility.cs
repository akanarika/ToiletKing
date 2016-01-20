using UnityEngine;
using System.Collections;

public static class Utility {

    public static Vector2 toVector2(this Vector3 vect)
    {
        return new Vector2(vect.x, vect.z);
    }

    public static Vector3 toVector3(this Vector2 vect)
    {
        return new Vector3(vect.x, 0, vect.y);
    }

    public static Vector2 angleToVector(float angle) {
        return new Vector2(Mathf.Sin(angle * Mathf.PI / 180 ), Mathf.Cos(angle * Mathf.PI / 180));
    }

    public class Pair<T, K>
    {
        public T first;
        public K second;

        public Pair(T a, K b)
        {
            first = a;
            second = b;
        }

        public Pair()
        {

        }

        public override string ToString()
        {
            return first.ToString() + ", " + second.ToString();
        }

    }
}
