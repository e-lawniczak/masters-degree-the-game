using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperFunctions : MonoBehaviour
{
    public static float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0f)
        {
            return 1f;
        }
        else if (dir < 0f)
        {
            return -1f;
        }
        else
        {
            return 0f;
        }
    }
    public class DashInfo
    {
        public float dashCd { get; set; }
        public float currentCd { get; set; }
        public bool canDash { get; set; }
    }
    public static int CalculateFinalScore(int totalPoints, float totalTime)
    {
        int score = totalPoints * 3000;
        score = (int)(score / totalTime);

        return score;
    }
}
