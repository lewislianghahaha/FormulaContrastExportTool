using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaContrastExportTool
{
    public class GlobalClasscs
    {
        /// <summary>
        /// 记录运算后的数据集
        /// </summary>
        public struct ExportData
        {
            public DataTable Exportsamedt;
            public DataTable Exportdiffdt;
        }

        public static ExportData ExData;

    }
}
