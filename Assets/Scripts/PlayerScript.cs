using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;

    public Text score;
    public Text winText;
    public Text livesText;

    private int scoreValue = 0;
    private int level = 1;
    private int lives = 3;

    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioClip coin;
    public AudioClip damage;
    public AudioSource musicSource;
    public AudioSource soundEffects;

    Animator anim;
    private bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = scoreValue.ToString();
        SetLivesText();
        winText.text = "";
        musicSource.clip = musicClipOne;
        musicSource.Play();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            anim.SetInteger("State", 1);

        if((Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)) && anim.GetInteger("State") != 2)
            anim.SetInteger("State", 0);

        if (Input.GetKeyDown(KeyCode.W))
            anim.SetInteger("State", 2);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            soundEffects.clip = coin;
            soundEffects.Play();
            if (scoreValue == 4)
            {
                if(level == 1)
                {
                    scoreValue = 0;
                    score.text = scoreValue.ToString();
                    transform.position = new Vector2(42.84f, 2f);
                    level = 2;
                    lives = 3;
                    SetLivesText();
                }
                else
                {
                    winText.text = "You Win! Game created by Chris Tillis.";
                    musicSource.clip = musicClipTwo;
                    musicSource.Play();
                }
            }
        }

        if (collision.collider.tag == "Enemy")
        {
            collision.gameObject.SetActive(false);
            lives = lives - 1;
            soundEffects.clip = damage;
            soundEffects.Play();
            SetLivesText();
            
        }

        if (collision.collider.tag == "Ground")
            anim.SetInteger("State", 0);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
                
            }

            
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Trampoline"))
            rd2d.AddForce(new Vector2(0, 16), ForceMode2D.Impulse);

        if (other.gameObject.CompareTag("Key"))
            other.gameObject.SetActive(false);
    }

    void SetLivesText()
    {
        livesText.text = "Lives: " + lives.ToString();
        if (lives <= 0)
        {
            winText.text = "You Lose!";
            this.gameObject.SetActive(false);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}