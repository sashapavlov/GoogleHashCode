namespace GH2018
{
    public class Ride
    {
        public int Id { get; set; }
        public int StartRow { get; set; }
        public int StartColumn { get; set; }

        public int FinishRow { get; set; }
        public int FinishColumn { get; set; }

        public int EarliestStart { get; set; }
        public int LatestFinish { get; set; }
    }
}