using UnityEngine;
using SangoCommon.Constants;

//Developer : SangonomiyaSakunovi
//Discription: Player Capsulr.

public class MovePlayerCapsuleController : MonoBehaviour
{
    private float movementSpeed = 8f;
    private float jumpSpeed = 20f;
    private float runMultiplier = 15f;
    private float gravity = -50f;

    private float currentTime;
    Vector3 velocity;
    private CharacterController characterController;
    //The Attribute of Avater
    private Vector3 lastPosition = Vector3.zero;
    private Quaternion lastRotation = new Quaternion(0, 0, 0, 0);
    private float moveOffsetLimit = 0.2f;
    private float angleLimit = 30f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        currentTime = 0;
    }

    private void Update()
    {
        SetMove();
        SetJump();
        SyncPlayerPosition();
    }

    private void SyncPlayerPosition()
    {
        if (currentTime > TimeConstant.SyncPlayerTransformTime)
        {
            lastPosition = transform.position;
            lastRotation = transform.rotation;
            CacheSystem.Instance.syncPlayerTransformRequest.SetPlayerTransform(transform.position, transform.rotation);            
            currentTime = 0;
        }
        currentTime += Time.deltaTime;
    }

    private void SetMove()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(horizontal, 0, vertical);
        dir = Quaternion.Euler(0, CameraController.Instance.transform.rotation.eulerAngles.y, 0) * dir;
        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(dir);
            Vector3 movement = transform.forward;
            characterController.Move(movement * movementSpeed * Time.deltaTime);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                characterController.Move(movement * runMultiplier * Time.deltaTime);
            }
        }
    }

    private void SetJump()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
        if (Input.GetButton("Jump") && characterController.isGrounded)
        {
            velocity.y += jumpSpeed;
        }
    }
}
