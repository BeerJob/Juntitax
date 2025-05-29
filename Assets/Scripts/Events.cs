using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class Events : MonoBehaviour
{
    private string _eventPath = "Eventos";
    
    public TextMeshProUGUI _nameField;

    public TextMeshProUGUI _organizeField;

    public TextMeshProUGUI _locField;
    public TextMeshProUGUI _geolocField;
    public TextMeshProUGUI _assistField;    

    public Button _createButton;    
    // Start is called before the first frame update
    void Start()
    {
        _createButton.onClick.AddListener(() =>
        {
            var eventData = new EventsData
            {
                Name_event = _nameField.text,
                Location_event = _locField.text,
                Locgeo_event = _geolocField.text,
                Created_event = _organizeField.text.Split(',') ,
                Assist_event = _assistField.text.Split(',')
            };

            var firestore = FirebaseFirestore.DefaultInstance;
            firestore.Collection(_eventPath).AddAsync(eventData);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
