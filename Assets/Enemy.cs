using UnityEngine;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private float redColorDuration = 1;

    public float timer;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if(timer < 0 && sr.color != Color.white)
            sr.color = Color.white; 

    }

    public void TakeDamage()
    {
        sr.color = Color.red;
        timer = redColorDuration;
    }
}
