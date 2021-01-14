using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private GameObject target;
    private Controller controller;
    private bool isDragging;
    private Vector3 screenPosition;
    private Vector3 offset;

    private void Start()
    {
        controller = GameObject.Find("Controller").GetComponent<Controller>();
    }

    void Update()
    {
        if (!controller.getPauseState() && !controller.playerGO.GetComponent<Movement>().getInTerminalState() && !controller.playerGO.GetComponent<Movement>().getRidingStatus())
        {
            if (target == null)
                isDragging = false;

            if (Input.GetMouseButtonDown(0))
            {
                target = null;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
                    if (hit.collider.gameObject.tag == "draggable")
                        target = hit.collider.gameObject;

                if (target != null)
                {
                    isDragging = true;
                    screenPosition = Camera.main.WorldToScreenPoint(target.transform.position);
                    offset = target.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                target = null;
            }

            if (isDragging)
            {
                Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);
                Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offset;
                target.transform.position = currentPosition;
            }
        }
    }

    public void clearTarget()
    {
        target = null;
        isDragging = false;
    }
}
