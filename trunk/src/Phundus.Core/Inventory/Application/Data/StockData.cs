namespace Phundus.Core.Inventory.Application.Data
{
    public class StockData
    {
        private string _stockId;

        protected StockData()
        {
            
        }

        public StockData(string stockId)
        {
            _stockId = stockId;
        }

        public virtual string StockId
        {
            get { return _stockId; }
            set { _stockId = value; }
        }

        public virtual int ConcurrencyVersion { get; set; }

        public virtual int ArticleId { get; set; }
    }
}