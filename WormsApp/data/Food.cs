namespace WormsApp.Data
{
    public record Food(Coordinates Coordinates, long ExpiredAt)
    {
        public override string ToString()
        {
            return Coordinates.ToString();
        }
    };
}