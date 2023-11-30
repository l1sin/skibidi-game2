using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PauseManager
{
    public static bool Paused;
    public static void TogglePause(bool state)
    {
        Paused = state;
        if (Paused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
