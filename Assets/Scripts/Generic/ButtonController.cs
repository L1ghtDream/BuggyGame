using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public enum type
    {
        Door,
        AND_gate
    };

    /// <summary>
    /// Door -> Takes 1 argument as the button needed to be pressed is order to get the state
    /// AND_gate -> Takes 2 arguments in order to AND them and get the state
    /// </summary>
    public List<GameObject> trigers;
    public type Type;
    public List<bool> states = new List<bool>()
            {
                false, false
            };


    public bool state = false;
    private Controller controller;

    private void Awake()
    {
        controller = GameObject.Find("Controller").GetComponent<Controller>();
    }


    private void Update()
    {
        if(Type == type.Door)
        {
            if (trigers[0].GetComponent<ButtonScript>() != null)
            {
                if (trigers[0].GetComponent<ButtonScript>().getState())
                {
                    state = true;
                    if (transform.GetChild(0).localPosition.y < 1)
                        transform.GetChild(0).localPosition = new Vector3(transform.GetChild(0).localPosition.x, transform.GetChild(0).localPosition.y + Time.deltaTime, transform.GetChild(0).localPosition.z);
                }

                else
                {
                    state = false;
                    if (transform.GetChild(0).localPosition.y > 0)
                        transform.GetChild(0).localPosition = new Vector3(transform.GetChild(0).localPosition.x, transform.GetChild(0).localPosition.y - Time.deltaTime, transform.GetChild(0).localPosition.z);
                }
            }
            else if (trigers[0].GetComponent<ButtonController>() != null)
            {
                if (trigers[0].GetComponent<ButtonController>().getState())
                {
                    state = true;
                    if (transform.GetChild(0).localPosition.y < 1)
                        transform.GetChild(0).localPosition = new Vector3(transform.GetChild(0).localPosition.x, transform.GetChild(0).localPosition.y + Time.deltaTime, transform.GetChild(0).localPosition.z);
                }
                else
                {
                    state = false;
                    if (transform.GetChild(0).localPosition.y > 0)
                        transform.GetChild(0).localPosition = new Vector3(transform.GetChild(0).localPosition.x, transform.GetChild(0).localPosition.y - Time.deltaTime, transform.GetChild(0).localPosition.z);
                }
            }
        }
        else if(Type == type.AND_gate)
        {

            for (int i=0;i<=1;i++)
            {
                if (trigers[i].GetComponent<ButtonScript>() != null)
                {
                    states[i] = trigers[i].GetComponent<ButtonScript>().getState();

                    if (states[i])
                        transform.GetChild(i + 1).GetComponent<MeshRenderer>().material = controller.RedLightOn;
                    else
                        transform.GetChild(i + 1).GetComponent<MeshRenderer>().material = controller.RedLightOff;
                }
                else if (trigers[i].GetComponent<ButtonController>() != null)
                {
                    states[i] = trigers[i].GetComponent<ButtonController>().getState();

                    if (states[i])
                        transform.GetChild(i + 1).GetComponent<MeshRenderer>().material = controller.RedLightOn;
                    else
                        transform.GetChild(i + 1).GetComponent<MeshRenderer>().material = controller.RedLightOff;
                }
            }

            //Update the output
            if (states[0] && states[1])
            {
                state = true;
                transform.GetChild(3).GetComponent<MeshRenderer>().material = controller.RedLightOn;
            }
            else
            {
                state = false;
                transform.GetChild(3).GetComponent<MeshRenderer>().material = controller.RedLightOff;
            }

        }
    }

    public bool getState()
    {
        return state;
    }
}
