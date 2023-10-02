namespace serverside.Controllers
{
    internal class DataTablesResult<T>
    {
        public object Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public object Data { get; set; }
    }
}