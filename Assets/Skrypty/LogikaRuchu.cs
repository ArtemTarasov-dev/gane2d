using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogikaRuchu : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Wartość prędkości gracza")]
    [Range(0.0f, 4.0f)]
    public float predkoscRuchu = 3.0f;

    [SerializeField]
    Vector2 ruch = new Vector2();

    Animator animator;

    string stanyAnimacji = "StanyAnimacji";
    Rigidbody2D rb2D;

    enum Stany
    {
        left = 1,
        down = 4,
        right = 3,
        up = 2,
        stop = 5,
        die = 6
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        UpdateState();
    }

    void FixedUpdate()
    {
        RuchPostaci();
    }

    private void RuchPostaci()
    {
        ruch.x = Input.GetAxisRaw("Horizontal");
        ruch.y = Input.GetAxisRaw("Vertical");
        ruch.Normalize();
        rb2D.velocity = ruch * predkoscRuchu;
    }

    private void UpdateState()
    {
        if (ruch.x > 0)
        {
            animator.SetInteger(stanyAnimacji, (int)Stany.right);
        }
        else if (ruch.x < 0)
        {
            animator.SetInteger(stanyAnimacji, (int)Stany.left);
        }
        else if (ruch.y > 0)
        {
            animator.SetInteger(stanyAnimacji, (int)Stany.up);
        }
        else if (ruch.y < 0)
        {
            animator.SetInteger(stanyAnimacji, (int)Stany.down);
        }
        else
        {
            animator.SetInteger(stanyAnimacji, (int)Stany.stop);
        }
    }

    public void Die()
    {
        animator.SetTrigger("die");
    }
}

