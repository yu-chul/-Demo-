using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Died : MonoBehaviour
{
    public static Died instance;  //单例创建
    public GameObject diedMenuUI;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void diedUIShow()
    {
        diedMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void newRun()
    {
        diedMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        Destroy(GameObject.Find("Player"));

        Destroy(GameObject.Find("LayerGenerator"));
        SceneManager.LoadScene ("#2-singlegame");
    }
    public void QuitGame()
    {
        Time.timeScale = 1.0f;
        diedMenuUI.SetActive(false);
        Destroy(GameObject.Find("Player"));

        Destroy(GameObject.Find("LayerGenerator"));
        SceneManager.LoadScene ("#1-main");
    }
}
