using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    private Controller controller;

    private void Start()
    {
        controller = GameObject.Find("Controller").GetComponent<Controller>();
    }

    void Update()
    {
        if(!controller.getPauseState())
            if(!controller.getPlayerRidingStatus())
                transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"), 0, 0));
    }
}
