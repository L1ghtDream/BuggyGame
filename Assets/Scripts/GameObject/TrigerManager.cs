using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TrigerManager : MonoBehaviour
{
    public List<float> atributes;
    private List<float> Bug1_allowedLevels = new List<float>()
    {
        6
    };

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            GameObject.Find("Controller").GetComponent<LevelManager>().passLevel.Invoke(atributes);

            if (!atributes.Contains(-3))
                gameObject.SetActive(false);
            if(checkCommon(atributes, Bug1_allowedLevels))
            {
                List<float> var1 = new List<float>();
                foreach (int atribute in atributes)
                    if (atribute > 0)
                        var1.Add(atribute);
                atributes = var1;
                atributes.Add(-4);
            }   
        }
    }

    private bool checkCommon(List<float> list1, List<float> list2 )
    {
        return list1.Intersect(list2).Any();
    }

    public void useUp()
    {
        if (!atributes.Contains(-3))
            gameObject.SetActive(false);
        if (checkCommon(atributes, Bug1_allowedLevels))
        {
            List<float> var1 = new List<float>();
            foreach (int atribute in atributes)
                if (atribute > 0)
                    var1.Add(atribute);
            atributes = var1;
            atributes.Add(-4);
        }
    }
}
