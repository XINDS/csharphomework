using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.Threading;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        int start = 0;
        string http1;
        string http2;
        string http3;
        int page;
        string url;
        string temp;
        string[] temparr;
        string[] pmidarr;
        HtmlAttribute a;
        HtmlAttribute b;
        StreamWriter idsw;
        StreamWriter errormsg;

        StreamReader srpmid;
        StreamWriter date;
        StreamWriter abstr;
        StreamWriter tot_abstr;
        StreamWriter keyword;
        StreamWriter tot_keyword;
        List<string> addmsg_l;
        public Form1()
        {
            InitializeComponent();
        }
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (start == 1)
            {
                StreamReader streamReader = new StreamReader(webBrowser1.DocumentStream, Encoding.GetEncoding(webBrowser1.Document.Encoding));
                HtmlAgilityPack.HtmlDocument htmldoc = new HtmlAgilityPack.HtmlDocument();
                htmldoc.LoadHtml(streamReader.ReadToEnd());
                HtmlNodeCollection newsListHot1 = htmldoc.DocumentNode.SelectNodes(".//meta[@name]");
                if (newsListHot1 != null)
                {
                    foreach (HtmlNode news in newsListHot1)
                    {
                        a = news.Attributes["name"];
                        b = news.Attributes["content"];
                        if (a.Value.ToString() == "log_displayeduids")
                        {
                            temp = b.Value;
                            temparr = temp.Split(',');
                            pmidarr = temp.Split(',');
                            errormsg.WriteLine(page);
                            foreach (string c in temparr)
                            {
                                idsw.WriteLine(c);
                                idsw.Flush();
                            }
                        }
                    }
                }
                else
                {
                    errormsg.WriteLine("读取pmid错误,页数:" + page);//写入没有抓到pmid的页、数量
                }
                int datenum = 0;
                newsListHot1 = htmldoc.DocumentNode.SelectNodes(".//span[@class]");
                if (newsListHot1 != null)
                {
                    foreach (HtmlNode news in newsListHot1)
                    {
                        a = news.Attributes["class"];
                        if (a.Value.ToString() == "cit")
                        {
                            temp = news.InnerText;
                            temp = temp.Replace('\n', ' ');
                            temp = temp.Replace('\r', ' ');
                            //temparr = temp.Split(';');
                            //temp = temparr[0];
                            date.WriteLine(pmidarr[datenum / 2] + ';' + temp);
                            date.Flush();
                            datenum++;
                        }
                    }
                }
                else
                {
                    errormsg.WriteLine("时间错误页数:" + page);
                }
                int absnum = 0;
                HtmlNodeCollection nodelist;
                newsListHot1 = htmldoc.DocumentNode.SelectNodes(".//div[@class]");
                if (newsListHot1 != null)
                {
                    foreach (HtmlNode news in newsListHot1)
                    {
                        a = news.Attributes["class"];
                        b = news.Attributes["id"];
                        if (a.Value.ToString() == "abstract" && b.Value.ToString() == ("search-result-" + page.ToString() + "-" + ((absnum + 1).ToString()) + "-abstract"))
                        {
                            abstr = new StreamWriter("./temp/abstract/" + pmidarr[absnum] + ".txt");
                            keyword = new StreamWriter("./temp/keyword/" + pmidarr[absnum] + ".txt");

                            nodelist = news.SelectNodes("./div/p/text()");
                            if (nodelist != null)
                            {
                                foreach (HtmlNode nn in nodelist)
                                {
                                    temp = nn.InnerText;
                                    temp = temp.Replace('\n', ' ');
                                    temp = temp.Replace('\r', ' ');
                                    //temparr = temp.Split(';');
                                    //temp = temparr[0];
                                    temp = temp.Trim();
                                    //tot_abstr.WriteLine(temp);
                                    abstr.WriteLine(temp);
                                    abstr.Flush();
                                }
                            }
                            nodelist = news.SelectNodes("./p");
                            if (nodelist != null)
                            {
                                foreach (HtmlNode nn in nodelist)
                                {
                                    temp = nn.InnerText;
                                    temp = temp.Replace('\n', ' ');
                                    temp = temp.Replace('\r', ' ');
                                    temp = temp.Trim();
                                    //tot_keyword.WriteLine(temp);
                                    keyword.WriteLine(temp);
                                    keyword.Flush();
                                    //temparr = temp.Split(';');
                                    /*foreach(string ss in temparr)
                                    {
                                        if(ss.Length>=1)
                                        {
                                            trimstr = ss.Trim();
                                            tot_keyword.WriteLine(trimstr);
                                            keyword.WriteLine(trimstr);
                                        }
                                    }*/
                                }
                            }
                            absnum++;
                            abstr.Close();
                            keyword.Close();
                        }
                    }
                }
                else
                {
                    errormsg.WriteLine("摘要关键词错误页数:" + page);
                }



                errormsg.Flush();


                page++;
                //absnum = 0;
                //datenum = 0;

                if (page == 46 && http2 == "2010-2013")
                {
                    http2 = "2014-2016";
                    page = 1;
                }
                if (page == 47 && http2 == "2014-2016")
                {
                    http2 = "2017-2019";
                    page = 1;
                }
                if (page == 51 && http2 == "2017-2019")
                {
                    http2 = "2020-2020";
                    page = 1;
                }
                if (page == 15 && http2 == "2020-2020")
                {
                    start = 2;
                    idsw.Close();
                    errormsg.Close();
                    date.Close();
                    addmsg();

                    //webBrowser1.Url = new Uri(Application.StartupPath + "/html/addmsg.html");

                }
                else if (page <= 50)
                {
                    url = http1 + http2 + http3 + page.ToString();
                    webBrowser1.Url = new Uri(url);
                }
            }
            if (start == 2)
            {
                StreamReader streamReader = new StreamReader(webBrowser1.DocumentStream, Encoding.GetEncoding(webBrowser1.Document.Encoding));
                HtmlAgilityPack.HtmlDocument htmldoc = new HtmlAgilityPack.HtmlDocument();
                htmldoc.LoadHtml(streamReader.ReadToEnd());
                HtmlNodeCollection newsListHot1 = htmldoc.DocumentNode.SelectNodes(".//span[@class]");

                if (newsListHot1 != null)
                {
                    foreach (HtmlNode news in newsListHot1)
                    {
                        a = news.Attributes["class"];
                        if (a.Value.ToString() == "cit" || a.Value.ToString() == "chapter-contribution-date")
                        {
                            temp = news.InnerText;
                            temp = temp.Replace('\n', ' ');
                            temp = temp.Replace('\r', ' ');
                            //temparr = temp.Split(';');
                            //temp = temparr[0];
                            date.WriteLine(http2 + ';' + temp);
                            break;
                        }
                    }
                }
                else
                {
                    errormsg.WriteLine(http2);

                }

                date.Flush();
                errormsg.Flush();

                addmsg_l.RemoveAt(0);
                if (addmsg_l.Count == 0)
                {
                    start = 0;
                    date.Close();
                    errormsg.Close();
                    webBrowser1.Url = new Uri(Application.StartupPath + "/html/complete.html");
                }
                else
                {
                    http2 = addmsg_l[0];
                    webBrowser1.Url = new Uri(http1 + http2);
                }
            }
            if(start==3)
            {
                int[] check = new int[100000000];
                for (int i = 0; i < 100000000; i++)
                {
                    check[i] = 0;
                }
                StreamReader sr1 = new StreamReader("./temp/pmid/pmid.txt");
                StreamReader sr2 = new StreamReader("./temp/date/date.txt");
                StreamWriter sw1 = new StreamWriter("./data/fin_pmid.txt");
                StreamWriter sw2 = new StreamWriter("./data/fin_date.txt");
                string line;
                int iline;
                while ((line = sr1.ReadLine()) != null)
                {
                    iline = int.Parse(line);
                    check[iline]++;
                }
                for (int i = 0; i < 100000000; i++)
                {
                    if (check[i] >= 1)
                    {
                        sw1.WriteLine(i.ToString());
                    }
                    check[i] = 0;
                }
                sw1.Flush();
                sw1.Close();
                sr1.Close();
                string[] strtemp;
                string[] str1 = new string[100000000];
                string[] str2 = new string[100000000];
                while ((line = sr2.ReadLine()) != null)
                {
                    strtemp = line.Split(';');
                    str1[int.Parse(strtemp[0])] = strtemp[0];
                    check[int.Parse(strtemp[0])] = 1;
                    str2[int.Parse(strtemp[0])] = strtemp[1];
                }
                for (int i = 0; i < 100000000; i++)
                {
                    if (check[i] == 1)
                    {
                        sw2.WriteLine(str1[i] + ";" + str2[i]);
                    }
                }

                sw2.Flush();
                sw2.Close();
                sr2.Close();
                check = null;
                str1 = null;
                str2 = null;
                GC.Collect();
                StreamWriter[,] id = new StreamWriter[11, 17];
                StreamWriter[,] number = new StreamWriter[11, 17];
                int[,] num = new int[11, 17];
                for (int i = 0; i < 11; i++)
                {
                    for (int j = 0; j < 17; j++)
                    {
                        id[i, j] = new StreamWriter("./data/id/" + (2010 + i).ToString() + "/" + j.ToString() + ".txt");
                        number[i, j] = new StreamWriter("./data/number/" + (2010 + i).ToString() + "/" + j.ToString() + ".txt");
                        num[i, j] = 0;
                    }
                }
                StreamReader date;
                date = new StreamReader("./data/fin_date.txt");
                List<string> ls = new List<string>();
                string time;
                string pmid;
                string[] month = new string[13] { " ", "jan", "feb", "mar", "apr", "may", "jun", "jul", "aug", "sep", "oct", "nov", "dec" };

                while ((line = date.ReadLine()) != null)
                {
                    strtemp = line.Split(';');
                    pmid = strtemp[0];
                    time = strtemp[1];
                    for (int i = 2010; i <= 2020; i++)
                    {
                        if (time.IndexOf(i.ToString(), StringComparison.OrdinalIgnoreCase) != -1)
                        {
                            id[i - 2010, 0].WriteLine(pmid);
                            id[i - 2010, 0].Flush();
                            num[i - 2010, 0]++;
                            for (int j = 1; j <= 12; j++)
                            {
                                if (time.IndexOf(month[j], StringComparison.OrdinalIgnoreCase) != -1)
                                {
                                    id[i - 2010, j].WriteLine(pmid);
                                    id[i - 2010, j].Flush();
                                    num[i - 2010, j]++;
                                    switch (j)
                                    {
                                        case 1:
                                            id[i - 2010, 13].WriteLine(pmid);
                                            id[i - 2010, 13].Flush();
                                            num[i - 2010, 13]++;
                                            break;
                                        case 2:
                                            id[i - 2010, 13].WriteLine(pmid);
                                            id[i - 2010, 13].Flush();
                                            num[i - 2010, 13]++;
                                            break;
                                        case 3:
                                            id[i - 2010, 13].WriteLine(pmid);
                                            id[i - 2010, 13].Flush();
                                            num[i - 2010, 13]++;
                                            break;
                                        case 4:
                                            id[i - 2010, 14].WriteLine(pmid);
                                            id[i - 2010, 14].Flush();
                                            num[i - 2010, 14]++;
                                            break;
                                        case 5:
                                            id[i - 2010, 14].WriteLine(pmid);
                                            id[i - 2010, 14].Flush();
                                            num[i - 2010, 14]++;
                                            break;
                                        case 6:
                                            id[i - 2010, 14].WriteLine(pmid);
                                            id[i - 2010, 14].Flush();
                                            num[i - 2010, 14]++;
                                            break;
                                        case 7:
                                            id[i - 2010, 15].WriteLine(pmid);
                                            id[i - 2010, 15].Flush();
                                            num[i - 2010, 15]++;
                                            break;
                                        case 8:
                                            id[i - 2010, 15].WriteLine(pmid);
                                            id[i - 2010, 15].Flush();
                                            num[i - 2010, 15]++;
                                            break;
                                        case 9:
                                            id[i - 2010, 15].WriteLine(pmid);
                                            id[i - 2010, 15].Flush();
                                            num[i - 2010, 15]++;
                                            break;
                                        case 10:
                                            id[i - 2010, 16].WriteLine(pmid);
                                            id[i - 2010, 16].Flush();
                                            num[i - 2010, 16]++;
                                            break;
                                        case 11:
                                            id[i - 2010, 16].WriteLine(pmid);
                                            id[i - 2010, 16].Flush();
                                            num[i - 2010, 16]++;
                                            break;
                                        case 12:
                                            id[i - 2010, 16].WriteLine(pmid);
                                            id[i - 2010, 16].Flush();
                                            num[i - 2010, 16]++;
                                            break;

                                    }
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                date.Close();


                for (int i = 0; i < 11; i++)
                {
                    for (int j = 0; j < 17; j++)
                    {
                        id[i, j].Close();
                        number[i, j].WriteLine(num[i, j]);
                        number[i, j].Flush();
                        number[i, j].Close();
                    }
                }
                str1 = null;
                str2 = null;
                GC.Collect();

                number_year();
                number_month();
                number_quarter();

                keyword_prework();

                wordcloud_keyword_year();
                wordcloud_keyword_month();
                wordcloud_keyword_quarter();

                abstract_prework();

                wordcloud_abstract_year();
                wordcloud_abstract_month();
                wordcloud_abstract_quarter();

                start = 0;
                webBrowser1.Url = new Uri(Application.StartupPath + "/html/work_complete.html");
            }
        }
        void addmsg()
        {
            http1 = "https://pubmed.ncbi.nlm.nih.gov/";
            start = 2;
            StreamReader sr1 = new StreamReader("./temp/date/date.txt");
            StreamReader sr2 = new StreamReader("./temp/pmid/pmid.txt");
            int[] a = new int[100000000];
            int[] b = new int[100000000];
            for (int i = 0; i < 100000000; i++)
            {
                a[i] = 0;
                b[i] = 0;
            }
            string s1;
            while (true)
            {
                s1 = sr1.ReadLine();
                if (s1 == null)
                {
                    break;
                }
                s1 = s1.Split(';')[0];
                a[int.Parse(s1)]++;
            }
            while (true)
            {
                s1 = sr2.ReadLine();
                if (s1 == null)
                {
                    break;
                }
                b[int.Parse(s1)]++;
            }
            sr1.Close();
            sr2.Close();

            date = new StreamWriter("./temp/date/date.txt", true);
            errormsg = new StreamWriter("error.txt");
            addmsg_l = new List<string>();
            for (int i = 0; i < 100000000; i++)
            {
                if (b[i] > 0 && a[i] == 0)
                {
                    addmsg_l.Add(i.ToString());
                }
            }
            if (addmsg_l.Count > 0)
            {
                http2 = addmsg_l[0];
                webBrowser1.Url = new Uri(http1 + http2);
            }
            else
            {
                start = 0;
                date.Close();
                errormsg.Close();
                webBrowser1.Url = new Uri(Application.StartupPath + "/html/complete.html");

            }
        }

        void check_dir()
        {
            if(!Directory.Exists("./data"))
            {
                Directory.CreateDirectory("./data");
            }
            if (!Directory.Exists("./html"))
            {
                Directory.CreateDirectory("./html");
            }
            if (!Directory.Exists("./temp"))
            {
                Directory.CreateDirectory("./temp");
            }
            if(!Directory.Exists("./data/id"))
            {
                Directory.CreateDirectory("./data/id");
            }
            if (!Directory.Exists("./data/keyword"))
            {
                Directory.CreateDirectory("./data/keyword");
            }
            if (!Directory.Exists("./data/abstract"))
            {
                Directory.CreateDirectory("./data/abstract");
            }
            if (!Directory.Exists("./data/number"))
            {
                Directory.CreateDirectory("./data/number");
            }
            if (!Directory.Exists("./temp/pmid"))
            {
                Directory.CreateDirectory("./temp/pmid");
            }
            if (!Directory.Exists("./temp/keyword"))
            {
                Directory.CreateDirectory("./temp/keyword");
            }
            if (!Directory.Exists("./temp/abstract"))
            {
                Directory.CreateDirectory("./temp/abstract");
            }
            if (!Directory.Exists("./temp/date"))
            {
                Directory.CreateDirectory("./temp/date");
            }
            for(int i=2010;i<=2020;i++)
            {
                if (!Directory.Exists("./data/id/"+i.ToString()))
                {
                    Directory.CreateDirectory("./data/id/"+i.ToString());
                }
                if (!Directory.Exists("./data/keyword/" + i.ToString()))
                {
                    Directory.CreateDirectory("./data/keyword/" + i.ToString());
                }
                if (!Directory.Exists("./data/number/" + i.ToString()))
                {
                    Directory.CreateDirectory("./data/number/" + i.ToString());
                }
                if (!Directory.Exists("./data/abstract/" + i.ToString()))
                {
                    Directory.CreateDirectory("./data/abstract/" + i.ToString());
                }
            }

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            check_dir();
            combobox_prework();
            //string u = "https://www.baidu.com";
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Url = new Uri(Application.StartupPath+"/html/index.html");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            start = 1;
            idsw = new StreamWriter("./temp/pmid/pmid.txt", true);
            errormsg = new StreamWriter("error.txt");
            date = new StreamWriter("./temp/date/date.txt", true);
            http1 = "https://pubmed.ncbi.nlm.nih.gov/?term=thyroid%20carcinoma&filter=simsearch1.fha&filter=years.";
            http2 = "2010-2013";
            http3 = "&format=abstract&size=200&page=";
            page = 1;
            //string http3 = http1 + http2;
            //string url = "https://www.baidu.com/";
            //string url = "https://pubmed.ncbi.nlm.nih.gov/?term=thyroid+carcinoma&filter=simsearch1.fha&filter=years.2010-2013&format=abstract&size=200&page=1";
            url = http1 + http2 + http3 + page.ToString();

            webBrowser1.Url = new Uri(url);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate(Application.StartupPath + "/html/work.html");
            start = 3;
        }

        void number_year()
        {
            string html1 = "<!DOCTYPE html>" + "\n" +
"<html>" + "\n" +
"<head>" + "\n" +
"    <meta charset=\"utf-8\">" + "\n" +
"<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"/>" +
"    <title>ECharts</title>" + "\n" +
"    <script src=\"echarts.min.js\" ></script>" + "\n" +
"</head>" + "\n" +
"<body>" + "\n" +
"    <div id=\"main\" style =\"width: 960px;height:540px;\"></div>" + "\n" +
"    <script type=\"text/javascript\" > " + "\n" +
"        var myChart = echarts.init(document.getElementById('main'));" + "\n" +
"            var option = {" + "\n" +
"            title: {" + "\n" +
"                text: '每年论文数量统计'" + "\n" +
"            }," + "\n" +
"            tooltip: {}," + "\n" +
"            legend: {" + "\n" +
"                data:['数量']" + "\n" +
"            }," + "\n" +
"            xAxis: {" + "\n" +
"                data: ["; string html2 = "]" + "\n" +
 "            }," + "\n" +
 "            yAxis: {}," + "\n" +
 "            series: [{" + "\n" +
 "                name: '数量'," + "\n" +
 "                type: 'bar'," + "\n" +
 "                data: ["; string html3 = "]" + "\n" +
  "            }]" + "\n" +
  "        };" + "\n" +
  "        myChart.setOption(option);" + "\n" +
  "    </script>" + "\n" +
  "</body>" + "\n" +
  "</html>";
            string data1 = "";
            for (int i = 2010; i <= 2020; i++)
            {
                if (i > 2010)
                {
                    data1 += ',';
                }
                data1 += '\"';
                data1 += i.ToString();
                data1 += "年";
                data1 += '\"';
            }
            string data2 = "";
            StreamReader sr;
            string st;
            for (int i = 2010; i <= 2020; i++)
            {
                sr = new StreamReader("./data/number/" + i.ToString() + "/0.txt");
                st = sr.ReadLine();
                sr.Close();
                if (i > 2010)
                {
                    data2 += ',';
                }
                data2 += st;

            }
            StreamWriter sw = new StreamWriter("./html/number_year.html");
            sw.WriteLine(html1 + data1 + html2 + data2 + html3);
            sw.Flush();
            sw.Close();
        }
        void number_month()
        {
            string html1 = "<!DOCTYPE html>" + "\n" +
"<html>" + "\n" +
"<head>" + "\n" +
"    <meta charset=\"utf-8\">" + "\n" +
"<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"/>" +
"    <title>ECharts</title>" + "\n" +
"    <script src=\"echarts.min.js\" ></script>" + "\n" +
"</head>" + "\n" +
"<body>" + "\n" +
"    <div id=\"main\" style =\"width: 960px;height:540px;\"></div>" + "\n" +
"    <script type=\"text/javascript\" > " + "\n" +
"        var myChart = echarts.init(document.getElementById('main'));" + "\n" +
"            var option = {" + "\n" +
"            title: {" + "\n" +
"                text: '"; string html2 = "年每月论文数量统计'" + "\n" +
 "            }," + "\n" +
 "            tooltip: {}," + "\n" +
 "            legend: {" + "\n" +
 "                data:['数量']" + "\n" +
 "            }," + "\n" +
 "            xAxis: {" + "\n" +
 "                data: ["; string html3 = "]" + "\n" +
  "            }," + "\n" +
  "            yAxis: {}," + "\n" +
  "            series: [{" + "\n" +
  "                name: '数量'," + "\n" +
  "                type: 'bar'," + "\n" +
  "                data: ["; string html4 = "]" + "\n" +
   "            }]" + "\n" +
   "        };" + "\n" +
   "        myChart.setOption(option);" + "\n" +
   "    </script>" + "\n" +
   "</body>" + "\n" +
   "</html>";
            string data1 = "";
            string data2 = "";
            string st;
            string data1_2019 = "";
            string data2_2019 = "";
            string data1_2020 = "";
            string data2_2020 = "";
            for (int i = 2010; i <= 2020; i++)
            {
                StreamWriter sw = new StreamWriter("./html/number_month_" + i.ToString() + ".html");
                for (int j = 1; j <= 12; j++)
                {
                    StreamReader sr = new StreamReader("./data/number/" + i.ToString() + "/" + j.ToString() + ".txt");
                    st = sr.ReadLine();
                    sr.Close();
                    if (j > 1)
                    {
                        data1 += ',';
                        data2 += ',';
                    }
                    data1 += '\"';
                    data1 += (j.ToString() + "月");
                    data1 += '\"';
                    data2 += '\"';
                    data2 += st;
                    data2 += '\"';
                }
                if (i == 2019)
                {
                    data2_2019 = data2;
                }
                if (i == 2020)
                {
                    data2_2020 = data2;
                }
                sw.WriteLine(html1 + i.ToString() + html2 + data1 + html3 + data2 + html4);
                sw.Flush();
                sw.Close();
                data1 = "";
                data2 = "";
            }
            data1_2019 = "\"2019年1月\",\"2019年2月\",\"2019年3月\",\"2019年4月\",\"2019年5月\",\"2019年6月\",\"2019年7月\",\"2019年8月\",\"2019年9月\",\"2019年10月\",\"2019年11月\",\"2019年12月\"";
            data1_2020 = "\"2020年1月\",\"2020年2月\",\"2020年3月\",\"2020年4月\",\"2020年5月\",\"2020年6月\",\"2020年7月\",\"2020年8月\",\"2020年9月\",\"2020年10月\",\"2020年11月\",\"2020年12月\"";
            StreamWriter sww = new StreamWriter("./html/number_month_2019-2020.html");
            html3 = html3.Replace("'bar'", "'line'");

            sww.WriteLine(html1 + "2019-2020" + html2 + data1_2019 + ',' + data1_2020 + html3 + data2_2019 + ',' + data2_2020 + html4);
            sww.Flush();
            sww.Close();
        }
        void number_quarter()
        {
            string html1 = "<!DOCTYPE html>" + "\n" +
"<html>" + "\n" +
"<head>" + "\n" +
"    <meta charset=\"utf-8\">" + "\n" +
"<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"/>" +
"    <title>ECharts</title>" + "\n" +
"    <script src=\"echarts.min.js\" ></script>" + "\n" +
"</head>" + "\n" +
"<body>" + "\n" +
"    <div id=\"main\" style =\"width: 960px;height:540px;\"></div>" + "\n" +
"    <script type=\"text/javascript\" > " + "\n" +
"        var myChart = echarts.init(document.getElementById('main'));" + "\n" +
"            var option = {" + "\n" +
"            title: {" + "\n" +
"                text: '"; string html2 = "年每季论文数量统计'" + "\n" +
 "            }," + "\n" +
 "            tooltip: {}," + "\n" +
 "            legend: {" + "\n" +
 "                data:['数量']" + "\n" +
 "            }," + "\n" +
 "            xAxis: {" + "\n" +
 "                data: ["; string html3 = "]" + "\n" +
  "            }," + "\n" +
  "            yAxis: {}," + "\n" +
  "            series: [{" + "\n" +
  "                name: '数量'," + "\n" +
  "                type: 'bar'," + "\n" +
  "                data: ["; string html4 = "]" + "\n" +
   "            }]" + "\n" +
   "        };" + "\n" +
   "        myChart.setOption(option);" + "\n" +
   "    </script>" + "\n" +
   "</body>" + "\n" +
   "</html>";
            string data1 = "";
            string data2 = "";
            string st;
            string data1_2019 = "";
            string data2_2019 = "";
            string data1_2020 = "";
            string data2_2020 = "";
            for (int i = 2010; i <= 2020; i++)
            {

                StreamWriter sw = new StreamWriter("./html/number_quarter_" + i.ToString() + ".html");
                for (int j = 13; j <= 16; j++)
                {
                    StreamReader sr = new StreamReader("./data/number/" + i.ToString() + "/" + j.ToString() + ".txt");
                    st = sr.ReadLine();
                    sr.Close();
                    if (j > 13)
                    {
                        data1 += ',';
                        data2 += ',';
                    }
                    data1 += '\"';
                    data1 = data1 + "第" + ((j - 12).ToString() + "季度");
                    data1 += '\"';
                    data2 += '\"';
                    data2 += st;
                    data2 += '\"';

                }
                if (i == 2019)
                {
                    data2_2019 = data2;
                }
                if (i == 2020)
                {
                    data2_2020 = data2;
                }
                sw.WriteLine(html1 + i.ToString() + html2 + data1 + html3 + data2 + html4);
                sw.Flush();
                sw.Close();
                data1 = "";
                data2 = "";
            }
            data1_2019 = "\"2019年第1季度\",\"2019年第2季度\",\"2019年第3季度\",\"2019年第4季度\"";
            data1_2020 = "\"2020年第1季度\",\"2020年第2季度\",\"2020年第3季度\",\"2020年第4季度\"";
            StreamWriter sww = new StreamWriter("./html/number_quarter_2019-2020.html");
            html3 = html3.Replace("'bar'", "'line'");

            sww.WriteLine(html1 + "2019-2020" + html2 + data1_2019 + ',' + data1_2020 + html3 + data2_2019 + ',' + data2_2020 + html4);
            sww.Flush();
            sww.Close();
        }
        void keyword_prework()
        {
            string line;
            string key;
            string[] keyarr;

            for (int i = 2010; i <= 2020; i++)
            {
                StreamWriter q1 = new StreamWriter("./data/keyword/" + i.ToString() + "/13.txt");
                StreamWriter q2 = new StreamWriter("./data/keyword/" + i.ToString() + "/14.txt");
                StreamWriter q3 = new StreamWriter("./data/keyword/" + i.ToString() + "/15.txt");
                StreamWriter q4 = new StreamWriter("./data/keyword/" + i.ToString() + "/16.txt");
                StreamWriter ysw = new StreamWriter("./data/keyword/" + i.ToString() + "/0.txt");
                for (int j=1;j<=12;j++)
                {
                    StreamReader idsr = new StreamReader("./data/id/" + i.ToString() + "/" + j.ToString() + ".txt");
                    StreamWriter sw = new StreamWriter("./data/keyword/" + i.ToString() +"/"+j.ToString()+ ".txt");
                    while ((line = idsr.ReadLine()) != null)
                    {
                        StreamReader sr = new StreamReader("./temp/keyword/" + line + ".txt");
                        while ((key = sr.ReadLine()) != null)
                        {
                            if (key.IndexOf("Trial registration:", StringComparison.OrdinalIgnoreCase) != -1)
                            {
                                continue;
                            }
                            key = key.Replace("Keywords:", " ");
                            key = key.Replace('.', ' ');
                            while (true)
                            {
                                if (key.Replace("  ", " ").Length == key.Length)
                                {
                                    break;
                                }
                                else
                                {
                                    key = key.Replace("  ", " ");
                                }
                            }
                            keyarr = key.Split(';');
                            foreach (string ss in keyarr)
                            {
                                key = ss.Trim();
                                sw.WriteLine(key);
                                switch (j)
                                {
                                    case 1:q1.WriteLine(key); break;
                                    case 2: q1.WriteLine(key); break;
                                    case 3: q1.WriteLine(key); break;
                                    case 4: q2.WriteLine(key); break;
                                    case 5: q2.WriteLine(key); break;
                                    case 6: q2.WriteLine(key); break;
                                    case 7: q3.WriteLine(key); break;
                                    case 8: q3.WriteLine(key); break;
                                    case 9: q3.WriteLine(key); break;
                                    case 10: q4.WriteLine(key); break;
                                    case 11: q4.WriteLine(key); break;
                                    case 12: q4.WriteLine(key); break;
                                }
                                ysw.WriteLine(key);
                                //totsw.WriteLine(keystr);
                            }
                        }
                        sr.Close();
                    }
                    sw.Flush();
                    sw.Close();
                    idsr.Close();
                }
                ysw.Flush();
                ysw.Close();
                q1.Flush();
                q2.Flush();
                q3.Flush();
                q4.Flush();
                q1.Close();
                q2.Close();
                q3.Close();
                q4.Close();

            }


        }
        void wordcloud_keyword_year()
        {
            string html1 =
            "<!DOCTYPE html>" + "\n" +
"<html>" + "\n" +
"    <head>" + "\n" +
"        <meta charset=\"utf-8\">" + "\n" +
"<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"/>" + "\n" +
"        <script src='echarts.min-4.8.0.js'></script>" + "\n" +
"        <script src='echarts-wordcloud.min.js'></script>" + "\n" +
"    </head>" + "\n" +
"    <body>" + "\n" +
"        <style>" + "\n" +
"            html, body, #main {" + "\n" +
"                width: 100%;" + "\n" +
"                height: 100%;" + "\n" +
"                margin: 0;" + "\n" +
"            }" + "\n" +
"        </style>" + "\n" +
"        <div id='main'></div>" + "\n" +
"        <script>" + "\n" +
"            var chart = echarts.init(document.getElementById('main'));" + "\n" +
"            var keywords = {";
            string html2 =
"            };" + "\n" +
"            var data = [];" + "\n" +
"            for (var name in keywords) {" + "\n" +
"                data.push({" + "\n" +
"                    name: name," + "\n" +
"                    value: Math.sqrt(keywords[name])" + "\n" +
"                })" + "\n" +
"            }" + "\n" +
"            var maskImage = new Image();" + "\n" +
"            var option = {" + "\n" +
"	             title: {" + "\n" +
"                text: '"; string html3 = "论文keyword词云'" + "\n" +
 "            }," + "\n" +
 "                series: [ {" + "\n" +
 "                    type: 'wordCloud'," + "\n" +
 "                    sizeRange: [15, 100]," + "\n" +
 "                    rotationRange: [-90, 90]," + "\n" +
 "                    rotationStep: 45," + "\n" +
 "                    gridSize: 2," + "\n" +
 "                    shape: 'pentagon'," + "\n" +
 "                    maskImage: maskImage," + "\n" +
 "                    drawOutOfBound: false," + "\n" +
 "                    textStyle: {" + "\n" +
 "                        normal: {" + "\n" +
 "                            color: function () {" + "\n" +
 "                                return 'rgb(' + [" + "\n" +
 "                                    Math.round(Math.random() * 160)," + "\n" +
 "                                    Math.round(Math.random() * 160)," + "\n" +
 "                                    Math.round(Math.random() * 160)" + "\n" +
 "                                ].join(',') + ')';" + "\n" +
 "                            }" + "\n" +
 "                        }," + "\n" +
 "                        emphasis: {" + "\n" +
 "                            color: 'red'" + "\n" +
 "                        }" + "\n" +
 "                    }," + "\n" +
 "                    data: data.sort(function (a, b) {" + "\n" +
 "                        return b.value  - a.value;" + "\n" +
 "                    })" + "\n" +
 "                } ]" + "\n" +
 "            };" + "\n" +
 "            maskImage.onload = function () {" + "\n" +
 "                option.series[0].maskImage" + "\n" +
 "                chart.setOption(option);" + "\n" +
 "            }" + "\n" +
 "            maskImage.src = 'data:image/png;base64,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';" + "\n" +
 "            window.onresize = function () {" + "\n" +
 "                chart.resize();" + "\n" +
 "            }" + "\n" +
 "        </script>" + "\n" +
 "    </body>" + "\n" +
 "</html>";
            StreamReader sr1;
            StreamWriter sw;
            string line1;
            for (int i = 2010; i <= 2020; i++)
            {
                Dictionary<string, int> dic = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                sr1 = new StreamReader("./data/keyword/" + i.ToString() + "/0.txt");
                while ((line1 = sr1.ReadLine()) != null)
                {
                    if (dic.ContainsKey(line1))
                    {
                        dic[line1]++;
                    }
                    else
                    {
                        dic.Add(line1, 1);
                    }
                }
                sr1.Close();

                string data1 = "";
                string data2;
                int num = 0;
                foreach (var va in dic)
                {
                    if (va.Value < 2&&i>=2014)
                    {
                        continue;
                    }
                    if(va.Value<3&&i>=2017)
                    {
                        continue;
                    }
                    if (num > 0)
                    {
                        data1 += ',';
                        data1 += '\n';
                    }
                    data1 += '\"';
                    data1 += va.Key;
                    data1 += '\"';
                    data1 += ':';
                    data1 += va.Value;
                    num++;
                }
                data2 = i.ToString() + "年";
                sw = new StreamWriter("./html/wordcloud_keyword_year_" + i.ToString() + ".html");
                sw.WriteLine(html1 + data1 + html2 + data2 + html3);
                sw.Flush();
                sw.Close();
            }
        }
        void wordcloud_keyword_month()
        {
            string html1 =
            "<!DOCTYPE html>" + "\n" +
"<html>" + "\n" +
"    <head>" + "\n" +
"        <meta charset=\"utf-8\">" + "\n" +
"<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"/>" + "\n" +
"        <script src='echarts.min-4.8.0.js'></script>" + "\n" +
"        <script src='echarts-wordcloud.min.js'></script>" + "\n" +
"    </head>" + "\n" +
"    <body>" + "\n" +
"        <style>" + "\n" +
"            html, body, #main {" + "\n" +
"                width: 100%;" + "\n" +
"                height: 100%;" + "\n" +
"                margin: 0;" + "\n" +
"            }" + "\n" +
"        </style>" + "\n" +
"        <div id='main'></div>" + "\n" +
"        <script>" + "\n" +
"            var chart = echarts.init(document.getElementById('main'));" + "\n" +
"            var keywords = {";
            string html2 =
"            };" + "\n" +
"            var data = [];" + "\n" +
"            for (var name in keywords) {" + "\n" +
"                data.push({" + "\n" +
"                    name: name," + "\n" +
"                    value: Math.sqrt(keywords[name])" + "\n" +
"                })" + "\n" +
"            }" + "\n" +
"            var maskImage = new Image();" + "\n" +
"            var option = {" + "\n" +
"	             title: {" + "\n" +
"                text: '"; string html3 = "论文keyword词云'" + "\n" +
 "            }," + "\n" +
 "                series: [ {" + "\n" +
 "                    type: 'wordCloud'," + "\n" +
 "                    sizeRange: [15, 100]," + "\n" +
 "                    rotationRange: [-90, 90]," + "\n" +
 "                    rotationStep: 45," + "\n" +
 "                    gridSize: 2," + "\n" +
 "                    shape: 'pentagon'," + "\n" +
 "                    maskImage: maskImage," + "\n" +
 "                    drawOutOfBound: false," + "\n" +
 "                    textStyle: {" + "\n" +
 "                        normal: {" + "\n" +
 "                            color: function () {" + "\n" +
 "                                return 'rgb(' + [" + "\n" +
 "                                    Math.round(Math.random() * 160)," + "\n" +
 "                                    Math.round(Math.random() * 160)," + "\n" +
 "                                    Math.round(Math.random() * 160)" + "\n" +
 "                                ].join(',') + ')';" + "\n" +
 "                            }" + "\n" +
 "                        }," + "\n" +
 "                        emphasis: {" + "\n" +
 "                            color: 'red'" + "\n" +
 "                        }" + "\n" +
 "                    }," + "\n" +
 "                    data: data.sort(function (a, b) {" + "\n" +
 "                        return b.value  - a.value;" + "\n" +
 "                    })" + "\n" +
 "                } ]" + "\n" +
 "            };" + "\n" +
 "            maskImage.onload = function () {" + "\n" +
 "                option.series[0].maskImage" + "\n" +
 "                chart.setOption(option);" + "\n" +
 "            }" + "\n" +
 "            maskImage.src = 'data:image/png;base64,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';" + "\n" +
 "            window.onresize = function () {" + "\n" +
 "                chart.resize();" + "\n" +
 "            }" + "\n" +
 "        </script>" + "\n" +
 "    </body>" + "\n" +
 "</html>";

            StreamReader sr1;
            StreamWriter sw;
            string line1;

            for (int i = 2010; i <= 2020; i++)
            {
                for (int j = 1; j <= 12; j++)
                {
                    Dictionary<string, int> dic = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                    sr1 = new StreamReader("./data/keyword/" + i.ToString() + "/"+j.ToString()+".txt");
                    while ((line1 = sr1.ReadLine()) != null)
                    {
                        if (dic.ContainsKey(line1))
                        {
                            dic[line1]++;
                        }
                        else
                        {
                            dic.Add(line1, 1);
                        }
                    }
                    sr1.Close();
                    string data1 = "";
                    string data2;
                    int num = 0;
                    foreach (var va in dic)
                    {
                        //if (va.Value < 2)
                        //{
                        //    continue;
                        //}
                        if (num > 0)
                        {
                            data1 += ',';
                            data1 += '\n';
                        }
                        data1 += '\"';
                        data1 += va.Key;
                        data1 += '\"';
                        data1 += ':';
                        data1 += va.Value;
                        num++;
                    }
                    data2 = i.ToString() + "年" + j.ToString() + "月";
                    sw = new StreamWriter("./html/wordcloud_keyword_month_" + i.ToString() + "_" + j.ToString() + ".html");
                    sw.WriteLine(html1 + data1 + html2 + data2 + html3);
                    sw.Flush();
                    sw.Close();
                }
            }
        }
        void wordcloud_keyword_quarter()
        {
            string html1 =
            "<!DOCTYPE html>" + "\n" +
"<html>" + "\n" +
"    <head>" + "\n" +
"        <meta charset=\"utf-8\">" + "\n" +
"<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"/>" + "\n" +
"        <script src='echarts.min-4.8.0.js'></script>" + "\n" +
"        <script src='echarts-wordcloud.min.js'></script>" + "\n" +
"    </head>" + "\n" +
"    <body>" + "\n" +
"        <style>" + "\n" +
"            html, body, #main {" + "\n" +
"                width: 100%;" + "\n" +
"                height: 100%;" + "\n" +
"                margin: 0;" + "\n" +
"            }" + "\n" +
"        </style>" + "\n" +
"        <div id='main'></div>" + "\n" +
"        <script>" + "\n" +
"            var chart = echarts.init(document.getElementById('main'));" + "\n" +
"            var keywords = {";
            string html2 =
"            };" + "\n" +
"            var data = [];" + "\n" +
"            for (var name in keywords) {" + "\n" +
"                data.push({" + "\n" +
"                    name: name," + "\n" +
"                    value: Math.sqrt(keywords[name])" + "\n" +
"                })" + "\n" +
"            }" + "\n" +
"            var maskImage = new Image();" + "\n" +
"            var option = {" + "\n" +
"	             title: {" + "\n" +
"                text: '"; string html3 = "论文keyword词云'" + "\n" +
 "            }," + "\n" +
 "                series: [ {" + "\n" +
 "                    type: 'wordCloud'," + "\n" +
 "                    sizeRange: [15, 100]," + "\n" +
 "                    rotationRange: [-90, 90]," + "\n" +
 "                    rotationStep: 45," + "\n" +
 "                    gridSize: 2," + "\n" +
 "                    shape: 'pentagon'," + "\n" +
 "                    maskImage: maskImage," + "\n" +
 "                    drawOutOfBound: false," + "\n" +
 "                    textStyle: {" + "\n" +
 "                        normal: {" + "\n" +
 "                            color: function () {" + "\n" +
 "                                return 'rgb(' + [" + "\n" +
 "                                    Math.round(Math.random() * 160)," + "\n" +
 "                                    Math.round(Math.random() * 160)," + "\n" +
 "                                    Math.round(Math.random() * 160)" + "\n" +
 "                                ].join(',') + ')';" + "\n" +
 "                            }" + "\n" +
 "                        }," + "\n" +
 "                        emphasis: {" + "\n" +
 "                            color: 'red'" + "\n" +
 "                        }" + "\n" +
 "                    }," + "\n" +
 "                    data: data.sort(function (a, b) {" + "\n" +
 "                        return b.value  - a.value;" + "\n" +
 "                    })" + "\n" +
 "                } ]" + "\n" +
 "            };" + "\n" +
 "            maskImage.onload = function () {" + "\n" +
 "                option.series[0].maskImage" + "\n" +
 "                chart.setOption(option);" + "\n" +
 "            }" + "\n" +
 "            maskImage.src = 'data:image/png;base64,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';" + "\n" +
 "            window.onresize = function () {" + "\n" +
 "                chart.resize();" + "\n" +
 "            }" + "\n" +
 "        </script>" + "\n" +
 "    </body>" + "\n" +
 "</html>";
            StreamReader sr1;
            StreamWriter sw;
            string line1;
            for (int i = 2010; i <= 2020; i++)
            {
                for (int j = 13; j <= 16; j++)
                {
                    Dictionary<string, int> dic = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                    sr1 = new StreamReader("./data/keyword/" + i.ToString()+"/"+j.ToString()+ ".txt");
                    while ((line1 = sr1.ReadLine()) != null)
                    {
                        if (dic.ContainsKey(line1))
                        {
                            dic[line1]++;
                        }
                        else
                        {
                            dic.Add(line1, 1);
                        }
                    }
                    sr1.Close();
                    string data1 = "";
                    string data2;
                    int num = 0;
                    foreach (var va in dic)
                    {
                        //if (va.Value < 2)
                        //{
                        //    continue;
                        //}
                        if (num > 0)
                        {
                            data1 += ',';
                            data1 += '\n';
                        }
                        data1 += '\"';
                        data1 += va.Key;
                        data1 += '\"';
                        data1 += ':';
                        data1 += va.Value;
                        num++;
                    }
                    data2 = i.ToString() + "年第" + (j - 12).ToString() + "季度";
                    sw = new StreamWriter("./html/wordcloud_keyword_quarter_" + i.ToString() + "_" + (j - 12).ToString() + ".html");
                    sw.WriteLine(html1 + data1 + html2 + data2 + html3);
                    sw.Flush();
                    sw.Close();
                }
            }
        }

        void abstract_prework()
        {
            StreamReader stop = new StreamReader("./data/stop.txt");
            List<string> stopl = new List<string>();
            string line0;
            while ((line0 = stop.ReadLine()) != null)
            {
                stopl.Add(line0);
            }
            for(int i=0;i<100;i++)
            {
                stopl.Add(i.ToString());
            }
            stop.Close();

            string line;
            string key;
            string[] keyarr;
            bool endflag;
            for (int i = 2010; i <= 2020; i++)
            {
                StreamWriter q1 = new StreamWriter("./data/abstract/" + i.ToString() + "/13.txt");
                StreamWriter q2 = new StreamWriter("./data/abstract/" + i.ToString() + "/14.txt");
                StreamWriter q3 = new StreamWriter("./data/abstract/" + i.ToString() + "/15.txt");
                StreamWriter q4 = new StreamWriter("./data/abstract/" + i.ToString() + "/16.txt");
                StreamWriter ysw = new StreamWriter("./data/abstract/" + i.ToString() + "/0.txt");
                for (int j = 1; j <= 12; j++)
                {
                    StreamReader idsr = new StreamReader("./data/id/" + i.ToString() + "/" + j.ToString() + ".txt");
                    StreamWriter sw = new StreamWriter("./data/abstract/" + i.ToString() + "/" + j.ToString() + ".txt");
                    while ((line = idsr.ReadLine()) != null)
                    {
                        StreamReader sr = new StreamReader("./temp/abstract/" + line + ".txt");
                        endflag = false;
                        while ((key = sr.ReadLine()) != null)
                        {
                            if(key=="")
                            {
                                continue;
                            }
                            endflag = true;
                            key = key.Replace(',', ' ');
                            key = key.Replace('.', ' ');
                            key = key.Replace('(', ' ');
                            key = key.Replace(')', ' ');
                            key = key.Replace(';', ' ');
                            key = key.Replace(':', ' ');
                            key = key.Replace('\'', ' ');
                            key = key.Replace('\"', ' ');
                            key = key.Replace('/', ' ');
                            key = key.Replace('-', ' ');
                            key = key.Replace('‑', ' ');
                            key = key.Replace('%', ' ');
                            key=System.Text.RegularExpressions.Regex.Replace(key, @"\d", "");
                            key = key.Replace("&", "");
                            key = key.Replace("±", "");
                            key = key.Replace("=", "");
                            key = key.Replace("[", "");
                            key = key.Replace("]", "");

                            while (true)
                            {
                                if (key.Replace("  ", " ").Length == key.Length)
                                {
                                    break;
                                }
                                else
                                {
                                    key = key.Replace("  ", " ");
                                }
                            }
                            key = key.Trim();
                            keyarr = key.Split(' ');
                            foreach (string ss in keyarr)
                            {
                                key = ss.Trim();
                                if (stopl.Contains(key, StringComparer.OrdinalIgnoreCase))
                                {
                                    continue;
                                }
                                if(key.Length<=2)
                                {
                                    continue;
                                }
                                sw.WriteLine(key); 
                                switch (j)
                                {
                                    case 1: q1.WriteLine(key); break;
                                    case 2: q1.WriteLine(key); break;
                                    case 3: q1.WriteLine(key); break;
                                    case 4: q2.WriteLine(key); break;
                                    case 5: q2.WriteLine(key); break;
                                    case 6: q2.WriteLine(key); break;
                                    case 7: q3.WriteLine(key); break;
                                    case 8: q3.WriteLine(key); break;
                                    case 9: q3.WriteLine(key); break;
                                    case 10: q4.WriteLine(key); break;
                                    case 11: q4.WriteLine(key); break;
                                    case 12: q4.WriteLine(key); break;
                                }
                                ysw.WriteLine(key);
                                //totsw.WriteLine(keystr);
                            }
                        }
                        
                        if(endflag)
                        {
                            sw.WriteLine("endflag");
                            ysw.WriteLine("endflag"); 
                            switch (j)
                            {
                                case 1: q1.WriteLine("endflag"); break;
                                case 2: q1.WriteLine("endflag"); break;
                                case 3: q1.WriteLine("endflag"); break;
                                case 4: q2.WriteLine("endflag"); break;
                                case 5: q2.WriteLine("endflag"); break;
                                case 6: q2.WriteLine("endflag"); break;
                                case 7: q3.WriteLine("endflag"); break;
                                case 8: q3.WriteLine("endflag"); break;
                                case 9: q3.WriteLine("endflag"); break;
                                case 10: q4.WriteLine("endflag"); break;
                                case 11: q4.WriteLine("endflag"); break;
                                case 12: q4.WriteLine("endflag"); break;
                            }
                            endflag = false;
                        }
                        sr.Close();

                    }
                    sw.Flush();
                    sw.Close();
                    idsr.Close();
                }
                ysw.Flush();
                ysw.Close();
                q1.Flush();
                q2.Flush();
                q3.Flush();
                q4.Flush();
                q1.Close();
                q2.Close();
                q3.Close();
                q4.Close();
            }

        }
        public class word
        {
            public word()
            {
                num = 0;
                intxtnum = 0;
                notadd = true;
            }
            
            public int num;//某一word的频率
            public bool notadd;//在某一文章中，已加入此word计数
            public int intxtnum;//多少文章有此word
        }


        void wordcloud_abstract_year()
        {
            string html1 =
            "<!DOCTYPE html>" + "\n" +
"<html>" + "\n" +
"    <head>" + "\n" +
"        <meta charset=\"utf-8\">" + "\n" +
"<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"/>" + "\n" +
"        <script src='echarts.min-4.8.0.js'></script>" + "\n" +
"        <script src='echarts-wordcloud.min.js'></script>" + "\n" +
"    </head>" + "\n" +
"    <body>" + "\n" +
"        <style>" + "\n" +
"            html, body, #main {" + "\n" +
"                width: 100%;" + "\n" +
"                height: 100%;" + "\n" +
"                margin: 0;" + "\n" +
"            }" + "\n" +
"        </style>" + "\n" +
"        <div id='main'></div>" + "\n" +
"        <script>" + "\n" +
"            var chart = echarts.init(document.getElementById('main'));" + "\n" +
"            var keywords = {";
            string html2 =
"            };" + "\n" +
"            var data = [];" + "\n" +
"            for (var name in keywords) {" + "\n" +
"                data.push({" + "\n" +
"                    name: name," + "\n" +
"                    value: Math.sqrt(keywords[name])" + "\n" +
"                })" + "\n" +
"            }" + "\n" +
"            var maskImage = new Image();" + "\n" +
"            var option = {" + "\n" +
"	             title: {" + "\n" +
"                text: '"; string html3 = "论文abstract词云'" + "\n" +
 "            }," + "\n" +
 "                series: [ {" + "\n" +
 "                    type: 'wordCloud'," + "\n" +
 "                    sizeRange: [15, 100]," + "\n" +
 "                    rotationRange: [-90, 90]," + "\n" +
 "                    rotationStep: 45," + "\n" +
 "                    gridSize: 2," + "\n" +
 "                    shape: 'pentagon'," + "\n" +
 "                    maskImage: maskImage," + "\n" +
 "                    drawOutOfBound: false," + "\n" +
 "                    textStyle: {" + "\n" +
 "                        normal: {" + "\n" +
 "                            color: function () {" + "\n" +
 "                                return 'rgb(' + [" + "\n" +
 "                                    Math.round(Math.random() * 160)," + "\n" +
 "                                    Math.round(Math.random() * 160)," + "\n" +
 "                                    Math.round(Math.random() * 160)" + "\n" +
 "                                ].join(',') + ')';" + "\n" +
 "                            }" + "\n" +
 "                        }," + "\n" +
 "                        emphasis: {" + "\n" +
 "                            color: 'red'" + "\n" +
 "                        }" + "\n" +
 "                    }," + "\n" +
 "                    data: data.sort(function (a, b) {" + "\n" +
 "                        return b.value  - a.value;" + "\n" +
 "                    })" + "\n" +
 "                } ]" + "\n" +
 "            };" + "\n" +
 "            maskImage.onload = function () {" + "\n" +
 "                option.series[0].maskImage" + "\n" +
 "                chart.setOption(option);" + "\n" +
 "            }" + "\n" +
 "            maskImage.src = 'data:image/png;base64,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';" + "\n" +
 "            window.onresize = function () {" + "\n" +
 "                chart.resize();" + "\n" +
 "            }" + "\n" +
 "        </script>" + "\n" +
 "    </body>" + "\n" +
 "</html>";
            StreamReader sr1;
            StreamWriter sw;
            string line1;
            int totnum = 0;
            int totwordnum = 0;
            for (int i = 2010; i <= 2020; i++)
            {
                sr1 = new StreamReader("./data/number/" + i.ToString() + "/0.txt");
                line1 = sr1.ReadLine();
                totnum = int.Parse(line1);
                Dictionary<string, word> dic = new Dictionary<string, word>(StringComparer.OrdinalIgnoreCase);

                sr1 = new StreamReader("./data/abstract/" + i.ToString() + "/0.txt");
                while ((line1 = sr1.ReadLine()) != null)
                {
                    if(line1=="endflag")
                    {
                        foreach(var va in dic)
                        {
                            dic[va.Key].notadd = true;
                        }
                        continue;
                    }
                    else
                    {
                        totwordnum++;
                    }
                    if (dic.ContainsKey(line1))
                    {
                        dic[line1].num++;
                        if (dic[line1].notadd == true)
                        {
                            dic[line1].intxtnum++;
                            dic[line1].notadd = false;
                        }
                    }
                    else
                    {
                        word temp = new word();
                        temp.num++;
                        temp.notadd = false;
                        temp.intxtnum++;
                        dic.Add(line1, temp);
                    }
                }
                sr1.Close();

                string data1 = "";
                string data2;
                int num = 0;
                foreach (var va in dic)
                {
                    //if (va.Value < 2)
                    //{
                    //    continue;
                    //}
                    
                    double tf = (double)va.Value.num / (double)totwordnum;
                    double idf = Math.Log((double)totnum / (double)(va.Value.intxtnum + 1), 2);
                    if(tf*idf<=0.002)
                    {
                        continue;
                    }
                    if (num > 0)
                    {
                        data1 += ',';
                        data1 += '\n';
                    }
                    data1 += '\"';
                    data1 += va.Key;
                    data1 += '\"';
                    data1 += ':';
                    data1 += tf*idf; 
                    num++;
                }
                data2 = i.ToString() + "年";
                sw = new StreamWriter("./html/wordcloud_abstract_year_" + i.ToString() + ".html");
                sw.WriteLine(html1 + data1 + html2 + data2 + html3);
                sw.Flush();
                sw.Close();
                totwordnum = 0;
            }
        }

        void wordcloud_abstract_month()
        {

            string html1 =
            "<!DOCTYPE html>" + "\n" +
"<html>" + "\n" +
"    <head>" + "\n" +
"        <meta charset=\"utf-8\">" + "\n" +
"<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"/>" + "\n" +
"        <script src='echarts.min-4.8.0.js'></script>" + "\n" +
"        <script src='echarts-wordcloud.min.js'></script>" + "\n" +
"    </head>" + "\n" +
"    <body>" + "\n" +
"        <style>" + "\n" +
"            html, body, #main {" + "\n" +
"                width: 100%;" + "\n" +
"                height: 100%;" + "\n" +
"                margin: 0;" + "\n" +
"            }" + "\n" +
"        </style>" + "\n" +
"        <div id='main'></div>" + "\n" +
"        <script>" + "\n" +
"            var chart = echarts.init(document.getElementById('main'));" + "\n" +
"            var keywords = {";
            string html2 =
"            };" + "\n" +
"            var data = [];" + "\n" +
"            for (var name in keywords) {" + "\n" +
"                data.push({" + "\n" +
"                    name: name," + "\n" +
"                    value: Math.sqrt(keywords[name])" + "\n" +
"                })" + "\n" +
"            }" + "\n" +
"            var maskImage = new Image();" + "\n" +
"            var option = {" + "\n" +
"	             title: {" + "\n" +
"                text: '"; string html3 = "论文abstract词云'" + "\n" +
 "            }," + "\n" +
 "                series: [ {" + "\n" +
 "                    type: 'wordCloud'," + "\n" +
 "                    sizeRange: [15, 100]," + "\n" +
 "                    rotationRange: [-90, 90]," + "\n" +
 "                    rotationStep: 45," + "\n" +
 "                    gridSize: 2," + "\n" +
 "                    shape: 'pentagon'," + "\n" +
 "                    maskImage: maskImage," + "\n" +
 "                    drawOutOfBound: false," + "\n" +
 "                    textStyle: {" + "\n" +
 "                        normal: {" + "\n" +
 "                            color: function () {" + "\n" +
 "                                return 'rgb(' + [" + "\n" +
 "                                    Math.round(Math.random() * 160)," + "\n" +
 "                                    Math.round(Math.random() * 160)," + "\n" +
 "                                    Math.round(Math.random() * 160)" + "\n" +
 "                                ].join(',') + ')';" + "\n" +
 "                            }" + "\n" +
 "                        }," + "\n" +
 "                        emphasis: {" + "\n" +
 "                            color: 'red'" + "\n" +
 "                        }" + "\n" +
 "                    }," + "\n" +
 "                    data: data.sort(function (a, b) {" + "\n" +
 "                        return b.value  - a.value;" + "\n" +
 "                    })" + "\n" +
 "                } ]" + "\n" +
 "            };" + "\n" +
 "            maskImage.onload = function () {" + "\n" +
 "                option.series[0].maskImage" + "\n" +
 "                chart.setOption(option);" + "\n" +
 "            }" + "\n" +
 "            maskImage.src = 'data:image/png;base64,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';" + "\n" +
 "            window.onresize = function () {" + "\n" +
 "                chart.resize();" + "\n" +
 "            }" + "\n" +
 "        </script>" + "\n" +
 "    </body>" + "\n" +
 "</html>";
            StreamReader sr1;
            StreamWriter sw;
            string line1;
            int totnum = 0;
            int totwordnum = 0;
            for (int i = 2010; i <= 2020; i++)
            {
                for(int j=1;j<=12;j++)
                {
                    sr1 = new StreamReader("./data/number/" + i.ToString() + "/"+j.ToString()+".txt");
                    line1 = sr1.ReadLine();
                    totnum = int.Parse(line1);
                    Dictionary<string, word> dic = new Dictionary<string, word>(StringComparer.OrdinalIgnoreCase);

                    sr1 = new StreamReader("./data/abstract/" + i.ToString() + "/"+j.ToString()+".txt");
                    while ((line1 = sr1.ReadLine()) != null)
                    {
                        if (line1 == "endflag")
                        {
                            foreach (var va in dic)
                            {
                                dic[va.Key].notadd = true;
                            }
                            continue;
                        }
                        else
                        {
                            totwordnum++;
                        }
                        if (dic.ContainsKey(line1))
                        {
                            dic[line1].num++;
                            if (dic[line1].notadd == true)
                            {
                                dic[line1].intxtnum++;
                                dic[line1].notadd = false;
                            }
                        }
                        else
                        {
                            word temp = new word();
                            temp.num++;
                            temp.notadd = false;
                            temp.intxtnum++;
                            dic.Add(line1, temp);
                        }
                    }
                    sr1.Close();

                    string data1 = "";
                    string data2;
                    int num = 0;
                    foreach (var va in dic)
                    {
                        //if (va.Value < 2)
                        //{
                        //    continue;
                        //}

                        double tf = (double)va.Value.num / (double)totwordnum;
                        double idf = Math.Log((double)totnum / (double)(va.Value.intxtnum + 1), 2);
                        if (tf * idf <= 0.002)
                        {
                            continue;
                        }
                        if (num > 0)
                        {
                            data1 += ',';
                            data1 += '\n';
                        }
                        data1 += '\"';
                        data1 += va.Key;
                        data1 += '\"';
                        data1 += ':';
                        data1 += tf * idf;
                        num++;
                    }
                    data2 = i.ToString() + "年"+j.ToString()+"月";
                    sw = new StreamWriter("./html/wordcloud_abstract_month_" + i.ToString() +"_"+j.ToString()+ ".html");
                    sw.WriteLine(html1 + data1 + html2 + data2 + html3);
                    sw.Flush();
                    sw.Close();
                    totwordnum = 0;
                }
            }
        }

        void wordcloud_abstract_quarter()
        {
            string html1 =
            "<!DOCTYPE html>" + "\n" +
"<html>" + "\n" +
"    <head>" + "\n" +
"        <meta charset=\"utf-8\">" + "\n" +
"<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"/>" + "\n" +
"        <script src='echarts.min-4.8.0.js'></script>" + "\n" +
"        <script src='echarts-wordcloud.min.js'></script>" + "\n" +
"    </head>" + "\n" +
"    <body>" + "\n" +
"        <style>" + "\n" +
"            html, body, #main {" + "\n" +
"                width: 100%;" + "\n" +
"                height: 100%;" + "\n" +
"                margin: 0;" + "\n" +
"            }" + "\n" +
"        </style>" + "\n" +
"        <div id='main'></div>" + "\n" +
"        <script>" + "\n" +
"            var chart = echarts.init(document.getElementById('main'));" + "\n" +
"            var keywords = {";
            string html2 =
"            };" + "\n" +
"            var data = [];" + "\n" +
"            for (var name in keywords) {" + "\n" +
"                data.push({" + "\n" +
"                    name: name," + "\n" +
"                    value: Math.sqrt(keywords[name])" + "\n" +
"                })" + "\n" +
"            }" + "\n" +
"            var maskImage = new Image();" + "\n" +
"            var option = {" + "\n" +
"	             title: {" + "\n" +
"                text: '"; string html3 = "论文abstract词云'" + "\n" +
 "            }," + "\n" +
 "                series: [ {" + "\n" +
 "                    type: 'wordCloud'," + "\n" +
 "                    sizeRange: [15, 100]," + "\n" +
 "                    rotationRange: [-90, 90]," + "\n" +
 "                    rotationStep: 45," + "\n" +
 "                    gridSize: 2," + "\n" +
 "                    shape: 'pentagon'," + "\n" +
 "                    maskImage: maskImage," + "\n" +
 "                    drawOutOfBound: false," + "\n" +
 "                    textStyle: {" + "\n" +
 "                        normal: {" + "\n" +
 "                            color: function () {" + "\n" +
 "                                return 'rgb(' + [" + "\n" +
 "                                    Math.round(Math.random() * 160)," + "\n" +
 "                                    Math.round(Math.random() * 160)," + "\n" +
 "                                    Math.round(Math.random() * 160)" + "\n" +
 "                                ].join(',') + ')';" + "\n" +
 "                            }" + "\n" +
 "                        }," + "\n" +
 "                        emphasis: {" + "\n" +
 "                            color: 'red'" + "\n" +
 "                        }" + "\n" +
 "                    }," + "\n" +
 "                    data: data.sort(function (a, b) {" + "\n" +
 "                        return b.value  - a.value;" + "\n" +
 "                    })" + "\n" +
 "                } ]" + "\n" +
 "            };" + "\n" +
 "            maskImage.onload = function () {" + "\n" +
 "                option.series[0].maskImage" + "\n" +
 "                chart.setOption(option);" + "\n" +
 "            }" + "\n" +
 "            maskImage.src = 'data:image/png;base64,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';" + "\n" +
 "            window.onresize = function () {" + "\n" +
 "                chart.resize();" + "\n" +
 "            }" + "\n" +
 "        </script>" + "\n" +
 "    </body>" + "\n" +
 "</html>";
            StreamReader sr1;
            StreamWriter sw;
            string line1;
            int totnum = 0;
            int totwordnum = 0;
            for (int i = 2010; i <= 2020; i++)
            {
                for (int j = 13; j <= 16; j++)
                {
                    sr1 = new StreamReader("./data/number/" + i.ToString() + "/" + j.ToString() + ".txt");
                    line1 = sr1.ReadLine();
                    totnum = int.Parse(line1);
                    Dictionary<string, word> dic = new Dictionary<string, word>(StringComparer.OrdinalIgnoreCase);

                    sr1 = new StreamReader("./data/abstract/" + i.ToString() + "/" + j.ToString() + ".txt");
                    while ((line1 = sr1.ReadLine()) != null)
                    {
                        if (line1 == "endflag")
                        {
                            foreach (var va in dic)
                            {
                                dic[va.Key].notadd = true;
                            }
                            continue;
                        }
                        else
                        {
                            totwordnum++;
                        }
                        if (dic.ContainsKey(line1))
                        {
                            dic[line1].num++;
                            if (dic[line1].notadd == true)
                            {
                                dic[line1].intxtnum++;
                                dic[line1].notadd = false;
                            }
                        }
                        else
                        {
                            word temp = new word();
                            temp.num++;
                            temp.notadd = false;
                            temp.intxtnum++;
                            dic.Add(line1, temp);
                        }
                    }
                    sr1.Close();

                    string data1 = "";
                    string data2;
                    int num = 0;
                    foreach (var va in dic)
                    {
                        //if (va.Value < 2)
                        //{
                        //    continue;
                        //}

                        double tf = (double)va.Value.num / (double)totwordnum;
                        double idf = Math.Log((double)totnum / (double)(va.Value.intxtnum + 1), 2);
                        if (tf * idf <= 0.002)
                        {
                            continue;
                        }
                        if (num > 0)
                        {
                            data1 += ',';
                            data1 += '\n';
                        }
                        data1 += '\"';
                        data1 += va.Key;
                        data1 += '\"';
                        data1 += ':';
                        data1 += tf * idf;
                        num++;
                    }
                    data2 = i.ToString() + "年第" + (j-12).ToString() + "季度";
                    sw = new StreamWriter("./html/wordcloud_abstract_quarter_" + i.ToString() + "_" + (j-12).ToString() + ".html");
                    sw.WriteLine(html1 + data1 + html2 + data2 + html3);
                    sw.Flush();
                    sw.Close();
                    totwordnum = 0;
                }
            }
        }



        List<string> combol1;
        List<string> href1;
        List<string> combol2;
        List<string> href2;
        List<string> combol3;
        List<string> href3;
        string type;
        void combobox_prework()
        {
            type = "";
            href1 = new List<string>();
            href2 = new List<string>();
            href3 = new List<string>();
            combol1 = new List<string>();
            combol2 = new List<string>();
            combol3 = new List<string>();
            combol1.Add("2010-2020年论文数量统计");
            href1.Add("/html/number_year.html");
            combol1.Add("2019-2020年每月论文数量统计");
            href1.Add("/html/number_month_2019-2020.html");
            combol1.Add("2019-2020年每季度论文数量统计");
            href1.Add("/html/number_quarter_2019-2020.html");
            combol1.Add("2010年每月论文数量统计");
            href1.Add("/html/number_month_2010.html");
            combol1.Add("2011年每月论文数量统计");
            href1.Add("/html/number_month_2011.html");
            combol1.Add("2012年每月论文数量统计");
            href1.Add("/html/number_month_2012.html");
            combol1.Add("2013年每月论文数量统计");
            href1.Add("/html/number_month_2013.html");
            combol1.Add("2014年每月论文数量统计");
            href1.Add("/html/number_month_2014.html");
            combol1.Add("2015年每月论文数量统计");
            href1.Add("/html/number_month_2015.html");
            combol1.Add("2016年每月论文数量统计");
            href1.Add("/html/number_month_2016.html");
            combol1.Add("2017年每月论文数量统计");
            href1.Add("/html/number_month_2017.html");
            combol1.Add("2018年每月论文数量统计");
            href1.Add("/html/number_month_2018.html");
            combol1.Add("2019年每月论文数量统计");
            href1.Add("/html/number_month_2019.html");
            combol1.Add("2020年每月论文数量统计");
            href1.Add("/html/number_month_2020.html");
            combol1.Add("2010年每季度论文数量统计");
            href1.Add("/html/number_quarter_2010.html");
            combol1.Add("2011年每季度论文数量统计");
            href1.Add("/html/number_quarter_2011.html");
            combol1.Add("2012年每季度论文数量统计");
            href1.Add("/html/number_quarter_2012.html");
            combol1.Add("2013年每季度论文数量统计");
            href1.Add("/html/number_quarter_2013.html");
            combol1.Add("2014年每季度论文数量统计");
            href1.Add("/html/number_quarter_2014.html");
            combol1.Add("2015年每季度论文数量统计");
            href1.Add("/html/number_quarter_2015.html");
            combol1.Add("2016年每季度论文数量统计");
            href1.Add("/html/number_quarter_2016.html");
            combol1.Add("2017年每季度论文数量统计");
            href1.Add("/html/number_quarter_2017.html");
            combol1.Add("2018年每季度论文数量统计");
            href1.Add("/html/number_quarter_2018.html");
            combol1.Add("2019年每季度论文数量统计");
            href1.Add("/html/number_quarter_2019.html");
            combol1.Add("2020年每季度论文数量统计");
            href1.Add("/html/number_quarter_2020.html");

            combol2.Add("2010年");
            href2.Add("2010");
            combol2.Add("2011年");
            href2.Add("2011");
            combol2.Add("2012年");
            href2.Add("2012");
            combol2.Add("2013年");
            href2.Add("2013");
            combol2.Add("2014年");
            href2.Add("2014");
            combol2.Add("2015年");
            href2.Add("2015");
            combol2.Add("2016年");
            href2.Add("2016");
            combol2.Add("2017年");
            href2.Add("2017");
            combol2.Add("2018年");
            href2.Add("2018");
            combol2.Add("2019年");
            href2.Add("2019");
            combol2.Add("2020年");
            href2.Add("2020");

            combol3.Add("全年论文词云");
            href3.Add("/html/wordcloud+year++");
            combol3.Add("1月论文词云");
            href3.Add("/html/wordcloud+month+_1");
            combol3.Add("2月论文词云");
            href3.Add("/html/wordcloud+month+_2");
            combol3.Add("3月论文词云");
            href3.Add("/html/wordcloud+month+_3");
            combol3.Add("4月论文词云");
            href3.Add("/html/wordcloud+month+_4");
            combol3.Add("5月论文词云");
            href3.Add("/html/wordcloud+month+_5");
            combol3.Add("6月论文词云");
            href3.Add("/html/wordcloud+month+_6");
            combol3.Add("7月论文词云");
            href3.Add("/html/wordcloud+month+_7");
            combol3.Add("8月论文词云");
            href3.Add("/html/wordcloud+month+_8");
            combol3.Add("9月论文词云");
            href3.Add("/html/wordcloud+month+_9");
            combol3.Add("10月论文词云");
            href3.Add("/html/wordcloud+month+_10");
            combol3.Add("11月论文词云");
            href3.Add("/html/wordcloud+month+_11");
            combol3.Add("12月论文词云");
            href3.Add("/html/wordcloud+month+_12");
            combol3.Add("第1季度论文词云");
            href3.Add("/html/wordcloud+quarter+_1");
            combol3.Add("第2季度论文词云");
            href3.Add("/html/wordcloud+quarter+_2");
            combol3.Add("第3季度论文词云");
            href3.Add("/html/wordcloud+quarter+_3");
            combol3.Add("第4季度论文词云");
            href3.Add("/html/wordcloud+quarter+_4");


            comboBox1.DataSource = combol1;
            comboBox2.DataSource = combol2;
            comboBox3.DataSource = combol3;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            for(int i=0;i<combol1.Count;i++)
            {
                if(comboBox1.Text==combol1[i])
                {
                    webBrowser1.Url = new Uri(Application.StartupPath + href1[i]);
                    break;
                }
            }
        }


        private void button5_Click(object sender, EventArgs e)
        {
            type = "keyword";
            string s1="";
            for(int i=0;i<combol2.Count;i++)
            {
                if(comboBox2.Text==combol2[i])
                {
                    s1 = href2[i];
                    break;
                }
            }
            string s2="";
            for(int i=0;i<combol3.Count;i++)
            {
                if(comboBox3.Text==combol3[i])
                {
                    s2 = href3[i];
                    break;
                }
            }
            string[] arr = s2.Split('+');
            string url = Application.StartupPath + arr[0] + '_' + type + '_' + arr[1] + '_' + s1  + arr[2]+".html";
            webBrowser1.Url = new Uri(url);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            type = "abstract";
            string s1 = "";
            for (int i = 0; i < combol2.Count; i++)
            {
                if (comboBox2.Text == combol2[i])
                {
                    s1 = href2[i];
                    break;
                }
            }
            string s2 = "";
            for (int i = 0; i < combol3.Count; i++)
            {
                if (comboBox3.Text == combol3[i])
                {
                    s2 = href3[i];
                    break;
                }
            }
            string[] arr = s2.Split('+');
            string url = Application.StartupPath + arr[0] + '_' + type + '_' + arr[1] + '_' + s1  + arr[2] + ".html";
            webBrowser1.Url = new Uri(url);
        }
    }
}
