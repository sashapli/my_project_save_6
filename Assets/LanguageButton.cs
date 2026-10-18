using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using YG;

public class LanguageButton : MonoBehaviour
{
	public GameObject ButPlay;
	public GameObject ButGarage;

	private void Awake()
	{

		SwitchLanguage(YandexGame.savesData.language);
	}

	private void OnEnable() => YandexGame.SwitchLangEvent += SwitchLanguage;
	private void OnDisable() => YandexGame.SwitchLangEvent -= SwitchLanguage;

	public void SwitchLanguage(string lang)
	{
		switch (lang)
		{
			case "ru":
				ButPlay.SetActive(false);
				ButGarage.SetActive(false);
				break;
			default:
				ButPlay.SetActive(true);
				ButGarage.SetActive(true);
				break;
		}
	}
}
