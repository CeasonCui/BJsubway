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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BJsubway
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 

    

    public class Station {//站点信息
        public ArrayList mas = new ArrayList();
        public int index;
        public int x;
        public int y;
        public string name;
        public int ft;
        public Station(string name,int i,int x,int y,int ft){
            this.name = name;
            index = i;
            this.x = x;
            this.y = y;
            this.ft = ft;
        }
        public void Insert_last(int master) {
            mas.Add(master);
        }
    }

    public class SubwayLine {//线信息
        public int start;
        public int end;
        public int length;
        public int master;
        public SubwayLine(int start, int end, int length) {
            this.start = start;
            this.end = end;
            this.length = length;
        }

    }

    public class VisitItem {
        public int index = 0;
        public Station item;
        public VisitItem(int i, Station item_in) {
            index = i;
            item = item_in;
        }
    }



    public partial class MainWindow : Window
    {
        public List<string> sts_name_all = new List<string>();
        public ArrayList lines_all = new ArrayList();
        public ArrayList sts_all = new ArrayList();
        public int change = 1;
        public int rout = 1;
        public bool loaded = false;
        //public int end_all;
        int start_station = -1;
        int end_station = -1;

        public void Circle(int x, int y,bool i)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Width = 14;
            ellipse.Height = 14;
            if (!i)
            {
                ellipse.Stroke = new SolidColorBrush(Colors.Black);
                ellipse.Fill = new SolidColorBrush(Colors.White);
            }
            else
            {
                ellipse.Stroke = new SolidColorBrush(Colors.Gray);
                ellipse.Fill = new SolidColorBrush(Colors.Green);
            }
            
            ellipse.SetValue(Canvas.LeftProperty, (double)(x - 7));
            ellipse.SetValue(Canvas.TopProperty, (double)(y - 7));
            mainPanel.Children.Add(ellipse);
        }

        public int[] Dijkstra(ArrayList line,ArrayList station,int start,int all,int mode,bool is_show) {
            int[] path = new int[all];
            int[] pathMin = new int[all];

            ArrayList paths = new ArrayList();
            ArrayList S = new ArrayList();
            ArrayList U = new ArrayList();
            for(int i =0;i< all; i++) {
                pathMin[i] =1000000000;//每点最小值
                path[i] = -1;//每点的上一个点
            }
            for(int i=0;i< all; i++)
            {
                if(i != start)
                    U.Add(i);
            }
            S.Add(start);

            int expand = start;
            path[start] = start;
            

            while (U.Count != 0) {
                for (int i = 0; i < line.Count; i++)
                {
                    if (expand == start)
                    {
                        if ((line[i] as SubwayLine).start == expand && (line[i] as SubwayLine).length > 0)
                        {
                            pathMin[(line[i] as SubwayLine).end] = (line[i] as SubwayLine).length;
                            path[(line[i] as SubwayLine).end] = expand;
                            if (is_show)
                            {
                                mainPanel.Children.Clear();
                                Print(lines_all, sts_all);
                                int t = (line[i] as SubwayLine).end;
                                //path = Dijkstra(lines_all, sts_all, start_station, sts_all.Count, 1, false);
                                while (path[t] != t)
                                {
                                    PrintLine(sts_all[path[t]] as Station, sts_all[t] as Station, Brushes.Green, 7);
                                    t = path[t];
                                    System.Threading.Thread.Sleep(1000);
                                }
                                //System.Threading.Thread.Sleep(200);
                            }
                            //PrintLine(station[(line[i] as SubwayLine).start] as Station, station[(line[i] as SubwayLine).end] as Station, Brushes.Black,7);
                        }
                    }
                    else
                    {
                        if ((line[i] as SubwayLine).start == expand && (line[i] as SubwayLine).length > 0)
                        {
                            if (pathMin[(line[i] as SubwayLine).end] > (line[i] as SubwayLine).length + pathMin[expand])
                            {
                                pathMin[(line[i] as SubwayLine).end] = (line[i] as SubwayLine).length + pathMin[expand];
                                if ((line[i] as SubwayLine).end != start)
                                {
                                    path[(line[i] as SubwayLine).end] = expand;
                                    //PrintLine(station[(line[i] as SubwayLine).start] as Station, station[(line[i] as SubwayLine).end] as Station, Brushes.Black,7);
                                    if (is_show)
                                    {
                                        mainPanel.Children.Clear();
                                        Print(lines_all, sts_all);
                                        int t = (line[i] as SubwayLine).end;
                                        //path = Dijkstra(lines_all, sts_all, start_station, sts_all.Count, 1, false);
                                        while (path[t] != t)
                                        {
                                            PrintLine(sts_all[path[t]] as Station, sts_all[t] as Station, Brushes.Green, 7);
                                            t = path[t];
                                            System.Threading.Thread.Sleep(200);
                                        }
                                        //System.Threading.Thread.Sleep(1000);

                                    }
                                }
                            }
                        }
                    }
                }

                S.Add(expand);
                for (int q = 0; q < U.Count; q++)
                {
                    if ((int)U[q] == expand)
                    {
                        U.RemoveAt(q);
                    }
                }

                int Template = 10000000;
                for (int j = 0; j < pathMin.Length; j++)
                {
                    if (pathMin[j] < Template && U.Contains(j))
                    {
                        expand = j;
                        Template = pathMin[j];
                    }
                }
            }
            return path;
        }


        public List<List<VisitItem>> FindAll(ArrayList line, ArrayList station, int start, int end, int all, int mode)
        {

            Stack<VisitItem> Visited = new Stack<VisitItem>();
            List<List<VisitItem>> paths = new List<List<VisitItem>>();
            VisitItem start_item = new VisitItem(0, station[start] as Station);
            Visited.Push(start_item);
            int change_min = 100000;
            int line_min = 1000000;
            //int line
            while (Visited.Count != 0)
            {
                bool expandable = false;
                for (int i = Visited.Peek().index; i < line.Count; i++)
                {
                    if ((line[i] as SubwayLine).start == Visited.Peek().item.index && (line[i] as SubwayLine).length > 0)
                    {
                        bool in_visited = false;
                        for (int t = 0; t < Visited.Count; t++)
                        {
                            if ((line[i] as SubwayLine).end == Visited.ToList<VisitItem>()[t].item.index)
                                in_visited = true;
                        }
                        if (!in_visited)
                        {
                            Visited.Peek().index = i + 1;
                            Visited.Push(new VisitItem(0, station[(line[i] as SubwayLine).end] as Station));
                            if (Visited.Peek().item.index == end)
                            {
                                int pre_line_mas = -1;
                                int change_line = 0;
                                int line_long = 0;
                                //int line_max = 0;
                                foreach (VisitItem vi in Visited)
                                {
                                    if (pre_line_mas == -1)
                                    {
                                        if (vi.item.index != end)
                                        {
                                            pre_line_mas = (line[vi.index - 1] as SubwayLine).master;
                                            line_long = (line[vi.index - 1] as SubwayLine).length;
                                        }

                                    }
                                    else
                                    {
                                        line_long += (line[vi.index - 1] as SubwayLine).length;
                                        if (pre_line_mas != (line[vi.index - 1] as SubwayLine).master)
                                        {
                                            change_line++;
                                            pre_line_mas = (line[vi.index - 1] as SubwayLine).master;
                                        }
                                    }
                                }
                                if (mode == 2)
                                {
                                    if (change_min > change_line)
                                    {
                                        line_min = line_long;
                                        change_min = change_line;
                                        paths.Add(Visited.ToList<VisitItem>());
                                    }
                                    if (change_min == change_line && line_min > line_long)
                                    {
                                        line_min = line_long;
                                        paths.Add(Visited.ToList<VisitItem>());
                                    }
                                }

                                else if (mode == 1)
                                {
                                    if (line_min > line_long)
                                    {
                                        line_min = line_long;
                                        paths.Add(Visited.ToList<VisitItem>());
                                    }
                                }
                                Visited.Pop();
                            }
                            expandable = true;
                            break;
                        }
                    }
                }
                if (!expandable)
                {
                    Visited.Pop();
                }
            }
            return paths;
        }

        public void PrintLine(Station a,Station b,SolidColorBrush color,int width)
        {
            LineGeometry myLineGeometry = new LineGeometry();
            myLineGeometry.StartPoint = new Point(a.x, a.y);
            myLineGeometry.EndPoint = new Point(b.x, b.y);
            Path myPath = new Path();
            myPath.Stroke = color;
            myPath.StrokeThickness = width;
            myPath.Data = myLineGeometry;
            mainPanel.Children.Add(myPath);
        }

        public void Print(ArrayList lines,ArrayList sts)
        {
            for (int a = 0; a < lines.Count; a++)
            {
                LineGeometry myLineGeometry = new LineGeometry();
                myLineGeometry.StartPoint = new Point((sts[(lines[a] as SubwayLine).start] as Station).x, (sts[(lines[a] as SubwayLine).start] as Station).y);
                myLineGeometry.EndPoint = new Point((sts[(lines[a] as SubwayLine).end] as Station).x, (sts[(lines[a] as SubwayLine).end] as Station).y);
                Path myPath = new Path();

                if ((lines[a] as SubwayLine).master == 1)
                {
                    myPath.Stroke = Brushes.Red;
                }
                else if ((lines[a] as SubwayLine).master == 2)
                {
                    myPath.Stroke = Brushes.DarkBlue;
                }
                else if ((lines[a] as SubwayLine).master == 5)
                {
                    myPath.Stroke = Brushes.Purple;
                }
                else if ((lines[a] as SubwayLine).master == 6)
                {
                    myPath.Stroke = Brushes.Orange;
                }
                else if ((lines[a] as SubwayLine).master == 10)
                {
                    myPath.Stroke = Brushes.Blue;
                }
                myPath.StrokeThickness = 5;
                myPath.Data = myLineGeometry;
                mainPanel.Children.Add(myPath);
            }
            //画图
            for (int a = 0; a < sts.Count; a++)
            {
                Circle((sts[a] as Station).x, (sts[a] as Station).y,false);
               // stationNum = a;
            }

            //添站名
            for (int a = 0; a < sts.Count; a++)
            {
                TextBlock text = new TextBlock() { Text = (sts[a] as Station).name };
                Thickness thick = new Thickness((sts[a] as Station).x+5, (sts[a] as Station).y+5, 0, 0);
                text.Margin = thick;
                this.mainPanel.Children.Add(text);
            }
        }



        public MainWindow()
        {

            InitializeComponent();
            comboBox.SelectedIndex = 0;
            comboBox1.SelectedIndex = 0;

            comboBox1.Visibility = System.Windows.Visibility.Collapsed;//隐藏
            rout2.Visibility = System.Windows.Visibility.Collapsed;//隐藏
            rout3.Visibility = System.Windows.Visibility.Collapsed;//隐藏
            //int stationNum = 0;
            if (change == 1 || change == 2)
            {
                rout = 1;
            }
            ArrayList sts = new ArrayList();
            ArrayList lines = new ArrayList();

            System.IO.StreamReader str_ply_1 = new System.IO.StreamReader("stationinfo.csv", System.Text.Encoding.Default);
            string[] array_str_line_1;
            string str_line_temp_1;
            int i = 0;
            while ((str_line_temp_1 = str_ply_1.ReadLine()) != null)
            {
                array_str_line_1 = str_line_temp_1.Split(new char[] { ',' });
                sts.Add(new Station((array_str_line_1[0]), i, int.Parse(array_str_line_1[2]), int.Parse(array_str_line_1[3]), int.Parse(array_str_line_1[1])));//放入站名、索引号，坐标
                //sts_name_all.Add((sts[sts.Count - 1] as Station).name);
                for (int j = 4; j < array_str_line_1.Length; j++)
                {
                    if (array_str_line_1[j] != "")
                    {

                        (sts[sts.Count - 1] as Station).Insert_last(int.Parse(array_str_line_1[j]));
                    }
                }
                i++;
            }
            
            
            System.IO.StreamReader str_ply = new System.IO.StreamReader("subwaySQL.csv",System.Text.Encoding.Default);
            string[] array_str_line;
            string str_line_temp;
            i=1;
            bool first = true;
            while ((str_line_temp = str_ply.ReadLine()) != null) {
                if (first) {
                    first = false;
                    continue;
                }
                array_str_line = str_line_temp.Split(new char[] { ',' });
                for (int j = 1; j < array_str_line.Length; j++) {
                    if (array_str_line[j] != "-1") {
                       
                        lines.Add(new SubwayLine(i-1, j-1, int.Parse(array_str_line[j])));
                        for(int a = 0; a < (sts[i - 1] as Station).mas.Count; a++)
                        {
                            for(int b = 0; b < (sts[j - 1] as Station).mas.Count; b++)
                            {
                                if(((int)((sts[i-1] as Station).mas[a])== (int)(sts[j - 1] as Station).mas[b]) && (((int)(sts[i - 1] as Station).mas[a] != 0) && ((int)(sts[j - 1] as Station).mas[b] != 0)))
                                {     
                                    (lines[lines.Count - 1] as SubwayLine).master = (int)((sts[i - 1] as Station).mas[a]);
                                    break;
                                }
                            }
                        }
                    }
                }
                i++;
            }
            for (int s = 0; s < sts.Count; s++)
            {
                sts_name_all.Add((sts[s] as Station).name); 
            }
            mainPanel.Children.Clear();
            Print(lines, sts);
            sts_all = sts;
            lines_all = lines;
            start.ItemsSource = sts_name_all;
            end.ItemsSource = sts_name_all;


        }

        private void SelectChanged_1(object sender, SelectionChangedEventArgs e)
        {
            loaded = false;
            string select = comboBox.SelectedValue.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");
            
            if (select == "最短路径")
            {
                change = 1;
                comboBox1.SelectedIndex = 0;
                comboBox1.Visibility = System.Windows.Visibility.Collapsed;//隐藏

            }
            else if (select == "最少换乘")
            {
                change = 2;
                comboBox1.SelectedIndex = 0;
                comboBox1.Visibility = System.Windows.Visibility.Collapsed;//隐藏

            }
        }

        private void SelectChanged_2(object sender, SelectionChangedEventArgs e)
        {
            string select = comboBox1.SelectedValue.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");
            if (select == "线路1")
            {
                rout = 1;
            }
            else if (select == "线路2")
            {
                rout = 2;
            }
            else if (select == "线路3")
            {
                rout = 3;
            }
            if (loaded)
                button_Click(null,null);


        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            loaded = true;
            comboBox1.Visibility = System.Windows.Visibility.Visible;//显示
            rout1.Visibility = System.Windows.Visibility.Collapsed;//隐藏
            rout2.Visibility = System.Windows.Visibility.Collapsed;//隐藏
            rout3.Visibility = System.Windows.Visibility.Collapsed;//隐藏
            List<List<VisitItem>> path_2 = new List<List<VisitItem>>();
            int[] path = { };
            int t = 0;
            bool is_station = false;
            mainPanel.Children.Clear();
            Print(lines_all, sts_all);
            
            while (true)
            {
                for (int i = 0; i <= sts_all.Count; i++)
                {
                    if (i == sts_all.Count)
                    {
                        MessageBox.Show("输入错误，请重新输入");
                        start.Text = string.Empty;
                        start.SelectedItem = string.Empty;
                        end.Text = string.Empty;
                        end.SelectedItem = string.Empty;
                        /*start.Text = "";
                        start_station = -1;
                        end.Text = "";
                        end_station = -1;*/
                        return;
                    }
                    if ((sts_all[i] as Station).name == start.Text)
                    {
                        start_station = (sts_all[i] as Station).index;
                        break;
                    }
                }
                for (int i = 0; i <= sts_all.Count; i++)
                {
                    if (i == sts_all.Count)
                    {
                        MessageBox.Show("输入错误，请重新输入");
                        start.Text = string.Empty;
                        start.SelectedItem = string.Empty;
                        end.Text = string.Empty;
                        end.SelectedItem = string.Empty;
                        /*start.Text = "";
                        start_station = -1;
                        end.Text = "";
                        end_station = -1;*/
                        return;
                    }
                    if ((sts_all[i] as Station).name == end.Text)
                    {
                        end_station = (sts_all[i] as Station).index;
                        break;
                    }
                }
                if (start_station >= 0 && start_station < sts_all.Count && end_station >= 0 && end_station < sts_all.Count)
                {
                    is_station = true;
                    break;
                }
                else {
                    return;
                }
            }
            if (change == 1)
            {
                rout1.Visibility = System.Windows.Visibility.Visible;//显示
                mainPanel.Children.Clear();
                Print(lines_all, sts_all);
                path_2 = FindAll(lines_all, sts_all, start_station, end_station, sts_all.Count, change);
                if (path_2.Count > 1)
                {
                    rout2.Visibility = System.Windows.Visibility.Visible;//显示
                    if (path_2.Count > 2)
                    {
                        rout3.Visibility = System.Windows.Visibility.Visible;//显示
                    }
                }

                if (rout == 1)
                {
                    listBox.Items.Clear();
                    Stack<Station> root_djs = new Stack<Station>();
                    mainPanel.Children.Clear();
                    Print(lines_all, sts_all);
                    t = end_station;
                    path = Dijkstra(lines_all, sts_all, start_station, sts_all.Count, 1, false);
                    while (path[t] != t)
                    {
                        PrintLine(sts_all[path[t]] as Station, sts_all[t] as Station, Brushes.Green, 7);
                        root_djs.Push((sts_all[t])as Station);
                        t = path[t];
                    }
                    root_djs.Push((sts_all[t]) as Station);
                    int root_master = 0;
                    int pre_master = 0;
                    int root_length = 0;
                    Station root_top = root_djs.Pop();
                    while (root_djs.Count != 0)
                    {
                        for (int l = 0; l < lines_all.Count; l++)
                        {
                            if (((lines_all[l] as SubwayLine).start == root_top.index) && ((lines_all[l] as SubwayLine).end == root_djs.Peek().index))
                            {
                                pre_master = root_master;
                                root_master = (lines_all[l] as SubwayLine).master;
                                if (pre_master != root_master)
                                {
                                    if (root_length != 0)
                                        listBox.Items.Add("     ↓" + root_length);
                                    listBox.Items.Add(root_top.name + "   "+root_master + "号线");
                                    root_length = 0;
                                }
                                root_length += (lines_all[l] as SubwayLine).length;
                                break;
                            }
                        }
                        
                        if (root_djs.Peek().index== end_station)
                        {
                            listBox.Items.Add("     ↓" + root_length);
                            listBox.Items.Add(root_djs.Peek().name );
                        }
                        root_top = root_djs.Pop();
                    }
                        
                   
                   
                }
                else if (rout == 2)
                {
                    listBox.Items.Clear();
                    int root_master = 0;
                    int pre_master = 0;
                    int root_length = 0;
                    List<VisitItem> list_temp_0 = path_2[path_2.Count - 2];
                    int q = 0;
                    for (q = list_temp_0.Count - 1; q >0; q--)
                    {
                        for (int p = 0; p < lines_all.Count; p++)
                        {
                            if(((lines_all[p] as SubwayLine).start==list_temp_0[q].item.index)&& ((lines_all[p] as SubwayLine).end == list_temp_0[q - 1].item.index))
                            {
                                pre_master = root_master;
                                root_master = (lines_all[p] as SubwayLine).master;
                                if (pre_master != root_master)
                                {
                                    if (root_length != 0)
                                        listBox.Items.Add("     ↓" + root_length);
                                    listBox.Items.Add(list_temp_0[q].item.name + "   " + root_master + "号线");
                                    root_length = 0;
                                }
                                root_length += (lines_all[p] as SubwayLine).length;
                                break;
                            }
                        }
                       
                        PrintLine(list_temp_0[q].item, list_temp_0[q - 1].item, Brushes.Green, 7);

                    }
                    if (q == 0)
                    {
                        listBox.Items.Add("     ↓" + root_length);
                        listBox.Items.Add(list_temp_0[q].item.name);
                    }
                }
                else if (rout == 3)
                {
                    listBox.Items.Clear();
                    int root_master = 0;
                    int pre_master = 0;
                    int root_length = 0;
                    List<VisitItem> list_temp_1 = path_2[path_2.Count - 3];
                    int q = 0;
                    for (q = list_temp_1.Count - 1; q > 0; q--)
                    {
                        for (int p = 0; p < lines_all.Count; p++)
                        {
                            if (((lines_all[p] as SubwayLine).start == list_temp_1[q].item.index) && ((lines_all[p] as SubwayLine).end == list_temp_1[q - 1].item.index))
                            {
                                pre_master = root_master;
                                root_master = (lines_all[p] as SubwayLine).master;
                                if (pre_master != root_master)
                                {
                                    if (root_length != 0)
                                        listBox.Items.Add("     ↓" + root_length);
                                    listBox.Items.Add(list_temp_1[q].item.name + "   " + root_master + "号线");
                                    root_length = 0;
                                }
                                root_length += (lines_all[p] as SubwayLine).length;
                                break;
                            }
                        }

                        PrintLine(list_temp_1[q].item, list_temp_1[q - 1].item, Brushes.Green, 7);

                    }
                    if (q == 0)
                    {
                        listBox.Items.Add("     ↓" + root_length);
                        listBox.Items.Add(list_temp_1[q].item.name);
                    }
                }
                path_2 = new List<List<VisitItem>>();
            }

            else if (change == 2)
            {
                rout1.Visibility = System.Windows.Visibility.Visible;//显示
                mainPanel.Children.Clear();
                Print(lines_all, sts_all);
                path_2 = FindAll(lines_all, sts_all, start_station, end_station, sts_all.Count, change);
                if (path_2.Count > 1)
                {
                    rout2.Visibility = System.Windows.Visibility.Visible;//显示
                    if (path_2.Count > 2)
                    {
                        rout3.Visibility = System.Windows.Visibility.Visible;//显示
                    }
                }
                if (is_station)
                {
                    if (rout == 1)
                    {
                        listBox.Items.Clear();
                        int root_master = 0;
                        int pre_master = 0;
                        int root_length = 0;
                        List<VisitItem> list_temp_0 = path_2[path_2.Count - 1];
                        int q = 0;
                        for (q = list_temp_0.Count - 1; q > 0; q--)
                        {
                            for (int p = 0; p < lines_all.Count; p++)
                            {
                                if (((lines_all[p] as SubwayLine).start == list_temp_0[q].item.index) && ((lines_all[p] as SubwayLine).end == list_temp_0[q - 1].item.index))
                                {
                                    pre_master = root_master;
                                    root_master = (lines_all[p] as SubwayLine).master;
                                    if (pre_master != root_master)
                                    {
                                        if (root_length != 0)
                                            listBox.Items.Add("     ↓" + root_length);
                                        listBox.Items.Add(list_temp_0[q].item.name + "   " + root_master + "号线");
                                        root_length = 0;
                                    }
                                    root_length += (lines_all[p] as SubwayLine).length;
                                    break;
                                }
                            }

                            PrintLine(list_temp_0[q].item, list_temp_0[q - 1].item, Brushes.Green, 7);

                        }
                        if (q == 0)
                        {
                            listBox.Items.Add("     ↓" + root_length);
                            listBox.Items.Add(list_temp_0[q].item.name);
                        }
                    }
                    else if (rout == 2)
                    {
                        listBox.Items.Clear();
                        int root_master = 0;
                        int pre_master = 0;
                        int root_length = 0;
                        List<VisitItem> list_temp_1 = path_2[path_2.Count - 2];
                        int q = 0;
                        for (q = list_temp_1.Count - 1; q > 0; q--)
                        {
                            for (int p = 0; p < lines_all.Count; p++)
                            {
                                if (((lines_all[p] as SubwayLine).start == list_temp_1[q].item.index) && ((lines_all[p] as SubwayLine).end == list_temp_1[q - 1].item.index))
                                {
                                    pre_master = root_master;
                                    root_master = (lines_all[p] as SubwayLine).master;
                                    if (pre_master != root_master)
                                    {
                                        if (root_length != 0)
                                            listBox.Items.Add("     ↓" + root_length);
                                        listBox.Items.Add(list_temp_1[q].item.name + "   " + root_master + "号线");
                                        root_length = 0;
                                    }
                                    root_length += (lines_all[p] as SubwayLine).length;
                                    break;
                                }
                            }

                            PrintLine(list_temp_1[q].item, list_temp_1[q - 1].item, Brushes.Green, 7);

                        }
                        if (q == 0)
                        {
                            listBox.Items.Add("     ↓" + root_length);
                            listBox.Items.Add(list_temp_1[q].item.name);
                        }
                    }
                    else if (rout == 3)
                    {
                        listBox.Items.Clear();
                        int root_master = 0;
                        int pre_master = 0;
                        int root_length = 0;
                        List<VisitItem> list_temp_2 = path_2[path_2.Count - 3];
                        int q = 0;
                        for (q = list_temp_2.Count - 1; q > 0; q--)
                        {
                            for (int p = 0; p < lines_all.Count; p++)
                            {
                                if (((lines_all[p] as SubwayLine).start == list_temp_2[q].item.index) && ((lines_all[p] as SubwayLine).end == list_temp_2[q - 1].item.index))
                                {
                                    pre_master = root_master;
                                    root_master = (lines_all[p] as SubwayLine).master;
                                    if (pre_master != root_master)
                                    {
                                        if (root_length != 0)
                                            listBox.Items.Add("     ↓" + root_length);
                                        listBox.Items.Add(list_temp_2[q].item.name + "   " + root_master + "号线");
                                        root_length = 0;
                                    }
                                    root_length += (lines_all[p] as SubwayLine).length;
                                    break;
                                }
                            }

                            PrintLine(list_temp_2[q].item, list_temp_2[q - 1].item, Brushes.Green, 7);

                        }
                        if (q == 0)
                        {
                            listBox.Items.Add("     ↓" + root_length);
                            listBox.Items.Add(list_temp_2[q].item.name);
                        }
                    }
                }
                path_2 = new List<List<VisitItem>>();
            }
        }

       private void button1_Click(object sender, RoutedEventArgs e)
        {
            adddelete wm = new adddelete(lines_all, sts_all);
            wm.Owner = this;
            wm.Show();
            wm.WindowState = WindowState.Normal;

        }

        private void start_changed(object sender, RoutedEventArgs e)
        {
            mainPanel.Children.Clear();
            Print(lines_all, sts_all);
            comboBox1.Visibility = System.Windows.Visibility.Collapsed;//隐藏
            for (int i = 0; i < sts_all.Count; i++)
            {
                if (start.Text == (sts_all[i] as Station).name)
                {
                    Circle((sts_all[i] as Station).x, (sts_all[i] as Station).y, true);
                }
            }
        }

        private void end_changed(object sender, RoutedEventArgs e)
        {
            comboBox1.Visibility = System.Windows.Visibility.Collapsed;//隐藏
            mainPanel.Children.Clear();
            Print(lines_all, sts_all);
            for (int i = 0; i < sts_all.Count; i++)
            {
                if (start.Text == (sts_all[i] as Station).name)
                {
                    Circle((sts_all[i] as Station).x, (sts_all[i] as Station).y, true);
                }
            }
            for (int j = 0; j < sts_all.Count; j++)
            {
                if (end.Text == (sts_all[j] as Station).name)
                {
                    Circle((sts_all[j] as Station).x, (sts_all[j] as Station).y, true);
                }
            }
        }

        private void mouse_left(object sender, MouseButtonEventArgs e)
        {
            mainPanel.Children.Clear();
            Print(lines_all, sts_all);
            Point p = e.GetPosition((IInputElement)sender);
            for (int i = 0; i < sts_all.Count; i++)
            {
                if ((p.X > (sts_all[i] as Station).x - 7) && (p.X < (sts_all[i] as Station).x + 7) && (p.Y > (sts_all[i] as Station).y - 7) && (p.Y < (sts_all[i] as Station).y + 7))
                {
                    start.Text = (sts_all[i] as Station).name;
                    Circle((sts_all[i] as Station).x, (sts_all[i] as Station).y, true);
                }
            }
           
        }

        private void mouse_right(object sender, MouseButtonEventArgs e)
        {
            mainPanel.Children.Clear();
            Print(lines_all, sts_all);
            for(int i = 0; i < sts_all.Count; i++)
            {
                if(start.Text== (sts_all[i] as Station).name)
                {
                    Circle((sts_all[i] as Station).x, (sts_all[i] as Station).y, true);
                }
            }
            Point p = e.GetPosition((IInputElement)sender);
            for (int j = 0; j < sts_all.Count; j++)
            {
                if ((p.X > (sts_all[j] as Station).x - 7) && (p.X < (sts_all[j] as Station).x + 7) && (p.Y > (sts_all[j] as Station).y - 7) && (p.Y < (sts_all[j] as Station).y + 7))
                {
                    {
                        end.Text = (sts_all[j] as Station).name;
                        Circle((sts_all[j] as Station).x, (sts_all[j] as Station).y, true);
                    }
                }
            }
            button_Click(null, null);
        }
    }
}
