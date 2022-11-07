using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainSenceUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mes;
    public GameObject set;
    
    public void startGame()
    {
        SceneManager.LoadScene ("#2-singlegame");
    }

    public void gameSet()
    {
        set.SetActive(true);
    }
    public void setClose()
    {
        set.SetActive(false);
    }
    public void gameClose()
    {
        mes.SetActive(false);
    }
    public void gameMessage()
    {
        mes.SetActive(true);
    }
    public void exitGame()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
