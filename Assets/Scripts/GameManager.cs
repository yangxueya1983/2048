using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public SoundPlayer soundPalyer;
    public Board board;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestText;

    private int _score;
    
    public CanvasGroup gameOver;
    public CanvasGroup gameSuccess;
    
    void Start()
    {
        NewGame();
    }


    void SetScore(int score)
    {
        _score = score;

        scoreText.text = score.ToString();
    }


    public void NewGame()
    {
        SetScore(0);

        bestText.text = LoadHisScore().ToString();

        gameOver.alpha = 0f;
        gameOver.interactable = false;
        gameSuccess.alpha = 0f;
        gameSuccess.interactable = false;
        gameSuccess.gameObject.SetActive(true);
        
        board.ClearBoard();
        board.SpawnTile();
        board.SpawnTile();
        board.enabled = true;

        

    }

    public void ShowGameOver()
    {
        board.enabled = false;
        gameSuccess.gameObject.SetActive(false);
        gameOver.interactable = true;
        
        StartCoroutine(Fade(gameOver, 1f, 1f));
        soundPalyer.PlayLose();
    }
    
    public void ShowGameSuccess()
    {
        board.enabled = false;
        gameSuccess.interactable = true;
        
        StartCoroutine(Fade(gameSuccess, 1f, 1f));
        soundPalyer.PlaySuccess();
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        float elapsed = 0f;
        float duration = 0.5f;
        float from = canvasGroup.alpha;

        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = to;
    }
    
    public void IncreaseScore(int score)
    {
        _score += score;
        SetScore(_score);
        
        SaveHisScore(_score);
    }
    
    private void SaveHisScore(int score)
    {
        int hiscore = LoadHisScore();

        if (score > hiscore) {
            PlayerPrefs.SetInt("HisScore", score);
        }
    }
    
    private int LoadHisScore()
    {
        return PlayerPrefs.GetInt("HisScore", 0);
    }
}
