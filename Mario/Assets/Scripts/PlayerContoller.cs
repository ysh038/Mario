using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerContoller : MonoBehaviour
{
    public float jumpVelocity;
    public float bouncingVelocity;
    public Vector2 velocity;

    private bool walk, walk_left, walk_right, jump;

    private Rigidbody2D playerRigidbody;

    public AudioSource audiosource;
    public AudioClip jumpSound;

    public enum PlayerState
    {
        jumping,
        idle,
        walking,
        bouncing
    }

    private PlayerState playerState = PlayerState.idle;
    private bool grounded = false;
    private bool bounce = false;
    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerInput();

        UpdatePlayerPosition();

        UpdateAnimationStates();
    }

    void UpdatePlayerPosition()
    {
        Vector3 pos = transform.localPosition;
        Vector3 scale = transform.localScale;

        if (walk)
        {
            if (walk_left)
            {
                pos.x -= velocity.x * Time.deltaTime;
                scale.x = -1;

                transform.localPosition = pos;
                transform.localScale = scale;
            }

            if (walk_right)
            {
                pos.x += velocity.x * Time.deltaTime;
                scale.x = 1;

                transform.localPosition = pos;
                transform.localScale = scale;
            }

        }

        if(jump && playerState != PlayerState.jumping)
        {
            playerState = PlayerState.jumping;

            grounded = false;

            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.AddForce(new Vector2(0, jumpVelocity));
            audiosource.Play();
        }

        if(bounce && playerState != PlayerState.bouncing)
        {
            playerState = PlayerState.bouncing;

            grounded = false;

            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.AddForce(new Vector2(0, bouncingVelocity));

            bounce = false;
            playerState = PlayerState.idle;
        }
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //Debug.Log("충돌중");

        if (collision.collider.tag != "Enemy")
        {
            if (collision.contacts[0].normal.y >= 0.9f)
            {
                grounded = true;

                playerState = PlayerState.idle;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("충돌함");
        if(collision.collider.tag == "QuestionBlock")
        {
            // *** 충돌지점 y좌표가 실제보다 0.0025 낮게나옴. 
            // *** 그래서 0.01정도 실제 충돌지점보다 여유를 둔 범위로 조건을 줌
            // !! *** 물음표 박스 뿐 아니라 다른 오브젝트도 그러하는지는 확인해야함.
            if (collision.contacts[0].point.y < collision.transform.localPosition.y - .5 && collision.contacts[0].point.y > collision.transform.localPosition.y - .51)
            {
                collision.collider.GetComponent<QuestionBlock>().QuestionBlockBounce();
            }
        }

        if (collision.collider.tag == "Enemy")
        {
            Debug.Log(collision.contacts[0].point.y + "\n" + collision.transform.localPosition.y);
            // 마리오가 커져버리면 이 부분 수정해야할듯
            // 얘는 차이가 좀 커서 0.02로 줬음
            if (collision.contacts[0].point.y > collision.transform.localPosition.y + .5 && collision.contacts[0].point.y < collision.transform.localPosition.y + .51)
            {
                bounce = true;

                collision.collider.GetComponent<EnemyAI>().Crush();
            }
            else
            {
                SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("벗어남");
        grounded = false;

        playerState = PlayerState.jumping;
    }

    void UpdateAnimationStates()
    {
        if(grounded && !walk && !bounce)
        {
            GetComponent<Animator>().SetBool("isJumping", false);
            GetComponent<Animator>().SetBool("isRunning", false);
        }

        if(grounded && walk)
        {
            GetComponent<Animator>().SetBool("isJumping", false);
            GetComponent<Animator>().SetBool("isRunning", true);
        }

        if(playerState == PlayerState.jumping)
        {
            GetComponent<Animator>().SetBool("isJumping", true);
            GetComponent<Animator>().SetBool("isRunning", false);
        }
    }

    void CheckPlayerInput()
    {
        bool input_left = Input.GetKey(KeyCode.LeftArrow);
        bool input_right = Input.GetKey(KeyCode.RightArrow);
        bool input_space = Input.GetKeyDown(KeyCode.Space);

        walk = input_left || input_right;

        walk_left = input_left && !input_right;

        walk_right = input_right && !input_left;

        jump = input_space;
    }
}