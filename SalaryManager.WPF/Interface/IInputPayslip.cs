﻿using System;
using SalaryManager.Domain.Repositories;

namespace SalaryManager.Infrastructure.Interface
{
    /// <summary>
    /// Interface - 給与明細入力
    /// </summary>
    /// <remarks>
    /// DBから給与明細の各項目のデータを取得・追加更新を可能にする。
    /// </remarks>
    public interface IInputPayslip
    {
        /// <summary> 初期化 </summary>
        /// <param name="entityDate">初期化する日付</param>
        public void Initialize(DateTime entityDate);

        /// <summary> リロード </summary>
        public void Reload();

        /// <summary> 再描画 </summary>
        public void Refresh();

        /// <summary> リセット </summary>
        public void Clear();

        /// <summary> 保存 </summary>
        /// <param name="transaction">トランザクション</param>
        public void Save(ITransactionRepository transaction);
    }
}
