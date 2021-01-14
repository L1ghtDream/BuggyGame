using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;

public class TerminalScript : MonoBehaviour
{
    private List<string> keys = new List<string>()
    {
        "backspace",
        //"delete",
        //"tab",
        //"clear",
        "return",
        //"pause",
        "escape",
        "space",
        //"up",
        //"down",
        //"right",
        //"left",
        //"insert",
        //"home",
        //"end",
        //"page up",
        //"page down",
        //"f1",
        //"f2",
        //"f3",
        //"f4",
        //"f5",
        //"f6",
        //"f7",
        //"f8",
        //"f9",
        //"f10",
        //"f11",
        //"f12",
        //"f13",
        //"f14",
        //"f15",
        "0",
        "1",
        "2",
        "3",
        "4",
        "5",
        "6",
        "7",
        "8",
        "9",
        //"!",
        //"\"",
        //"#",
        //"$",
        //"&",
        //"'",
        //"(",
        //")",
        //"*",
        //"+",
        //",",
        //"-",
        ".",
        //"/",
        //":",
        //";",
        //"<",
        //"=",
        //">",
        //"?",
        //"@",
        //"[",
        //"\\",
        //"]",
        //"^",
        //"_",
        //"`",
        "a",
        "b",
        "c",
        "d",
        "e",
        "f",
        "g",
        "h",
        "i",
        "j",
        "k",
        "l",
        "m",
        "n",
        "o",
        "p",
        "q",
        "r",
        "s",
        "t",
        "u",
        "v",
        "w",
        "x",
        "y",
        "z",
        //"numlock",
        //"caps lock",
        //"scroll lock",
        //"right shift",
        //"left shift",
        //"right ctrl",
        //"left ctrl",
        //"right alt",
        //"left alt"
     };

    private UnityEvent<List<string>> cd = new UnityEvent<List<string>>();
    private UnityEvent<List<string>> ls = new UnityEvent<List<string>>();
    private UnityEvent<List<string>> help = new UnityEvent<List<string>>();
    private UnityEvent<List<string>> nano = new UnityEvent<List<string>>();
    private UnityEvent<List<string>> logout = new UnityEvent<List<string>>();
    private UnityEvent<List<string>> clear = new UnityEvent<List<string>>();

    private Dictionary<string, UnityEvent<List<string>>> commands = new Dictionary<string, UnityEvent<List<string>>>();


    private List<string> joinText = new List<string>()
    {
        "User: ", "hint" ,
        "Pass: ", "************" ,
        "", "",
        "Last login: Level 1",
        "", "", "" ,
        "For help use `help`", "" ,
        "", "" ,
        "hint @lab: ~# ", ""
    };

    private static Dictionary<string, string> hints = new Dictionary<string, string>()
    {
        {"level1.hint", "[...]" }
    };

    private static Dictionary<string, string> locations = new Dictionary<string, string>()
    {
        {"~", "" },
        {"", "hints" },
        {"hints", "level1.hint     level2.hint" }
    };

    public bool joinState = false;
    public bool joined = false;
    private static bool inFile = false;

    private string buffer;
    private int i = 0;
    private int j = -1;
    private int k = 0;

    private static string location = "";
    private static string lastScreen = "";
    private static Transform thisTransform;

    private void Awake()
    {
        cd.AddListener(cd_Function);
        ls.AddListener(ls_Function);
        help.AddListener(help_Function);
        clear.AddListener(help_Function);
        logout.AddListener(logout_Function);
        nano.AddListener(nano_Function);
        clear.AddListener(clear_Function);

        commands.Add("cd", cd);
        commands.Add("ls", ls);
        commands.Add("help", help);
        commands.Add("nano", nano);
        commands.Add("logout", logout);
        commands.Add("clear", clear);
    }

    private void Start()
    {
        transform.GetChild(4).GetComponent<TextMeshPro>().text = "";
        thisTransform = transform;
    }

    private void Update()
    {
        if(i*2 < joinText.Count && joinState)
        {
            if (j == -1)
            {
                transform.GetChild(4).GetComponent<TextMeshPro>().text += joinText[i * 2];
                j++;
            }
            else if (j < joinText[i * 2 + 1].Length) 
            {
                if (k >= 10)
                {
                    transform.GetChild(4).GetComponent<TextMeshPro>().text += joinText[i * 2 + 1][j];
                    j++;
                    k = 0;
                }
                k++;
            }
            else if(j == joinText[i * 2 + 1].Length)
            {
                j = -1;
                i++;
                if(i*2<joinText.Count)
                    transform.GetChild(4).GetComponent<TextMeshPro>().text += "\n";
            }
        }
        else if (i*2 >= joinText.Count)
        {
            joinState = false;
            i = 0;
            j = -1;
        }


        if(!joinState && joined)
        {
            foreach (string key in keys)
            {
                if (Input.GetKeyDown(key))
                {
                    switch (key)
                    {
                        case "escape":
                            print("escape"); // working till here chekc the inFile
                            if(inFile)
                            {
                                inFile = false;
                                transform.GetChild(4).GetComponent<TextMeshPro>().text = lastScreen;
                            }
                            else
                            {
                                //logout
                            }
                            break;
                        case "backspace":
                            if(!inFile)
                            {
                                if (buffer.Length >= 1)
                                {
                                    transform.GetChild(4).GetComponent<TextMeshPro>().text = transform.GetChild(4).GetComponent<TextMeshPro>().text.Remove(transform.GetChild(4).GetComponent<TextMeshPro>().text.Length - 1, 1);
                                    buffer = buffer.Remove(buffer.Length - 1, 1);
                                }
                            }
                            break;
                        case "return":
                            if(!inFile)
                            {
                                List<string> args = buffer.Split(' ').ToList();
                                args.RemoveAt(0);
                                print(buffer.Split(' ')[0]);
                                commands[buffer.Split(' ')[0]].Invoke(args);
                                if(buffer.Split(' ')[0] != "nano")
                                    transform.GetChild(4).GetComponent<TextMeshPro>().text += craftNewLine();
                                buffer = "";
                            }
                            break;
                        case "space":
                            if(!inFile)
                            {
                                transform.GetChild(4).GetComponent<TextMeshPro>().text += " ";
                                buffer += " ";
                            }
                            break;
                        default:
                            if(!inFile)
                            {
                                transform.GetChild(4).GetComponent<TextMeshPro>().text += key;
                                buffer += key;
                            }
                            break;
                    }
                }
            }
        }
    }

    public void login()
    {
        i = 0;
        j = -1;
        joinState = true;
        joined = true;
        transform.GetChild(5).gameObject.SetActive(true);
    }

    private static void nano_Function(List<string> args)
    {

        if (args.Count > 1 && args != null)
        {
            printMessage("NumberOfArgumentsExceded. Command `nano` can have only 1 argument");
            return;
        }
        if(location == "hints")
        {
            lastScreen = thisTransform.GetChild(4).GetComponent<TextMeshPro>().text;
            thisTransform.GetChild(4).GetComponent<TextMeshPro>().text = hints[args[0]];
            return;
        }
        else 
            printMessage("The specified name `" + args[0] + "` has not beed found");
    }

    private static void clear_Function(List<string> args)
    {
        if (args.Count > 0)
        {
            printMessage("NumberOfArgumentsExceded. Command `clear` can have only 0 argument");
            return;
        }
        thisTransform.GetChild(4).GetComponent<TextMeshPro>().text = craftNewLine();
    }

    private static void logout_Function(List<string> args)
    {
        if (args.Count > 0)
        {
            printMessage("NumberOfArgumentsExceded. Command `logout` can have only 0 argument");
            return;
        }
        //logout
    }

    private static void help_Function(List<string> args)
    {
        if (args.Count > 0) 
        {
            printMessage("NumberOfArgumentsExceded. Command `help` can have only 0 argument");
            return;
        }
        printMessage("Commands:\nhelp -> Show this menu\n-ls -> List avanible directoryes and files\ncd [arg1]-> Change directory (~ = home)\nnano [arg1]-> Edit/Read files\nlogout -> Get out of the terminal");
    }

    private static void ls_Function(List<string> args)
    {
        if (args.Count > 0)
        {
            printMessage("NumberOfArgumentsExceded. Command `ls` can have only 0 argument");
            return;
        }
        printMessage(locations[location]);
    }

    private static void cd_Function(List<string> args)
    {
        if (args.Count > 1)
        {
            printMessage("NumberOfArgumentsExceded. Command `cd` can have only 1 argument");
            return;
        }
        if (locations.Keys.ToList().Contains(args[0]))
            location = args[0];
        else
            printMessage("The specified name `" + args[0] + "` has not beed found");
            
    }

    private static void printMessage(string message)
    {
        thisTransform.GetChild(4).GetComponent<TextMeshPro>().text += "\n" + message;
    }

    private static string craftNewLine()
    {
        return "\nhint @lab: ~" + location + "# ";
    }
}
