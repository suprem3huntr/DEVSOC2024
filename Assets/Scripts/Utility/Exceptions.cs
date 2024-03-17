using System;

public class DeckFullException : Exception
{
    public DeckFullException() : base("Your deck cannot handle the power of more cards")
    {

    }

    
}

public class OverStockException : Exception
{
    public OverStockException()
    {

    }
    public OverStockException(string cardName) : base ("You already have 4 " + cardName + "s in your deck")
    {

    }
}