using System.Collections.Generic;
using UnityEngine;

public class Resources : SingletonBase
{
	public IList<Sprite> Sprites;
	public IList<Sprite> Dealers;

	public Resources()
	{
		LoadSprites();
	}

	public void LoadSprites()
	{
		if (Sprites == null || Sprites.Count == 0)
		{
			Sprites = UnityEngine.Resources.LoadAll<Sprite>("Cards");
		}

		if (Dealers == null || Dealers.Count == 0)
		{
			Dealers = UnityEngine.Resources.LoadAll<Sprite>("Deck");
		}
	}
}
