using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour {
    public static int idLanguage=0;
    public static float OSTVolume=0.5f, SFXVolume=1;
    public static Dictionary<string, float> volumeOSTs = new Dictionary<string, float> { { "OST_menu", 0.5f }, { "OST_level", 0.5f } };
}