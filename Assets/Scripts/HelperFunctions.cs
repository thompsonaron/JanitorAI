using System;

public static class HelperFunctions
{
    public static System.Random randomizer = new System.Random();

    public static T RandomEnumElement<T>()
    {
        var values = Enum.GetValues(typeof(T));
        var randomIndex = randomizer.Next(values.Length);

        T randomEnumValue = (T)values.GetValue(randomIndex);

        return randomEnumValue;
    }

    public static float CalculateShortestPickupDistance(float currentPos, float pickupPos, float goalPos)
    {
        return Math.Abs(currentPos - pickupPos) + Math.Abs(pickupPos - goalPos);
    }
}
