using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    public List<Vector3> positions = new List<Vector3>();
    [Range(1, 100)]
    public int speed;
    public bool move = true;


    private int state;
    private int repetitions;
    private int counter;
    private Transform defaultParent;

    void Start()
    {
        state = 0;
        repetitions = 100 / speed * 35;
        defaultParent = transform.parent.parent;

    }

    void Update()
    {
        if(move)
        {
            if (state + 1 < positions.Count)
            {
                if (counter < repetitions)
                {
                    transform.parent.position = transform.parent.position + (positions[state + 1] - positions[state]) / repetitions;
                    counter++;
                }
                else
                {
                    state++;
                    counter = 0;
                }
            }
            else
            {
                positions.Reverse();
                state = 0;
            }
        }

    }
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.transform.tag == "Player")
            collision.transform.parent.parent = gameObject.transform.parent;
    }

    private void OnTriggerExit(Collider collision)  
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.parent.parent = null;
            transform.parent.parent = defaultParent;
        }
    }


}
