using Microsoft.Maps.Unity;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PinchSlider))]
public class MapZoomSlider : MonoBehaviour
{
    public MapRenderer mapRenderer;

    private PinchSlider slider;
    private bool isInteracting;
    private float defaultZoom;

    public void Awake()
    {
        slider = GetComponent<PinchSlider>();
        defaultZoom = mapRenderer.ZoomLevel;
        ResetZoom();
    }

    public void OnZoomSliderUpdated(SliderEventData eventData)
    {
        Debug.Log("OnZoomSliderUpdated");
        if (isInteracting)
        {
            var t = eventData.NewValue;
            mapRenderer.ZoomLevel = Mathf.Lerp(mapRenderer.MinimumZoomLevel, mapRenderer.MaximumZoomLevel, t);
        }
    }

    public void OnSliderInteractionStart()
    {
        Debug.Log("OnSliderInteractionStart");
        isInteracting = true;
        mapRenderer.GetComponent<MapInteractionController>()?.OnInteractionStarted.Invoke();
    }

    public void OnSliderInteractionEnded()
    {
        Debug.Log("OnSliderInteractionEnded");
        isInteracting = false;
        mapRenderer.GetComponent<MapInteractionController>()?.OnInteractionEnded.Invoke();
    }

    public void ResetZoom()
    {
        Debug.Log("ResetZoom");
        slider.SliderValue = (defaultZoom - mapRenderer.MinimumZoomLevel) /
            (mapRenderer.MaximumZoomLevel - mapRenderer.MinimumZoomLevel);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
