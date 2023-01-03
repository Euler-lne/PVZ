using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using OfficeOpenXml;
using System.IO;
using System;
using System.Reflection;

[InitializeOnLoad]
public class Startup
{
    //表格数据不变时就不用去读取
    public static bool needread = false;
    //这个方法会在运行前执行
    static Startup()
    {
        //只用生成一次就可以
        if (!needread)
            return;
        //表格的存储位置
        string path = Application.dataPath + "/Editor/LevelManager.xlsx";
        //导出数据后的资源名称
        FileInfo fileInfo = new FileInfo(path);
        //创建序列化类
        LevelData levelData = (LevelData)ScriptableObject.CreateInstance(typeof(LevelData));
        //打开Excel文件，using会在使用完毕后自动关闭读取的文件
        using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
        {
            //表格内具体表单：选择Zombie表单
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Zombie"];
            //反射语法
            //获取LevelItem的Type类型
            Type type = typeof(LevelItem);
            //遍历每一行
            for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
            {
                //拿到LevelItem的类型进行赋值
                LevelItem levelItem = new LevelItem();
                //遍历每一列
                for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                {
                    //用反射的方法对levelItem进行赋值，worksheet.GetValue(1,j)对表格2行j列的内容，第一行为LevelItem的每一个属性
                    FieldInfo variable = type.GetField(worksheet.GetValue(1, j).ToString());
                    //先拿到表格中的内容
                    string tableValue = worksheet.GetValue(i, j).ToString();
                    //将其赋值给对应的属性
                    variable.SetValue(levelItem, Convert.ChangeType(tableValue, variable.FieldType));
                }//每一行的LevelItem生成完成
                //当赋值完成就将其添加到列表中
                levelData.levelDataList.Add(levelItem);
            }
            //第二页
            worksheet = excelPackage.Workbook.Worksheets["ZombieNum"];
            type = typeof(LevelItemInfo);
            for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
            {
                LevelItemInfo levelItemInfo = new LevelItemInfo();
                for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                {
                    //拿到第一行的标题名称
                    FieldInfo variable = type.GetField(worksheet.GetValue(1, j).ToString());
                    //拿到相应的值转换为字符串
                    string tableValue = worksheet.GetValue(i, j).ToString();
                    //赋值给levelItemInfo，把levelItemInfo相应的标题名称对应的属性赋值，为第二个参数，注意要把字符串类型转换为相应的类型
                    variable.SetValue(levelItemInfo, Convert.ChangeType(tableValue, variable.FieldType));
                }
                //当赋值完成就将其添加到列表中
                levelData.levelInfoList.Add(levelItemInfo);
            }
        }
        //保存为ScriptableObject为.asset文件
        //保存到同一个文件夹下
        AssetDatabase.CreateAsset(levelData, "Assets/Resources/TableData/Level" + ".asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
