
using UnityEngine;

public class GameSpeedController : MonoBehaviour
{
    public void SetNormalSpeed()
    {
        Time.timeScale = 1f;
    }

    public void SetDoubleSpeed()
    {
        Time.timeScale = 2f;
    }
}
