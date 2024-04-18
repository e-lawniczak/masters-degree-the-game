using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float bulletSpeedOverwrite = -1f;
    private float bulletSpeed = RuntimeVariables.CannonBulletSpeed;
    [SerializeField] private float timeToLiveOverwrite = -1f;
    private float timeToLive = RuntimeVariables.CannonBulletLifetime;
    private Vector3 dir;
    private float speed;
    void Start()
    {
        speed = bulletSpeedOverwrite > 0 ? bulletSpeedOverwrite : bulletSpeed;
        timeToLive = timeToLiveOverwrite > 0 ? timeToLiveOverwrite : timeToLive;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += dir * Time.deltaTime * speed;
        timeToLive -= Time.deltaTime;
        if (timeToLive < 0)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject, 0.0f);
    }
    public void SetDirection(Vector3 v)
    {
        dir = v;
    }
    public void SetBulletSpeed(float v)
    {
        bulletSpeed = v;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.rigidbody && collision.rigidbody.name == LayerVariables.Player)
        {
            var pl = collision.collider.GetComponent<PlayerLogic>();
            pl.GetHitByEnemy(this.gameObject, this.transform.position);
        }
        Die();

    }

}
