namespace ZTALauncher.Common.Database.Table
{
    public interface ITable
    {
        /// <summary>
        /// return the table name
        /// </summary>
        /// <returns></returns>
        public string GetName();

        /// <summary>
        /// return the id of the inserted row
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public long Insert(ITable table);

        public ITable Read(long id);

        /// <summary>
        /// return the number of rows affected
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(long id);
    }
}
