using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using DarkTonic.MasterAudio;

public class Settings : MonoBehaviour
{
	private Slider volumeSlider;
	private Toggle[] difficultyToggle;

	private void Start()
	{
		volumeSlider = GameObject.Find("Canvas/Menus/Settings/VolumeSlider").GetComponent<Slider>();
		volumeSlider.value = MasterAudio.Instance._masterAudioVolume;
		volumeSlider.onValueChanged.AddListener(delegate { OnVolumeChanged(); });

		difficultyToggle = new Toggle[3];
		difficultyToggle = GameObject.Find("Canvas/Menus/Settings/DifficultyTogglegroup").GetComponentsInChildren<Toggle>();
		difficultyToggle[(int)GameManager.Instance.CurrentDifficulty].isOn = true;

		foreach(Toggle toggle in difficultyToggle)
		{
			toggle.onValueChanged.AddListener(delegate
			{
				 ToggleValueChanged(toggle);
			});
		}

		foreach (Button button in UIManager.Instance.Buttons)
		{
			if (button.transform.parent == transform || button.name == "MusicLeftHandle" || button.name == "MusicRightHandle"
				|| button.name == "VibesLeftHandle" || button.name == "VibesRightHandle")
			{
				button.onClick.AddListener(() => OnClickButton());		
			}
		}
	}

	void OnClickButton()
	{
		if (UIManager.Instance.CurrentClickedButton.name == "SettingsBackBtn")
		{
			Scene scene = SceneManager.GetActiveScene();

			if(scene.name == "Menus")
			{
				GameObject go = UIManager.Instance.Menus.Find(x => x.name == "MainMenu");
				UIManager.Instance.ChangeUIState(go);
			}

			else
			{
				GameObject go = UIManager.Instance.Menus.Find(x => x.name == "PauseMenu");
				UIManager.Instance.ChangeUIState(go);
			}
		}

		if (UIManager.Instance.CurrentClickedButton.name == "MusicLeftHandle")
		{
			GameObject go = UIManager.Instance.CurrentClickedButton;
			go.SetActive(false);

			Button leftHandle = UIManager.Instance.Buttons.Find(x => x.name == "MusicRightHandle");
			leftHandle.gameObject.SetActive(true);

			PlaylistController.InstanceByName("BGMusic").PlaylistVolume = 0f;
			volumeSlider.value = 0f;
		}

		if (UIManager.Instance.CurrentClickedButton.name == "MusicRightHandle")
		{
			GameObject go = UIManager.Instance.CurrentClickedButton; 
			go.SetActive(false);

			Button rightHandle = UIManager.Instance.Buttons.Find(x => x.name == "MusicLeftHandle");
			rightHandle.gameObject.SetActive(true);

			PlaylistController.InstanceByName("BGMusic").PlaylistVolume = 1f;
			volumeSlider.value = 1f;
		}

		if (UIManager.Instance.CurrentClickedButton.name == "VibesLeftHandle")
		{
			GameObject go = UIManager.Instance.CurrentClickedButton;
			go.SetActive(false);

			Button rightHandle = UIManager.Instance.Buttons.Find(x => x.name == "VibesRightHandle");
			rightHandle.gameObject.SetActive(true);
		}

		if (UIManager.Instance.CurrentClickedButton.name == "VibesRightHandle")
		{
			GameObject go = UIManager.Instance.CurrentClickedButton;
			go.SetActive(false);

			Button rightHandle = UIManager.Instance.Buttons.Find(x => x.name == "VibesLeftHandle");
			rightHandle.gameObject.SetActive(true);
		}
	}

	public void OnVolumeChanged()
	{
		PlaylistController.InstanceByName("BGMusic").PlaylistVolume = volumeSlider.value;
	}

	void ToggleValueChanged(Toggle toggle)
	{
		if(toggle.name == "EasyToggle")
		{
			GameManager.Instance.CurrentDifficulty = GameDifficulty.Easy;
		}

		else if (toggle.name == "MediumToggle")
		{
			GameManager.Instance.CurrentDifficulty = GameDifficulty.Medium;
		}

		else
		{
			GameManager.Instance.CurrentDifficulty = GameDifficulty.Hard;
		}
	}
}