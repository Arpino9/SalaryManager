namespace SalaryManager.Domain.Entities
{
    /// <summary>
    /// Entity - 自宅
    /// </summary>
    public sealed class HomeEntity
    {
        public HomeEntity(
            int id,
            string displayName,
            string postCode,
            string address,
            string address_google,
            string remarks)
        {
            ID             = id;
            DisplayName    = displayName;
            PostCode       = postCode;
            Address        = address;
            Address_Google = address_google;
            Remarks        = remarks;
        }

        /// <summary> ID </summary>
        public int ID { get; }
        
        /// <summary> 名称 </summary>
        public string DisplayName { get; }

        /// <summary> 郵便番号 </summary>
        public string PostCode { get; }

        /// <summary> 住所 </summary>
        public string Address { get; }

        /// <summary> 住所 </summary>
        public string Address_Google { get; }

        /// <summary> 備考 </summary>
        public string Remarks { get; }
    }
}
