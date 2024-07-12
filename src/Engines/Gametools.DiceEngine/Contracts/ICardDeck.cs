namespace GameTools.DiceEngine;

/// <summary>
/// Simulates the behavior of a stack of cards.
/// </summary>
public interface ICardDeck
{
    /// <summary>
    /// Shuffles the deck, then chooses one card to return.
    /// </summary>
    /// <typeparam name="T">The Type of the "Card"</typeparam>
    /// <param name="deck">An array of T, representing a finite set of options to choose from</param>
    /// <returns>One of the "cards" in the "deck"</returns>
    T PickOne<T>(T[] deck);
}
