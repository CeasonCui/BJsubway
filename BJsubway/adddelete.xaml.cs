using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Navigation;

namespace BJsubway
{
    /// <summary>
    /// adddelete.xaml 的交互逻辑
    /// </summary>
    public partial class adddelete : Window
    {
        int sts_master = 0;
        int change = 0;
        ArrayList lines_all
        {
            get;
            set;
        }

        ArrayList sts_all
        {
            get;
            set;
        }

        public adddelete(ArrayList lines, ArrayList sts)
        {
            InitializeComponent();
            lines_all = lines;
            sts_all = sts;
        }

        public bool IsNumberic(string oText)//判断字符串是否为数字
        {
            try
            {
                int Number = Convert.ToInt32(oText);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void SaveCSV(ArrayList lines, ArrayList sts, string fullPath1, string fullPath2)
        {
            FileInfo fi1 = new FileInfo(fullPath1);
            if (!fi1.Directory.Exists)
            {
                fi1.Directory.Create();
            }
            FileStream fs1 = new FileStream(fullPath1, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            StreamWriter sw1 = new StreamWriter(fs1, System.Text.Encoding.UTF8);
            string data = "";
            for (int i = 0; i < sts.Count; i++)
            {
                ArrayList array_sts = new ArrayList();
                array_sts.Add((sts[i] as Station).name);
                array_sts.Add("1");
                array_sts.Add(Convert.ToString((sts[i] as Station).x));
                array_sts.Add(Convert.ToString((sts[i] as Station).y));
                for(int m=0;m< (sts[i] as Station).mas.Count; m++)
                {
                    array_sts.Add(Convert.ToString((sts[i] as Station).mas[m]));
                }
                int d = 0;
                for(d = 0; d < array_sts.Count-1; d++)
                {
                    data = data + array_sts[d] as String + ',';
                }
                data = data + array_sts[d] as String + '\r';
            }
            sw1.WriteLine(data);
            sw1.Close();
            fs1.Close();
            FileInfo fi2 = new FileInfo(fullPath2);
            if (!fi2.Directory.Exists)
            {
                fi2.Directory.Create();
            }
            FileStream fs2 = new FileStream(fullPath2, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            StreamWriter sw2 = new StreamWriter(fs2, System.Text.Encoding.UTF8);

            string data_line = ",";
            int first = 0;
            for (; first < sts.Count - 1; first++)
            {
                data_line = data_line + (sts[first] as Station).name + ',';
            }
            data_line = data_line + (sts[first] as Station).name + '\r';

            for (int i = 0; i < sts.Count; i++)
            {
                for (int q = 0; q < sts.Count; q++)
                {
                    if (q == 0)
                        data_line = data_line + (sts[i] as Station).name + ',';
                    bool found = false;
                    for (int index = 0; index < lines.Count; index++)
                    {
                        if ((lines[index] as SubwayLine).start == (sts[i] as Station).index && (lines[index] as SubwayLine).end == (sts[q] as Station).index)
                        {
                            data_line = data_line + (lines[index] as SubwayLine).length.ToString() + ",";
                            found = true;
                            break;
                        }
                        else if ((sts[i] as Station).index == (sts[q] as Station).index)
                        {
                            data_line += "0,";
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                        data_line += "-1,";
                }
                data_line += "\r";
            }
            sw2.WriteLine(data_line);
            sw2.Close();
            fs2.Close();
            MessageBox.Show("文件保存成功！");
        }


        private void Select_add_delete(object sender, SelectionChangedEventArgs e)
        {
            string select = comboBox.SelectedValue.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");

            if (select == "增加站点")
            {
                change = 1;
                comboBox.SelectedIndex = 0;
                try
                {
                    lastdis.Visibility = System.Windows.Visibility.Visible;//显示
                    lastdisnum.Visibility = System.Windows.Visibility.Visible;//显示
                    laststana.Visibility = System.Windows.Visibility.Visible;//显示
                    laststation.Visibility = System.Windows.Visibility.Visible;//显示
                    nextstana.Visibility = System.Windows.Visibility.Visible;//显示
                    nextstation.Visibility = System.Windows.Visibility.Visible;//显示
                    nextdisnum.Visibility = System.Windows.Visibility.Visible;//显示
                    nextdis.Visibility = System.Windows.Visibility.Visible;//显示
                    linebox1.Visibility = System.Windows.Visibility.Visible;//显示
                    line_master.Visibility = System.Windows.Visibility.Visible;//显示
                    x_num.Visibility = System.Windows.Visibility.Visible;//显示
                    y_num.Visibility = System.Windows.Visibility.Visible;//显示
                    sts_x.Visibility = System.Windows.Visibility.Visible;//显示
                    sts_y.Visibility = System.Windows.Visibility.Visible;//显示

                    for (int i = 0; i < sts_all.Count; i++)
                    {
                        if (stana.Text == (sts_all[i] as Station).name)
                        {
                            warn_label.Content = "[!]此站已存在，继续添加则为换乘站";
                            x_num.Text = Convert.ToString((sts_all[i] as Station).x);
                            y_num.Text = Convert.ToString((sts_all[i] as Station).y);
                            x_num.IsEnabled = false;
                            y_num.IsEnabled = false;
                            break;
                        }
                        else
                        {
                            warn_label.Content = "";
                            x_num.Clear();
                            y_num.Clear();
                            x_num.IsEnabled = true;
                            y_num.IsEnabled = true;
                        }
                    }
                }
                catch { }
            }
            else if (select == "删除站点")
            {
                change = 2;
                comboBox.SelectedIndex = 1;
                lastdis.Visibility = System.Windows.Visibility.Collapsed;//隐藏
                lastdisnum.Visibility = System.Windows.Visibility.Collapsed;//隐藏
                laststana.Visibility = System.Windows.Visibility.Collapsed;//隐藏
                laststation.Visibility = System.Windows.Visibility.Collapsed;//隐藏
                nextstana.Visibility = System.Windows.Visibility.Collapsed;//隐藏
                nextstation.Visibility = System.Windows.Visibility.Collapsed;//隐藏
                nextdisnum.Visibility = System.Windows.Visibility.Collapsed;//隐藏
                nextdis.Visibility = System.Windows.Visibility.Collapsed;//隐藏
                linebox1.Visibility = System.Windows.Visibility.Collapsed;//隐藏
                line_master.Visibility = System.Windows.Visibility.Collapsed;//隐藏
                x_num.Visibility = System.Windows.Visibility.Collapsed;//隐藏
                y_num.Visibility = System.Windows.Visibility.Collapsed;//隐藏
                sts_x.Visibility = System.Windows.Visibility.Collapsed;//隐藏
                sts_y.Visibility = System.Windows.Visibility.Collapsed;//隐藏
                warn_label.Content = "";
                x_num.Clear();
                y_num.Clear();
                x_num.IsEnabled = true;
                y_num.IsEnabled = true;
            }
        }

        private void SelectLine_1(object sender, SelectionChangedEventArgs e)
        {
            try { this.nextstana.Items.Clear(); } catch { }

            string select = linebox1.SelectedValue.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");
            if (select == "1号线")
            {
                sts_master = 1;
            }
            else if (select == "2号线")
            {
                sts_master = 2;
            }
            else if (select == "5号线")
            {
                sts_master = 5;
            }
            else if (select == "6号线")
            {
                sts_master = 6;
            }
            else if (select == "10号线")
            {
                sts_master = 10;
            }

            int i = 0;
            for (i = 0; i < sts_all.Count; i++)
            {
                if ((laststana.Text == (sts_all[i] as Station).name))
                {
                    int j = 0;
                    
                    for (j = 0; j < lines_all.Count;)
                    {
                        if ((((sts_all[i] as Station).index == (lines_all[j] as SubwayLine).start)) && ((sts_all[i] as Station).index != (lines_all[j] as SubwayLine).end))
                        {
                            for(int m=0;m< (sts_all[(lines_all[j] as SubwayLine).end] as Station).mas.Count; m++)
                            {
                                if (((int)(sts_all[(lines_all[j] as SubwayLine).end] as Station).mas[m] == sts_master))
                                    nextstana.Items.Add((sts_all[(lines_all[j] as SubwayLine).end] as Station).name);
                            }
                            
                        }
                        j++;
                    }
                }
            }
            nextstana.SelectedIndex = 0;
        }

        private void finish_Click(object sender, RoutedEventArgs e)
        {
            lastdisnum.Clear();
            laststana.Clear();
            //nextdisnum.Clear();
            //nextstana.Text = "";
            stana.Clear();
            MainWindow owner = (MainWindow)this.Owner;
            owner.sts_all = sts_all;
            owner.lines_all = lines_all;
            owner.mainPanel.Children.Clear();
            owner.Print(lines_all, sts_all);
            Close();

        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            lastdisnum.Clear();
            laststana.Clear();
            //nextdisnum.Clear();
            //nextstana.Text = "";
            stana.Clear();
            MainWindow owner = (MainWindow)this.Owner;
            owner.sts_all = sts_all;
            owner.lines_all = lines_all;
            owner.mainPanel.Children.Clear();
            owner.Print(lines_all, sts_all);
            //Close();

        }

        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            if (change == 1)
            {

                string a = lastdisnum.Text;
                string b = nextdisnum.Text;
                if (a == "")
                {
                    MessageBox.Show("请输入距离");
                    return;

                }
                if ((x_num.Text == "")|| (y_num.Text == ""))
                {
                    MessageBox.Show("请输入坐标值");
                    return;
                }
                for (int i = 0; i <= sts_all.Count; i++)
                {
                    if (i == sts_all.Count)
                    {
                        MessageBox.Show("输入错误，请重新输入");
                        stana.Clear();
                        laststana.Clear();
                        //lastdisnum.Clear();
                        //nextdisnum.Clear();
                        //x_num.Clear();
                        //y_num.Clear();
                        return;
                    }
                    if(laststana.Text==(sts_all[i] as Station).name)
                    {
                        break;
                    }
                }
                if(IsNumberic(a)==false|| IsNumberic(b) == false)
                {
                    MessageBox.Show("距离需输入整数，请重新输入");
                    lastdisnum.Clear();
                    nextdisnum.Clear();
                    return;
                }
                if (IsNumberic(x_num.Text) == false || IsNumberic(y_num.Text) == false)
                {
                    MessageBox.Show("坐标需输入整数，请重新输入");
                    lastdisnum.Clear();
                    nextdisnum.Clear();
                    return;
                }
                int aint = Convert.ToInt32(a);
                int bint = Convert.ToInt32(b);
                int sts_x, sts_y;
                sts_x = Convert.ToInt32(x_num.Text);
                sts_y = Convert.ToInt32(y_num.Text);
                if (aint<=0|| bint<= 0)
                {
                    MessageBox.Show("输入的距离需大于0，请重新输入");
                    lastdisnum.Clear();
                    nextdisnum.Clear();
                    return;
                }
                if (sts_x <= 0 || sts_y <= 0)
                {
                    MessageBox.Show("输入的坐标值需大于0，请重新输入");
                    x_num.Clear();
                    y_num.Clear();
                    return;
                }
                for (int j = 0; j < lines_all.Count; j++)
                {
                    if ((sts_all[(lines_all[j] as SubwayLine).start] as Station).name == laststana.Text && (sts_all[(lines_all[j] as SubwayLine).end] as Station).name == nextstana.Text)
                    {
                        
                        for (int i = 0; i <= sts_all.Count; i++)
                        {
                            if (i == sts_all.Count)
                            {
                                sts_all.Add(new Station(stana.Text, sts_all.Count, sts_x, sts_y, 1));

                                (sts_all[sts_all.Count - 1] as Station).Insert_last(sts_master);
                                lines_all.Add(new SubwayLine(((sts_all[(lines_all[j] as SubwayLine).start] as Station).index), (sts_all.Count - 1), Convert.ToInt32(a)));
                                (lines_all[lines_all.Count - 1] as SubwayLine).master = sts_master;
                                lines_all.Add(new SubwayLine((sts_all.Count - 1), ((sts_all[(lines_all[j] as SubwayLine).start] as Station).index), Convert.ToInt32(a)));
                                (lines_all[lines_all.Count - 1] as SubwayLine).master = sts_master;
                                lines_all.Add(new SubwayLine(((sts_all[(lines_all[j] as SubwayLine).end] as Station).index), (sts_all.Count - 1), Convert.ToInt32(b)));
                                (lines_all[lines_all.Count - 1] as SubwayLine).master = sts_master;
                                lines_all.Add(new SubwayLine((sts_all.Count - 1), ((sts_all[(lines_all[j] as SubwayLine).end] as Station).index), Convert.ToInt32(b)));
                                (lines_all[lines_all.Count - 1] as SubwayLine).master = sts_master;
                                lines_all.RemoveAt(j);
                                break;
                            }
                            else if ((sts_all[i] as Station).name == stana.Text)
                            {
                                (sts_all[i] as Station).Insert_last(sts_master);
                                lines_all.Add(new SubwayLine(((sts_all[(lines_all[j] as SubwayLine).start] as Station).index), (sts_all[i] as Station).index, Convert.ToInt32(a)));
                                (lines_all[lines_all.Count - 1] as SubwayLine).master = sts_master;
                                lines_all.Add(new SubwayLine((sts_all[i] as Station).index, ((sts_all[(lines_all[j] as SubwayLine).start] as Station).index), Convert.ToInt32(a)));
                                (lines_all[lines_all.Count - 1] as SubwayLine).master = sts_master;
                                lines_all.Add(new SubwayLine(((sts_all[(lines_all[j] as SubwayLine).end] as Station).index), (sts_all[i] as Station).index, Convert.ToInt32(b)));
                                (lines_all[lines_all.Count - 1] as SubwayLine).master = sts_master;
                                lines_all.Add(new SubwayLine((sts_all[i] as Station).index, ((sts_all[(lines_all[j] as SubwayLine).end] as Station).index), Convert.ToInt32(b)));
                                (lines_all[lines_all.Count - 1] as SubwayLine).master = sts_master;
                                lines_all.RemoveAt(j);
                                break;
                            }
                        }
                    }
                    if ((sts_all[(lines_all[j] as SubwayLine).start] as Station).name == nextstana.Text && (sts_all[(lines_all[j] as SubwayLine).end] as Station).name == laststana.Text)
                    {
                        lines_all.RemoveAt(j);
                    }
                }
                    SaveCSV(lines_all, sts_all, "./savestation.csv", "./savelines.csv");
                    stana.Clear();
                    laststana.Clear();
                    lastdisnum.Clear();
                
            }
            else if (change == 2)
            {

                ArrayList starts = new ArrayList();
                ArrayList ends = new ArrayList();

                for (int i = 0; i <= sts_all.Count; i++)
                {
                    if (i == sts_all.Count)
                    {
                        MessageBox.Show("输入错误，请重新输入");
                        stana.Clear();
                        return;
                    }
                    if (stana.Text == (sts_all[i] as Station).name)
                    {

                        for (int a = 0; a < lines_all.Count; a++)
                        {
                            if (((lines_all[a] as SubwayLine).start == (sts_all[i] as Station).index) && ((lines_all[a] as SubwayLine).end != (sts_all[i] as Station).index))
                            {
                                SubwayLine line = lines_all[a] as SubwayLine;
                                ends.Add(line);
                            }
                            else if (((lines_all[a] as SubwayLine).end == (sts_all[i] as Station).index) && ((lines_all[a] as SubwayLine).start != (sts_all[i] as Station).index))
                            {
                                SubwayLine line = lines_all[a] as SubwayLine;
                                starts.Add(line);
                                //lines_all.RemoveAt(a);
                            }
                        }

                        for (int ind = 0; ind < sts_all.Count; ind++)
                        {
                            if ((sts_all[ind] as Station).index > i)
                            {
                                (sts_all[ind] as Station).index--;
                            }
                        }
                        for (int b = 0; b < lines_all.Count; b++)
                        {
                            if ((lines_all[b] as SubwayLine).start > i)
                            {
                                (lines_all[b] as SubwayLine).start--;
                            }
                            if ((lines_all[b] as SubwayLine).end > i)
                            {
                                (lines_all[b] as SubwayLine).end--;
                            }
                        }
                        sts_all.RemoveAt(i);
                        for (int ind_start = 0; ind_start < starts.Count; ind_start++)
                            lines_all.Remove(starts[ind_start]);
                        for (int ind_end = 0; ind_end < ends.Count; ind_end++)
                            lines_all.Remove(ends[ind_end]);
                        break;
                    }
                }
                for (int h = 0; h < starts.Count; h++)
                {
                    for (int n = 0; n < ends.Count; n++)
                    {
                        if ((starts[h] as SubwayLine).master == (ends[n] as SubwayLine).master)
                        {
                            //MessageBox.Show((sts_all[(starts[h] as SubwayLine).end] as Station).name+"," + (sts_all[(ends[n] as SubwayLine).start] as Station).name); 
                            lines_all.Add(new SubwayLine((starts[h] as SubwayLine).start, (ends[n] as SubwayLine).end, (starts[h] as SubwayLine).length + (ends[n] as SubwayLine).length));
                            (lines_all[lines_all.Count - 1] as SubwayLine).master = (starts[h] as SubwayLine).master;
                            //lines_all.Add(new SubwayLine((ends[n] as SubwayLine).end, (starts[h] as SubwayLine).start, (starts[h] as SubwayLine).length + (ends[n] as SubwayLine).length));
                        }
                    }
                }

                SaveCSV(lines_all, sts_all, "./savestation.csv", "./savelines.csv");
                stana.Clear();
                laststana.Clear();
                lastdisnum.Clear();
            }
        }

        private void TextChange(object sender, TextChangedEventArgs e)
        {
            try {
                this.nextstana.Items.Clear();
                this.linebox1.Items.Clear();
            } catch { }
            
            int i = 0;
            for (i = 0; i < sts_all.Count; i++)
            {
                if ((laststana.Text == (sts_all[i] as Station).name))
                {
                    for(int m=0;m< (sts_all[i] as Station).mas.Count; m++)
                    {
                            linebox1.Items.Add((sts_all[i] as Station).mas[m]+"号线");
                    }
                    break;
                }
            }
            linebox1.SelectedIndex = 0;
        }

        private void stana_taxtchanged(object sender, TextChangedEventArgs e)
        {
            for (int i=0; i < sts_all.Count; i++){
                if (stana.Text == (sts_all[i] as Station).name)
                {
                    if (change == 1)
                    {
                        warn_label.Content = "[!]此站已存在，继续添加则为换乘站";
                        x_num.Text = Convert.ToString((sts_all[i] as Station).x);
                        y_num.Text = Convert.ToString((sts_all[i] as Station).y);
                        x_num.IsEnabled = false;
                        y_num.IsEnabled = false;
                        //sts_x.IsEnabled = false;
                        //sts_y.IsEnabled = false;
                        break;
                    }
                    else if (change == 2)
                    {
                        warn_label.Content = "";
                        x_num.Clear();
                        y_num.Clear();
                        x_num.IsEnabled = true;
                        y_num.IsEnabled = true;
                    }
                }
                else
                {
                    warn_label.Content = "";
                    x_num.Clear();
                    y_num.Clear();
                    x_num.IsEnabled = true;
                    y_num.IsEnabled = true;
                }
            }
            
        }
    }
}
