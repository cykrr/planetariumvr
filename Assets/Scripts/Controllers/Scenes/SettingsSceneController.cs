using System.Net.Sockets;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Management;
using System.Collections;
using System.Linq;
using System.Net.NetworkInformation;
using System;

public class SettingsSceneController : MonoBehaviour
{
    public Button acceptButton;

    public Toggle toggleHost;
    public TMP_InputField inputFieldIP;

    void Start()
    {
        DisableVR();

        acceptButton.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt("isHost", Convert.ToInt32(toggleHost.isOn));
            PlayerPrefs.SetString("hostIp", inputFieldIP.text);
            PlayerPrefs.Save();

            SceneController.instance.PreviousScene();
            StartCoroutine(ReactivateXR());
        });

        toggleHost.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("isHost", 0));
        inputFieldIP.text = PlayerPrefs.GetString("hostIp", "");
    }

    public string GetLocalIP()
    {
        foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            // Solo interfaces activas
            if (ni.OperationalStatus != OperationalStatus.Up)
                continue;

            // Opcional: limitar a interfaces de tipo WiFi o Ethernet
            if (ni.NetworkInterfaceType != NetworkInterfaceType.Wireless80211 &&
                ni.NetworkInterfaceType != NetworkInterfaceType.Ethernet)
                continue;

            foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
            {
                if (ip.Address.AddressFamily == AddressFamily.InterNetwork &&
                    !IPAddress.IsLoopback(ip.Address))
                {
                    return ip.Address.ToString();
                }
            }
        }

        return "";
    }

    private void DisableVR()
    {
        XRGeneralSettings.Instance.Manager.StopSubsystems();
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
    }

    IEnumerator ReactivateXR()
    {
        XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
        XRGeneralSettings.Instance.Manager.StartSubsystems();
        yield return null;
    }


}
