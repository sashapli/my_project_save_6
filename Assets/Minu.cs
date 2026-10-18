using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Minu : MonoBehaviour
{
    public GameObject Pauz;
    public bool isGame = true;
    public bool IsTogge;
 public void Garash()
    {
        SceneManager.LoadScene(1);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!IsTogge && isGame)
            {
                Pauz.SetActive(true);
                IsTogge = true;
                Time.timeScale = 0;
            }
            else
            {
                Pauz.SetActive(false);
                IsTogge = false;
                Time.timeScale = 1;
            }
        }
    }
    public void pauz()
    {
        if (isGame)
        {
            Time.timeScale = 0;
            Pauz.SetActive(true);
        }
    }
    public void pauzNot()
    {
        Time.timeScale = 1;
        Pauz.SetActive(false);
    }
    public void THome()
    {
        SceneManager.LoadScene(0);
    }
}
