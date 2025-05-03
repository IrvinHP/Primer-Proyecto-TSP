using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Events : MonoBehaviour
{
    public GameObject child;
    public GameObject parent;

    public VRWalk script;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void ChangeByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        script.enabled = true;

        child.transform.SetParent(parent.transform);

        child.transform.localPosition = new Vector3(2.08f, 1.19f);
        child.transform.localEulerAngles = new Vector3(0f, 79.3f, 0f);
        child.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
    }

    public void ChangeByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void ReloadCurrent()
    {
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex + 1);

        script.enabled = true;

        child.transform.SetParent(parent.transform);

        child.transform.localPosition = new Vector3(2.08f, 1.19f);
        child.transform.localEulerAngles = new Vector3(0f, 79.3f, 0f);
        child.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
    }
    public void ExitApp()
    {
        Application.Quit();
    }

    void Start()
    {
        script.enabled = false;
    }

    void Update()
    {
        
    }
}
