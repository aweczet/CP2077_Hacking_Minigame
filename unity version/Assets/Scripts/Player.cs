using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 5;
    public float turnSpeed = 10f;
    private GameObject messagePanel;
    private Item collectableItem = null;
    private TextWriter writer;
    
    

    // Start is called before the first frame update
    void Start()
    {
        messagePanel = GameObject.Find("UI/MessagePanel");
        if (messagePanel == null)
        {
            Debug.LogError("MessagePanel not found!");
        }
        messagePanel.gameObject.SetActive(false);

        writer = new TextWriter();
        if (writer == null)
        {
            Debug.LogError("TextWriter not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0);
        Vector3 moveDir = transform.forward * Input.GetAxis("Vertical") * speed;
        controller.Move(moveDir * Time.deltaTime - Vector3.up * .1f);
        if (Input.GetKeyDown(KeyCode.F) && collectableItem)
        {
            Debug.Log(collectableItem.itemName + " F");
            Destroy(collectableItem.gameObject);
            messagePanel.gameObject.SetActive(false);
            collectableItem = null;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            writer.DisplayStatus();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Collectable")
        {
            Item item = other.GetComponent<Item>();
            collectableItem = item;
            Debug.Log("Przed");
            Debug.Log(messagePanel);
            messagePanel.gameObject.SetActive(true);
            Debug.Log("Po");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Collectable")
        {
            messagePanel.gameObject.SetActive(false);
        }

    }
}

public enum statusType
{
    Damaged = -1,
    Unknown = 0,
    Normal = 1
}