using UnityEngine;

public class Menu : MonoBehaviour {
    [SerializeField]
    private GameObject canvasOptions, canvasMenu;

    private void Start() {   //Assim que o menu for carregado, o jogo precisa ser resetado
        GameController.GetInstance().ResetGame();
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void Options() {
        canvasMenu.SetActive(false);
        canvasOptions.SetActive(true);
    }

    public void ReturnToMenu() {
        canvasMenu.SetActive(true);
        canvasOptions.SetActive(false);
    }

    public void StartGame() {
        TransitionsController.GetInstance().LoadNextScene();
    }
}
