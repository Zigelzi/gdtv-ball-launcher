using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.UI
{
    public class ScreenDim : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }
}
