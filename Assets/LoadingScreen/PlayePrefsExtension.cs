using UnityEngine;

public static class PlayerPrefsHelper
{
    // Method to set a boolean value in PlayerPrefs
    public static void SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Method to get a boolean value from PlayerPrefs
    public static bool GetBool(string key, bool defaultValue = false)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetInt(key) == 1;
        }
        else
        {
            return defaultValue;
        }
    }
}