public class TutorialManager : GameManager
{
	public override void Awake()
	{
		base.Awake();

		handSize = 5;
		_maxHandSize = 5;
		GameMode = "Tutorial";
	}
}
