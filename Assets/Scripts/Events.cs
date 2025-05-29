using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System;

public class Events : MonoBehaviour
{
    private string _eventPath = "Eventos";
    
    public InputField _nameField;

    public InputField _organizeField;

    public InputField _locField;
    public InputField _geolocField;
    public InputField _geolocField1;
    public InputField _assistField;    

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
                Locgeo_event = new GeoPoint(Convert.ToDouble(_geolocField.text), Convert.ToDouble(_geolocField1.text)),
                Created_event = _organizeField.text.Split(','),
                Assist_event = _assistField.text.Split(',')
            };

            var firestore = FirebaseFirestore.DefaultInstance;
            firestore.Collection(_eventPath).AddAsync(eventData);
            
            _nameField.text = "";

            _organizeField.text = "";

            _locField.text = "";
            _geolocField.text = "";
            _geolocField1.text = "";
            _assistField.text = "";    
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
