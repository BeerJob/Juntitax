using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using Unity.VisualScripting;

public class MapUI : MonoBehaviour
{
    public GameObject map;
    public GameObject itemContainer;
    public GameObject pinPrefab;
    public GameObject itemPrefab;
    void Start()
    {
        ConstructElement(new Vector3(100, 150, 0), "Item 1", Color.red);
        ConstructElement(new Vector3(-200, -300, 0), "Item 2", Color.green);
        ConstructElement(new Vector3(-50, 200, 0), "Item 3", Color.blue);

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

    void ConstructElement(Vector3 position, string label, Color color)
    {
        GameObject item = Instantiate(itemPrefab, position, Quaternion.identity);
        item.transform.SetParent(itemContainer.transform, false);
        item.GetComponentInChildren<Text>().text = label;
        item.GetComponent<Image>().color = color;

        GameObject pin = Instantiate(pinPrefab, position, Quaternion.identity);
        pin.transform.SetParent(map.transform, false);
        pin.transform.localPosition = position;
        pin.GetComponent<Image>().color = color;
    }

    void ObtenerEventos()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        db.Collection("TestEventos").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error al obtener los documentos: " + task.Exception);
                return;
            }

            QuerySnapshot snapshot = task.Result;
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                Debug.Log($"Documento ID: {document.Id}");
                Dictionary<string, object> data = document.ToDictionary();
                float point;
                //Debug.Log("Position:" + float.TryParse(data["punto"], out point));

                //ConstructElement(new Vector3);
            }
        });
    }
}
