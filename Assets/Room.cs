using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class Room : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera vCam;
    private CameraManager camManager;
    
    
    // Start is called before the first frame update
    void Start()
    {
        camManager = FindObjectOfType<CameraManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ActivateRoom();
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DeActivateRoom();
        }
    }

    private void ActivateRoom()
    {
        vCam.enabled = true;

        if (camManager != null)
        {
            camManager.SetCurrentCam(vCam);
        }
        else
        {
            Debug.LogError("No cam manager in this scene");
        }
    }

    private void DeActivateRoom()
    {
        camManager.DeSelectCurrentCam(vCam);
        vCam.enabled = false;
    }





}
