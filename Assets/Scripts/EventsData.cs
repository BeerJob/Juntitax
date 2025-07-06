using Firebase.Firestore;

[FirestoreData]

public struct EventsData
{
    [FirestoreProperty]
    public string id_event {get; set;}
    [FirestoreProperty]
    public string name_event {get; set;}

    [FirestoreProperty]
    public string location_event {get; set;}

    [FirestoreProperty]
    public GeoPoint locgeo_event {get; set;}

    [FirestoreProperty]
    public string[] created_event {get; set;}

}