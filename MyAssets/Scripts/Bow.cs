using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

/**
 * This Script implements the whole logic of the bow, you can change the force of the bow and the time you need to draw the bow
 */
public class Bow : MonoBehaviour
{
    public GameObject arrow;
    public float force = 20;
    public float maxForce = 30;
    public float maxForceTime = 0.75f;
 
    private AudioSource audioSource;
    public AudioClip sfxFire;
    public AudioClip sfxReload;
    public AudioClip sfxPull;

    public GameObject arrowPrefab;
    private Vector3 arrowSpawnPoint;
    public InputActionProperty pinch;

    private bool arrowLoaded = false;
    [HideInInspector]public bool isPulling = false;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        arrowSpawnPoint = arrow.transform.localPosition;
    }
    private void Update()
    {
        if(pinch.action.ReadValue<float>() > 0.9)
        {
            arrowLoaded = true;
            PullArrow();
        } 

        if (pinch.action.ReadValue<float>() < 0.05 && arrowLoaded == true)
        {
            ShootArrow();
            if (!IsInvoking("ReloadArrow"))
            {
                Invoke("ReloadArrow", 0.5f);
            }
            isPulling = false;
            force = 0;
        }
    }

    private void ShootArrow()
    {
        if(arrow != null)
        {
            Rigidbody arrow_rigidbody = arrow.GetComponent<Rigidbody>();
            arrow.transform.SetParent(null);
            arrow.GetComponent<TrailRenderer>().enabled = true;
            arrow_rigidbody.isKinematic = false;
            arrow_rigidbody.AddForce(transform.forward * force, ForceMode.VelocityChange);
            arrow = null;
            arrowLoaded = false;
           // audioSource.Stop();
            audioSource.PlayOneShot(sfxFire);
        }
        else
        {
            Debug.Log("No Arrow Loaded!");
        }   
    }

    private void ReloadArrow()
    {
        arrow = Instantiate(arrowPrefab, transform.TransformPoint(arrowSpawnPoint), transform.rotation, transform);
        audioSource.PlayOneShot(sfxReload);
    }

    private void PullArrow()
    {
        if(arrow != null)
        {
            if (!isPulling)
            {
                force = 0;
                audioSource.clip = sfxPull;
                audioSource.Play();
                isPulling = true;
            }
            if(force < maxForce)
            {
                force += maxForce * (Time.deltaTime / maxForceTime);
               
            }
            arrow.transform.position = transform.TransformPoint(arrowSpawnPoint) + (arrow.transform.forward * -0.35f * (force / maxForce));
        }
    }
}
