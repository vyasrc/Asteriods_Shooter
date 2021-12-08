using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _playerThruster;
    [SerializeField]
    private float _fireRate = 0.3f;
    [SerializeField]
    private int _lives = 1;
    [SerializeField]
    private float _rotationSpeed = 720;
    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private int _score;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private AudioClip _laserSound;

    private AudioSource _audioSource;
    private UIManager _uiManager;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    private float _speedMultiplier = 2.0f;

    private SpawnManager _spawnManager;
    private float _canFire = -1f;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.Log("The Spawn Manager is null");
        }

        if (_uiManager == null)
        {
            Debug.Log("The UI Manager is null");
        }

        if (_audioSource == null)
        {
            Debug.Log("The Audio Source is null");
        }
        else
        {
            _audioSource.clip = _laserSound;
        }

    }

    // Update is called once per frame
    void Update()
    {

        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _isSpeedBoostActive = true;
            _playerThruster.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            _isSpeedBoostActive = false;
            _playerThruster.SetActive(false);
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -10.0f, 10.0f), Mathf.Clamp(transform.position.y, -3.8f, 3.8f), 0);

        Vector3 movementDirection = new Vector3(horizontalInput, verticalInput, 0);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);
        movementDirection.Normalize();

        if (_isSpeedBoostActive == false)
        {
            transform.Translate(movementDirection * _speed * inputMagnitude * Time.deltaTime, Space.World);
        }
        else
        {
            transform.Translate(movementDirection * _speed * _speedMultiplier * inputMagnitude * Time.deltaTime, Space.World);
        }
        

        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, transform.rotation);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position, transform.rotation);
        }

        _audioSource.Play();
        
    }

    public void Damage()
    {
        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives -= 1;
        
        if (_lives < 1)
        {
            _uiManager.GameOver(_lives);
            _spawnManager.OnPlayerDeath();
            _explosionPrefab.transform.localScale = new Vector3(0.4f, 0.4f, 0);
            Instantiate(_explosionPrefab, transform.position, transform.rotation);
            Destroy(this.gameObject);

        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
