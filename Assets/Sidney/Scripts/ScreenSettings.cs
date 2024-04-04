using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSettings : MonoBehaviour
{

    public void Change()
    {
        Screen.fullScreen = !Screen.fullScreen;
        print("gone fullscreen");
    }
}

