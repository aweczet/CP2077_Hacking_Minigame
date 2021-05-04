using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class TextWriter
{
    private GameObject statusUI;
    private statusType cameraStatus = statusType.Unknown;
    private statusType wheelsStatus = statusType.Unknown;
    private statusType batteryStatus = statusType.Unknown;
    private List<statusType> statusTypes = new List<statusType>();

    private MonoBehaviour _mb;
    private bool printingLetter = false;

    public TextWriter()
    {
        statusUI = GameObject.Find("UI/Status");
        statusUI.SetActive(false);
        statusTypes.Add(cameraStatus);
        statusTypes.Add(wheelsStatus);
        statusTypes.Add(batteryStatus);
        UpdateStatus(statusTypes);
        _mb = GameObject.FindObjectOfType<MonoBehaviour>();
        if (_mb == null)
        {
            Debug.LogError("MonoB not found!");
        }
    }

    public void UpdateStatus(List<statusType> newStatusTypes)
    {
        statusTypes.Clear();
        for (int i = 0; i < statusTypes.Count; i++)
        {
            statusTypes[i] = newStatusTypes[i];
            statusUI.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = statusTypes[i].ToString();
        }
    }

    public void DisplayStatus()
    {
        statusUI.SetActive(true);
        foreach (Transform child in statusUI.transform)
        {
            foreach (Transform textChildTransform in child.transform)
            {
                Text textChild = textChildTransform.GetComponent<Text>();
                String tmp = textChild.text;
                textChild.text = "";
                foreach (char c in tmp)
                {
                    DateTime startTime = DateTime.UtcNow;
                    while ((DateTime.UtcNow - startTime).Seconds < 2)
                    {
                        Debug.Log("Waiting XD");
                    }
                    textChild.text += c;
                }
            }
        }
    }
}