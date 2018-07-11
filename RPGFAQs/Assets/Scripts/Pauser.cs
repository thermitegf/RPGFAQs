using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pauser {

    private static bool _isPaused { get; set; }
	public static bool IsPaused
    {
        get { return _isPaused; }
    }

    public static void Pause(Object pauseLock)
    {
        PauseLocks.Add(pauseLock);
        Time.timeScale = 0f;
        _isPaused = true;
        Debug.Log($"Paused. Pauselocks count: ${PauseLocks.Count}");
    }

    public static void Unpause(Object pauseLock)
    {
        PauseLocks.Remove(pauseLock);
        if(PauseLocks.Count == 0)
        {
            Time.timeScale = 1f;
            _isPaused = false;
        }
        Debug.Log($"Unpaused. Pauselocks count: ${PauseLocks.Count}");
    }

    public static List<object> PauseLocks { get; private set; } = new List<object>();
}
