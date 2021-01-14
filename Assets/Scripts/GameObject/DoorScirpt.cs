using UnityEngine;

public class DoorScirpt : MonoBehaviour
{
    public GameObject triger;

    private void Update()
    {
        if(triger.GetComponent<ButtonScript>().getState())
        {
            if (transform.GetChild(0).localPosition.y < 1)
                transform.GetChild(0).localPosition = new Vector3(transform.GetChild(0).localPosition.x, transform.GetChild(0).localPosition.y + Time.deltaTime, transform.GetChild(0).localPosition.z);
        }
        else
        {
            if (transform.GetChild(0).localPosition.y > 0)
                transform.GetChild(0).localPosition = new Vector3(transform.GetChild(0).localPosition.x, transform.GetChild(0).localPosition.y - Time.deltaTime, transform.GetChild(0).localPosition.z);
        }
    }
}
