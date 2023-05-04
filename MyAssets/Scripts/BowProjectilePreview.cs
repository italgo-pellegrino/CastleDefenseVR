using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Bow))]

/**
 * This script implements the litter parable of the bow projectile preview
 */
public class BowProjectilePreview : MonoBehaviour
{
    public Material lineMaterial;
    public float distanceBetweenPoints = 0.3f;
    public int pointCount = 16;
    private Bow bow;
    private LineRenderer lineRenderer;
    public InputActionProperty pinch;
    public Color startColor;
    public Color endColor;
    // Start is called before the first frame update
    void Start()
    {
        bow = GetComponent<Bow>();
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;
        lineRenderer.material = lineMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if (bow.isPulling && bow.arrow != null)
        {
            CalculatePointsForLineRenderer();
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
    //Math that implement the litter parable
    private void CalculatePointsForLineRenderer()
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = pointCount;
        for(int i = 0; i < pointCount; i++)
        {
            float t = i * distanceBetweenPoints;
            Vector2 v = CalculateOnePointOfParabula(t);

            Vector3 forward = Quaternion.Euler(0, transform.eulerAngles.y, 0) * Vector3.forward;
            forward = forward.normalized;
            Vector3 v3 = bow.arrow.transform.position + new Vector3(v.x * forward.x, -v.y, v.x * forward.z);
            lineRenderer.SetPosition(i, v3);
        }
    }

    private Vector2 CalculateOnePointOfParabula(float t)
    {
        Vector2 v = new Vector2(0, 0);
        float angle = transform.eulerAngles.x % 360;
        v.x = bow.force * Mathf.Cos(angle * Mathf.Deg2Rad) * t;
        v.y = bow.force * Mathf.Sin(angle * Mathf.Deg2Rad) * t + 0.5f * -Physics.gravity.y * Mathf.Pow(t, 2);
        return v;
    }
}
