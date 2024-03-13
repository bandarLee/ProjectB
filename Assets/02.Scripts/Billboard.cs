using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Billboard : MonoBehaviour
{
    private CinemachineBrain cinemachineBrain;

    private void Start()
    {
        cinemachineBrain = FindObjectOfType<CinemachineBrain>();
    }
    void Update()
    {

        Transform camTransform = cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject.transform;

        var lookDirection = camTransform.position - transform.position;
        lookDirection.y = 0; 
        transform.rotation = Quaternion.LookRotation(lookDirection);

    }
}
