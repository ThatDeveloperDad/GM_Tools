namespace GameTools.DiceEngine;

public class DeckSimulator : ICardDeck
{
    public T? PickOne<T>(T[] deck)
    {
        if (deck.Length == 0)
            return default;

        // Shuffle the deck.
        var workingCopy = deck.ToArray();
        Random.Shared.Shuffle(workingCopy);

        // Pick the top card.
        var topCard = workingCopy[0];

        // Return the top card.
        return topCard;
    }
}
