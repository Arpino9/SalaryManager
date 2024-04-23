using SalaryManager.Domain.ValueObjects;

namespace SalaryManager.Domain.Entities
{
    public class CompanyEntity
    {
        public CompanyEntity(
            int id,
            int middleCategoryNo,
            string companyName,
            string postCode,
            string address,
            string address_Google,
            string remarks)
        {
            this.ID               = id;
            this.BusinessCategory = new BusinessCategoryValue(middleCategoryNo);
            this.CompanyName      = companyName;
            this.PostCode         = postCode;
            this.Address          = address;
            this.Address_Google   = address_Google;
            this.Remarks          = remarks;
        }

        /// <summary> ID </summary>
        public int ID { get; }

        public BusinessCategoryValue BusinessCategory { get; }

        public string CompanyName { get; }

        public string PostCode { get; }
        public string Address { get; }
        public string Address_Google { get; }
        public string Remarks { get; }
    }
}
