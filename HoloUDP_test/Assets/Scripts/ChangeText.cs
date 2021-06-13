using DataModelFromPhone;
using Microsoft.Geospatial;
using Microsoft.Maps.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour
{
    [SerializeField, Tooltip("文字列を反映するテキストフィールド")]
    private TextMeshPro TargetTextField;

    //public GameObject Map;
    [SerializeField]
    private MapRenderer mapRenderer;


    [SerializeField]
    private GameObject MapPinPrefab;

    [SerializeField]
    private MapPinLayer MapPinLayer;

    private GameObject currentLocationPin;
    private MapPin currentPin;
    private void Awake()
    {
        ////mapRenderer = GetComponent<MapRenderer>();
        //currentPin = new MapPin();
        //currentPin.Location = mapRenderer.Center;
        
        //this.MapPinLayer.ClusterMapPinPrefab
        //MapPinLayer.MapPins.Add(currentPin);
    }

    public void SetASCIIBytes(byte[] bytes)
    {
        string receivedMessage = Encoding.ASCII.GetString(bytes);

        var locationInfo = new GeolocationInfo();

        //JSONのシリアライズがうまく行かないので、正規表現でやる…
        string latitude = Regex.Match(receivedMessage, "(?<=\"Latitude\":).*(?=,)").Value;
        string longitude = Regex.Match(receivedMessage, "(?<=\"Longitude\":).*(?=,)").Value;
        string altitude = Regex.Match(receivedMessage, "(?<=\"Altitude\":).*(?=,)").Value;

        try
        {
            LatLon location = new LatLon(double.Parse(latitude), double.Parse(longitude));
            //Debug.Log($"Location Info: {locationInfo.Latitude} {locationInfo.Longitude} {locationInfo.Altitude} {Environment.NewLine}{receivedMessage}");
            Debug.Log($"Location Info: {location.LatitudeInDegrees} {location.LongitudeInDegrees} {altitude} {Environment.NewLine}{receivedMessage}");
            TargetTextField.text = $"Location Info: {location.LatitudeInDegrees} {location.LongitudeInDegrees} {altitude} {Environment.NewLine}{receivedMessage}";

            //これだとなめらかに移動しない。ぱっと切り替わる感じ。
            //mapRenderer.Center = center;

            //これだといいのかな
            var mapScene = new MapSceneOfLocationAndZoomLevel(location, mapRenderer.ZoomLevel);
            mapRenderer.SetMapScene(mapScene);
            //mapRenderer.WaitForLoad();
            //StartCoroutine(MoveToLocation(mapScene));
        }
        catch
        {

        }
    }

    private IEnumerator MoveToLocation(MapScene mapScene)
    {
        yield return mapRenderer.WaitForLoad();
        yield return mapRenderer.SetMapScene(mapScene);
        yield return mapRenderer.WaitForLoad();
    }

    #region テスト用
    [SerializeField]
    private MapRenderer _map = null;

    private static readonly List<MapScene> MapScenes =
    new List<MapScene>
    {
            // Space Needle -> zoom out to Seattle Center
            new MapSceneOfLocationAndZoomLevel(new LatLon(47.620365, -122.349305), 17.0f),
            // MOHAI/wooden boats
            new MapSceneOfLocationAndZoomLevel(new LatLon(47.626872, -122.336026), 17.0f),
            new MapSceneOfLocationAndZoomLevel(new LatLon(47.627584, -122.336609), 18.0f),
            new MapSceneOfLocationAndZoomLevel(new LatLon(47.628131, -122.336676), 19.0f),
            // Gas Works
            new MapSceneOfLocationAndZoomLevel(new LatLon(47.644556, -122.335012), 16.0f),
            new MapSceneOfLocationAndZoomLevel(new LatLon(47.645065, -122.334899), 18.0f),
            // St Marks
            new MapSceneOfLocationAndZoomLevel(new LatLon(47.631862, -122.321263), 16.0f),
            new MapSceneOfLocationAndZoomLevel(new LatLon(47.631862, -122.321263), 18.0f),
            // Volunteer Park
            new MapSceneOfLocationAndZoomLevel(new LatLon(47.630118, -122.314719), 16.75f),
            new MapSceneOfLocationAndZoomLevel(new LatLon(47.628994, -122.31457), 18.0f),
            // Cal Anderson Park
            new MapSceneOfLocationAndZoomLevel(new LatLon(47.618389, -122.319168), 17.0f),
            new MapSceneOfLocationAndZoomLevel(new LatLon(47.615401, -122.318979), 18.0f),
            // Columbia Center -> Downtown
            new MapSceneOfLocationAndZoomLevel(new LatLon(47.603035, -122.331509), 16.0f),
            // Stadiums
            new MapSceneOfLocationAndZoomLevel(new LatLon(47.595005, -122.331778), 17.0f),
            new MapSceneOfLocationAndZoomLevel(new LatLon(47.591358, -122.332373), 17.0f),
            // Waterfront
            new MapSceneOfLocationAndZoomLevel(new LatLon(47.606085, -122.342533), 18.5f),
            new MapSceneOfLocationAndZoomLevel(new LatLon(47.607588, -122.343334), 18.5f),
            // Pike Place Market
            new MapSceneOfLocationAndZoomLevel(new LatLon(47.61043, -122.343521), 19.5f),
            new MapSceneOfLocationAndZoomLevel(new LatLon(47.608699, -122.340571), 19.5f),
            // Zoom out
            new MapSceneOfLocationAndZoomLevel(new LatLon(47.608699, -122.340571), 15f),
    };
    private IEnumerator RunTour()
    {
        yield return _map.WaitForLoad();

        while (isActiveAndEnabled) // loop the tour as long as we are running.
        {
            foreach (var scene in MapScenes)
            {
                yield return _map.SetMapScene(scene);
                yield return _map.WaitForLoad();
                //yield return new WaitForSeconds(3.0f);
            }
        }
    }

    private IEnumerator RunTour2(MapScene mapScene)
    {
        yield return mapRenderer.SetMapScene(mapScene);
        yield return mapRenderer.WaitForLoad();
    }
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        //foreach (var scene in MapScenes)
        //    StartCoroutine(RunTour2(scene));
        //StartCoroutine(RunTour());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
