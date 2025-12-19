using Cinemachine;
using Cinemachine.Editor;
using System.Collections;
using UnityEditor.UIElements;
using UnityEngine;

public class DynamicZoomCC : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;

    // Camera zoom parameters
    float defaultOrthoSize;
    [SerializeField] float zoomedOrthoSize;
    [SerializeField] float zoomDuration = 1;
    Coroutine zoomRoutine; // Status: Checking if the camera's currently zooming

    private void Awake()
    {
        defaultOrthoSize = Camera.main.orthographicSize; // Set default!
        print("Default ortho size is " + defaultOrthoSize);
    }

    // If touching the player, zoom out:
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartZoom(zoomedOrthoSize);
        }
    }
    // Then, reset zoom when player exits the collider:
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartZoom(defaultOrthoSize);
        }
    }

    // Manipulating Cinemachine camera zoom!
    void StartZoom(float targetOrthoSize)
    {
        if (zoomRoutine != null) // Interrupts current zoom if active
        {
            StopCoroutine(zoomRoutine);
            print("Interrupted active zoom routine.");
        }
        zoomRoutine = StartCoroutine(DynamicZoom(targetOrthoSize));
        print("Now zooming to " + targetOrthoSize.ToString("F1"));
    }
    IEnumerator DynamicZoom(float targetOrthoSize)
    {
        print("Target ortho size is " + targetOrthoSize);

        float startSize = virtualCamera.m_Lens.OrthographicSize; // Start value
        float timeElapsed = 0f; // Setting up zoom timer

        while (timeElapsed < zoomDuration) {
            timeElapsed += Time.deltaTime; // Running timer
            float t = Mathf.SmoothStep(0, 1, timeElapsed / zoomDuration); // Smoothlyyy increasing value for Lerp 't'

            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startSize, targetOrthoSize, t); // Manipulating camera lens!
            yield return null;
        }
        // virtualCamera.m_Lens.OrthographicSize = targetOrthoSize; // Ensuring exact value
    }
}
