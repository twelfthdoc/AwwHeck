using System.Reflection;
using UnityEngine;

public class Settings : SingletonBase
{
	public bool displayTenAsLetter;
	public bool tutorialCompleted;
	public string gameMode;

	public Settings()
	{
		Load();
	}

	public void Load()
	{
		var fields = typeof(Settings).GetFields(~BindingFlags.Default);

		foreach (var field in fields)
		{
			if (!PlayerPrefs.HasKey(field.Name)) continue;

			switch (field.FieldType.ToString())
			{
				case "System.Single": // float
					field.SetValue(this, PlayerPrefs.GetFloat(field.Name, (float) field.GetValue(this)));
					break;
				case "System.Boolean": // bool
					field.SetValue(this, PlayerPrefs.GetInt(field.Name, (bool) field.GetValue(this) ? 1 : 0) != 0);
					break;
				case "System.Int32": // int
					field.SetValue(this, PlayerPrefs.GetInt(field.Name, (int) field.GetValue(this)));
					break;
				case "System.String": // string
					field.SetValue(this, PlayerPrefs.GetString(field.Name, (string) field.GetValue(this)));
					break;
				default:
					Debug.LogWarning($"{field.Name} has not been loaded because it is of type {field.FieldType}.");
					break;
			}
		}
	}

	public void Save()
	{
		var fields = typeof(Settings).GetFields(~BindingFlags.Default);

		foreach (var field in fields)
		{
			switch (field.FieldType.ToString())
			{
				case "System.Single": // float
					PlayerPrefs.SetFloat(field.Name, (float) field.GetValue(this));
					break;
				case "System.Boolean": // bool
					PlayerPrefs.SetInt(field.Name, (bool) field.GetValue(this) ? 1 : 0);
					break;
				case "System.Int32": // int
					PlayerPrefs.SetInt(field.Name, (int)field.GetValue(this));
					break;
				case "System.String": // string
					PlayerPrefs.SetString(field.Name, (string)field.GetValue(this));
					break;
				default:
					Debug.LogWarning($"{field.Name} has not been saved because it is of type {field.FieldType}.");
					break;
			}
		}
	}
}
