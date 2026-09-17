using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    [SerializeField] private CinemachineImpulseSource impulseSource;
    [SerializeField] private float force;

    public void Shake()
    {
        impulseSource.GenerateImpulse(force);
    }
}