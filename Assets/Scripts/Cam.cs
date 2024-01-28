using UnityEngine;

public class Cam : MonoBehaviour {
    private Player playerScript;
    private Vector3 follow;

    public Transform player;
    [SerializeField]
    private float distanceOffsetX, camMovementSmoothness, posInitY=0;

    private void Start() {
        playerScript = player.gameObject.GetComponent<Player>();
    }

    void FixedUpdate() {
        if (player != null) {
            //Fazendo a movimentação da câmera
            if (playerScript.horizontal > 0)
                distanceOffsetX = Mathf.Abs(distanceOffsetX);
            else if (playerScript.horizontal < 0)
                distanceOffsetX = (-1) * Mathf.Abs(distanceOffsetX);
            float followY = player.position.y > posInitY ? player.position.y : posInitY;
            follow = new Vector3(player.position.x + distanceOffsetX, followY, transform.position.z);

            transform.position = Vector3.Lerp(transform.position, follow, camMovementSmoothness * Time.deltaTime);
        }
    }
}