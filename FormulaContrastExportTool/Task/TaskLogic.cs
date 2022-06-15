using System.Data;

namespace FormulaContrastExportTool.Task
{
    //中转层
    public class TaskLogic
    {
        ImportDt importDt=new ImportDt();
        Generate generate=new Generate();
        ExportDt exportDt=new ExportDt();

        #region 变量参数
        private int _taskid;
        private string _fileAddress;       //文件地址
        private DataTable _dt;             //获取dt(从EXCEL获取的DT)
        private DataTable _exportdt;       //获取从运算得出的DT记录集

        private DataTable _resultTable;   //返回DT(运算使用)
        private bool _resultMark;        //返回是否成功标记

        #endregion

        #region Set
        /// <summary>
        /// 中转ID
        /// </summary>
        public int TaskId { set { _taskid = value; } }

        /// <summary>
        /// //接收文件地址信息
        /// </summary>
        public string FileAddress { set { _fileAddress = value; } }

        /// <summary>
        /// 获取dt(从EXCEL获取的DT)
        /// </summary>
        public DataTable Data { set { _dt = value; } }

        /// <summary>
        /// 获取从运算得出的DT相同记录集
        /// </summary>
        public DataTable Exportdt { set { _exportdt = value; } }

        #endregion


        #region Get
        /// <summary>
        ///  返回是否成功标记
        /// </summary>
        public bool ResultMark => _resultMark;

        /// <summary>
        ///返回DataTable至主窗体
        /// </summary>
        public DataTable ResultTable => _resultTable;
        #endregion


        public void StartTask()
        {
            switch (_taskid)
            {
                //导入
                case 0:
                    OpenExcelImportToDt(_fileAddress);
                    break;
                //运算
                case 1:
                    GenerateRecord(_dt);
                    break;
                //导出
                case 2:
                    ExportDtToExcel(_fileAddress, _exportdt);
                    break;
            }
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="fileAddress"></param>
        private void OpenExcelImportToDt(string fileAddress)
        {
            //若_resultTable有值,即先将其清空,再进行赋值
            if (_resultTable?.Rows.Count > 0)
            {
                _resultTable.Rows.Clear();
                _resultTable.Columns.Clear();
            }
             _resultTable = importDt.OpenExcelImporttoDt(fileAddress).Copy();
        }

        /// <summary>
        /// 运算
        /// </summary>
        private void GenerateRecord(DataTable dt)
        {
            //若_resultTable有值,即先将其清空,再进行赋值
            if (_resultTable?.Rows.Count > 0)
            {
                _resultTable.Rows.Clear();
                _resultTable.Columns.Clear();
            }
            generate.GenerateDt(dt);
            //var a = GlobalClasscs.ExData.Exportsamedt.Copy();
            //var b = GlobalClasscs.ExData.Exportdiffdt.Copy();
        }

        /// <summary>
        /// 导出
        /// </summary>
        private void ExportDtToExcel(string fileAddress, DataTable tempdt)
        {
            _resultMark = exportDt.ExportDtToExcel(fileAddress, tempdt);
        }

    }
}
