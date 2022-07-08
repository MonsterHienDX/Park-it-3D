using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonFunctions : MonoBehaviour
{
    public static List<int> CountSubString(string subString, string dataString)
    {
        List<int> positions = new List<int>();
        int pos = 0;
        while ((pos < dataString.Length) && (pos = dataString.IndexOf(subString, pos)) != -1)
        {
            positions.Add(pos);
            pos += subString.Length;
        }

        return positions;
    }

    public static void EnableButton(Button button, bool enable)
    {
        button.interactable = enable;
    }

    public static void PlayOneShotASound(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
}
