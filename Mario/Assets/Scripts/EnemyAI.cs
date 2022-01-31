using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    public Vector2 velocity;
    public bool isWalkingLeft = true;

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
        player = GameObject.Find("Player");
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
    }
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
}
