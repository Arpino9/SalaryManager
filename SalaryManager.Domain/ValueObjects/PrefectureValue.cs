namespace SalaryManager.Domain.ValueObjects;

/// <summary>
/// Value Object - 都道府県名
/// </summary>
public sealed record class PrefectureValue
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="prefecture">都道府県名</param>
    public PrefectureValue(string prefecture)
    {
        if (!this.ValidName.IsMatch(prefecture))
        {
            throw new ArgumentException("都道府県名ではありません。");
        }

        this.Text = prefecture;
    }

    #region Key

    /// <summary> Text </summary>
    public string Text { get; }

    #endregion

    #region Definition

    /// <summary> 有効な都道府県名 </summary>
    private Regex ValidName = new Regex(@".*都|.*道|.*府|.*県");

    /// <summary>
    /// 都道府県
    /// </summary>
    public static IList<string> Prefecture
    {
        get
        {
            var prefecture = new List<string>();

            prefecture.Add(string.Empty);
            prefecture.Add(Hokkaido.Text);
            prefecture.AddRange(Touhoku);
            prefecture.AddRange(Kanto);
            prefecture.AddRange(Hokutiku);
            prefecture.AddRange(Toukai);
            prefecture.AddRange(Kinkki);
            prefecture.AddRange(Tyuugoku);
            prefecture.AddRange(Sikoku);
            prefecture.AddRange(Kyusyu);
            prefecture.Add(Okinawa.Text);

            return prefecture;
        }
    }

    #endregion

    #region Definition - 北海道

    /// <summary> 北海道 </summary>
    private static PrefectureValue Hokkaido = new PrefectureValue("北海道");

    #endregion

    #region Definition - 東北

    /// <summary> 東北 </summary>
    private static IList<string> Touhoku 
        => new List<string>() { Aomori.Text, Iwate.Text, Miyagi.Text, Akita.Text,
                                Yamagata.Text, Fukushima.Text };

    /// <summary> 青森県 </summary>
    private static PrefectureValue Aomori = new PrefectureValue("青森県");

    /// <summary> 岩手県 </summary>
    private static PrefectureValue Iwate = new PrefectureValue("岩手県");

    /// <summary> 宮城県 </summary>
    private static PrefectureValue Miyagi = new PrefectureValue("宮城県");

    /// <summary> 秋田県 </summary>
    private static PrefectureValue Akita = new PrefectureValue("秋田県");

    /// <summary> 山形県 </summary>
    private static PrefectureValue Yamagata = new PrefectureValue("山形県");

    /// <summary> 福島県 </summary>
    private static PrefectureValue Fukushima = new PrefectureValue("福島県");

    #endregion

    #region Definition - 関東・甲信

    /// <summary> 関東・甲信 </summary>
    private static IList<string> Kanto
        => new List<string>() { Tokyo.Text, Kanagawa.Text, Saitama.Text, Chiba.Text,
                                Ibaraki.Text, Tochigi.Text, Gunma.Text, Yamanashi.Text,
                                Nagano.Text, };

    /// <summary> 東京都 </summary>
    private static PrefectureValue Tokyo = new PrefectureValue("東京都");

    /// <summary> 神奈川県 </summary>
    private static PrefectureValue Kanagawa = new PrefectureValue("神奈川県");

    /// <summary> 埼玉県 </summary>
    private static PrefectureValue Saitama = new PrefectureValue("埼玉県");

    /// <summary> 千葉県 </summary>
    private static PrefectureValue Chiba = new PrefectureValue("千葉県");

    /// <summary> 茨城県 </summary>
    private static PrefectureValue Ibaraki = new PrefectureValue("茨城県");

    /// <summary> 栃木県 </summary>
    private static PrefectureValue Tochigi = new PrefectureValue("栃木県");

    /// <summary> 群馬県 </summary>
    private static PrefectureValue Gunma = new PrefectureValue("群馬県");

    /// <summary> 山梨県 </summary>
    private static PrefectureValue Yamanashi = new PrefectureValue("山梨県");

    /// <summary> 長野県 </summary>
    private static PrefectureValue Nagano = new PrefectureValue("長野県");

    #endregion

    #region Definition - 北陸

    /// <summary> 北陸 </summary>
    private static IList<string> Hokutiku
        => new List<string>() { Niigata.Text, Toyama.Text, Ishikawa.Text, Fukui.Text, };

    /// <summary> 新潟県 </summary>
    private static PrefectureValue Niigata = new PrefectureValue("新潟県");

    /// <summary> 富山県 </summary>
    private static PrefectureValue Toyama = new PrefectureValue("富山県");

    /// <summary> 石川県 </summary>
    private static PrefectureValue Ishikawa = new PrefectureValue("石川県");

    /// <summary> 福井県 </summary>
    private static PrefectureValue Fukui = new PrefectureValue("福井県");

    #endregion

    #region Definition - 東海

    /// <summary> 東海 </summary>
    private static IList<string> Toukai 
        => new List<string>() { Aichi.Text, Gifu.Text, Shizuoka.Text, Mie.Text, };

    /// <summary> 愛知県 </summary>
    private static PrefectureValue Aichi = new PrefectureValue("愛知県");

    /// <summary> 岐阜県 </summary>
    private static PrefectureValue Gifu = new PrefectureValue("岐阜県");

    /// <summary> 静岡県 </summary>
    private static PrefectureValue Shizuoka = new PrefectureValue("静岡県");

    /// <summary> 三重県 </summary>
    private static PrefectureValue Mie = new PrefectureValue("三重県");

    #endregion

    #region Definition - 近畿

    /// <summary> 近畿 </summary>
    private static IList<string> Kinkki
        => new List<string>() { Osaka.Text, Hyogo.Text, Kyoto.Text, Shiga.Text, 
                                Nara.Text, Wakayama.Text, };

    /// <summary> 大阪府 </summary>
    private static PrefectureValue Osaka = new PrefectureValue("大阪府");

    /// <summary> 兵庫県 </summary>
    private static PrefectureValue Hyogo = new PrefectureValue("兵庫県");

    /// <summary> 京都府 </summary>
    private static PrefectureValue Kyoto = new PrefectureValue("京都府");

    /// <summary> 滋賀県 </summary>
    private static PrefectureValue Shiga = new PrefectureValue("滋賀県");

    /// <summary> 奈良県 </summary>
    private static PrefectureValue Nara = new PrefectureValue("奈良県");

    /// <summary> 和歌山県 </summary>
    private static PrefectureValue Wakayama = new PrefectureValue("和歌山県");

    #endregion

    #region Definition - 中国

    /// <summary> 中国 </summary>
    private static IList<string> Tyuugoku
        => new List<string>() { Tottori.Text, Shimane.Text, Okayama.Text, 
                                Hiroshima.Text, Yamaguchi.Text, };

    /// <summary> 鳥取県 </summary>
    private static PrefectureValue Tottori = new PrefectureValue("鳥取県");

    /// <summary> 島根県 </summary>
    private static PrefectureValue Shimane = new PrefectureValue("島根県");

    /// <summary> 岡山県 </summary>
    private static PrefectureValue Okayama = new PrefectureValue("岡山県");

    /// <summary> 広島県 </summary>
    private static PrefectureValue Hiroshima = new PrefectureValue("広島県");

    /// <summary> 山口県 </summary>
    private static PrefectureValue Yamaguchi = new PrefectureValue("山口県");

    #endregion

    #region Definition - 四国

    /// <summary> 四国 </summary>
    private static IList<string> Sikoku
        => new List<string>() { Tokushima.Text, Kagawa.Text, Ehime.Text, Kochi.Text, };

    /// <summary> 徳島県 </summary>
    private static PrefectureValue Tokushima = new PrefectureValue("徳島県");

    /// <summary> 香川県 </summary>
    private static PrefectureValue Kagawa = new PrefectureValue("香川県");

    /// <summary> 愛媛県 </summary>
    private static PrefectureValue Ehime = new PrefectureValue("愛媛県");

    /// <summary> 高知県 </summary>
    private static PrefectureValue Kochi = new PrefectureValue("高知県");

    #endregion

    #region Definition - 九州

    /// <summary> 九州 </summary>
    private static IList<string> Kyusyu
        => new List<string>() { Fukuoka.Text, Saga.Text, Nagasaki.Text, Kumamoto.Text,
                                Oita.Text, Miyazaki.Text, Kagoshima.Text, };

    /// <summary> 福岡県 </summary>
    private static PrefectureValue Fukuoka = new PrefectureValue("福岡県");

    /// <summary> 佐賀県 </summary>
    private static PrefectureValue Saga = new PrefectureValue("佐賀県");

    /// <summary> 長崎県 </summary>
    private static PrefectureValue Nagasaki = new PrefectureValue("長崎県");

    /// <summary> 熊本県 </summary>
    private static PrefectureValue Kumamoto = new PrefectureValue("熊本県");

    /// <summary> 大分県 </summary>
    private static PrefectureValue Oita = new PrefectureValue("大分県");

    /// <summary> 宮崎県 </summary>
    private static PrefectureValue Miyazaki = new PrefectureValue("宮崎県");

    /// <summary> 鹿児島県 </summary>
    private static PrefectureValue Kagoshima = new PrefectureValue("鹿児島県");

    #endregion

    #region Definition - 沖縄

    /// <summary> 沖縄県 </summary>
    private static PrefectureValue Okinawa = new PrefectureValue("沖縄県");

    #endregion

    #region Option

    /// <summary>
    /// 都道府県名からファイル名へ
    /// </summary>
    /// <param name="text">都道府県名</param>
    /// <returns>ファイル名</returns>
    public static string ConvertTextToFileName(string text)
    {
        // 北海道
        if (text == Hokkaido.Text) return nameof(Hokkaido);

        // 東北
        if (text == Aomori.Text) return nameof(Aomori);
        if (text == Iwate.Text) return nameof(Iwate);
        if (text == Miyagi.Text) return nameof(Miyagi);
        if (text == Akita.Text) return nameof(Akita);
        if (text == Yamagata.Text) return nameof(Yamagata);
        if (text == Fukushima.Text) return nameof(Fukushima);

        // 関東
        if (text == Tokyo.Text) return nameof(Tokyo);
        if (text == Kanagawa.Text) return nameof(Kanagawa);
        if (text == Saitama.Text) return nameof(Saitama);
        if (text == Chiba.Text) return nameof(Chiba);
        if (text == Ibaraki.Text) return nameof(Ibaraki);
        if (text == Tochigi.Text) return nameof(Tochigi);
        if (text == Gunma.Text) return nameof(Gunma);
        if (text == Yamanashi.Text) return nameof(Yamanashi);
        if (text == Nagano.Text) return nameof(Nagano);

        // 北陸
        if (text == Niigata.Text) return nameof(Niigata);
        if (text == Toyama.Text) return nameof(Toyama);
        if (text == Ishikawa.Text) return nameof(Ishikawa);
        if (text == Fukui.Text) return nameof(Fukui);

        // 東海
        if (text == Aichi.Text) return nameof(Aichi);
        if (text == Gifu.Text) return nameof(Gifu);
        if (text == Mie.Text) return nameof(Mie);
        if (text == Shizuoka.Text) return nameof(Shizuoka);

        // 近畿
        if (text == Osaka.Text) return nameof(Osaka);
        if (text == Hyogo.Text) return nameof(Hyogo);
        if (text == Kyoto.Text) return nameof(Kyoto);
        if (text == Shiga.Text) return nameof(Shiga);
        if (text == Nara.Text) return nameof(Nara);
        if (text == Wakayama.Text) return nameof(Wakayama);

        // 中国
        if (text == Tottori.Text) return nameof(Tottori);
        if (text == Shimane.Text) return nameof(Shimane);
        if (text == Okayama.Text) return nameof(Okayama);
        if (text == Hiroshima.Text) return nameof(Hiroshima);
        if (text == Yamaguchi.Text) return nameof(Yamaguchi);

        // 四国
        if (text == Tokushima.Text) return nameof(Tokushima);
        if (text == Kagawa.Text) return nameof(Kagawa);
        if (text == Ehime.Text) return nameof(Ehime);
        if (text == Kochi.Text) return nameof(Kochi);

        // 四国
        if (text == Fukuoka.Text) return nameof(Fukuoka);
        if (text == Saga.Text) return nameof(Saga);
        if (text == Nagasaki.Text) return nameof(Nagasaki);
        if (text == Kumamoto.Text) return nameof(Kumamoto);
        if (text == Oita.Text) return nameof(Oita);
        if (text == Miyazaki.Text) return nameof(Miyazaki);
        if (text == Kagoshima.Text) return nameof(Kagoshima);

        // 沖縄
        if (text == Okinawa.Text) return nameof(Okinawa);

        return string.Empty;
    }

    /// <summary> 北海道か </summary>
    public bool IsHokkaido => (this == PrefectureValue.Hokkaido);

    /// <summary> 東北か </summary>
    public bool IsTouhoku => (this == PrefectureValue.Aomori ||
                              this == PrefectureValue.Iwate ||
                              this == PrefectureValue.Miyagi ||
                              this == PrefectureValue.Akita ||
                              this == PrefectureValue.Yamagata ||
                              this == PrefectureValue.Fukushima);

    /// <summary> 関東・甲信か </summary>
    public bool IsKanto => (this == PrefectureValue.Tokyo ||
                            this == PrefectureValue.Kanagawa ||
                            this == PrefectureValue.Saitama ||
                            this == PrefectureValue.Chiba ||
                            this == PrefectureValue.Ibaraki ||
                            this == PrefectureValue.Tochigi ||
                            this == PrefectureValue.Gunma ||
                            this == PrefectureValue.Yamanashi ||
                            this == PrefectureValue.Nagano);

    /// <summary> 北陸か </summary>
    public bool IsHokuriku => (this == PrefectureValue.Niigata ||
                               this == PrefectureValue.Toyama ||
                               this == PrefectureValue.Ishikawa ||
                               this == PrefectureValue.Fukui);
    
    /// <summary> 東海か </summary>
    public bool IsToukai => (this == PrefectureValue.Aichi ||
                             this == PrefectureValue.Gifu ||
                             this == PrefectureValue.Shizuoka ||
                             this == PrefectureValue.Mie);

    /// <summary> 近畿か </summary>
    public bool IsKinkki => (this == PrefectureValue.Osaka ||
                             this == PrefectureValue.Hyogo ||
                             this == PrefectureValue.Kyoto ||
                             this == PrefectureValue.Shiga ||
                             this == PrefectureValue.Nara ||
                             this == PrefectureValue.Wakayama);

    /// <summary> 中国か </summary>
    public bool IsTyuugoku => (this == PrefectureValue.Tottori ||
                               this == PrefectureValue.Shimane ||
                               this == PrefectureValue.Okayama ||
                               this == PrefectureValue.Hiroshima ||
                               this == PrefectureValue.Yamaguchi);

    /// <summary> 四国か </summary>
    public bool IsSikoku => (this == PrefectureValue.Tokushima ||
                             this == PrefectureValue.Kagawa ||
                             this == PrefectureValue.Ehime ||
                             this == PrefectureValue.Kochi);

    /// <summary> 九州か </summary>
    public bool IsKyusyu => (this == PrefectureValue.Fukuoka || 
                             this == PrefectureValue.Saga || 
                             this == PrefectureValue.Nagasaki || 
                             this == PrefectureValue.Kumamoto || 
                             this == PrefectureValue.Oita || 
                             this == PrefectureValue.Miyazaki || 
                             this == PrefectureValue.Kagoshima);

    /// <summary> 沖縄か </summary>
    public bool IsOkinawa => (this == PrefectureValue.Okinawa);

    #endregion

}
