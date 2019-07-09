using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scores : MonoBehaviour
{
	private int scoreTabIndex;
	private List<Text> scorePositionText;
	private List<Text> playerNames;
	private List<Text> playerScores;
	private const int itemsPerTab = 6;

	private void Start()
	{
		foreach (Button button in UIManager.Instance.Buttons)
		{
			if (button.transform.parent == transform)
			{
				button.onClick.AddListener(() => OnClickButton());
			}
		}
	}

	private void Awake()
	{
		scoreTabIndex = 0;

		scorePositionText = new List<Text>();
		playerNames = new List<Text>();
		playerScores = new List<Text>();

		FindScorePositionNames();
		FindPlayerNames();
		FindPlayerScores();

		HighScoreManager.Instance.GetHighScores();
	}

	private void OnEnable()
	{
		ChangeScorePositionTexts();
		ChangeScoreNames();
		ChangePlayerScores();
	}

	void OnClickButton()
	{
		if (UIManager.Instance.CurrentClickedButton.name == "ScoresBackBtn")
		{
			GameObject go = UIManager.Instance.Menus.Find(x => x.name == "MainMenu");
			UIManager.Instance.ChangeUIState(go);
		}

		if (UIManager.Instance.CurrentClickedButton.name == "PreviousHighScore")
		{
			if (scoreTabIndex > 0)
			{
				scoreTabIndex--;
				ChangeScorePositionTexts();
				ChangeScoreNames();
				ChangePlayerScores();
			}
		}

		if (UIManager.Instance.CurrentClickedButton.name == "NextHighScore")
		{
			if (HighScoreManager.Instance.PlayerScores.Count > itemsPerTab * (scoreTabIndex + 1) + 1)
			{
				scoreTabIndex++;
				ChangeScorePositionTexts();
				ChangeScoreNames();
				ChangePlayerScores();
			}
		}
	}

	void ChangeScorePositionTexts()
	{
		int startingIdex = itemsPerTab * scoreTabIndex + 1;

		foreach (Text text in scorePositionText)
		{
			text.text = startingIdex.ToString();
			startingIdex++;
		}
	}

	void ChangeScoreNames()
	{
		int startingIdex = itemsPerTab * scoreTabIndex + 1;

		foreach (Text text in playerNames)
		{
			if (startingIdex <= HighScoreManager.Instance.PlayerScores.Count)
			{
				text.text = HighScoreManager.Instance.PlayerScores[startingIdex - 1].PlayerName;
			}

			else
			{
				text.text = "";
			}

			startingIdex++;
		}
	}

	void ChangePlayerScores()
	{
		int startingIdex = itemsPerTab * scoreTabIndex + 1;

		foreach (Text text in playerScores)
		{
			if (startingIdex <= HighScoreManager.Instance.PlayerScores.Count)
			{
				text.text = HighScoreManager.Instance.PlayerScores[startingIdex - 1].Score.ToString();
			}

			else
			{
				text.text = "";
			}

			startingIdex++;
		}
	}

	void FindScorePositionNames()
	{
		RectTransform parent = GameObject.Find("Canvas/Menus/Scores/ScorePositions").GetComponent<RectTransform>();

		foreach (RectTransform text in parent)
		{
			Text temp = text.GetChild(0).GetComponent<Text>();
			scorePositionText.Add(temp);
		}
	}

	void FindPlayerNames()
	{
		RectTransform parent = GameObject.Find("Canvas/Menus/Scores/ScoreNames").GetComponent<RectTransform>();

		foreach (RectTransform text in parent)
		{
			Text temp = text.GetComponent<Text>();
			playerNames.Add(temp);
		}
	}

	void FindPlayerScores()
	{
		RectTransform parent = GameObject.Find("Canvas/Menus/Scores/ScorePoints").GetComponent<RectTransform>();

		foreach (RectTransform text in parent)
		{
			Text temp = text.GetComponent<Text>();
			playerScores.Add(temp);
		}
	}
}
