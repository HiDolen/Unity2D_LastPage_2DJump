using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using DG.Tweening;
using TMPro;

public class StartButton : MonoBehaviour
{
    public SpriteRenderer square;
    public TextMeshProUGUI text;

    private void Start()
    {

    }

    public void StartGame()
    {
        DOTween.To(() => text.color, x => text.color = x, text.color - new Color(0, 0, 0, 1f), 0.8f);
        DOTween.To(() => square.color, x => square.color = x, new Color(0, 0, 0, 1), 0.8f).OnComplete(LoadScene);
    }

    void LoadScene()
    {
        SceneManager.LoadScene("MainScene");
    }

}
