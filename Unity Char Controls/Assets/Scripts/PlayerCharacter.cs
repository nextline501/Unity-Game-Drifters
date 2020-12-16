using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour
{
    private CharacterController _controller;
    private Vector3 direction;

    [SerializeField] private float _movespeed = 2.0f;
    [SerializeField] private float slideSpeed = 4.0f;
    [SerializeField] private float _gravity = 9.81f;
    [SerializeField] private float _jumpPower = 7f;
    [SerializeField] private float _dubbleJumpMultiply = 1f;
    [SerializeField] private float _rotationSpeed = 100f;

    private bool keyWasPressed = false;
    private float _directionY;
    private bool canDubbleJump = false;

    public GameObject mainCamera;
    public GameObject aimCamera;
    public GameObject jumpCamera;
    public GameObject aimReticle;
    public GameObject followTargetAim;
    public GameObject jumpTarget;


    private bool slopeSteeperThanSlopeLimit;
    public float slideFriction = 0.3f;
    private Vector3 hitNormal;

    public float sensitivityX = 200F;
    public float sensitivityY = 200F;

    Animator _animator;
    float blendVelocity = 0.0f;
    public float blendAcceleration = 2.0f;
    public float blendDeceleration = 2.0f;
    private string currentAnimationState;
    const string PLAYER_IDEL = "Idel";
    const string PLAYER_RUN = "Run";
    const string PLAYER_JUMP = "Jump";
    const string PLAYER_AIM = "Aim";
    const string PLAYER_SHOOT = "Shoot";
    private bool isRunning;
    private bool isAiming = false;
    private bool isShooting;
    private bool isJumping;

    private int health;

    public SupplyControlUI supplyControl;
    public WindGustControler windGustControl;
    public MountainControler mountainControl;
    public BloodOakControler bloodControl;
    public CherryControler cherryControl;

    //public Transform startPos;

    // Start is called before the first frame update
    void Start()
    {
        health = 5;
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        mainCamera.SetActive(true);
        aimCamera.SetActive(false);
        aimReticle.SetActive(false);
        jumpCamera.SetActive(false);

        supplyControl.GetComponent<Text>();
        windGustControl.GetComponent<Text>();
        mountainControl.GetComponent<Text>();
        bloodControl.GetComponent<Text>();
        cherryControl.GetComponent<Text>();
    }

    void ChangeAnimationState(string newState)
    {
        if(currentAnimationState == newState)
        {
            return;
        }
        _animator.Play(newState);

        currentAnimationState = newState;

    }

    // Update is called once per frame
    void Update()
    {

        //Bools for current action Running.
        if(Input.GetKeyDown("w") && _controller.isGrounded)
        {
            isRunning = true;
        }

        if (Input.GetKeyUp("w"))
        {
            isRunning = false;
        }

        if (Input.GetKey("w") && blendVelocity < 1.0f)
        {
            blendVelocity += Time.deltaTime * blendAcceleration;
        }
        
        if (!Input.GetKey("w") && blendVelocity > 0.0f)
        {
            blendVelocity -= Time.deltaTime * blendDeceleration;
        }

        if(!Input.GetKey("w") && blendVelocity < 0.0f)
        {
            blendVelocity = 0.0f;
        }

        _animator.SetFloat("BlendVelocity", blendVelocity);

        //Animation Logic
        if (isRunning)
        {
            _animator.Play("Blend Tree");
            ChangeAnimationState(PLAYER_RUN);
        } else if (isJumping)
        {
            ChangeAnimationState(PLAYER_JUMP);
        } else if (isAiming)
        {
            ChangeAnimationState(PLAYER_AIM);
        } else if (isShooting)
        {
            ChangeAnimationState(PLAYER_SHOOT);
        } else
        {
            _animator.Play("Blend Tree");
            ChangeAnimationState(PLAYER_IDEL);
        }

        //Input jump
        if (Input.GetButtonDown("Jump"))
        {
            keyWasPressed = true;
            isRunning = false;
            isJumping = true;
            StartCoroutine(jumpToFalse());
            StartCoroutine(jumpEarlyCheck());
        }

        

        //Camera Switches
        if (Input.GetMouseButton(1) && _controller.isGrounded)
        {
            isAiming = true;
            mainCamera.SetActive(false);
            jumpCamera.SetActive(false);
            aimCamera.SetActive(true);

            aimReticle.SetActive(true);

        }
        else if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
            Cursor.visible = true;
            mainCamera.SetActive(true);
            aimCamera.SetActive(false);

            aimReticle.SetActive(false);
        }

        if (!_controller.isGrounded)
        {
            mainCamera.SetActive(false);
            aimCamera.SetActive(false);
            jumpCamera.SetActive(true);
        }
        else if(!isAiming)
        {
            mainCamera.SetActive(true);
        }

        if (isAiming)
        {
            RotateAim();
        }

        if (Input.GetKeyDown("t"))
        {
            _controller.transform.position = GameMaster.Instance.lastCheckPoint.position;
        }
    }

    void FixedUpdate()
    {
        if (_controller.isGrounded && Vector3.Angle(Vector3.up, hitNormal) <= _controller.slopeLimit)
        {
            canDubbleJump = true;

            if (keyWasPressed)
            {
                _directionY = _jumpPower;
            }
        }
        else
        {
            if (keyWasPressed && canDubbleJump)
            {
                //jumpTarget.transform.Rotate(5f, 0, 0);
                _directionY = _jumpPower * _dubbleJumpMultiply;
                canDubbleJump = false;
            }
        }

        /*
        if (_controller.isGrounded && !keyWasPressed)
        {
            jumpTarget.transform.localRotation = Quaternion.Euler(new Vector3(1,0,0) * 20);
        }*/

        //set jump to false
        keyWasPressed = false;
        
        //Set the direction
        direction = (transform.forward * Input.GetAxis("Vertical"));// + (transform.right * Input.GetAxis("Horizontal"));
        direction = direction.normalized * _movespeed;

        //gravity
        _directionY -= _gravity * Time.deltaTime;
        direction.y = _directionY;

        //determine if the controller should be grounded or not
        slopeSteeperThanSlopeLimit = Vector3.Angle(Vector3.up, hitNormal) <= _controller.slopeLimit;

        //Then, before calling m_CharacterController.Move (in the FixedUpdate), add sideways speed to allow it go down:
        if (!slopeSteeperThanSlopeLimit)
        {
            direction.x += (1f - hitNormal.y) * hitNormal.x * (slideSpeed - slideFriction);
            direction.z += (1f - hitNormal.y) * hitNormal.z * (slideSpeed - slideFriction);
        }

        
        if (!Input.GetKey("t"))
        {   //Move
            _controller.Move(direction * Time.deltaTime * _movespeed);
            //Rotate
            transform.Rotate(0, (Input.GetAxis("Horizontal") * Time.deltaTime * _rotationSpeed), 0);
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
    }

    void RotateAim()
    {
        if (_controller.isGrounded)
        {
            Cursor.visible = false;
            float mouseX = Input.GetAxis("Mouse X") * sensitivityX * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;

            //Rotates the character left and right when swipe mouse.
            transform.Rotate(0, mouseX, 0);
            followTargetAim.transform.Rotate(-mouseY, 0, 0);
        }
    }

    private IEnumerator jumpToFalse()
    {
        yield return new WaitForSeconds(1.5f);
        isJumping = false;
    }

    private IEnumerator jumpEarlyCheck()
    {
        yield return new WaitForSeconds(0.5f);
        if (_controller.isGrounded)
        {
            isJumping = false;
        }
    }

    public void Hurt(int damage)
    {
        health -= damage;
        Debug.Log(health);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 11)
        {
            Destroy(other.gameObject);
            supplyControl.updateScore();
        }

        if(other.gameObject.layer == 12)
        {
            Destroy(other.gameObject);
            windGustControl.updateScore();
        }

        if (other.gameObject.layer == 13)
        {
            Destroy(other.gameObject);
            mountainControl.updateScore();
        }

        if(other.gameObject.layer == 14)
        {
            Destroy(other.gameObject);
            bloodControl.updateScore();
        }

        if (other.gameObject.layer == 15)
        {
            Destroy(other.gameObject);
            cherryControl.updateScore();
        }
    }
}
