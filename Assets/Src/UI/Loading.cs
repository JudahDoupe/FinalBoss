using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Text IpAdress;
    public Text WaitingText;
    public Image Background;

    public void Update()
    {
        IpAdress.text = $"Local IP Address: {LocalIPAddress()}";
    }

    public void SetVisible(bool isVisible)
    {
        IpAdress.gameObject.SetActive(isVisible);
        WaitingText.gameObject.SetActive(isVisible);
        Background.gameObject.SetActive(isVisible);
    }

    private string LocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }
}
