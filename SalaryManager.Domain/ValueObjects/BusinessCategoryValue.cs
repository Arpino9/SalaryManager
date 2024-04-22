using SalaryManager.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace SalaryManager.Domain.ValueObjects
{
    /// <summary>
    /// Value Object - 業種
    /// </summary>
    public sealed record class BusinessCategoryValue
    {
        /// <summary> 農業、林業、漁業 </summary>
        private static readonly BusinessCategoryValue Agriculture            = new BusinessCategoryValue("農業、林業、漁業");

        /// <summary> 鉱業、採石業、砂利採取業 </summary>
        private static readonly BusinessCategoryValue Mining                 = new BusinessCategoryValue("鉱業、採石業、砂利採取業");

        /// <summary> 建設業 </summary>
        private static readonly BusinessCategoryValue Construction          = new BusinessCategoryValue("建設業");
        
        /// <summary> 食料品、飲料・たばこ・飼料製造業 </summary>
        private static readonly BusinessCategoryValue Food                   = new BusinessCategoryValue("食料品、飲料・たばこ・飼料製造業");

        /// <summary> 繊維工業 </summary>
        private static readonly BusinessCategoryValue Textile                = new BusinessCategoryValue("繊維工業");
        
        /// <summary> 木材・木製品、パルプ・紙・紙加工品製造業 </summary>
        private static readonly BusinessCategoryValue Timber                 = new BusinessCategoryValue("木材・木製品、パルプ・紙・紙加工品製造業");

        /// <summary> 化学工業 </summary>
        private static readonly BusinessCategoryValue Chemistry              = new BusinessCategoryValue("化学工業");
        
        /// <summary> 石油製品・石炭製品製造業 </summary>
        private static readonly BusinessCategoryValue Petroleum              = new BusinessCategoryValue("石油製品・石炭製品製造業");

        /// <summary> 窯業・土石製品製造業 </summary>
        private static readonly BusinessCategoryValue Pottery                = new BusinessCategoryValue("窯業・土石製品製造業");

        /// <summary> 鉄鋼業 </summary>
        private static readonly BusinessCategoryValue Steel                  = new BusinessCategoryValue("鉄鋼業");

        /// <summary> 非鉄金属製造業 </summary>
        private static readonly BusinessCategoryValue NonFerrousMetalworking = new BusinessCategoryValue("非鉄金属製造業");

        /// <summary> 金属製品製造業 </summary>
        private static readonly BusinessCategoryValue Metalware              = new BusinessCategoryValue("金属製品製造業");

        /// <summary> はん用機械器具製造業 </summary>
        private static readonly BusinessCategoryValue GeneralMetalware       = new BusinessCategoryValue("はん用機械器具製造業");

        /// <summary> 生産用機械器具製造業 </summary>
        private static readonly BusinessCategoryValue Production             = new BusinessCategoryValue("生産用機械器具製造業");

        /// <summary> 業務用機械器具製造業 </summary>
        private static readonly BusinessCategoryValue Operation              = new BusinessCategoryValue("業務用機械器具製造業");
        
        /// <summary> 電気機械器具製造業 </summary>
        private static readonly BusinessCategoryValue Electrodynamic         = new BusinessCategoryValue("電気機械器具製造業");
        
        /// <summary> 情報通信機械器具、電子部品・デバイス・電子回路製造業 </summary>
        private static readonly BusinessCategoryValue Device                 = new BusinessCategoryValue("情報通信機械器具、電子部品・デバイス・電子回路製造業");

        /// <summary> 輸送機械器具製造業 </summary>
        private static readonly BusinessCategoryValue TransportMachinery     = new BusinessCategoryValue("輸送機械器具製造業");
        
        /// <summary> その他の製造業 </summary>
        private static readonly BusinessCategoryValue OtherManufacture       = new BusinessCategoryValue("その他の製造業");
        
        /// <summary> 電気・ガス・熱供給・水道業 </summary>
        private static readonly BusinessCategoryValue Electricity            = new BusinessCategoryValue("電気・ガス・熱供給・水道業");
        
        /// <summary> 情報通信業 </summary>
        private static readonly BusinessCategoryValue Telecommunications     = new BusinessCategoryValue("情報通信業");
        
        /// <summary> 運輸業 </summary>
        private static readonly BusinessCategoryValue Transportation         = new BusinessCategoryValue("運輸業");

        /// <summary> 卸売業、小売業 </summary>
        private static readonly BusinessCategoryValue Wholesale              = new BusinessCategoryValue("卸売業、小売業");

        /// <summary> 金融業、保険業 </summary>
        private static readonly BusinessCategoryValue Finance                = new BusinessCategoryValue("金融業、保険業");
        
        /// <summary> 不動産業 </summary>
        private static readonly BusinessCategoryValue RealEstate             = new BusinessCategoryValue("不動産業");
        
        /// <summary> 宿泊業、飲食サービス業 </summary>
        private static readonly BusinessCategoryValue LodgingAndFood         = new BusinessCategoryValue("宿泊業、飲食サービス業");
        
        /// <summary> 教育、学習支援、医療、福祉、複合サービス業 </summary>
        private static readonly BusinessCategoryValue Education              = new BusinessCategoryValue("教育、学習支援、医療、福祉、複合サービス業");

        /// <summary> サービス業 </summary>
        private static readonly BusinessCategoryValue Service                = new BusinessCategoryValue("サービス業");

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="largeCategory">大区分</param>
        public BusinessCategoryValue(string largeCategory)
        {
            this.Value = largeCategory;
        }

        /// <summary> Value </summary>
        public string Value { get; }

        /// <summary>
        /// 業種(大分類)
        /// </summary>
        public static IList<BusinessCategoryValue> LargeCategory
            => new List<BusinessCategoryValue> 
                { Agriculture, Mining, Construction, Food, Textile, Timber, Chemistry, Petroleum, Pottery,
                  Steel, NonFerrousMetalworking, Metalware, GeneralMetalware, Production, Operation,
                  Electrodynamic, Device, TransportMachinery, OtherManufacture, Electricity, 
                  Telecommunications, Transportation, Wholesale, Finance, RealEstate, LodgingAndFood,
                  Education, Service };

        /// <summary>
        /// 業種(中分類)
        /// </summary>
        private IDictionary<string, string> MiddleCategory
            => MiddleCategory_Agriculture.Union(MiddleCategory_Agriculture)
                                         .Union(MiddleCategory_Mining)
                                         .Union(MiddleCategory_Construct1ion)
                                         .Union(MiddleCategory_Food)
                                         .Union(MiddleCategory_Textile)
                                         .Union(MiddleCategory_Timber)
                                         .Union(MiddleCategory_Chemistry)
                                         .Union(MiddleCategory_Petroleum)
                                         .Union(MiddleCategory_Pottery)
                                         .Union(MiddleCategory_Steel)
                                         .Union(MiddleCategory_NonFerrousMetalworking)
                                         .Union(MiddleCategory_Metalware)
                                         .Union(MiddleCategory_GeneralMetalware)
                                         .Union(MiddleCategory_Production)
                                         .Union(MiddleCategory_Operation)
                                         .Union(MiddleCategory_Electrodynamic)
                                         .Union(MiddleCategory_Device)
                                         .Union(MiddleCategory_TransportMachinery)
                                         .Union(MiddleCategory_OtherManufacture)
                                         .Union(MiddleCategory_Electricity)
                                         .Union(MiddleCategory_Telecommunications)
                                         .Union(MiddleCategory_Transportation)
                                         .Union(MiddleCategory_Wholesale)
                                         .Union(MiddleCategory_Finance)
                                         .Union(MiddleCategory_RealEstate)
                                         .Union(MiddleCategory_LodgingAndFood)
                                         .Union(MiddleCategory_Education)
                                         .Union(MiddleCategory_Service)
                                         .ToDictionary(x => x.Key, x => x.Value);

        /// <summary>
        /// 農業、林業、漁業 
        /// </summary>
        private IDictionary<string, string> MiddleCategory_Agriculture
            => new List<(string No, string Name)>() 
                { 
                    ("0101", "農業"), 
                    ("0102", "林業"), 
                    ("0103", "漁業・水産養殖業"),
                }.ToDictionary(x => x.No, x => x.Name);

        /// <summary> 
        /// 鉱業、採石業、砂利採取業 
        /// </summary>
        private IDictionary<string, string> MiddleCategory_Mining
            => new List<(string No, string Name)>() 
                { 
                    ("0201", "鉱業、採石業、砂利採取業"),
                }.ToDictionary(x => x.No, x => x.Name);

        /// <summary> 
        /// 建設業 
        /// </summary>
        private IDictionary<string, string> MiddleCategory_Construct1ion
            => new List<(string No, string Name)>() 
                { 
                    ("0301", "建設業"),
                }.ToDictionary(x => x.No, x => x.Name);

        /// <summary> 
        /// 食料品、飲料・たばこ・飼料製造業 
        /// </summary>
        private IDictionary<string, string> MiddleCategory_Food
            => new List<(string No, string Name)>() 
                { 
                    ("0401", "食料品製造業"), 
                    ("0402", "飲料製造業"), 
                    ("0403", "たばこ製造業"), 
                    ("0404", "飼料・有機質肥料製造業"),
                }.ToDictionary(x => x.No, x => x.Name);

        /// <summary> 
        /// 繊維工業 
        /// </summary>
        private IDictionary<string, string> MiddleCategory_Textile
            => new List<(string No, string Name)>() 
            { 
                ("0501", "製糸業、紡績業、化学繊維・ねん糸等製造業"),
                ("0502", "織物業、ニット生地製造業"),
                ("0503", "染色整理業、綱・網・レース・繊維粗製品製造業"),
                ("0504", "衣服・その他の繊維製品製造業"),
            }.ToDictionary(x => x.No, x => x.Name);

        /// <summary> 
        /// 木材・木製品、パルプ・紙・紙加工品製造業 
        /// </summary>
        private IDictionary<string, string> MiddleCategory_Timber
            => new List<(string No, string Name)>()
            {
                ("0601", "木材・木製品製造業"),
                ("0602", "パルプ・紙製造業"),
                ("0603", "紙加工品製造業"),
            }.ToDictionary(x => x.No, x => x.Name);

        /// <summary> 
        /// 化学工業
        /// </summary>
        private IDictionary<string, string> MiddleCategory_Chemistry
            => new List<(string No, string Name)>()
            {
                ("0701", "化学肥料製造業"),
                ("0702", "無機化学工業製品製造業"),
                ("0703", "有機化学工業製品製造業"),
                ("0704", "油脂加工製品・石けん・合成洗剤・界面活性剤・塗料製造業"),
                ("0705", "医薬品製造業"),
                ("0706", "化粧品・歯磨、その他の化粧用調整品製造業"),
                ("0707", "その他の化学工業"),
            }.ToDictionary(x => x.No, x => x.Name);

        /// <summary> 
        /// 石油製品・石炭製品製造業
        /// </summary>
        private IDictionary<string, string> MiddleCategory_Petroleum
             => new List<(string No, string Name)>()
            {
                ("0801", "石油精製業"),
                ("0802", "その他の石油製品・石炭製品製造業"),
            }.ToDictionary(x => x.No, x => x.Name);

        /// <summary> 
        /// 窯業・土石製品製造業
        /// </summary>
        private IDictionary<string, string> MiddleCategory_Pottery
             => new List<(string No, string Name)>()
            {
                ("0901", "ガラス・同製品製造業"),
                ("0902", "セメント・同製品製造業"),
                ("0903", "その他の窯業・土石製品製造業"),
            }.ToDictionary(x => x.No, x => x.Name);

        /// <summary> 
        /// 鉄鋼業
        /// </summary>
        private IDictionary<string, string> MiddleCategory_Steel
             => new List<(string No, string Name)>()
            {
                ("1001", "銑鉄・粗鋼・鋼材製造業"),
                ("1002", "鋳鍛造品・その他の鉄鋼製品製造業"),
            }.ToDictionary(x => x.No, x => x.Name);

        /// <summary> 
        /// 非鉄金属製造業
        /// </summary>
        private IDictionary<string, string> MiddleCategory_NonFerrousMetalworking
             => new List<(string No, string Name)>()
            {
                ("1101", "非鉄金属製錬・精製業"),
                ("1102", "その他の非鉄金属製品製造業"),
            }.ToDictionary(x => x.No, x => x.Name);

        /// <summary> 
        /// 金属製品製造業
        /// </summary>
        private IDictionary<string, string> MiddleCategory_Metalware
             => new List<(string No, string Name)>()
            {
                ("1201", "建設用・建築用金属製品製造業"),
                ("1202", "その他の金属製品製造業"),
            }.ToDictionary(x => x.No, x => x.Name);

        /// <summary> 
        /// はん用機械器具製造業
        /// </summary>
        private IDictionary<string, string> MiddleCategory_GeneralMetalware
             => new List<(string No, string Name)>()
            {
                ("1301", "一般産業用機械・装置製造業"),
                ("1302", "その他のはん用機械器具製造業"),
            }.ToDictionary(x => x.No, x => x.Name);

        /// <summary> 
        /// 生産用機械器具製造業
        /// </summary>
        private IDictionary<string, string> MiddleCategory_Production
             => new List<(string No, string Name)>()
            {
                ("1401", "農業用機械、建設機械・鉱山機械、繊維機械製造業"),
                ("1402", "生活関連産業用機械・基礎素材産業用機械製造業"),
                ("1403", "金属加工機械製造業"),
                ("1404", "半導体・フラットパネルディスプレイ製造装置製造業"),
                ("1405", "その他の生産用機械器具製造業"),
            }.ToDictionary(x => x.No, x => x.Name);

        /// <summary> 
        /// 業務用機械器具製造業
        /// </summary>
        private IDictionary<string, string> MiddleCategory_Operation
             => new List<(string No, string Name)>()
            {
                ("1501", "事務用・サービス用・娯楽用機械器具製造業"),
                ("1502", "光学機械器具・レンズ製造業"),
                ("1503", "その他の業務用機械器具製造業"),
            }.ToDictionary(x => x.No, x => x.Name);

        /// <summary> 
        /// 電気機械器具製造業
        /// </summary>
        private IDictionary<string, string> MiddleCategory_Electrodynamic
             => new List<(string No, string Name)>()
            {
                ("1601", "産業用電気機械器具製造業"),
                ("1602", "民生用電気機械器具製造業"),
                ("1603", "電子応用装置製造業 "),
                ("1604", "その他の電気機械器具製造業 "),
            }.ToDictionary(x => x.No, x => x.Name);

        /// <summary> 
        /// 情報通信機械器具、電子部品・デバイス・電子回路製造業
        /// </summary>
        private IDictionary<string, string> MiddleCategory_Device
             => new List<(string No, string Name)>()
            {
                ("1701", "通信機械器具・同関連機械器具、映像・音響機械器具製造業"),
                ("1702", "電子計算機・同附属装置製造業"),
                ("1703", "電子部品・デバイス・電子回路製造業"),
            }.ToDictionary(x => x.No, x => x.Name);

        /// <summary> 
        /// 輸送機械器具製造業
        /// </summary>
        private IDictionary<string, string> MiddleCategory_TransportMachinery
             => new List<(string No, string Name)>()
            {
                ("1801", "自動車、自動車車体・附随車製造業"),
                ("1802", "自動車部分品・附属品製造業"),
                ("1803", "その他の輸送用機械器具製造業"),
            }.ToDictionary(x => x.No, x => x.Name);

        /// <summary> 
        /// その他の製造業
        /// </summary>
        private IDictionary<string, string> MiddleCategory_OtherManufacture
             => new List<(string No, string Name)>()
            {
                ("1901", "家具・装備品製造業"),
                ("1902", "印刷・同関連業"),
                ("1903", "プラスチック製品製造業"),
                ("1904", "ゴム製品製造業"),
                ("1905", "なめし革・同製品・毛皮製造業"),
                ("1906", "その他の製造業"),
            }.ToDictionary(x => x.No, x => x.Name);

        /// <summary> 
        /// 電気・ガス・熱供給・水道業
        /// </summary>
        private IDictionary<string, string> MiddleCategory_Electricity
             => new List<(string No, string Name)>()
            {
                ("2001", "電気業、ガス業、熱供給業、水道業"),
            }.ToDictionary(x => x.No, x => x.Name);

        /// <summary> 
        /// 情報通信業
        /// </summary>
        private IDictionary<string, string> MiddleCategory_Telecommunications
             => new List<(string No, string Name)>()
            {
                ("2101", "通信業"),
                ("2102", "放送業"),
                ("2103", "情報サービス業"),
                ("2104", "インターネット附随サービス業"),
            }.ToDictionary(x => x.No, x => x.Name);

        // <summary> 
        /// 運輸業
        /// </summary>
        private IDictionary<string, string> MiddleCategory_Transportation
             => new List<(string No, string Name)>()
            {
                ("2201", "鉄道業、道路旅客運送業、道路貨物運送業、水運業、航空運輸業、郵便業"),
                ("2202", "倉庫業・運輸に附帯するサービス業"),
            }.ToDictionary(x => x.No, x => x.Name);

        // <summary> 
        /// 卸売業、小売業
        /// </summary>
        private IDictionary<string, string> MiddleCategory_Wholesale
             => new List<(string No, string Name)>()
            {
                ("2301", "卸売業"),
                ("2302", "小売業"),
            }.ToDictionary(x => x.No, x => x.Name);

        // <summary> 
        /// 金融業、保険業
        /// </summary>
        private IDictionary<string, string> MiddleCategory_Finance
             => new List<(string No, string Name)>()
            {
                ("2401", "金融業、保険業"),
            }.ToDictionary(x => x.No, x => x.Name);

        // <summary> 
        /// 不動産業
        /// </summary>
        private IDictionary<string, string> MiddleCategory_RealEstate
             => new List<(string No, string Name)>()
            {
                ("2501", "不動産業"),
            }.ToDictionary(x => x.No, x => x.Name);

        // <summary> 
        /// 宿泊業、飲食サービス業
        /// </summary>
        private IDictionary<string, string> MiddleCategory_LodgingAndFood
             => new List<(string No, string Name)>()
            {
                ("2701", "宿泊業"),
                ("2702", "飲食店"),
                ("2703", "持ち帰り・配達飲食サービス業"),
            }.ToDictionary(x => x.No, x => x.Name);

        // <summary> 
        /// 教育、学習支援、医療、福祉、複合サービス業
        /// </summary>
        private IDictionary<string, string> MiddleCategory_Education
             => new List<(string No, string Name)>()
            {
                ("2801", "教育、学習支援"),
                ("2802", "医療、福祉"),
                ("2803", "複合サービス業"),
            }.ToDictionary(x => x.No, x => x.Name);

        // <summary> 
        /// サービス業
        /// </summary>
        private IDictionary<string, string> MiddleCategory_Service
             => new List<(string No, string Name)>()
            {
                ("2901", "経営コンサルタント業、純粋持株会社"),
                ("2902", "広告業"),
                ("2903", "学術研究、専門・技術サービス業"),
                ("2904", "生活関連サービス業、娯楽業"),
                ("2905", "その他のサービス業"),
            }.ToDictionary(x => x.No, x => x.Name);

        public string GetMiddleCategoryValue(string number)
        {
            MiddleCategory.TryGetValue(number, out var value);
            return value;
        }

        public string GetMiddleCategoryKey(string value)
            => MiddleCategory.Where(x => x.Value == value).Select(x => x.Key).FirstOrDefault();

        public IDictionary<string, string> GetMiddleCategoryList(string largeCategory)
        {
            if (largeCategory == null) { return  null; }

            if      (largeCategory == Agriculture.Value)            return MiddleCategory_Agriculture;
            else if (largeCategory == Mining.Value)                 return MiddleCategory_Mining;
            else if (largeCategory == Construction.Value)           return MiddleCategory_Construct1ion;
            else if (largeCategory == Food.Value)                   return MiddleCategory_Food;
            else if (largeCategory == Construction.Value)           return MiddleCategory_Construct1ion;
            else if (largeCategory == Textile.Value)                return MiddleCategory_Textile;
            else if (largeCategory == Timber.Value)                 return MiddleCategory_Timber;
            else if (largeCategory == Chemistry.Value)              return MiddleCategory_Chemistry;
            else if (largeCategory == Petroleum.Value)              return MiddleCategory_Petroleum;
            else if (largeCategory == Pottery.Value)                return MiddleCategory_Pottery;
            else if (largeCategory == Steel.Value)                  return MiddleCategory_Steel;
            else if (largeCategory == NonFerrousMetalworking.Value) return MiddleCategory_NonFerrousMetalworking;
            else if (largeCategory == Metalware.Value)              return MiddleCategory_Metalware;
            else if (largeCategory == GeneralMetalware.Value)       return MiddleCategory_GeneralMetalware;
            else if (largeCategory == Production.Value)             return MiddleCategory_Production;
            else if (largeCategory == Operation.Value)              return MiddleCategory_Operation;
            else if (largeCategory == Electrodynamic.Value)         return MiddleCategory_Electrodynamic;
            else if (largeCategory == Device.Value)                 return MiddleCategory_Device;
            else if (largeCategory == TransportMachinery.Value)     return MiddleCategory_TransportMachinery;
            else if (largeCategory == OtherManufacture.Value)       return MiddleCategory_OtherManufacture;
            else if (largeCategory == Electricity.Value)            return MiddleCategory_Electricity;
            else if (largeCategory == Telecommunications.Value)     return MiddleCategory_Telecommunications;
            else if (largeCategory == Transportation.Value)         return MiddleCategory_Transportation;
            else if (largeCategory == Wholesale.Value)              return MiddleCategory_Wholesale;
            else if (largeCategory == Finance.Value)                return MiddleCategory_Finance;
            else if (largeCategory == RealEstate.Value)             return MiddleCategory_RealEstate;
            else if (largeCategory == LodgingAndFood.Value)         return MiddleCategory_LodgingAndFood;
            else if (largeCategory == Education.Value)              return MiddleCategory_Education;
            else if (largeCategory == Service.Value)                return MiddleCategory_Service;

            throw new FormatException("業種の大分類が不正値・もしくは登録されていません。");
        }

        public override string ToString()
            => this.Value;
    }
}
