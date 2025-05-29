using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections.Generic;

public class MapUI : MonoBehaviour
{
    public GameObject map;
    public GameObject itemContainer;
    public GameObject pinPrefab;
    public GameObject itemPrefab;
    private Color[] colors = new Color[]
    {
        Color.red, Color.green, Color.blue, Color.yellow, Color.cyan,
        new Color(1f, 0.5f, 0f), new Color(0.5f, 0f, 1f), new Color(0.5f, 1f, 0.5f)
    };
    private int currentColorIndex = 0;
    void Start()
    {
        ConstructElement(new Vector3(100, 150, 0), "Item 1");
        ConstructElement(new Vector3(-200, -300, 0), "Item 2");
        ConstructElement(new Vector3(-50, 200, 0), "Item 3");

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("Firebase está listo.");
                ObtenerEventos();
            }
            else
            {
                Debug.LogError("No se pudo resolver todas las dependencias de Firebase: " + dependencyStatus);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ConstructElement(Vector3 position, string label = "Evento sin nombre", string owner = "Dueño desconocido", string location = "Sin sala")
    {
        Color color = colors[currentColorIndex];
        currentColorIndex = (currentColorIndex + 1) % colors.Length;

        GameObject item = Instantiate(itemPrefab, position, Quaternion.identity);
        item.transform.SetParent(itemContainer.transform, false);
        item.transform.GetChild(0).GetComponent<Text>().text = label;
        item.transform.GetChild(1).GetComponent<Text>().text = location;
        item.transform.GetChild(2).GetComponent<Text>().text = owner;
        item.transform.GetChild(3).GetComponent<Image>().color = color;

        GameObject pin = Instantiate(pinPrefab, position, Quaternion.identity);
        pin.transform.SetParent(map.transform, false);
        pin.transform.localPosition = position;
        pin.GetComponent<Image>().color = color;
    }


    void ObtenerEventos()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        db.Collection("Eventos").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error al obtener los documentos: " + task.Exception);
                return;
            }

            QuerySnapshot snapshot = task.Result;
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                var data = document.ToDictionary();

                // Obtener nombre
                string nombre = data.ContainsKey("name_event") ? data["name_event"].ToString() : "Evento sin nombre";

                // Obtener punto (GeoPoint)
                Vector3 position = Vector3.zero;
                if (data.ContainsKey("locgeo_event"))
                {
                    GeoPoint geo = (GeoPoint)data["locgeo_event"];

                    position = GeoToMapPosition(geo.Latitude, geo.Longitude);
                }

                //Obtener dueño (string[0])
                string dueño = "Dueño: desconocido";
                if (data.ContainsKey("created_event"))
                {
                    List<object> createdEvent = data["created_event"] as List<object>;
                    if (createdEvent != null && createdEvent.Count > 0)
                    {
                        dueño = createdEvent[0].ToString();
                    }
                }

                //Obtener locación (string)
                string locacion = data.ContainsKey("location_event") ? data["location_event"].ToString() : "Sin sala";

                Debug.Log($"Evento: {nombre}, Dueño: {dueño}, Ubicación: {locacion}, Posición: {position}");
                ConstructElement(position, nombre, dueño, locacion);
            }
        });
    }
    Vector3 GeoToMapPosition(double lat, double lng)
    {
        // Define el rango de tu mapa en lat/lng
        double minLat = -90;
        double maxLat = 90;
        double minLng = -180;
        double maxLng = 180;

        float mapWidth = 1000f;
        float mapHeight = 1000f;

        // Normalizar lat/lng entre 0 y 1
        float xNorm = (float)((lng - minLng) / (maxLng - minLng));
        float yNorm = (float)((lat - minLat) / (maxLat - minLat));

        // Escalar a las dimensiones del mapa
        float x = xNorm * mapWidth - mapWidth / 2f;
        float y = yNorm * mapHeight - mapHeight / 2f;

        return new Vector3(x, y, 0);
    }

}
