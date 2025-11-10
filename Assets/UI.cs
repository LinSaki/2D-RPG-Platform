using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance; //singleton 
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI killCountText;

    private int killCount;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        timerText.text = Time.time.ToString("F2") + "s"; //F2 = shows 2 numbers after the decimal
    }

    public void AddKillCount()
    {
        killCount++;
        killCountText.text = killCount.ToString();
    }
}
