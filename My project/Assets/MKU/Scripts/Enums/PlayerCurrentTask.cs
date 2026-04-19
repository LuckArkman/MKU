namespace MKU.Scripts.Enums
{
    public enum PlayerCurrentTask
    {
        None,
        JustWalking = 1,
        Interacting = 2,
        Gathering = 4,
        CollectingItem = 8,
        Combating = 16,
    }
}