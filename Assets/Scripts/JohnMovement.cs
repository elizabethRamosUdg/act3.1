using UnityEngine;

public class JohnMovement : MonoBehaviour
{
    public GameObject BulletPrefab;
    public float JumpForce;
    public float Speed;
    public bool Grounded;

    private Rigidbody2D Rigidbody2D;
    private Animator Animator;
    private float Horizontal;
    // Time before last shoot
    private float LastShoot;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");

        // Direction of John
        if (Horizontal < 0.0f) transform.localScale = new Vector3(-1.0f, 1f, 1f);
        else if (Horizontal > 0.0f) transform.localScale = new Vector3(1.0f, 1f, 1f);

        // Animation
        Animator.SetBool("running", Horizontal != 0.0f);

        Debug.DrawRay(transform.position, Vector3.down * 0.1f, Color.red);
        if (Physics2D.Raycast(transform.position, Vector3.down, 0.1f))
        {
            Grounded = true;
        }
        else Grounded = false;

        // Jump (W)
        if (Input.GetKeyDown(KeyCode.W) && Grounded)
        {
            Jump();
        }

        // Shoot (Space)
        if (Input.GetKey(KeyCode.Space) && Time.time > LastShoot + 0.25f)
        {
            Shoot();
            // Last shoot
            LastShoot = Time.time;
        }
        
    }

    // Jump 
    private void Jump()
    {
        Rigidbody2D.AddForce(Vector2.up * JumpForce);
    }

    // Shoot 
    private void Shoot()
    {   
        Vector3 direction;

        // Right
        if(transform.localScale.x == 1) direction = Vector3.right;
        else direction = Vector3.left;

        GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.1f, Quaternion.identity);
        bullet.GetComponent<BulletScript>().SetDirection(direction);
    }

    // Update for items with Physics
    private void FixedUpdate()
    {
        Rigidbody2D.velocity = new Vector2(Horizontal, Rigidbody2D.velocity.y);
    }
}
