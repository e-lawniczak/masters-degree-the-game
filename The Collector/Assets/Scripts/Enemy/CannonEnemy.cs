using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonEnemy : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float shootDelay = 2f;
    [SerializeField] private float activationRadius = 25f;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private GameObject player;
    [SerializeField] private bool rightCannon;
    [SerializeField] private bool leftCannon;
    [SerializeField] private bool upCannon;
    [SerializeField] private bool downCannon;


    private Vector2 playerPos;
    private bool isActive;
    private bool canShoot;
    private float counter = 0f;


    private void Start()
    {
        playerPos = player.GetComponent<Transform>().transform.position;
        isActive = false;
        canShoot = true;
    }

    private void Update()
    {
        CheckForPlayer();
        if (isActive)
        {
            Fire();
        }
    }
    void Fire()
    {
        if (canShoot)
        {
            // fire
            canShoot = false;
            Debug.Log(bulletSpawn.transform.position);
            GameObject b = Instantiate(bullet, bulletSpawn.transform.position, transform.rotation);
            b.GetComponent<BulletScript>().SetDirection(PickDirectionVector());
            b.GetComponent<BulletScript>().SetBulletSpeed(bulletSpeed);
        }
        else
        {
            counter += Time.deltaTime;
            if (counter > shootDelay)
            {
                canShoot = true;
                counter = 0f;
            }
        }
    }
    void CheckForPlayer()
    {
        playerPos = player.GetComponent<Transform>().transform.position;
        float dist = Vector2.Distance(transform.position, playerPos);
        isActive = dist < activationRadius;
    }
    Vector3 PickDirectionVector()
    {
        if (leftCannon)
            return Vector3.left;
        if (upCannon)
            return Vector3.up;
        if (downCannon)
            return Vector3.down;
        return Vector3.right;
    }
    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position, activationRadius);
    //}
}
