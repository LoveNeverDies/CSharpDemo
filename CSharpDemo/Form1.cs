using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace CSharpDemo
{
    public partial class Form1 : Form
    {
        public object XMLDocuemnt { get; private set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void DownloadSmallSpeak_Click(object sender, EventArgs e)
        {
            //STM_MAP_BINCODEMAP();
            //return;
            //string val = "jjj-0123-10p";
            //val = "123456789011121qdga";
            //val = val.Substring(0, 15);
            //val = new string(new string(val.ToCharArray().Reverse().ToArray()).Split('-')[0].ToCharArray().Reverse().ToArray());
            Down<SmallSpeakByBQG>();
        }

        void STM_MAP_BINCODEMAP()
        {
            //insertintoBincodemap
            /*STRIPID	料条ID'{0}',*/
            /*CREATED_TIME	创建时间TO_DATE('{1}', 'SYYYY-MM-DD HH24:MI:SS'),*/
            /*ID	ID'{2}',*/
            #region insertSQL
            var insertintoBincodemap = @"INSERT INTO EAPPRD.STM_MAP_BINCODEMAP VALUES (
            /*STRIPID	料条ID*/
            '{0}',
            /*LOTID	批次ID*/
            'TSO236200109103',
            /*EQPID	设备ID*/
            'DB-504-0506',
            /*PROCESS	工序*/
            '167',
            /*X	三光 X(列)方向上*/
            '{3}',
            /*Y	三光 Y(行)方向上*/
            '{4}',
            /*C	三光原点位置 1:表示右上角为原点，0：表示左上角为原点*/
            '1',
            /*I	三光 结果值*/
            '0000',
            /*RES	处理结果 PASS/FAIL*/
            'PASS',
            /*CREATOR	创建人*/
            'cim123',
            /*CREATED_TIME	创建时间*/
            TO_DATE('{1}','SYYYY-MM-DD HH24:MI:SS'),
            /*UPDATOR	修改人*/
            NULL,
            /*UPDATED_TIME	修改时间*/
            NULL,
            /*REMARK	备注*/
            NULL,
            /*ID	ID*/
            '{2}',
            /*FX	晶圆坐标X*/
            NULL,
            /*FY	晶圆坐标Y*/
            NULL,
            /*WAFER_ID	晶圆ID*/
            NULL,
            /*REMAKE	三光检测标识*/
            NULL);";
            /*STRIPID	料条ID*/
            /*LOTID	批次ID*/
            /*EQPID	设备ID*/
            /*PROCESS	工序*/
            /*X	三光 X(列)方向上*/
            /*Y	三光 Y(行)方向上*/
            /*C	三光原点位置 1:表示右上角为原点，0：表示左上角为原点*/
            /*I	三光 结果值*/
            /*RES	处理结果 PASS/FAIL*/
            /*CREATOR	创建人*/
            /*CREATED_TIME	创建时间*/
            /*UPDATOR	修改人*/
            /*UPDATED_TIME	修改时间*/
            /*REMARK	备注*/
            /*ID	ID*/
            /*FX	晶圆坐标X*/
            /*FY	晶圆坐标Y*/
            /*WAFER_ID	晶圆ID*/
            /*REMAKE	三光检测标识*/
            #endregion

            string path = @"D:\File\Glory\TianShui\FCDA\strip map";
            DirectoryInfo root = new DirectoryInfo(path);
            var xmlPath = root.GetFiles();
            StringBuilder sql = new StringBuilder();

            string stripmapContent = "";
            int id = 35694267;
            //料条号
            string lth = "";
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            foreach (var item in xmlPath)
            {
                string xmlpathitem = item.FullName;
                if (xmlpathitem.Contains(".xml") == false)
                {
                    continue;
                }
                StringBuilder strb = new StringBuilder();
                XmlDocument document = new XmlDocument();
                using (StreamReader sr = new StreamReader(xmlpathitem, Encoding.Default))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        strb.AppendLine(line);
                    }
                }
                var str = strb.ToString();
                stripmapContent = str.Remove(str.IndexOf("<TransferMap"), str.IndexOf("</TransferMap>") - str.IndexOf("<TransferMap") + 14);
                //str = Regex.Replace(str, "^(<)[\\s\\S]*?(>)$", string.Empty);
                document.LoadXml(stripmapContent);
                //var TransferMap = document.GetElementsByTagName("TransferMap")[0];
                //TransferMap.ParentNode.RemoveChild(TransferMap);
                lth = document.GetElementsByTagName("Substrate")[0].Attributes["SubstrateId"].Value;
                for (int x = 1; x <= 36; x++)
                {
                    for (int y = 1; y <= 8; y++)
                    {
                        sql.AppendFormat(insertintoBincodemap, lth, nowtime, ++id, x, y).AppendLine();
                    }
                }
            }

            File.WriteAllText(@"D:\File\Glory\TianShui\FCDA\STM_MAP_BINCODEMAP_SQL.txt", sql.ToString());
        }

        void STM_MAP_CONFIG()
        {
            string path = @"D:\File\Glory\TianShui\FCDA\strip map";
            DirectoryInfo root = new DirectoryInfo(path);
            var xmlPath = root.GetFiles();
            StringBuilder sql = new StringBuilder();

            string stripmapContent = "";
            int id = 156037;
            //料条号
            string lth = "";
            //次工序
            string gx = "167";
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            foreach (var item in xmlPath)
            {
                string xmlpathitem = item.FullName;
                if (xmlpathitem.Contains(".xml") == false)
                {
                    continue;
                }
                StringBuilder strb = new StringBuilder();
                XmlDocument document = new XmlDocument();
                using (StreamReader sr = new StreamReader(xmlpathitem, Encoding.Default))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        strb.AppendLine(line);
                    }
                }
                var str = strb.ToString();
                stripmapContent = str.Remove(str.IndexOf("<TransferMap"), str.IndexOf("</TransferMap>") - str.IndexOf("<TransferMap") + 14);
                //str = Regex.Replace(str, "^(<)[\\s\\S]*?(>)$", string.Empty);
                document.LoadXml(stripmapContent);
                //var TransferMap = document.GetElementsByTagName("TransferMap")[0];
                //TransferMap.ParentNode.RemoveChild(TransferMap);
                lth = document.GetElementsByTagName("Substrate")[0].Attributes["SubstrateId"].Value;
                sql.AppendFormat("INSERT INTO EAPPRD.STM_MAP_CONFIG VALUES ('{0}', '{1}', '{2}', '{3}', 'TSO236200109103', 'DB-504-0506', '2', '8', '14', '1', '1', '0', 'cim123', TO_DATE('{4}', 'SYYYY-MM-DD HH24:MI:SS'), NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);", ++id, stripmapContent, lth, gx, nowtime).AppendLine();
            }

            File.WriteAllText(@"D:\File\Glory\TianShui\FCDA\SQL.txt", sql.ToString());
        }

        public void Down<T>() where T : BaseSmallSpeak, new()
        {
            var helper = new T();
            var html = helper.HttpGet(string.Empty);
            helper.HandleHtml(html);
        }
    }
}
