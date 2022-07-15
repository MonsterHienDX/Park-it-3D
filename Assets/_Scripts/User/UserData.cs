using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserData
{
    private const string Level_Number = "user_level_number";

    public static int LevelNumber
    {
        get => SDKPlayerPrefs.GetInt(Level_Number, 0);
        set => SDKPlayerPrefs.SetInt(Level_Number, value);
    }

}