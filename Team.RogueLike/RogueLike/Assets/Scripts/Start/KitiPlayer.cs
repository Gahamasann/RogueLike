using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitiPlayer : MonoBehaviour
{
    public float speed;
    Vector3 velocity;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    //移動処理
    void Move()
    {
        velocity = Vector3.zero;
        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            if (transform.position.x <= 7.7)
            {
                velocity.x = 1;
            }
            else
            {
                velocity.x = 0;
            }
        }
        if (Input.GetAxisRaw("Horizontal") == -1)
        {
            if (transform.position.x >= -7.7)
            {
                velocity.x = -1;
            }
            else
            {
                velocity.x = 0;
            }
        }
        if (Input.GetAxisRaw("Vertical") == 1)
        {
            if (transform.position.y <= 9.2)
            {
                velocity.y = 1;
            }
            else
            {
                velocity.y = 0;
            }
        }
        if (Input.GetAxisRaw("Vertical") == -1)
        {
            if (transform.position.y >= -9.2)
            {
                velocity.y = -1;
            }
            else
            {
                velocity.y = 0;
            }
        }

        velocity.Normalize();//正規化
        transform.position += velocity * speed * Time.deltaTime;
    }
}
