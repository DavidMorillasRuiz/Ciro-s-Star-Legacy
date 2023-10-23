using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField] private float upForce = 1000f; 
    [SerializeField] public float force = 10f;
    [SerializeField] bool canJump;
    public float jumpCooldown;

    [Header("Attack")]
    bool canAttack = true;
    [SerializeField] float attackInterval;

    [Header("Dash")]
    [SerializeField] bool canDash;
    public float dashCooldown;
    [SerializeField] private float dashForce = 500f;

    [Header("PlayerController")]
    private Rigidbody rb;
    private PlayerInput playerInput;
    private Vector2 input;
    private Vector3 _input;
    [SerializeField] float turnSpeed = 360;
    [SerializeField] Transform orientation;

    [Header("LifePlayer")]
    public float life = 50;
    public float lifeMax = 50;
    [SerializeField] Image barraLife;

    [Header("Ground Check")]
    public float playerHeight;
    public float groundDrag;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] bool isGrounded;

    [Header("Effect")]
    [SerializeField] ParticleSystem dashParticle;
    [SerializeField] ParticleSystem attackParticle;
    [SerializeField] float attackeffectColdown;

     private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        canJump = true;
        canDash = true;
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GatherInput();
        Look();
        GroundCheck();
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        input = playerInput.actions["Move"].ReadValue<Vector2>().normalized;
        anim.SetFloat("Movimiento", Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        ActualizarLife();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        float velX = input.x * force;
        float velY = input.y * force;
        rb.velocity = new Vector3(velX, rb.velocity.y, velY);
    }

    public void Attack(InputAction.CallbackContext callbackContext)
    {
        if(callbackContext.performed && canAttack)
        {
            AudioManager.Instance.PlaySFX(0);
            attackParticle.Play();
            anim.SetTrigger("Attack");
            canAttack = false;
            StartCoroutine(ResetAttack());
            Debug.Log("Atacando");
        }
    }


    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackeffectColdown);
        attackParticle.Stop();
        yield return new WaitForSeconds(attackInterval);
        canAttack = true;
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if(callbackContext.performed && isGrounded && canJump)
        {
            AudioManager.Instance.PlaySFX(3);
            canJump = false;
            rb.AddForce(transform.up * upForce);
            Invoke("ResetJump", jumpCooldown);
            Debug.Log("Saltando");
        }
     
    }

    public void Dash(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && canDash)
        {
            AudioManager.Instance.PlaySFX(1);
            dashParticle.Play();
            canDash = false;
            rb.AddForce(transform.forward.normalized * dashForce);
            Invoke("ResetDash", dashCooldown);
            Debug.Log("Dash");
        }
            
    }


    void GroundCheck()
    {
        //Esta tocando el suelo cuando: Se lanza un rayo(origen del rayo, direcion del rayo, longitud del rayo, capa que toca el rayo)
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight, groundLayer);
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    void ResetJump()
    {
        canJump = true;
    }

    void ResetDash()
    {
        canDash = true;
        dashParticle.Stop();
    }


    void GatherInput()
    {
        _input = new Vector3 (input.x, 0, input.y);
    }
    void Look()
    {
      if(_input != Vector3.zero)
      {
            var relative = (transform.position + _input) - transform.position;
            var rot = Quaternion.LookRotation(relative, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * Time.deltaTime);
               
      }
        
    }


    public void damagePlayer(float hit)
    {
        life -= hit;
        anim.SetTrigger("Damage");
        AudioManager.Instance.PlaySFX(2);
    }

    void ActualizarLife()
    {
        barraLife.fillAmount = life / lifeMax;

    }
    public void RecibirCura(float cura)
    {
        life += cura;

        if (life > lifeMax)
        {
            life = lifeMax;
        }
    }

    public void ControlsChanged(PlayerInput playerInput)
    {
        Debug.Log("Cambio de Dispositivo:" + playerInput.currentControlScheme);
    }
}
