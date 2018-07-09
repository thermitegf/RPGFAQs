using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pauser {

    private static bool _isPaused { get; set; }
	public static bool IsPaused
    {
        get { return _isPaused; }
    }

    public static void SetPause(bool value, Object pauseLock = null)
    {
        if (PauseLock != null && PauseLock != pauseLock)
        {
            return;
        }
        Time.timeScale = value ? 0f : 1f;
        _isPaused = value;
        PauseLock = value ? pauseLock : null;
    }

    private static Object PauseLock { get; set; }
}
