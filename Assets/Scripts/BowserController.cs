using UnityEngine;

public class BowserController : MonoBehaviour
{
    public GameObject firePrefab;
    public Transform fireSpawnPoint;
    public float fireSpeed = 5f;

    public Transform player;

    public float fireRate = 2f;

    private float nextFireTime = 0f;

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            ShootFire();
            nextFireTime = Time.time + fireRate;
        }

        // 플레이어가 왼쪽 => scale의 x값은 양수
        if (transform.position.x > player.position.x)
        {
            Vector3 scale = transform.localScale;
            if (scale.x < 0)
            {
                scale = new Vector3(-scale.x, scale.y, scale.z);
                transform.localScale = scale;
            }
        }
        else // 플레이어가 오른쪽 => scale의 x값은 음수
        {
            Vector3 scale = transform.localScale;
            if (scale.x > 0)
            {
                scale = new Vector3(-scale.x, scale.y, scale.z);
                transform.localScale = scale;
            }
        }
    }

    void ShootFire()
    {
        GameObject fire = Instantiate(firePrefab, fireSpawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = fire.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            if (transform.localScale.x > 0)
            {
                rb.linearVelocity = new Vector2(-fireSpeed, 0);
            }
            else
            {
                rb.linearVelocity = new Vector2(fireSpeed, 0);
            }
        }

        //Destroy(fire, 5f);
    }
}
