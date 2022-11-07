using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PausePanel : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    bool isPrash=true;
    public Staff staff;
    public DamgerUI da;
    public RangeUI ra;
    public ShootSpUI shp;
    public SpeedUI sp;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown ("p"))
        {
            
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPaused = false;
    }
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        staff.UpdateStaff();
        da.UpdateDamgerUI();
        ra.UpdateRange();
        shp.UpdateShootSpUI();
        sp.UpdateSpeedUI();
        Time.timeScale = 0.0f;
        GameIsPaused = true;
    }
    void Esc()
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }
    public void newRun()
    {
        GameIsPaused = false;
        Time.timeScale = 1.0f;
        Destroy(GameObject.Find("Player"));

        Destroy(GameObject.Find("LayerGenerator"));
        SceneManager.LoadScene ("#2-singlegame");
    }

    public void QuitGame()
    {
        Time.timeScale = 1.0f;
        Destroy(GameObject.Find("Player"));

        Destroy(GameObject.Find("LayerGenerator"));
        SceneManager.LoadScene ("#1-main");
        //UnityEditor.EditorApplication.isPlaying = false;
        //Application.Quit();
    }
}