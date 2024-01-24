using UnityEngine;

public class Cam : MonoBehaviour {
    public Transform player;
    private Vector3 follow;
    [SerializeField]
    private float leftDistance, downDistance, camMovementSmoothness;
    private float posInitY;

    private void Start() {
        posInitY = transform.position.y;
    }

    void FixedUpdate() {
        if (player != null) {
            //Fazendo a movimentação da câmera
            if (player.position.y > posInitY + 2)
                follow = new Vector3(player.position.x + leftDistance, player.position.y, transform.position.z);
            else
                follow = new Vector3(player.position.x + leftDistance, posInitY, transform.position.z);

            //transform.position = follow;
            transform.position = Vector3.Lerp(transform.position, follow, camMovementSmoothness * Time.deltaTime);
        }
    }
}