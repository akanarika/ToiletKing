using UnityEngine;
using System.Collections;

public static class Mode {

    public static class Normal
    {
        public static float DriftForce = 0.2f;
        public static float WaterDirectionChangeRate = 0.1f;
        public static float WaterDirectionChangeTime = 20f;
    }

    public static class Hard
    {
        public static float DriftForce = 0.6f;
        public static float WaterDirectionChangeRate = 0.5f;
        public static float WaterDirectionChangeTime = 6f;
    }

}
