using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IpInput : MonoBehaviour
{
    public Text text;
    public Fight fight;

    public void Update()
    {
        if (text.text != "")
        {
            fight.SetIpAddress(text.text);
        }
    }
}
