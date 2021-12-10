namespace WormsApp.Data
{
    public class Intent
    {
        public IntentType Type { get; }
        public MoveDirection Direction { get; }

        public Intent(IntentType type, MoveDirection direction)
        {
            Type = type;
            Direction = direction;
        }

        public enum IntentType
        {
            Move,
            MakeChild,
            Nothing
        }

        public enum MoveDirection
        {
            Right,
            Left,
            Up,
            Down
        }
    }
}