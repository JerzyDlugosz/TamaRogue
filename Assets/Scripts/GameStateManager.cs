using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;

    public AudioManager audioManager;

    public int LoadingSceneNumber;

    public int ChosenDeck;

    List<Vector2Int> resolutions;
    public int currentResolution = 2;


    private void Awake()
    {

        currentResolution = PlayerPrefs.GetInt("Resolution", 2);
        resolutions = new List<Vector2Int>() { new Vector2Int(128, 128), new Vector2Int(256, 256), new Vector2Int(512, 512), new Vector2Int(640, 640), new Vector2Int(768, 768), new Vector2Int(896, 896), new Vector2Int(1024, 1024) };

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void ChangeRes()
    {
        currentResolution++;
        if (currentResolution > resolutions.Count - 1)
        {
            currentResolution = 0;
        }
        Debug.Log($"ResChangeTo: {currentResolution}");
        PlayerPrefs.SetInt("Resolution", currentResolution);
        Screen.SetResolution(resolutions[currentResolution].x, resolutions[currentResolution].y, false);
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "PersistentScene")
        {
            OnGameInit();
            SceneManager.LoadScene(1);
            audioManager.OnMapChange((Zone)1);
        }
    }

    private void OnGameInit()
    {
        //Cursor.visible = false;
    }

    public void LoadScene(int sceneNumber)
    {
        DOTween.KillAll();
        StartCoroutine(LoadAsyncScene(sceneNumber));
    }

    public void LoadGameSceneWithLoadingScreen()
    {
        DOTween.KillAll();
        StartCoroutine(LoadAsyncGameScene());
    }

    public void ExitApplication()
    {
        DOTween.KillAll();
        Application.Quit();
    }

    IEnumerator LoadAsyncScene(int sceneNumber)
    {


        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneNumber);
        while (!asyncLoad.isDone)
        {
            //Debug.Log(asyncLoad.progress);
            yield return null;
        }
        audioManager.OnMapChange((Zone)sceneNumber);
    }

    IEnumerator LoadAsyncGameScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(LoadingSceneNumber);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

}
