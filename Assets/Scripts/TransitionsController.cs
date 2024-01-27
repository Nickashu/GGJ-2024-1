using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionsController : MonoBehaviour {
    public GameObject bgTransitions;

    private static TransitionsController instance;
    private Animator animTransitionScenes;
    private float transistionTimeScenes = 2f;

    public static TransitionsController GetInstance() {
        return instance;
    }

    private void Start() {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        animTransitionScenes = bgTransitions.GetComponent<Animator>();
        if (SceneManager.GetActiveScene().name.Contains("Menu")) {
            if(SoundController.GetInstance().numTimesMenu == 0) 
                SoundController.GetInstance().numTimesMenu = 1;
            else
                animTransitionScenes.SetBool("fadeOut", true);
        }
        else
            animTransitionScenes.SetBool("fadeOut", true);

        //playSceneMusic();   //Toca a música correspondente ao mudar de cena
    }

    private void playSceneMusic() {
        if (SceneManager.GetActiveScene().name.Contains("Menu")) {
            //SoundController.GetInstance().PlaySound("OST_menu", null);
        }
        else if (SceneManager.GetActiveScene().name.Contains("Inicial")) {
            //SoundController.GetInstance().PlaySound("OST_safe", null);
        }
        else if (SceneManager.GetActiveScene().name.Contains("Final")) {
            //SoundController.GetInstance().PlaySound("OST_menu", null);
        }
    }

    public void LoadNextScene() {
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1) {    //Se estivermos na última cena
            //Debug.Log("Jogo terminado!");
            StartCoroutine(LoadScene(0));   //Carregando a primeira cena novamente (menu)
        }
        else
            StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));   //Carregando a próxima cena
    }

    public void LoadMainScene() {
        StartCoroutine(LoadScene(1));   //Carregando a cena principal
    }
    public void LoadMenu() {
        StartCoroutine(LoadScene(0));   //Carregando o menu
    }

    private IEnumerator LoadScene(int sceneIndex) {
        animTransitionScenes.SetBool("fadeIn", true);
        yield return new WaitForSeconds(transistionTimeScenes);
        SceneManager.LoadScene(sceneIndex);
    }
}
