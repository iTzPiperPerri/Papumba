namespace DA_Assets.Shared
{
    public class RoutineResult<RESULT, ERROR>
    {
        public bool Success { get; set; }
        public RESULT Result { get; set; }
        public ERROR Error { get; set; }
    }

    public delegate void Return<RESULT, ERROR>(RoutineResult<RESULT, ERROR> coroutineResult);
}