using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{

    private CinemachineVirtualCamera currentVcam;
    private CinemachineBasicMultiChannelPerlin vCamPerlin;
    private bool canCountDown = false;
    private float count = 0f;
    float savedAmplitude;
    private GameObject currentShaker;
    private bool shaking = false;


    // Update is called once per frame
    void Update()
    {

       // Debug.Log("Active");
        if (canCountDown)
        {
            count -= Time.deltaTime;
            if (count <= 0f)
            {
                vCamPerlin.m_AmplitudeGain = 0;
                canCountDown = false;
            }
        }
    }

    public void ShakeCam(float intensity, float frequency, float duration, GameObject objectCallingShake)
    {
        if (currentVcam == null)
        {
            Debug.LogError("Cam manager doesnt have a current vCam");
            return;
        }

        // if camera is shaking already check if asking for stronger shake 
        else if (count > 0 && vCamPerlin.m_AmplitudeGain>0)
        {
            if (intensity > vCamPerlin.m_AmplitudeGain)
            {
                vCamPerlin.m_FrequencyGain = frequency;
                vCamPerlin.m_AmplitudeGain = intensity;
                count = duration;
                canCountDown = true;
                currentShaker = objectCallingShake;
            }
        }

        else
        {
            vCamPerlin.m_FrequencyGain = frequency;
            vCamPerlin.m_AmplitudeGain = intensity;
            count = duration;
            canCountDown = true;
            currentShaker = objectCallingShake;
        }

    }

    public void StopShake(GameObject objectAskingStop)
    {
        if (objectAskingStop == currentShaker)
        {
            savedAmplitude = vCamPerlin.m_AmplitudeGain;
            vCamPerlin.m_AmplitudeGain = 0;
        }
    }

    public void ContinueShake( GameObject objectAskingContinue)
    {

        if (objectAskingContinue == currentShaker && count > 0)
        {
            vCamPerlin.m_AmplitudeGain = savedAmplitude;
        }
    }

    public void SetCurrentCam(CinemachineVirtualCamera camToSet)
    {
        currentVcam = camToSet;
        vCamPerlin = currentVcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void DeSelectCurrentCam(CinemachineVirtualCamera camToDeselect)
    {
        if (currentVcam == camToDeselect)
        {
            vCamPerlin.m_AmplitudeGain = 0;
            currentVcam = null;
            vCamPerlin = null;
        }
    }
}
