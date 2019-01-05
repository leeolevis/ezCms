namespace ez.Core.Excel
{
    public class ExcelEntityFactory
    {
        #region single pattern
        private ExcelEntityFactory()
        {

        }

        // A private static instance of the same class
        private static readonly ExcelEntityFactory Instance = null;

        static ExcelEntityFactory()
        {
            // create the instance only if the instance is null
            Instance = new ExcelEntityFactory();
        }

        public static ExcelEntityFactory GetInstance() => Instance;

        #endregion

        public IReadFromExcel CreateReadFromExcel()
        {
            IReadFromExcel result = null;
            result = new ReadFromExcel();
            return result;
        }

        public IWriteToExcel CreateWriteToExcel()
        {
            IWriteToExcel result = null;
            result = new WriteToExcel();
            return result;
        }
    }
}
