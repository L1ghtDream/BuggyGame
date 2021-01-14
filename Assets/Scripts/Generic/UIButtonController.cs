using UnityEngine;
using UnityEngine.UI;

public class UIButtonController : MonoBehaviour
{
    private Controller controller;

    private void Awake()
    {
        controller = GameObject.Find("Controller").GetComponent<Controller>();

        switch (gameObject.name)
        {
            case "Resume":
                GetComponent<Button>().onClick.AddListener(() => resumeGame());
                break;
            case "Options":
                GetComponent<Button>().onClick.AddListener(() => openOptions());
                break;
            case "Load":
                GetComponent<Button>().onClick.AddListener(() => openLoadLevel());
                break;
            case "QuitMenu":
                GetComponent<Button>().onClick.AddListener(() => openMainMenu());
                break;
            case "QuitDesktop":
                GetComponent<Button>().onClick.AddListener(() => quitGame());
                break;
            default:
                if(gameObject.name.Contains("Level"))
                    GetComponent<Button>().onClick.AddListener(() => loadLevel(gameObject.transform.GetSiblingIndex()));
                break;
        }
    }

    private void resumeGame()
    {
        controller.changePauseState();
    }

    private void openOptions()
    {
        
    }

    private void openLoadLevel()
    {
        controller.openLoadLevel();
    }
    
    private void openMainMenu()
    {

    }

    private void quitGame()
    {

    }

    private void loadLevel(int level)
    {
        controller.loadLevel(level);
    }


}

