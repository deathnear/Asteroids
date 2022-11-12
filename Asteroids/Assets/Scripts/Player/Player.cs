﻿using System;
using DefaultNamespace.Audio;
using Enemies;
using UnityEngine;
using WeaponSystem;

[RequireComponent(typeof(PlayerWeapon))]
[RequireComponent(typeof(Invulnerability))]
[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    public event Action Died;
    
    [SerializeField] private ExplosionSFX _explosionSfx;
    [SerializeField] private AudioClip _soundDeath;
    [SerializeField] private float _rotatePerSecond;
    [SerializeField] private float _speedPerSecond;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private AudioSource _acclerationSoundSource;
    
    public Vector3 _acceleration;
    private Weapon _playerWeapon;
    private Invulnerability _invulnerability;

    private void Awake()
    {
        _playerWeapon = GetComponent<PlayerWeapon>();
        _invulnerability = GetComponent<Invulnerability>();
    }

    private void Start()
    {
        _invulnerability.Activate();
    }
    
    public void Accelerate()
    {
        _acceleration = _acceleration + transform.up * (_speedPerSecond * Time.deltaTime);

        _acceleration = Vector2.ClampMagnitude(_acceleration, _maxSpeed);

        if (!_acclerationSoundSource.isPlaying)
        {
            _acclerationSoundSource.Play();
        }
    }

    public void SlowDown()
    {
        _acceleration = _acceleration - _acceleration.normalized * _speedPerSecond * (Time.deltaTime / 15f);
    }

    public void Shoot()
    {
        if (_playerWeapon.CanShoot())
        {
            _playerWeapon.Shoot(transform.up);
        }
    }

    public void Move()
    {
        transform.position += _acceleration * Time.deltaTime;
    }

    public void RotateAt(float direction)
    {
        transform.Rotate(Vector3.forward, _rotatePerSecond * Time.deltaTime * direction, Space.World);
    }

    public void LookAt(Vector2 position)
    {
        Vector2 directionLook = (position - (Vector2)transform.position).normalized;

        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, directionLook);
            
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, _rotatePerSecond * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out UfoBullet ufoBullet) ||
            other.gameObject.TryGetComponent(out Asteroid asteroid))
        {
            Destroy();
        }
    }

    private void Destroy()
    {
        Instantiate(_explosionSfx).Init(_soundDeath);
        
        Died?.Invoke();
        
        Destroy(gameObject);
    }
}
