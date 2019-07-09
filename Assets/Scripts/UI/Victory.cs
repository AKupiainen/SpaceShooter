using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{
	private Text points;
	private Text enemiesDestoryed;
	private Text gameTime;
	private Image starCount;

	private void Awake()
	{
		foreach (Button button in UIManager.Instance.Buttons)
		{
			if (button.transform.parent == transform)
			{
				button.onClick.AddListener(() => OnClickButton());
			}
		}

		points = GameObject.Find("Canvas/Menus/Victory/SummaryTexts/Points").GetComponent<Text>();
		enemiesDestoryed = GameObject.Find("Canvas/Menus/Victory/SummaryTexts/DestroyedEnemies").GetComponent<Text>();
		gameTime = GameObject.Find("Canvas/Menus/Victory/SummaryTexts/GameTime").GetComponent<Text>();
	}

	private void OnEnable()
	{
		points.text = PlayerManager.Instance.PlayerScore.ToString();
		enemiesDestoryed.text = LevelManager.Instance.EnemiesDestroyed.ToString();

		int minutes = Mathf.FloorToInt(LevelManager.Instance.GameTime / 60F);
		int seconds = Mathf.FloorToInt(LevelManager.Instance.GameTime - minutes * 60);
		string formatedTime = string.Format("{0:0}:{1:00}", minutes, seconds);

		gameTime.text = formatedTime;
	}

	void OnClickButton()
	{
		if(UIManager.Instance.CurrentClickedButton.name == "ReplayBtn")
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		if (UIManager.Instance.CurrentClickedButton.name == "MainMenuBtn")
		{
			SceneManager.LoadScene("Menus");
		}
	}
}