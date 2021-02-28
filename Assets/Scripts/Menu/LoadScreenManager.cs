using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LoadScreenManager : MonoBehaviour
{
    public string sceneToLoad;
    AsyncOperation loadingOperation;
    [SerializeField]
    Slider progressBar;

    public LoadScreenManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        progressBar.value = Mathf.Clamp01(loadingOperation.progress / 0.9f);
    }

    public void LoadScene(string _sceneToload)
    {
        loadingOperation = SceneManager.LoadSceneAsync(_sceneToload);
    }
}
