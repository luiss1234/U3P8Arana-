using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody playerRb;
    public float jumpForce = 10;
    public float floatForce;
    
    public float gravityModifier;
    public bool isOnGround = true;
    public bool gameOver = false;

    private Animator playerAnim;

    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;

    public AudioClip jumpSound;
    public AudioClip crashSound;

    private AudioSource playerAudio;
    public bool canDoubleJump = false;


    public float dash = 2;
    public bool isDashing = false;

    public bool doubleSpeed = false;





    // Start is called before the first frame update
    void Start()
    {
        playerAudio = GetComponent<AudioSource>();


        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        playerAnim = GetComponent<Animator>();

        playerAudio.PlayOneShot(jumpSound, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            doubleSpeed = true;
            playerAnim.SetFloat("Speed_Multiplier", 2.0f);
        }

        else if (doubleSpeed)
        {
            doubleSpeed = false;
            playerAnim.SetFloat("Speed_Muultiplier", 1.0f);
        }





        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver)
        {
            playerRb.AddForce(Vector3.up * floatForce * Time.deltaTime, ForceMode.Impulse);
            isOnGround = false;
            playerAnim.SetTrigger("Jump_trig");
            dirtParticle.Stop();
        }

        if (Input.GetKeyDown(KeyCode.Space) && playerRb.velocity.y > 0f && canDoubleJump && !gameOver)
        {
            canDoubleJump = false;

            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);


            playerAnim.SetTrigger("Jump_trig");
            playerAudio.PlayOneShot(jumpSound, 1);


        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        

        if(collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            dirtParticle.Play();
        } else if(collision.gameObject.CompareTag("Obstacles"))
        {
            Debug.Log("Game Over");
            gameOver = true;

            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            explosionParticle.Play();
            dirtParticle.Stop();

            playerAudio.PlayOneShot(crashSound, 1.0f);
        }
    }

}
