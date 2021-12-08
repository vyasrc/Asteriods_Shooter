using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private int _lives = 1;
    [SerializeField]
    private Sprite newSprite2;
    [SerializeField]
    private Sprite newSprite3;
    [SerializeField]
    private Sprite newSprite4;
    [SerializeField]
    private Sprite newSprite5;
    [SerializeField]
    private float _reflect_speed = 1.0f;
    [SerializeField]
    private float _collision_speed = 2.0f;
    [SerializeField]
    private GameObject _explosionPrefab;

    private bool _start = true;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Player _player;
    


    // Start is called before the first frame update
    void Start()
    {
        _rb = transform.GetComponent<Rigidbody2D>();
        _spriteRenderer = transform.GetComponent<SpriteRenderer>();
        _player = GameObject.Find("Player").GetComponent<Player>();

    }
    // Update is called once per frame
    void Update()
    {   
        
        if (_start)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }

        if (transform.position.y < -3.8f)
        {
            _start = false;
            Vector3 pushEnemy = new Vector3(Random.Range(-5f, 5f), Random.Range(0, 5f), 0f);
            _rb.AddForce(pushEnemy * _reflect_speed);

        }
        else if (transform.position.y > 3.8f)
        {
            _start = false;
            Vector3 pushEnemy = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 0f), 0f);
            _rb.AddForce(pushEnemy * _reflect_speed);
        }
        else if (transform.position.x < -10.0f)
        {
            _start = false;
            Vector3 pushEnemy = new Vector3(Random.Range(0f, 5f), Random.Range(-5f, 5f), 0f);
            _rb.AddForce(pushEnemy * _reflect_speed);
        }
        else if (transform.position.x > 10.0f)
        {
            _start = false;
            Vector3 pushEnemy = new Vector3(Random.Range(-5f, 0f), Random.Range(-5f, 5f), 0f);
            _rb.AddForce(pushEnemy * _reflect_speed);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (_player != null)
            {
                _player.Damage();
            }
            Destroy(this.gameObject);
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            _lives += 1;
            switch (_lives)
            {
                case 2:
                    ChangeAsteriodBehaviour(newSprite2);
                    break;
                case 3:
                    ChangeAsteriodBehaviour(newSprite3);
                    break;
                case 4:
                    ChangeAsteriodBehaviour(newSprite4);
                    break;
                case 5:
                    ChangeAsteriodBehaviour(newSprite5);
                    break;
                default:
                    ChangeScore(50);
                    _explosionPrefab.transform.localScale = new Vector3(0.3f, 0.3f, 0);
                    Instantiate(_explosionPrefab, transform.position, transform.rotation);
                    Destroy(this.gameObject);
                    break;

            }

            
            ChangeDirection(_collision_speed, _rb.velocity.normalized);
        }

        if (other.tag == "Enemy")
        {
            ChangeDirection(_collision_speed, _rb.velocity.normalized);

        }


    }

    void ChangeAsteriodBehaviour(Sprite newSprite)
    {
        _spriteRenderer.sprite = newSprite;
        transform.localScale = new Vector3(0.3f, 0.3f, 0);
        ChangeScore(10);
    }

    void ChangeScore(int points)
    {
        if (_player != null)
        {
            _player.AddScore(points);
        }
    }

    void ChangeDirection(float speed, Vector2 direction)
    {
        
        //Debug.Log(direction);
        if (direction != Vector2.zero)
        {
            _rb.velocity = new Vector2(direction.x * -1, direction.y * -1) * speed;
        }
        else
        {
            _rb.velocity = new Vector2(direction.x + (Random.Range(-1f, 1f)), direction.y + (Random.Range(-1f, 1f))) * speed;
        }
        
    }

}
