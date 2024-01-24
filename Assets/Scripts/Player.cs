using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {
    private Rigidbody2D rb;

    public Transform groundCheck;
    public LayerMask groundLayer;

    [SerializeField]
    private float movementSpeed, jumpPower, movementSmoothness, jumpSmoothness;
    private float smoothMove=0;
    private bool lookingRight=true;

    public float horizontal=0;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        //Fazendo a movimentação do player:
        if (horizontal > 0) {
            smoothMove = Mathf.Lerp(smoothMove, 1, movementSmoothness);
            rb.velocity = new Vector2(smoothMove * movementSpeed, rb.velocity.y);
            if (!lookingRight)
                flipPlayer();
        }
        else if (horizontal < 0) {
            smoothMove = Mathf.Lerp(smoothMove, -1, movementSmoothness);
            rb.velocity = new Vector2(smoothMove * movementSpeed, rb.velocity.y);
            if (lookingRight)
                flipPlayer();
        }
        else {
            smoothMove = Mathf.Lerp(smoothMove, 0, movementSmoothness);
            rb.velocity = new Vector2(smoothMove * movementSpeed, rb.velocity.y);
        }

    }

    private bool isOnGround() {   //Método para verificar se o jogador está encostado no chão
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void flipPlayer() {   //Método para fazer o sprite do jogador virar
        lookingRight = !lookingRight;
        Vector3 locScale = transform.localScale;
        locScale.x *= -1;
        transform.localScale = locScale;
    }

    //Estes métodos detectam os eventos de inputs:
    public void SetMovement(InputAction.CallbackContext value) {
        horizontal = value.ReadValue<Vector2>().x;
        if (value.canceled)    //Se o evento for cancelado
            horizontal = 0;
    }
    public void SetJump(InputAction.CallbackContext value) {
        if (value.performed) {   //Quando o evento acontecer
            if (isOnGround()) 
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }
        if (value.canceled) {    //Se o evento for cancelado
            if (rb.velocity.y > 0)
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpSmoothness);
        }
    }
}
