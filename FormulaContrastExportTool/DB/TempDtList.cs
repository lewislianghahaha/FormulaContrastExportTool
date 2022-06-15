using System;
using System.Data;

namespace FormulaContrastExportTool.DB
{
    //临时表
    public class TempDtList
    {
        /// <summary>
        /// 导入模板(横向)
        /// </summary>
        /// <returns></returns>
        public DataTable Get_Importdt()
        {
            var dt = new DataTable();
            for (var i = 0; i < 30; i++)
            {
                var dc = new DataColumn();

                switch (i)
                {
                    case 0:
                        dc.ColumnName = "制造商";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 1:
                        dc.ColumnName = "颜色描述";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 2:
                        dc.ColumnName = "内部色号";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 3:
                        dc.ColumnName = "标准色号";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 4:
                        dc.ColumnName = "mark";
                        dc.DataType = Type.GetType("System.Int32"); 
                        break;
                    case 5:
                        dc.ColumnName = "层";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 6:
                        dc.ColumnName = "色母1";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 7:
                        dc.ColumnName = "色母量1";
                        dc.DataType = Type.GetType("System.Decimal");
                        break;
                    case 8:
                        dc.ColumnName = "色母2";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 9:
                        dc.ColumnName = "色母量2";
                        dc.DataType = Type.GetType("System.Decimal");
                        break;
                    case 10:
                        dc.ColumnName = "色母3";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 11:
                        dc.ColumnName = "色母量3";
                        dc.DataType = Type.GetType("System.Decimal");
                        break;
                    case 12:
                        dc.ColumnName = "色母4";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 13:
                        dc.ColumnName = "色母量4";
                        dc.DataType = Type.GetType("System.Decimal");
                        break;
                    case 14:
                        dc.ColumnName = "色母5";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 15:
                        dc.ColumnName = "色母量5";
                        dc.DataType = Type.GetType("System.Decimal");
                        break;
                    case 16:
                        dc.ColumnName = "色母6";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 17:
                        dc.ColumnName = "色母量6";
                        dc.DataType = Type.GetType("System.Decimal");
                        break;
                    case 18:
                        dc.ColumnName = "色母7";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 19:
                        dc.ColumnName = "色母量7";
                        dc.DataType = Type.GetType("System.Decimal");
                        break;
                    case 20:
                        dc.ColumnName = "色母8";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 21:
                        dc.ColumnName = "色母量8";
                        dc.DataType = Type.GetType("System.Decimal");
                        break;
                    case 22:
                        dc.ColumnName = "色母9";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 23:
                        dc.ColumnName = "色母量9";
                        dc.DataType = Type.GetType("System.Decimal");
                        break;
                    case 24:
                        dc.ColumnName = "色母10";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 25:
                        dc.ColumnName = "色母量10";
                        dc.DataType = Type.GetType("System.Decimal");
                        break;
                    case 26:
                        dc.ColumnName = "色母11";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 27:
                        dc.ColumnName = "色母量11";
                        dc.DataType = Type.GetType("System.Decimal");
                        break;
                    case 28:
                        dc.ColumnName = "色母12";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 29:
                        dc.ColumnName = "色母量12";
                        dc.DataType = Type.GetType("System.Decimal");
                        break;
                }
                dt.Columns.Add(dc);
            }
            return dt;
        }

        /// <summary>
        /// 根据导入的数据获取Mark值,用于循环使用
        /// </summary>
        /// <returns></returns>
        public DataTable Get_MarkIdList()
        {
            var dt = new DataTable();
            for (var i = 0; i < 1; i++)
            {
                var dc = new DataColumn();

                switch (i)
                {
                    case 0:
                        dc.ColumnName = "Mark";
                        dc.DataType = Type.GetType("System.String");
                        break;
                }
                dt.Columns.Add(dc);
            }
            return dt;
        }


        /// <summary>
        /// 根据Mark值及导入数据源,并将数据源从横向转为纵向(用于比较使用)
        /// </summary>
        /// <returns></returns>
        public DataTable Get_ColorCodeTempdt()
        {
            var dt = new DataTable();
            for (var i = 0; i < 5; i++)
            {
                var dc = new DataColumn();

                switch (i)
                {
                    case 0:
                        dc.ColumnName = "内部色号";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 1:
                        dc.ColumnName = "层";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 2:
                        dc.ColumnName = "色母名称";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 3:
                        dc.ColumnName = "色母量";
                        dc.DataType = Type.GetType("System.Decimal");
                        break;
                    case 4:
                        dc.ColumnName = "mark";
                        dc.DataType = Type.GetType("System.String");
                        break;
                }
                dt.Columns.Add(dc);
            }
            return dt;
        }

    }
}
