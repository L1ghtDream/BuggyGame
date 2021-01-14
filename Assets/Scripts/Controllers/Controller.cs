using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Class;

public class Controller : MonoBehaviour
{
    public GameObject playerGO;
    public GameObject Levels;
    public GameObject PauseGUI;
    public GameObject InGameGUI;
    public GameObject loadLevelUI;

    public Material RedLightOn;
    public Material RedLightOff;

    public List<Sprite> levelsPreviwes;
    public Sprite unknown;
    public Sprite soon;


    private bool pause;
    private bool loadLevelStatus;
    private Player player = new Player(0);

    private void Awake()    
    {
        DontDestroyOnLoad(gameObject);

        for (int i = 1; i < Levels.transform.childCount; i++)
        {
            Destroy(Levels.transform.GetChild(i).gameObject);
            Destroy(Levels.transform.GetChild(0).GetChild(i - 1).gameObject);
        }

        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        InGameGUI.SetActive(true);
        PauseGUI.SetActive(false);
        loadLevelUI.SetActive(false);
    }

    private void Update()
    {
        if(!playerGO.GetComponent<Movement>().getInTerminalState())
        if(Input.GetKeyDown(KeyCode.Escape))
            changePauseState();
    }

    public bool getPauseState()
    {
        return pause;
    }

    public void changePauseState()
    {
        pause = !pause;
        PauseGUI.SetActive(pause);
        if(!pause)
        {
            loadLevelUI.SetActive(pause);
            loadLevelStatus = false;
        }

        InGameGUI.SetActive(!pause);
        Cursor.visible = pause;

        if (pause)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    public void openLoadLevel()
    {
        loadLevelUI.SetActive(true);
        PauseGUI.SetActive(false);
        loadLevelStatus = true;

        for (int i = 1; i < loadLevelUI.transform.childCount; i++) 
        {
            if(levelsPreviwes.Count >= i)
            {
                if (i <= player.level)
                    loadLevelUI.transform.GetChild(i).GetComponent<Image>().sprite = levelsPreviwes[i - 1];
                else
                    loadLevelUI.transform.GetChild(i).GetComponent<Image>().sprite = unknown;
            }
            else
                loadLevelUI.transform.GetChild(i).GetComponent<Image>().sprite = soon;
        }
    }

    public void loadLevel(int level)
    {
        if(player.level - 1 >= level)
            playerGO.GetComponent<Movement>().loadLevel(-level);
    }

    public void finishLevel(int level)
    {
        if(player.level < level)
            player.finishLevel(level);
    }

    public bool getLoadLevelStatus()
    {
        return loadLevelStatus;
    }

    public bool getPlayerRidingStatus()
    {
        return playerGO.GetComponent<Movement>().getRidingStatus();
    }
}
