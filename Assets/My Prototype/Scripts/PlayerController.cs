using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    // Singleton Pattern
    public static PlayerController instance;

    // Variables
    [Header("Variables")] 
    [SerializeField] private float _walkSpeed = 150f;
    [SerializeField] private float _jumpForce = 7f;
    [SerializeField] private float _doubleJumpForce = 4f;

    // Components
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;

    // States
    public bool IsMovingInX { get; private set; }
    public float DirectionInX { get; private set; }
    public bool IsStoppedInX { get; private set; }
    public bool IsGrounded { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsFalling { get; private set; }
    public bool HasDoubleJumped { get; private set; }

    private void Awake() => instance = this;

    private void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        UpdateStates();
        SyncSpriteFlip();

        if (Input.GetButtonDown("Jump") && IsGrounded)
            Jump(_jumpForce);
        else if (Input.GetButtonDown("Jump") && (IsJumping || IsFalling) && !HasDoubleJumped && !IsGrounded) {
            HasDoubleJumped = true;
            Jump(_doubleJumpForce);
        }
    }

    private void FixedUpdate() {
        // X axis movement
        if (IsMovingInX) MoveInX();
        else StopInX();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag.Equals("Ground")) {
            IsGrounded = true;
            IsJumping = false;
            IsFalling = false;
            HasDoubleJumped = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.tag.Equals("Ground") && _rb.velocity.y == 0)
            IsGrounded = false;
    }

    private void MoveInX() =>
        _rb.velocity = new Vector2(_walkSpeed * DirectionInX * Time.fixedDeltaTime, _rb.velocity.y);

    private void StopInX() => _rb.velocity = new Vector2(0, _rb.velocity.y);

    private void Jump(float force) {
        IsJumping = true;
        IsFalling = false;
        IsGrounded = false;
        _rb.velocity = new Vector2(_rb.velocity.x, force);
    }
    
    private void SyncSpriteFlip() {
        if (DirectionInX == -1) _spriteRenderer.flipX = true;
        else if (DirectionInX == 1) _spriteRenderer.flipX = false;
    }

    private void UpdateStates() {
        // X axis
        IsMovingInX = Input.GetAxisRaw("Horizontal") != 0;
        DirectionInX = Input.GetAxisRaw("Horizontal");
        IsStoppedInX = _rb.velocity.x == 0;

        // Y axis
        IsJumping = _rb.velocity.y > 0;
        IsFalling = _rb.velocity.y < 0;

        PrintStates();
    }

    private void PrintStates() {
        Debug.Log($"Grounded {IsGrounded} | Jumping {IsJumping}");
    }
}