using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Text lifeText;
    public int life;

    public AudioSource audioSouce;

    // Start is called before the first frame update
    void Start()
    {
        life = PlayerPrefs.GetInt("Life", life);
        lifeText.text = " x " + life;

        audioSouce = GetComponent<AudioSource>();
    }

    
    // Update is called once per frame
    void Update()
    {
        AddLife();

        RestartGame();
    }

    public void AddLife()
    {
        //Debug.Log("Addlife");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioSouce.Play();
            life++;
            PlayerPrefs.SetInt("Life", life);
            lifeText.text = " x " + life;
        }
    }

    private void RestartGame()
    {
        //Debug.Log("RestartGame");

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (life > 0)
            {
                life--;
                PlayerPrefs.SetInt("Life", life);
                SceneManager.LoadScene("Level1", LoadSceneMode.Single);
            }
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Life", 0);
    }
}
