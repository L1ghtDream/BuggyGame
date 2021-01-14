using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    private bool state;
    private bool changeState;

    private Controller controller;



    private void Awake()
    {
        controller = GameObject.Find("Controller").GetComponent<Controller>();
    }

    private void Update()
    {
        if (changeState && !controller.getPauseState())
        {
            state = !state;
            changeState = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(!controller.getPauseState())
            state = true;
        else
            changeState = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!controller.getPauseState())
            state = false;
        else
            changeState = true;
    }



    public bool getState()
    {
        return state;
    }
}
