using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SalaryManager.Domain.ValueObjects
{
    /// <summary>
    /// Value Object - 都道府県名
    /// </summary>
    public sealed class PrefectureValue : ValueObject<PrefectureValue>
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

            this.Value = prefecture;
        }

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
                prefecture.Add(Hokkaido.Value);
                prefecture.AddRange(Touhoku);
                prefecture.AddRange(Kanto);
                prefecture.AddRange(Hokutiku);
                prefecture.AddRange(Toukai);
                prefecture.AddRange(Kinkki);
                prefecture.AddRange(Tyuugoku);
                prefecture.AddRange(Sikoku);
                prefecture.AddRange(Kyusyu);
                prefecture.Add(Okinawa.Value);

                return prefecture;
            }
        }

        #endregion

        #region Definition - 北海道

        /// <summary> 北海道 </summary>
        private static PrefectureValue Hokkaido = new PrefectureValue("北海道");

        #endregion

        #region Definition - 東北

        /// <summary>
        /// 東北地方
        /// </summary>
        private static IList<string> Touhoku
        {
            get
            {
                return new List<string>()
                {
                    Aomori.Value,
                    Iwate.Value,
                    Miyagi.Value,
                    Akita.Value,
                    Yamagata.Value,
                    Fukushima.Value
                };
            }
        }

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

        /// <summary>
        /// 関東・甲信
        /// </summary>
        private static IList<string> Kanto
        {
            get
            {
                return new List<string>()
                {
                    Tokyo.Value,
                    Kanagawa.Value,
                    Saitama.Value,
                    Chiba.Value,
                    Ibaraki.Value,
                    Tochigi.Value,
                    Gunma.Value,
                    Yamanashi.Value,
                    Nagano.Value,
                };
            }
        }

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

        /// <summary>
        /// 北陸
        /// </summary>
        private static IList<string> Hokutiku
        {
            get
            {
                return new List<string>()
                {
                    Niigata.Value,
                    Toyama.Value,
                    Ishikawa.Value,
                    Fukui.Value,
                };
            }
        }

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

        /// <summary>
        /// 東海
        /// </summary>
        private static IList<string> Toukai
        {
            get
            {
                return new List<string>()
                {
                    Aichi.Value,
                    Gifu.Value,
                    Shizuoka.Value,
                    Mie.Value,
                };
            }
        }

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

        /// <summary>
        /// 近畿
        /// </summary>
        private static IList<string> Kinkki
        {
            get
            {
                return new List<string>()
                {
                    Osaka.Value,
                    Hyogo.Value,
                    Kyoto.Value,
                    Shiga.Value,
                    Nara.Value,
                    Wakayama.Value,
                };
            }
        }

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

        /// <summary>
        /// 中国
        /// </summary>
        private static IList<string> Tyuugoku
        {
            get
            {
                return new List<string>()
                {
                    Tottori.Value,
                    Shimane.Value,
                    Okayama.Value,
                    Hiroshima.Value,
                    Yamaguchi.Value,
                };
            }
        }

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

        /// <summary>
        /// 四国
        /// </summary>
        private static IList<string> Sikoku
        {
            get
            {
                return new List<string>()
                {
                    Tokushima.Value,
                    Kagawa.Value,
                    Ehime.Value,
                    Kochi.Value,
                };
            }
        }

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

        /// <summary>
        /// 九州
        /// </summary>
        private static IList<string> Kyusyu
        {
            get
            {
                return new List<string>()
                {
                    Fukuoka.Value,
                    Saga.Value,
                    Nagasaki.Value,
                    Kumamoto.Value,
                    Oita.Value,
                    Miyazaki.Value,
                    Kagoshima.Value,
                };
            }
        }

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
            if (text == Hokkaido.Value) return nameof(Hokkaido);

            // 東北
            if (text == Aomori.Value) return nameof(Aomori);
            if (text == Iwate.Value) return nameof(Iwate);
            if (text == Miyagi.Value) return nameof(Miyagi);
            if (text == Akita.Value) return nameof(Akita);
            if (text == Yamagata.Value) return nameof(Yamagata);
            if (text == Fukushima.Value) return nameof(Fukushima);

            // 関東
            if (text == Tokyo.Value) return nameof(Tokyo);
            if (text == Kanagawa.Value) return nameof(Kanagawa);
            if (text == Saitama.Value) return nameof(Saitama);
            if (text == Chiba.Value) return nameof(Chiba);
            if (text == Ibaraki.Value) return nameof(Ibaraki);
            if (text == Tochigi.Value) return nameof(Tochigi);
            if (text == Gunma.Value) return nameof(Gunma);
            if (text == Yamanashi.Value) return nameof(Yamanashi);
            if (text == Nagano.Value) return nameof(Nagano);

            // 北陸
            if (text == Niigata.Value) return nameof(Niigata);
            if (text == Toyama.Value) return nameof(Toyama);
            if (text == Ishikawa.Value) return nameof(Ishikawa);
            if (text == Fukui.Value) return nameof(Fukui);

            // 東海
            if (text == Aichi.Value) return nameof(Aichi);
            if (text == Gifu.Value) return nameof(Gifu);
            if (text == Mie.Value) return nameof(Mie);
            if (text == Shizuoka.Value) return nameof(Shizuoka);

            // 近畿
            if (text == Osaka.Value) return nameof(Osaka);
            if (text == Hyogo.Value) return nameof(Hyogo);
            if (text == Kyoto.Value) return nameof(Kyoto);
            if (text == Shiga.Value) return nameof(Shiga);
            if (text == Nara.Value) return nameof(Nara);
            if (text == Wakayama.Value) return nameof(Wakayama);

            // 中国
            if (text == Tottori.Value) return nameof(Tottori);
            if (text == Shimane.Value) return nameof(Shimane);
            if (text == Okayama.Value) return nameof(Okayama);
            if (text == Hiroshima.Value) return nameof(Hiroshima);
            if (text == Yamaguchi.Value) return nameof(Yamaguchi);

            // 四国
            if (text == Tokushima.Value) return nameof(Tokushima);
            if (text == Kagawa.Value) return nameof(Kagawa);
            if (text == Ehime.Value) return nameof(Ehime);
            if (text == Kochi.Value) return nameof(Kochi);

            // 四国
            if (text == Fukuoka.Value) return nameof(Fukuoka);
            if (text == Saga.Value) return nameof(Saga);
            if (text == Nagasaki.Value) return nameof(Nagasaki);
            if (text == Kumamoto.Value) return nameof(Kumamoto);
            if (text == Oita.Value) return nameof(Oita);
            if (text == Miyazaki.Value) return nameof(Miyazaki);
            if (text == Kagoshima.Value) return nameof(Kagoshima);

            // 沖縄
            if (text == Okinawa.Value) return nameof(Okinawa);

            return string.Empty;
        }

        /// <summary>
        /// 北海道か
        /// </summary>
        public bool IsHokkaido
        {
            get
            {
                return (this == PrefectureValue.Hokkaido);
            }
        }

        /// <summary>
        /// 東北か
        /// </summary>
        public bool IsTouhoku
        {
            get
            {
                return (this == PrefectureValue.Aomori ||
                        this == PrefectureValue.Iwate ||
                        this == PrefectureValue.Miyagi ||
                        this == PrefectureValue.Akita ||
                        this == PrefectureValue.Yamagata ||
                        this == PrefectureValue.Fukushima);
            }
        }

        /// <summary>
        /// 関東・甲信か
        /// </summary>
        public bool IsKanto
        {
            get
            {
                return (this == PrefectureValue.Tokyo ||
                        this == PrefectureValue.Kanagawa ||
                        this == PrefectureValue.Saitama ||
                        this == PrefectureValue.Chiba ||
                        this == PrefectureValue.Ibaraki ||
                        this == PrefectureValue.Tochigi ||
                        this == PrefectureValue.Gunma ||
                        this == PrefectureValue.Yamanashi ||
                        this == PrefectureValue.Nagano);
            }
        }

        /// <summary>
        /// 北陸か
        /// </summary>
        public bool IsHokuriku
        {
            get
            {
                return (this == PrefectureValue.Niigata ||
                        this == PrefectureValue.Toyama ||
                        this == PrefectureValue.Ishikawa ||
                        this == PrefectureValue.Fukui);
            }
        }

        /// <summary>
        /// 東海か
        /// </summary>
        public bool IsToukai
        {
            get
            {
                return (this == PrefectureValue.Aichi ||
                        this == PrefectureValue.Gifu ||
                        this == PrefectureValue.Shizuoka ||
                        this == PrefectureValue.Mie);
            }
        }

        /// <summary>
        /// 近畿か
        /// </summary>
        public bool IsKinkki
        {
            get
            {
                return (this == PrefectureValue.Osaka ||
                        this == PrefectureValue.Hyogo ||
                        this == PrefectureValue.Kyoto ||
                        this == PrefectureValue.Shiga ||
                        this == PrefectureValue.Nara ||
                        this == PrefectureValue.Wakayama);
            }
        }

        /// <summary>
        /// 中国か
        /// </summary>
        public bool IsTyuugoku
        {
            get
            {
                return (this == PrefectureValue.Tottori ||
                        this == PrefectureValue.Shimane ||
                        this == PrefectureValue.Okayama ||
                        this == PrefectureValue.Hiroshima ||
                        this == PrefectureValue.Yamaguchi);
            }
        }

        /// <summary>
        /// 四国か
        /// </summary>
        public bool IsSikoku
        {
            get
            {
                return (this == PrefectureValue.Tokushima ||
                        this == PrefectureValue.Kagawa ||
                        this == PrefectureValue.Ehime ||
                        this == PrefectureValue.Kochi);
            }
        }

        /// <summary>
        /// 九州か
        /// </summary>
        public bool IsKyusyu
        {
            get
            {
                return (this == PrefectureValue.Fukuoka ||
                        this == PrefectureValue.Saga ||
                        this == PrefectureValue.Nagasaki ||
                        this == PrefectureValue.Kumamoto ||
                        this == PrefectureValue.Oita ||
                        this == PrefectureValue.Miyazaki ||
                        this == PrefectureValue.Kagoshima);
            }
        }

        /// <summary>
        /// 沖縄か
        /// </summary>
        public bool IsOkinawa
        {
            get
            {
                return (this == PrefectureValue.Okinawa);
            }
        }

        #endregion

        #region Key

        /// <summary> Value </summary>
        public string Value { get; }

        #endregion

        protected override bool EqualsCore(PrefectureValue other)
        {
            return (this.Value.Equals(other.Value));
        }
    }
}
