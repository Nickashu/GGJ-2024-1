using UnityEngine;

public class Menu : MonoBehaviour {
    [SerializeField]
    private GameObject canvasMenu;

    private void Start() {   //Assim que o menu for carregado, o jogo precisa ser resetado
        GameController.GetInstance().ResetGame();
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void StartGame() {
        TransitionsController.GetInstance().LoadNextScene();
    }
}
