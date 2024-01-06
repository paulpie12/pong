using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallMovement : MonoBehaviour
{
    [SerializeField] private float InitalSpeed = 10;
    [SerializeField] private float SpeedIncrease = 0.25f;
    [SerializeField] private Text PlayerScore;
    [SerializeField] private Text AIScore;

    private int hitCounter;
    private Rigidbody2D rb;
    Renderer Ren;
    void Start()
    {
        Ren = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody2D>();
        Invoke("StartBall", 2f);
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, InitalSpeed + (SpeedIncrease * hitCounter));
    }

    private void StartBall()
    {
        rb.velocity = new Vector2(-1, 0) * (InitalSpeed + SpeedIncrease * hitCounter);
    }

    private void ResetBall()
    {
        Ren.material.color = Color.white;
        rb.velocity = new Vector2(0, 0);
        transform.position = new Vector2(0, 0);
        hitCounter = 0;
        Invoke("StartBall", 2f);

    }
    private void PlayerBounce(Transform myObject)
    {
        hitCounter++;
        Vector2 ballpos = transform.position;
        Vector2 playerpos = myObject.position;

        float xDirection, yDirection;
        if(transform.position.x > 0)
        {
            xDirection = -1;
        }
        else
        {
            xDirection = 1;
        }
        yDirection = (ballpos.y - playerpos.y) / myObject.GetComponent<Collider2D>().bounds.size.y;
        if(yDirection == 0)
        {
            yDirection = 0.25f;
        }
        rb.velocity = new Vector2(xDirection, yDirection) * (InitalSpeed + (SpeedIncrease * hitCounter));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player" || collision.gameObject.name == "AI")
        {
            if (collision.gameObject.name == "PlayerGoal")
            {
                ParticleSystem ps = GameObject.Find("Explosion").GetComponent<ParticleSystem>();
                ps.transform.Rotate(0, 0, 0);
                ps.Play();
                Debug.Log("hit");
            }
            else if (collision.gameObject.name == "AiGoal")
            {
                ParticleSystem ps = GameObject.Find("Explosion").GetComponent<ParticleSystem>();
                ps.transform.Rotate(0, 0, -180);
                ps.Play();
            }
            PlayerBounce(collision.transform);
            //color change
            Color randomcolor = new Color(Random.value, Random.value, Random.value);
            Ren.material.color = randomcolor;
        }

 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.position.x > 0)
        {
            //ParticleSystem ps = GameObject.Find("Explosion").GetComponent<ParticleSystem>();
            //ps.transform.Rotate(0, 0, 0);
            //ps.Play();
            ResetBall();
            PlayerScore.text = (int.Parse(PlayerScore.text) + 1).ToString();
        }
        else if (transform.position.x < 0)
        {
            //ParticleSystem ps = GameObject.Find("Explosion").GetComponent<ParticleSystem>();
            //ps.transform.Rotate(0, 0, -180);
           // ps.Play();
            ResetBall();
            AIScore.text = (int.Parse(AIScore.text) + 1).ToString();
        }
    }

}
