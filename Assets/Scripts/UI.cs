using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public static UI instance; //singleton 
    [SerializeField] private GameObject gameOverUI;
    [Space]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI killCountText;
    [SerializeField] private GameObject mobileControlUI;

    private int killCount;
    private float timer = 0f;
    private bool isGameOver = false;

    private void Awake()
    {
        instance = this;
        Time.timeScale = 1; //reset timer to normal
    }

    private void Update()
    {
        #if UNITY_ANDROID || UNITY_IOS
             mobileControlUI.setActive(true);
        #endif
        if (!isGameOver)
            timer += Time.deltaTime;

        timerText.text = timer.ToString("F2") + "s"; //F2 = shows 2 numbers after the decimal
    }

    public void EnableGameOverUI()
    {
        isGameOver = true;
        Time.timeScale = 0.5f; //slows down timer
        gameOverUI.SetActive(true);
    }

    public void RestartLevel()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    public void AddKillCount()
    {
        killCount++;
        killCountText.text = killCount.ToString();
    }
}
