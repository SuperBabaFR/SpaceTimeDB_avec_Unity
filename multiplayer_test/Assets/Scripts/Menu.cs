using System;
using TMPro;
using UnityEngine;

public class Menu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject Canvas;
    public TMP_InputField input_pseudo;

    void Start()
    {
        Canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowCanvas() {
        Canvas.SetActive(true);
    }

    public void Play() {
        string pseudo = input_pseudo.text;

        if (pseudo == "") {
            Debug.LogWarning("Champ pseudo vide :");
            return;
        }

        Debug.Log("Pseudo choisi : " + input_pseudo.text);

        Canvas.SetActive(false);

        GameManager.Conn.Reducers.EnterGame(pseudo);
    }

}
