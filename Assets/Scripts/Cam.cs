using UnityEngine;

public class Cam : MonoBehaviour {
    private Player playerScript;
    private Vector3 follow;

    public Transform player;
    [SerializeField]
    private float distanceOffset, camMovementSmoothness;
    private float posInitY;

    private void Start() {
        playerScript = player.gameObject.GetComponent<Player>();
        posInitY = transform.position.y;
    }

    void FixedUpdate() {
        if (player != null) {
            //Fazendo a movimentação da câmera
            if (playerScript.horizontal > 0)
                distanceOffset = Mathf.Abs(distanceOffset);
            else if (playerScript.horizontal < 0)
                distanceOffset = (-1) * Mathf.Abs(distanceOffset);
            follow = new Vector3(player.position.x + distanceOffset, player.position.y, transform.position.z);


            //transform.position = follow;
            transform.position = Vector3.Lerp(transform.position, follow, camMovementSmoothness * Time.deltaTime);
        }
    }
}