using Firebase.Firestore;

[FirestoreData]

public struct EventsData
{
    [FirestoreProperty]
    public string Name_event {get; set;}

    [FirestoreProperty]
    public string Location_event {get; set;}

    [FirestoreProperty]
    public string Locgeo_event {get; set;}

    [FirestoreProperty]
    public string[] Created_event {get; set;}

    [FirestoreProperty]
    public string[] Assist_event {get; set;}

}