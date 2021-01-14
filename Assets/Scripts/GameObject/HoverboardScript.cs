using UnityEngine;

public class HoverboardScript : MonoBehaviour
{
    private GameObject defaultParrent;

    private void OnCollisionEnter(Collision collider)
    {
        print(collider.gameObject.name + " " + gameObject.name);
        if (transform.parent.tag == "Player")
        {
            transform.parent.GetComponent<Movement>().collided();
            print("Colided");
        }
    }

    public void unRide()
    {
        if (defaultParrent != null)
            transform.parent = defaultParrent.transform;
        else
        {
            GameObject global = GameObject.Find("Controller").GetComponent<Controller>().Levels.transform.GetChild(0).gameObject;
            transform.parent = global.transform.GetChild(global.transform.childCount -1);
        }    

    }
}
