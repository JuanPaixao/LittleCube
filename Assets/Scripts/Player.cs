using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _movHor, _movVer;
    public float moveTimes, speed;
    public bool canMove, left, right, up, down, end, isMoving;
    private Rigidbody2D _rb;
    [SerializeField] private string _direction;
    private RaycastHit2D _rayLeft, _rayRight, _rayUp, _rayDown;
    [SerializeField] private LayerMask _layerMask;
    private SpriteRenderer _spriteRenderer;
    private Player_Animations _playerAnimations;
    void Start()
    {
        _playerAnimations = GetComponent<Player_Animations>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = Vector2.zero;
    }
    void Update()
    {
        _movHor = Input.GetAxisRaw("Horizontal");
        _movVer = Input.GetAxisRaw("Vertical");
        _spriteRenderer.flipX = (_direction == "movingLeft") ? true : false;
        if (!end)
        {
            Check();
            Movement();
        }
        else
        {
            _playerAnimations.isVictorious();
        }
        if (!isMoving)
        {
            StartCoroutine(CheckGameOver());
        }
    }

    private void Check()
    {
        left = Physics2D.Raycast(this.transform.position, Vector2.left, 0.53525f, _layerMask);
        right = Physics2D.Raycast(this.transform.position, Vector2.right, 0.53525f, _layerMask);
        up = Physics2D.Raycast(this.transform.position, Vector2.up, 0.535f, _layerMask);
        down = Physics2D.Raycast(this.transform.position, Vector2.down, 0.535f, _layerMask);
        Debug.Log(_rb.velocity);
        Debug.DrawRay(this.transform.position, Vector2.left, Color.cyan, 1f);
        Debug.DrawRay(this.transform.position, Vector2.right, Color.cyan, 1f);
        Debug.DrawRay(this.transform.position, Vector2.up, Color.cyan, 1f);
        Debug.DrawRay(this.transform.position, Vector2.down, Color.cyan, 1f);

    }

    private void Movement()
    {
        if (canMove && !isMoving)
        {
            if (!left)
            {
                if (_movHor <= -0.5f && _movVer == 0)
                {
                    _direction = "movingLeft";
                    StartCoroutine(MoveLeftCoroutine());
                }
            }

            if (!right)
            {
                if (_movHor >= 0.5f && _movVer == 0)
                {
                    _direction = "movingRight";
                    StartCoroutine(MoveRightCoroutine());
                }
            }

            if (!down)
            {
                if (_movVer <= -0.5f && _movHor == 0)
                {
                    _direction = "movingDown";
                    StartCoroutine(MoveDownCoroutine());
                }
            }

            if (!up)
            {
                if (_movVer >= 0.5f && _movHor == 0)
                {
                    _direction = "movingUp";
                    StartCoroutine(MoveUpCoroutine());
                }
            }
        }

    }

    private IEnumerator MoveRightCoroutine()
    {
        while (_direction == "movingRight" && !right)
        {
            _rb.transform.Translate(Vector2.right * speed);
            yield return new WaitForSeconds(0.01f);
            canMove = false;
            isMoving = true;
        }
        isMoving = false;
    }
    private IEnumerator MoveLeftCoroutine()
    {
        while (_direction == "movingLeft" && !left)
        {
            _rb.transform.Translate(Vector2.left * speed);
            yield return new WaitForSeconds(0.01f);
            canMove = false;
            isMoving = true;
        }
        isMoving = false;
    }
    private IEnumerator MoveUpCoroutine()
    {
        while (_direction == "movingUp" && !up)
        {
            _rb.transform.Translate(Vector2.up * speed / 1.5f);
            yield return new WaitForSeconds(0.01f);
            canMove = false;
            isMoving = true;
        }
        isMoving = false;
    }
    private IEnumerator MoveDownCoroutine()
    {
        while (_direction == "movingDown" && !down)
        {
            _rb.transform.Translate(Vector2.down * speed / 1.5f);
            yield return new WaitForSeconds(0.01f);
            canMove = false;
            isMoving = true;
        }
        isMoving = false;
    }
    private IEnumerator CheckGameOver()
    {
        while (!isMoving)
        {
            if (moveTimes <= 0 && !end)
            {
                _playerAnimations.isDefeated();
            }

            yield return new WaitForSeconds(0.2f);
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Trigger"))
        {
            if (moveTimes > 0)
            {
                canMove = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Trigger"))
        {
            moveTimes--;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        isMoving = false;
        if (other.gameObject.name == "End")
        {
            end = true;
            canMove = false;
            _direction = null;
        }
    }
}

