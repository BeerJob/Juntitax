using System;
using Firebase.Firestore;

[FirestoreData]

public struct EventsData
{
    [FirestoreProperty]
    public string id_event { get; set; }
    [FirestoreProperty]
    public string id_organizer { get; set; }
    [FirestoreProperty]
    public string name_event { get; set; }

    [FirestoreProperty]
    public string location_event { get; set; }

    [FirestoreProperty]
    public GeoPoint locgeo_event { get; set; }

    [FirestoreProperty]
    public DateTime date_event { get; set; }

    [FirestoreProperty]
    public DateTime created_event { get; set; }
}

[FirestoreData]
public struct UsersData
{
    [FirestoreProperty]
    public string id_user { get; set; }
    [FirestoreProperty]
    public string name_user { get; set; }
    [FirestoreProperty]
    public string password_user { get; set; }
}

[FirestoreData]
public struct ParticipantionsData
{
    [FirestoreProperty]
    public string id_user { get; set; }
    [FirestoreProperty]
    public string id_event { get; set; }
    [FirestoreProperty]
    public string id_participation { get; set; }
    [FirestoreProperty]
    public DateTime date_participation { get; set; }
}