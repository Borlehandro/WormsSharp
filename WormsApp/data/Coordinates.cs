namespace WormsApp.Data
{
    public record Coordinates(int X, int Y)
    {
        public override string ToString()
        {
            return "(" + X + "," + Y + ")";
        }
    }
}