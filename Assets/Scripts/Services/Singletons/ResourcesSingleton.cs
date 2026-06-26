using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Services
{
	public class ResourcesSingleton : SingletonBase
	{
		public IList<Sprite> Sprites;

		public ResourcesSingleton()
		{
			LoadSprites();
		}

		public void LoadSprites()
		{
			if (Sprites == null || Sprites.Count == 0)
			{
				Sprites = Resources.LoadAll<Sprite>("Cards");
			}
		}
	}
}
