using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Data.Common;

public class BD : MonoBehaviour
{
    //Singleton
    public static BD instance;
    private FirebaseFirestore db;
    private bool isConnected = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("Firebase está listo.");
                db = FirebaseFirestore.DefaultInstance;
                isConnected = true;
                EventsData eventData = new EventsData
                {
                    id_event = "1",
                    name_event = "Evento de prueba",
                    location_event = "Ubicación de prueba",
                    locgeo_event = new GeoPoint(37.7749, -122.4194),
                    created_event = new string[] { "Creador1", "Creador2" },
                    assist_event = new string[] { "Asistente1", "Asistente2" }
                };
                CreateEvent(eventData, OnCreate);
            }
            else
            {
                Debug.LogError("No se pudo resolver todas las dependencias de Firebase: " + dependencyStatus);
            }
        });
        ;
    }
    void OnCreate(bool status)
    {
        ReadEvent("Evento de prueba", OnRead);
    }
    void OnRead(EventsData data, bool status)
    {
        if (status)
        {
            Debug.Log("Evento leído correctamente: " + data.id_event);
        }
        else
        {
            Debug.LogError("Error al leer el evento.");
        }
    }

    public bool IsConnected() => isConnected;

    public FirebaseFirestore GetDatabase()
    {
        if (isConnected)
        {
            return db;
        }
        else
        {
            Debug.LogError("No se puede acceder a la base de datos porque no está conectado.");
            return null;
        }
    }

    //CRUD events
    public void CreateEvent(EventsData eventData, System.Action<bool> callback)
    {
        if (!isConnected)
        {
            Debug.LogError("No se puede crear el evento porque no está conectado a Firebase.");
            callback(false);
            return;
        }

        DocumentReference docRef = db.Collection("Eventos").Document(eventData.id_event);
        docRef.SetAsync(eventData).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                Debug.Log("Evento creado correctamente.");
                callback(true);
            }
            else
            {
                Debug.LogError("Error al crear el evento: " + task.Exception);
                callback(false);
            }
        });
    }
    public void ReadEvent(string id_event, System.Action<EventsData, bool> callback)
    {
        if (!isConnected)
        {
            Debug.LogError("No se puede leer el evento porque no está conectado a Firebase.");
            callback(default, false);
            return;
        }

        DocumentReference docRef = db.Collection("Eventos").Document(id_event);
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    EventsData eventData = snapshot.ConvertTo<EventsData>();
                    callback(eventData, true);
                }
                else
                {
                    Debug.LogWarning("El evento no existe.");
                    callback(default, false);
                }
            }
            else
            {
                Debug.LogError("Error al leer el evento: " + task.Exception);
                callback(default, false);
            }
        });
    }
    public void UpdateEvent(string id_event, EventsData eventData, System.Action<bool> callback)
    {
        if (!isConnected)
        {
            Debug.LogError("No se puede actualizar el evento porque no está conectado a Firebase.");
            callback(false);
            return;
        }

        DocumentReference docRef = db.Collection("Eventos").Document(id_event);
        docRef.SetAsync(eventData, SetOptions.MergeAll).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                Debug.Log("Evento actualizado correctamente.");
                callback(true);
            }
            else
            {
                Debug.LogError("Error al actualizar el evento: " + task.Exception);
                callback(false);
            }
        });
    }
    public void DeleteEvent(string id_event, System.Action<bool> callback)
    {
        if (!isConnected)
        {
            Debug.LogError("No se puede eliminar el evento porque no está conectado a Firebase.");
            callback(false);
            return;
        }

        DocumentReference docRef = db.Collection("Eventos").Document(id_event);
        docRef.DeleteAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                Debug.Log("Evento eliminado correctamente.");
                callback(true);
            }
            else
            {
                Debug.LogError("Error al eliminar el evento: " + task.Exception);
                callback(false);
            }
        });
    }
    //
}
