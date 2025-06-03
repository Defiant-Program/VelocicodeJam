using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform triangleMarker;

    [SerializeField] Vector3 moveDirection;
    [SerializeField] float moveSpeed = 5f;

    float x;
    float y;

    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(x, y).normalized;
    }

    private void FixedUpdate()
    {
        rb.velocity = moveDirection * moveSpeed;

        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            sr.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
}
