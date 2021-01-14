using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public enum types
    {
        dev,
        prod
    };
    public enum levels
    {
        Level1 = 1,
        Level2 = 2,
        Level3 = 3,
        Level4 = 4,
        Level5 = 5,
        Level6 = 6,
        Level7 = 7,
        Level8 = 8,
        Level9 = 9,
        Level10 = 10,
        Level11 = 11
    };

    public types state;
    public levels level;

    private CharacterController movementController;
    private Controller controller;
    private LevelManager levelManager;

    private Vector3 playerVelocity;

    private float speed = 5f;
    private float jumpPower = 1.0f;
    private float gravity = -9.81f;

    private bool grounded;
    private bool jumpGrounded;
    private bool isRiding;
    private bool isInTerminal;



    public float movementVelocity;

    private List<Vector3> l = new List<Vector3>()
    {
         new Vector3(0, -11.7600002f, 0),
         new Vector3(23.9300003f, 7.13000011f, -24.3600006f),
         new Vector3(33.2000008f, 7.4000001f, -37.3199997f),
         new Vector3(10.8800001f, 7.4000001f, -36.4599991f),
         new Vector3(-3.50999999f,7.09100008f,-62.3400002f),
         new Vector3(-3.50999999f,7.09100008f,-84),
         new Vector3(-11.9799995f,20.8099995f,-110.519997f),
         new Vector3(-55.7099991f,20.7999992f,-132.360001f),
         new Vector3(-26.7399998f,21.0100002f,-143.190002f),
         new Vector3(-30f,21.25f,-189f),
         new Vector3(-107.400002f,21.25f,-189f)
    };


    private void Start()
    {
        movementController = GetComponent<CharacterController>();
        controller = GameObject.Find("Controller").GetComponent<Controller>();
        levelManager = GameObject.Find("Controller").GetComponent<LevelManager>();

        if (state == types.dev)
            loadLevel((int)level);
        else
            loadLevel(1);



        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if(!controller.getLoadLevelStatus())
        {
            if(!isInTerminal)
            {
                if (!isRiding)
                {
                    if (!controller.getPauseState())
                        transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0));

                    Vector3 player = transform.position;

                    grounded = Physics.Raycast(player, new Vector3(0, -1, 0), 1.001f);
                    jumpGrounded = Physics.Raycast(player, new Vector3(0, -1, 0), 1.5f);

                    if (grounded && playerVelocity.y < 0)
                        playerVelocity.y = 0f;

                    Vector3 move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
                    movementController.Move(move * Time.deltaTime * speed);

                    if (Input.GetButtonDown("Jump") && jumpGrounded)
                        playerVelocity.y += Mathf.Sqrt(jumpPower * -9.0f * gravity);

                    if (transform.parent == null)
                        playerVelocity.y += gravity * Time.deltaTime * 3;

                    movementController.Move(playerVelocity * Time.deltaTime);
                }
                else
                {
                    if (!controller.getPauseState())
                        transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal"), 0));

                    if (Input.GetAxis("Vertical") > 0)
                        movementVelocity += 0.02f;
                    else if (Input.GetAxis("Vertical") == 0)
                        movementVelocity -= 0.02f;
                    else if (Input.GetAxis("Vertical") <= 0)
                        movementVelocity -= 0.04f;
                    if (movementVelocity < 0)
                        movementVelocity = 0;
                    movementController.Move(transform.forward * movementVelocity * Time.deltaTime * speed);
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 100))
                    {
                        switch (hit.transform.tag)
                        {
                            case "HoverBoard":
                                transform.parent = hit.collider.transform;

                                transform.localEulerAngles = new Vector3(0, 90, 0);
                                transform.GetChild(0).localEulerAngles = new Vector3(0, 0, 0);
                                transform.localPosition = new Vector3(0, 0, 0);

                                transform.parent = null;

                                hit.collider.transform.parent = transform;
                                hit.collider.transform.localPosition = new Vector3(0, -0.85f, 0);

                                isRiding = true;
                                break;
                            case "Terminal":
                                hit.collider.gameObject.GetComponent<TerminalScript>().login();
                                isInTerminal = true;
                                transform.GetChild(0).gameObject.SetActive(false);
                                break;
                        }

                    }
                }
                if (Input.GetKeyDown(KeyCode.LeftShift))
                    if (isRiding)
                    {
                        transform.GetChild(2).GetComponent<HoverboardScript>().unRide();
                        isRiding = false;
                        movementVelocity = 0;
                    }   
            }
        }
    }

    public void loadLevel(int level)
    {
        gameObject.transform.position = l[Mathf.Abs(level) - 1];
        if (level < 0)
            levelManager.passLevel.Invoke(new List<float>() { -2, Mathf.Abs(level) - 1 + 0.1f });
        else
            levelManager.passLevel.Invoke(new List<float>() { level - 1 + 0.1f });
    }

    public bool getRidingStatus()
    {
        return isRiding;
    }

    public float getMovementVelocity()
    {
        return movementVelocity;
    }    

    public void collided()
    {
        movementVelocity = 0;
    }

    public void setInTerminaState(bool state)
    {
        isInTerminal = state;
    }

    public bool getInTerminalState()
    {
        return isInTerminal;
    }
}