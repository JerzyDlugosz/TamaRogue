using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;

    public AudioManager audioManager;

    public int LoadingSceneNumber;

    public int ChosenDeck;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
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
