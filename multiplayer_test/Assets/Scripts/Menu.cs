using System;
using TMPro;
using UnityEngine;

public class Menu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject Canvas;
    public TMP_InputField input_pseudo;

    public static event Action OnLocalPlayerGameEntered;


    void Start()
    {
        Canvas.SetActive(false);
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
        OnLocalPlayerGameEntered?.Invoke();
    }

}
