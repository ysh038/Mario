using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    //public float gravity;
    public Vector2 velocity;
    //public float walkSpeed;
    public bool isWalkingLeft = true;

    //public LayerMask floorMask;
    //public LayerMask wallMask;

    //private bool grounded = false;

    private bool shouldDie = false;
    private float deathTimer = 0;

    public float timeBeforeDestroy = 1.0f;

    public AudioSource audioSource;
    public GameObject player;

    private enum EnemyState
    {
        walking,
        falling,
        dead
    }

    private EnemyState state = EnemyState.falling;

    // Start is called before the first frame update
    void Start()
    {
        velocity = Vector2.zero;
        audioSource = GetComponent<AudioSource>();
        isWalkingLeft = true;
    }

    // Update is called once per frame
    void Update()
    {
        OnBecameVisible();

        UpdateEnemyPosition();

        CheckCrushed();
    }


    public void Crush()
    {
        audioSource.Play();
        state = EnemyState.dead;
        GetComponent<Animator>().SetBool("isCrushed", true);

        GetComponent<Collider2D>().enabled = false;

        shouldDie = true;
    }

    void CheckCrushed()
    {
        if (shouldDie)
        {
            if(deathTimer <= timeBeforeDestroy)
            {
                deathTimer += Time.deltaTime;
            }
            else
            {
                shouldDie = false;

                Destroy(this.gameObject);
            }
        }
    }

    void UpdateEnemyPosition()
    {
        if(state != EnemyState.dead)
        {
            Vector3 pos = transform.localPosition;
            Vector3 scale = transform.localScale;

            if (state == EnemyState.walking)
            {
                if (isWalkingLeft)
                {
                    pos.x -= velocity.x * Time.deltaTime;

                    scale.x = -1;
                }
                else
                {
                    pos.x += velocity.x * Time.deltaTime;

                    scale.x = 1;
                }
            }
            transform.localPosition = pos;
            transform.localScale = scale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag != "Player" && collision.collider.tag != "Ground")
        {
            Debug.Log("d");
            if (isWalkingLeft)
            {
                isWalkingLeft = false;
            }
            else
            {
                isWalkingLeft = true;
            }
        }
        /*
        if (collision.collider.tag == "Player")
        {
            Debug.Log(collision.contacts[0].point.y+"\n"+ collision.transform.localPosition.y);
            // 마리오가 커져버리면 이분 수정해야할듯
            // 얘는 차이가 좀 커서 0.02로 줬음
            if (collision.contacts[0].point.y < collision.transform.localPosition.y - 1 && collision.contacts[0].point.y > collision.transform.localPosition.y - 1.02)
            {
                Crush();
            }
            else
            {
                SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
            }
        }*/
    }

    /*   Vector3 CheckGround (Vector3 pos)
       {
           Vector2 originLeft = new Vector2(pos.x - 0.5f + 0.2f, pos.y - .5f);
           Vector2 originMiddle = new Vector2(pos.x, pos.y - .5f);
           Vector2 originRight = new Vector2(pos.x + 0.5f - 0.2f, pos.y - .5f);

           RaycastHit2D groundLeft = Physics2D.Raycast(originLeft, Vector2.down, velocity.y * Time.deltaTime, floorMask);
           RaycastHit2D groundMiddle = Physics2D.Raycast(originMiddle, Vector2.down, velocity.y * Time.deltaTime, floorMask);
           RaycastHit2D groundRight = Physics2D.Raycast(originRight, Vector2.down, velocity.y * Time.deltaTime, floorMask);

           if(groundLeft.collider != null || groundMiddle.collider != null || groundRight.collider != null)
           {
               RaycastHit2D hitRay = groundLeft;

               if (groundLeft)
               {
                   hitRay = groundLeft;
               }
               else if(groundMiddle){
                   hitRay = groundMiddle;
               }
               else if (groundRight)
               {
                   hitRay = groundRight;
               }

               if(hitRay.collider.tag == "Player")
               {
                   Debug.Log("ddd");
                   SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
               }

               pos.y = hitRay.collider.bounds.center.y + hitRay.collider.bounds.size.y / 2 + .5f;

               grounded = false;

               velocity.y = 0;

               state = EnemyState.walking;
           }
           else
           {
               if(state != EnemyState.falling)
               {
                   Fall();
               }
           }

           return pos;

       }
   */
    /*
       void CheckWalls(Vector3 pos, float direction)
       {
           Vector2 originTop = new Vector2(pos.x + direction * 0.4f, pos.y + .5f - 0.2f);
           Vector2 originMiddle = new Vector2(pos.x + direction * 0.4f, pos.y);
           Vector2 originBottom = new Vector2(pos.x + direction * 0.4f, pos.y - .5f + 0.2f);

           RaycastHit2D wallTop = Physics2D.Raycast(originTop, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);
           RaycastHit2D wallMiddle = Physics2D.Raycast(originMiddle, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);
           RaycastHit2D wallBottom = Physics2D.Raycast(originBottom, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);

           if(wallTop.collider != null || wallMiddle.collider != null || wallBottom.collider != null)
           {
               RaycastHit2D hitRay = wallTop;

               if (wallTop)
               {
                   hitRay = wallTop;
               }
               else if (wallMiddle)
               {
                   hitRay = wallMiddle;
               }
               else if (wallBottom)
               {
                   hitRay = wallBottom;
               }

               if (hitRay.collider.tag == "Player")
               {
                   SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
               }

               isWalkingLeft = !isWalkingLeft;

           }
       }
    */
    private void OnBecameVisible()
    {
        // 물음표 상자와 마찬가지로 범위 설정해야 작동함
        if (player.transform.localPosition.x >= gameObject.transform.localPosition.x - 15 &&
            player.transform.localPosition.x < gameObject.transform.localPosition.x - 14)
        {
            velocity = new Vector2(2, 0);
            state = EnemyState.walking;
        }
    }

    /*void Fall()
    {
        velocity.y = 0;

        state = EnemyState.falling;

        grounded = false;
    }
    */
}
