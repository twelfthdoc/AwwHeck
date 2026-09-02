public static class NpcBehaviourExtension
{
	public static int CardsPlayed(this PlayerPosition playerId, int forehand) => forehand + 4 - (int)playerId;
}
