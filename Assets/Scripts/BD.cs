using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Data.Common;
using System;

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
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    Debug.Log("Firebase está listo.");
                    db = FirebaseFirestore.DefaultInstance;
                    isConnected = true;
                }
                else
                {
                    Debug.LogError("No se pudo resolver todas las dependencias de Firebase: " + dependencyStatus);
                }
            });
        }
        else
        {
            Destroy(gameObject);
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
    //CRUD users
    public void CreateUser(UsersData userData, System.Action<bool> callback)
    {
        if (!isConnected)
        {
            Debug.LogError("No se puede crear el usuario porque no está conectado a Firebase.");
            callback(false);
            return;
        }

        DocumentReference docRef = db.Collection("Usuarios").Document(userData.id_user);
        docRef.SetAsync(userData).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                Debug.Log("Usuario creado correctamente.");
                callback(true);
            }
            else
            {
                Debug.LogError("Error al crear el usuario: " + task.Exception);
                callback(false);
            }
        });
    }
    public void ReadUser(string id_user, System.Action<UsersData, bool> callback)
    {
        if (!isConnected)
        {
            Debug.LogError("No se puede leer el usuario porque no está conectado a Firebase.");
            callback(default, false);
            return;
        }

        DocumentReference docRef = db.Collection("Usuarios").Document(id_user);
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    UsersData userData = snapshot.ConvertTo<UsersData>();
                    callback(userData, true);
                }
                else
                {
                    Debug.LogWarning("El usuario no existe.");
                    callback(default, false);
                }
            }
            else
            {
                Debug.LogError("Error al leer el usuario: " + task.Exception);
                callback(default, false);
            }
        });
    }
    public void UpdateUser(string id_user, UsersData userData, System.Action<bool> callback)
    {
        if (!isConnected)
        {
            Debug.LogError("No se puede actualizar el usuario porque no está conectado a Firebase.");
            callback(false);
            return;
        }

        DocumentReference docRef = db.Collection("Usuarios").Document(id_user);
        docRef.SetAsync(userData, SetOptions.MergeAll).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                Debug.Log("Usuario actualizado correctamente.");
                callback(true);
            }
            else
            {
                Debug.LogError("Error al actualizar el usuario: " + task.Exception);
                callback(false);
            }
        });
    }
    public void DeleteUser(string id_user, System.Action<bool> callback)
    {
        if (!isConnected)
        {
            Debug.LogError("No se puede eliminar el usuario porque no está conectado a Firebase.");
            callback(false);
            return;
        }

        DocumentReference docRef = db.Collection("Usuarios").Document(id_user);
        docRef.DeleteAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                Debug.Log("Usuario eliminado correctamente.");
                callback(true);
            }
            else
            {
                Debug.LogError("Error al eliminar el usuario: " + task.Exception);
                callback(false);
            }
        });
    }
    //
    //CRUD participations
    public void CreateParticipation(ParticipantionsData participationData, System.Action<bool> callback)
    {
        if (!isConnected)
        {
            Debug.LogError("No se puede crear la participación porque no está conectado a Firebase.");
            callback(false);
            return;
        }

        DocumentReference docRef = db.Collection("Participaciones").Document(participationData.id_participation);
        docRef.SetAsync(participationData).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                Debug.Log("Participación creada correctamente.");
                callback(true);
            }
            else
            {
                Debug.LogError("Error al crear la participación: " + task.Exception);
                callback(false);
            }
        });
    }
    public void ReadParticipation(string id_participation, System.Action<ParticipantionsData, bool> callback)
    {
        if (!isConnected)
        {
            Debug.LogError("No se puede leer la participación porque no está conectado a Firebase.");
            callback(default, false);
            return;
        }

        DocumentReference docRef = db.Collection("Participaciones").Document(id_participation);
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    ParticipantionsData participationData = snapshot.ConvertTo<ParticipantionsData>();
                    callback(participationData, true);
                }
                else
                {
                    Debug.LogWarning("La participación no existe.");
                    callback(default, false);
                }
            }
            else
            {
                Debug.LogError("Error al leer la participación: " + task.Exception);
                callback(default, false);
            }
        });
    }
    public void UpdateParticipation(string id_participation, ParticipantionsData participationData, System.Action<bool> callback)
    {
        if (!isConnected)
        {
            Debug.LogError("No se puede actualizar la participación porque no está conectado a Firebase.");
            callback(false);
            return;
        }

        DocumentReference docRef = db.Collection("Participaciones").Document(id_participation);
        docRef.SetAsync(participationData, SetOptions.MergeAll).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                Debug.Log("Participación actualizada correctamente.");
                callback(true);
            }
            else
            {
                Debug.LogError("Error al actualizar la participación: " + task.Exception);
                callback(false);
            }
        });
    }
    public void DeleteParticipation(string id_participation, System.Action<bool> callback)
    {
        if (!isConnected)
        {
            Debug.LogError("No se puede eliminar la participación porque no está conectado a Firebase.");
            callback(false);
            return;
        }

        DocumentReference docRef = db.Collection("Participaciones").Document(id_participation);
        docRef.DeleteAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                Debug.Log("Participación eliminada correctamente.");
                callback(true);
            }
            else
            {
                Debug.LogError("Error al eliminar la participación: " + task.Exception);
                callback(false);
            }
        });
    }
    //Other utilities methods
    public void GetAllEvents(System.Action<List<EventsData>, bool> callback)
    {
        if (!isConnected)
        {
            Debug.LogError("No se puede obtener los eventos porque no está conectado a Firebase.");
            callback(null, false);
            return;
        }

        db.Collection("Eventos").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                List<EventsData> eventsList = new List<EventsData>();
                foreach (DocumentSnapshot document in task.Result.Documents)
                {
                    EventsData eventData = document.ConvertTo<EventsData>();
                    eventsList.Add(eventData);
                }
                callback(eventsList, true);
            }
            else
            {
                Debug.LogError("Error al obtener los eventos: " + task.Exception);
                callback(null, false);
            }
        });
    }
    public void GetAllUsers(System.Action<List<UsersData>, bool> callback)
    {
        if (!isConnected)
        {
            Debug.LogError("No se puede obtener los usuarios porque no está conectado a Firebase.");
            callback(null, false);
            return;
        }

        db.Collection("Usuarios").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                List<UsersData> usersList = new List<UsersData>();
                foreach (DocumentSnapshot document in task.Result.Documents)
                {
                    UsersData userData = document.ConvertTo<UsersData>();
                    usersList.Add(userData);
                }
                callback(usersList, true);
            }
            else
            {
                Debug.LogError("Error al obtener los usuarios: " + task.Exception);
                callback(null, false);
            }
        });
    }
    public void GetAllUserEvents(string id_user, System.Action<List<EventsData>, bool> callback)
    {
        if (!isConnected)
        {
            Debug.LogError("No se puede obtener los eventos del usuario porque no está conectado a Firebase.");
            callback(null, false);
            return;
        }

        db.Collection("Participaciones").WhereEqualTo("id_user", id_user).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                List<EventsData> userEventsList = new List<EventsData>();
                foreach (DocumentSnapshot document in task.Result.Documents)
                {
                    ParticipantionsData participationData = document.ConvertTo<ParticipantionsData>();
                    ReadEvent(participationData.id_event, (eventData, status) =>
                    {
                        if (status)
                        {
                            userEventsList.Add(eventData);
                        }
                    });
                }
                callback(userEventsList, true);
            }
            else
            {
                Debug.LogError("Error al obtener los eventos del usuario: " + task.Exception);
                callback(null, false);
            }
        });
    }
    public void GetAllUserParticipations(string id_user, System.Action<List<ParticipantionsData>, bool> callback)
    {
        if (!isConnected)
        {
            Debug.LogError("No se puede obtener las participaciones del usuario porque no está conectado a Firebase.");
            callback(null, false);
            return;
        }

        db.Collection("Participaciones").WhereEqualTo("id_user", id_user).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                List<ParticipantionsData> participationsList = new List<ParticipantionsData>();
                foreach (DocumentSnapshot document in task.Result.Documents)
                {
                    ParticipantionsData participationData = document.ConvertTo<ParticipantionsData>();
                    participationsList.Add(participationData);
                }
                callback(participationsList, true);
            }
            else
            {
                Debug.LogError("Error al obtener las participaciones del usuario: " + task.Exception);
                callback(null, false);
            }
        });
    }
    public void GetAllEventParticipations(string id_event, System.Action<List<ParticipantionsData>, bool> callback)
    {
        if (!isConnected)
        {
            Debug.LogError("No se puede obtener las participaciones del evento porque no está conectado a Firebase.");
            callback(null, false);
            return;
        }

        db.Collection("Participaciones").WhereEqualTo("id_event", id_event).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                List<ParticipantionsData> participationsList = new List<ParticipantionsData>();
                foreach (DocumentSnapshot document in task.Result.Documents)
                {
                    ParticipantionsData participationData = document.ConvertTo<ParticipantionsData>();
                    participationsList.Add(participationData);
                }
                callback(participationsList, true);
            }
            else
            {
                Debug.LogError("Error al obtener las participaciones del evento: " + task.Exception);
                callback(null, false);
            }
        });
    }
}
