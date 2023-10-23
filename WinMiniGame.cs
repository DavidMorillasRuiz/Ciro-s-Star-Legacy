using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinMiniGame : MonoBehaviour
{
    [SerializeField] GameObject winPanel;
    public bool menuWinner;
    // Start is called before the first frame update
    void Start()
    {
        winPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hideMenu()
    {
        winPanel.SetActive(false);
        menuWinner = false;
    }

    public void winMenuShow()
    {
        winPanel.SetActive(true);
        menuWinner = true;
    }

    public void changeScene()
    {
        SceneManager.LoadScene(5);
    }

  
}
