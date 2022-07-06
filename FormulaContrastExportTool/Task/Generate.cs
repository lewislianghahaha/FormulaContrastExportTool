using System;
using System.Data;
using FormulaContrastExportTool.DB;

namespace FormulaContrastExportTool.Task
{
    //运算
    public class Generate
    {
        TempDtList tempDtList=new TempDtList();

        //记录整理对比后的相同与不相同的结果集记录
        private DataTable _samedt;
        private DataTable _diffdt;

        public void GenerateDt(DataTable sourcedt)
        {
            //记录每行的markid
            var markid = 0;

            //导出临时表(结构相同)
            GlobalClasscs.ExData.Exportsamedt = tempDtList.Get_Importdt();
            GlobalClasscs.ExData.Exportdiffdt = tempDtList.Get_Importdt();

            //根据Mark值及导入数据源,并将数据源从横向转为纵向(用于比较使用)
            var colorcodetemp = tempDtList.Get_ColorCodeTempdt();

            foreach (DataRow rows in sourcedt.Rows)
            {
                markid = Convert.ToInt32(rows[4]);

                //初始化时,先将数据源插入至colorcodetemp内
                if (colorcodetemp.Rows.Count==0)
                {
                    //根据sourcedt获取mark对应的记录集
                    var dtlrows = sourcedt.Select("mark='" + markid + "'");
                    colorcodetemp = GetColorantDt(dtlrows, colorcodetemp).Copy();
                    //数据对比
                    ComparData(rows,colorcodetemp);
                    //最后分别将‘相同’结果集与‘不相同’结果集分别插入至GlobalClasscs.ExData.Exportsamedt与GlobalClasscs.ExData.Exportdiffdt内
                    GlobalClasscs.ExData.Exportsamedt.Merge(_samedt);
                    GlobalClasscs.ExData.Exportdiffdt.Merge(_diffdt);
                }
                //判断,当循环的行中的mark与中间表的mark是一致,就不用重新进入转换方法;
                else if (markid == Convert.ToInt32(colorcodetemp.Rows[0][4]))
                {
                    //数据对比
                    ComparData(rows, colorcodetemp);
                    //最后分别将‘相同’结果集与‘不相同’结果集分别插入至GlobalClasscs.ExData.Exportsamedt与GlobalClasscs.ExData.Exportdiffdt内
                    GlobalClasscs.ExData.Exportsamedt.Merge(_samedt);
                    GlobalClasscs.ExData.Exportdiffdt.Merge(_diffdt);
                }
                //反之,需先将colorcodetemp表内的记录清空,再进行插入转换数据
                else
                {
                    //先将colorcodetemp表内容清空,再插入记录
                    colorcodetemp.Rows.Clear();

                    //根据sourcedt获取mark对应的记录集
                    var dtlrows = sourcedt.Select("mark='" + markid + "'");
                    colorcodetemp=GetColorantDt(dtlrows, colorcodetemp).Copy();
                    //数据对比
                    ComparData(rows, colorcodetemp);
                    //最后分别将‘相同’结果集与‘不相同’结果集分别插入至GlobalClasscs.ExData.Exportsamedt与GlobalClasscs.ExData.Exportdiffdt内
                    GlobalClasscs.ExData.Exportsamedt.Merge(_samedt);
                    GlobalClasscs.ExData.Exportdiffdt.Merge(_diffdt);
                }
                //循环完一行后,将_samedt与_diffdt内的记录清空
                _samedt.Rows.Clear();
                _diffdt.Rows.Clear();
            }
            //var a = GlobalClasscs.ExData.Exportsamedt;
            //var b = GlobalClasscs.ExData.Exportdiffdt;
        }

        /// <summary>
        /// 将循环行与colorcodetemp内进行比较,若相同,即将循环行插入至‘_samedt’内,反之,插入至'_diffdt'(重)
        /// </summary>
        /// <param name="rows">当前循环行</param>
        /// <param name="compartempdt">进行对比的纵向色母明细记录</param>
        private void ComparData(DataRow rows,DataTable compartempdt)
        {
            //定义‘相同’及‘不相同’临时表
            _samedt = tempDtList.Get_Importdt().Clone();
            _diffdt = tempDtList.Get_Importdt().Clone();

            //定义中转比较临时表
            var compardt = tempDtList.Get_ColorCodeTempdt();

            //标记是否有相同记录
            var remarkid = false;

            //先根据rows得出对应的‘内部色号’及‘层’信息
            var colorcode = Convert.ToString(rows[3]);
            var level = Convert.ToString(rows[7]);

            //根据所获取的‘内部色号’及‘层’记录放到compartempdt内进行获取相关数据集
            var dtlrows = compartempdt.Select("内部色号='"+ colorcode +"' and 层='"+ level +"'");

            //如果dtlrows返回的行数只有一行,即马上将数据插入至diffdt内
            if (dtlrows.Length == 1)
            {
                _diffdt.Merge(GenerateDt(rows, _diffdt));
            }
            else
            {
                //判断条件:1)将总行数不是相同的排除 2)使用dtlrows中的‘色母名称’及‘色母量’进行循环对比
                //核心:将dtlrows的‘色母名称’及‘色母量’的记录集 循环 与 compartempdt(不包含dtlrows内自身记录)进行对比;
                //当查找到相同的记录,即将rows插入至_samedt内,若循环完成后,还没有发现相同,即将rows插入至_diffdt内
                foreach (DataRow row in compartempdt.Rows)
                {
                    //循环时,当碰到与colorcode及level值相同,即跳过(不与自身进行比较)
                    if (Convert.ToString(row[0]) == colorcode && Convert.ToString(row[1]) == level) continue;

                    //判断dtlrows.Length与loopdtlrows的行数是否一致,一致才能继续
                    var loopdtlrows = compartempdt.Select("内部色号='" + Convert.ToString(row[0]) + "' and 层 ='" + Convert.ToString(row[1]) + "'");

                    if (dtlrows.Length != loopdtlrows.Length) continue;
                    else
                    {
                        //判断若loopdtlrows内的‘内部色号’及‘层’在compardt存在,即表示前一行已进行判断,不用进行对比运算
                        if (compardt.Select("内部色号='" + Convert.ToString(row[0]) + "' and 层='" + Convert.ToString(row[1]) + "'").Length > 0) continue;
                        else
                        {
                            //先清空compardt,再插入相关数据至compardt,再进行对比
                            compardt.Rows.Clear();
                            compardt = GenerateCompaDt(loopdtlrows, compardt).Copy();
                            //进行对比
                            for (var i = 0; i < dtlrows.Length; i++)
                            {
                                var comparcount = compardt.Select("色母名称='" + Convert.ToString(dtlrows[i][2]) + "' and 色母量='" + Convert.ToString(dtlrows[i][3]) + "'").Length;
                                remarkid = comparcount > 0;
                            }
                            //若循环完成后,remarkid为true,并且rows中的‘内部色号’及‘层’不在_samedt,即将rows记录插入至_samedt内,并跳出所有循环
                            if (remarkid)
                            {
                                if (_samedt.Select("内部色号='"+ Convert.ToString(rows[3]) + "' and 层='"+ Convert.ToString(rows[7]) + "'").Length == 0)
                                {
                                    _samedt.Merge(GenerateDt(rows, _samedt));
                                   // var a = _samedt;
                                    break;   //重;若不设置这个,就会将‘相同’的记录都重复插入至_diffdt内变成不相同数据
                                }
                            }
                        }
                    }
                }
                //如在对比临时表内,找不到相同的记录,即将rows插入至diffdt内
                if (!remarkid)
                {
                    _diffdt.Merge(GenerateDt(rows, _diffdt));
                }
            }
        }

        /// <summary>
        /// 将循环找出的‘对比记录’插入至tempdt内
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="tempdt"></param>
        /// <returns></returns>
        private DataTable GenerateCompaDt(DataRow[] rows,DataTable tempdt)
        {
            foreach (DataRow t in rows)
            {
                tempdt.ImportRow(t);
            }
            return tempdt;
        }


        /// <summary>
        /// 将结果插入至临时表
        /// </summary>
        /// <param name="rows">循环行</param>
        /// <param name="tempdt"></param>
        /// <returns></returns>
        private DataTable GenerateDt(DataRow rows,DataTable tempdt)
        {
            tempdt.ImportRow(rows);
            return tempdt;
        }

        /// <summary>
        /// 将通过markid获取的数据集进行数据转换,从横向变成纵向(重)
        /// </summary>
        /// <returns></returns>
        private DataTable GetColorantDt(DataRow[] rows,DataTable tempdt)
        {
            for (var i = 0; i < rows.Length; i++)
            {
                //j为col的下标值(10~30)
                for (var j = 10; j < 30; j++)
                {
                    //当碰到‘色母编码’为空即跳过,但j继续自增
                    if (Convert.ToString(rows[i][j + 0]) == "")
                    {
                        j ++;   //j下标值自增1(重)
                        continue;
                    }
                    else
                    {
                        var newrow = tempdt.NewRow();
                        newrow[0] = Convert.ToString(rows[i][3]);                    //内部色号
                        newrow[1] = Convert.ToString(rows[i][7]);                    //层
                        newrow[2] = Convert.ToString(rows[i][j+0]);                  //色母名称
                        newrow[3] = Convert.ToString(rows[i][j+0+1])==""
                            ?(object)DBNull.Value:Convert.ToDecimal(rows[i][j+0+1]); //色母量
                        tempdt.Rows.Add(newrow);
                        newrow[4] = Convert.ToString(rows[i][4]);                    //mark
                        j++;
                    }
                }
            }
            return tempdt;
        }


    }
}
