using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public GameObject Levels;
    public GameObject Player;

    public UnityEvent<List<float>> passLevel = new UnityEvent<List<float>>();
    public List<GameObject> LevelsPrefabs = new List<GameObject>();
    public List<GameObject> GlobalLevelsPrefabs = new List<GameObject>();

    private void Awake()
    {   
        passLevel.AddListener(onLevelPass);
    }

    /// <summary>
    /// -4 -> Load only the global
    /// -3 -> [*] Reserved for Class TrigerManager
    /// -2 -> Clear all
    /// -1 -> Clear Global
    /// 0-x -> Load Level Specified
    /// </summary>

    private void onLevelPass(List<float> atributes)
    {
        if(atributes.Count == 2)
            print("Call " + atributes.Count + " " + atributes[0] + " " + atributes[1]);
        foreach (float atribute in atributes)
        {
            switch (atribute)
            {
                case -4:
                    break;

                case -3:
                    break;

                case -2:
                    Player.GetComponent<DragAndDrop>().clearTarget();
                    for (int i = 1; i < Levels.transform.childCount; i++)
                        Destroy(Levels.transform.GetChild(i).gameObject);
                    for (int i = 0; i < Levels.transform.GetChild(0).childCount; i++)
                        Destroy(Levels.transform.GetChild(0).GetChild(i).gameObject);
                    break;

                case -1:
                    Player.GetComponent<DragAndDrop>().clearTarget();
                    for (int i = 0; i < Levels.transform.GetChild(0).childCount - 1; i++)
                        Destroy(Levels.transform.GetChild(0).GetChild(i).gameObject);
                    break;

                default:
                    if (!atributes.Contains(-4))
                        if ((int)atribute != 0)
                            Destroy(Levels.transform.GetChild(1).gameObject);

                    if (atribute % 1 != 0)
                    {
                        Instantiate(GlobalLevelsPrefabs[(int)atribute], Vector3.zero, Quaternion.identity).transform.parent = Levels.transform.GetChild(0);
                        if (!atributes.Contains(-4))
                        {
                            GameObject clone = Instantiate(LevelsPrefabs[(int)atribute], Vector3.zero, Quaternion.identity);
                            clone.transform.parent = Levels.transform;
                            clone.transform.GetChild(0).gameObject.SetActive(true);
                            try
                            {
                                clone.transform.GetChild(0).GetComponent<TrigerManager>().useUp();
                            }
                            catch (NullReferenceException e)
                            {
                                print(e.Data);
                            }
                        }
                    }

                    GameObject.Find("Controller").GetComponent<Controller>().finishLevel((int)atribute + 1);

                    if ((int)atribute + 1 < GlobalLevelsPrefabs.Count)
                    {
                        try
                        {
                            Instantiate(GlobalLevelsPrefabs[(int)atribute + 1], Vector3.zero, Quaternion.identity).transform.parent = Levels.transform.GetChild(0);
                        }
                        catch (ArgumentOutOfRangeException e)
                        {
                            Debug.LogError(atribute);
                        }

                        if (!atributes.Contains(-4))
                        {
                            GameObject clone = Instantiate(LevelsPrefabs[(int)atribute + 1], Vector3.zero, Quaternion.identity);
                            clone.transform.parent = Levels.transform;
                            clone.transform.GetChild(0).gameObject.SetActive(true);
                        }
                    }
                    break;
            }
        }
    }
}