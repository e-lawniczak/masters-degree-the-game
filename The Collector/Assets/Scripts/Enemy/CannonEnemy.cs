using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonEnemy : MonoBehaviour
{
    [SerializeField] private float bulletSpeedOverwrite = -1;
    private float bulletSpeed = RuntimeVariables.CannonBulletSpeed;
    [SerializeField] private float shootDelayOverwrite = -1;
    private float shootDelay = RuntimeVariables.CannonBulletReload;
    [SerializeField] private float activationRadius = 25f;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private GameObject player;
    [SerializeField] private bool rightCannon;
    [SerializeField] private bool leftCannon;
    [SerializeField] private bool upCannon;
    [SerializeField] private bool downCannon;
    [SerializeField] private float initialDelay;


    private Vector2 playerPos;
    private bool isActive;
    private bool canShoot;
    private float counter = 0f;
    private float delayCounter = 0f;
    private float speed;
    private float reload;


    private void Start()
    {
        speed = bulletSpeedOverwrite > 0 ? bulletSpeedOverwrite : bulletSpeed;
        reload = shootDelayOverwrite > 0 ? shootDelayOverwrite : shootDelay;
        if (player != null)
        {
            playerPos = player.GetComponent<Transform>().transform.position;

        }
        isActive = false;
        canShoot = true;
    }

    private void Update()
    {
        CheckForPlayer();
        if (isActive)
        {
            delayCounter -= Time.deltaTime;
            if (delayCounter < 0)
            {
                Fire();
            }
        }
    }
    void Fire()
    {
        if (canShoot)
        {
            // fire
            canShoot = false;
            GameObject b = Instantiate(bullet, bulletSpawn.transform.position, transform.rotation);
            b.GetComponent<BulletScript>().SetDirection(PickDirectionVector());
            b.GetComponent<BulletScript>().SetBulletSpeed(speed);
            b.SetActive(true);
        }
        else
        {
            counter += Time.deltaTime;
            if (counter > reload)
            {
                canShoot = true;
                counter = 0f;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.rigidbody && collision.rigidbody.name == LayerVariables.Player)
        {
            var pl = collision.collider.GetComponent<PlayerLogic>();

            pl.GetHitByEnemy(this.gameObject, this.transform.position);
        }
    }
    void CheckForPlayer()
    {
        if (!player)
        {
            isActive = true;
            return;
        }
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
