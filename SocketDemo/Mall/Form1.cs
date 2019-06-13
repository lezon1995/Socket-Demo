using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Mall.component;
using Mall.dto;
using Mall.Properties;
using Newtonsoft.Json;

namespace Mall
{
    public partial class Form1 : Form
    {
        //        Thread th;
        private Item item1;
        private Item item2;
        private Item item3;
        private Item item4;
        private Commodity commodity1;
        private Commodity commodity2;
        private Commodity commodity3;
        private Commodity commodity4;
        private static List<Commodity> list = new List<Commodity>();
        private static List<Item> ItemList = new List<Item>();
        private static int sum = 0;
        private static Socket socketSend;
        Dictionary<int, Commodity> dictionary = new Dictionary<int, Commodity>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            #region 轮播图滚动

            //            th = new Thread
            //            (
            //                delegate ()
            //                {
            //                    //3就是要循环轮数了
            //                    for (int i = 0; i < 10; i++)
            //                    {
            //                        //调用方法
            //                        //设置图片的位置和显示时间（1000 为1秒）
            //                        ChangeImage(Resources.banner, 1000);
            //                        ChangeImage(Resources.banner, 1000);
            //                        ChangeImage(Resources.banner, 1000);
            //                    }
            //                }
            //            );
            //            th.IsBackground = true;
            //            th.Start();

            #endregion

            SetData();

            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = true;//不显示程序窗口
            p.Start();//启动程序
            string str = "adb forward tcp:12580 tcp:18888";
//            string str = "ipconfig";
            //向cmd窗口发送输入信息
            p.StandardInput.WriteLine(str + "&exit");
            p.StandardInput.AutoFlush = true;
            //获取cmd窗口的输出信息
            string output = p.StandardOutput.ReadToEnd();

            //StreamReader reader = p.StandardOutput;
            //string line=reader.ReadLine();
            //while (!reader.EndOfStream)
            //{
            //    str += line + "  ";
            //    line = reader.ReadLine();
            //}
            p.WaitForExit();//等待程序执行完退出进程
            p.Close();
//            MessageBox.Show(output);
            Connect();
//            if (Connect())
//            {
//                DeviceLogin();
//            }
            Thread thread = new Thread(Recive);
            thread.Start();
        }

        private void DeviceLogin()
        {
            LoginParam param = new LoginParam("1000", "abc123", "000000002", System.Guid.NewGuid().ToString("N"));
            string json = JsonConvert.SerializeObject(param);
            byte[] body = Encoding.UTF8.GetBytes(json);
            int length = body.Length;
            byte[] contentLength = BitConverter.GetBytes(length);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(contentLength);
            }

            byte[] bytes = copybyte(contentLength, body);
            socketSend.Send(bytes);
        }

        private Boolean Connect()
        {
            //创建通信的Socket
            socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse(Settings.Default.IP);
            IPEndPoint point = new IPEndPoint(ip, Settings.Default.PORT);
            //连接到对应的IP地址和端口号
            try
            {
                socketSend.Connect(point);
            }
            catch (Exception e)
            {
                MessageBox.Show("连接失败");
                button4.Enabled = false;
                return false;
            }
            button4.Enabled = true;
            return true;
            //            MessageBox.Show("连接成功");
        }

        private void SetData()
        {
            labelSum.Text = "￥" + sum / 100.0;

            commodity1 = new Commodity(1, "精品琯溪红心蜜柚", 1990, 1, 500);
            item1 = new Item(commodity1, this);
            commodity2 = new Commodity(2, "澳大利亚进口脐橙", 2550, 1, 500);
            item2 = new Item(commodity2, this);
            commodity3 = new Commodity(3, "徐香绿心猕猴桃", 2880, 1, 500);
            item3 = new Item(commodity3, this);
            commodity4 = new Commodity(4, "新西兰进口皇后红苹果", 3930, 1, 500);
            item4 = new Item(commodity4, this);

            ItemList.Add(item1);
            ItemList.Add(item2);
            ItemList.Add(item3);
            ItemList.Add(item4);
        }

        /// <summary>
        /// 改变图片
        /// </summary>
        /// <param name="img">图片</param>
        /// <param name="millisecondsTimeOut">切换图片间隔时间</param>
        private void ChangeImage(Image img, int millisecondsTimeOut)
        {
            Invoke(new Action(() => { pictureBox1.Image = img; })
            );
            Thread.Sleep(millisecondsTimeOut);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }


        public void AddToShoppingCart(object sender, EventArgs e)
        {
            Control label = (sender as Control);
            switch (label.Name)
            {
                case "label_1":
                    sum += commodity1.price;
                    if (dictionary.ContainsKey(1))
                    {
                        dictionary[1].Num++;
                        UpdateData(1);
                    }
                    else
                    {
                        labelSum.Text = "￥" + sum / 100.0;
                        if (commodity1.Num == 0)
                        {
                            commodity1.Num = 1;
                        }
                        dictionary.Add(1, commodity1);
                        flowLayoutPanel1.Controls.Add(item1);
                    }

                    //                    MessageBox.Show(flowLayoutPanel1.Name);
                    break;
                case "label_2":
                    sum += commodity2.price;
                    if (dictionary.ContainsKey(2))
                    {
                        dictionary[2].Num++;
                        UpdateData(2);
                    }
                    else
                    {
                        labelSum.Text = "￥" + sum / 100.0;
                        if (commodity2.Num == 0)
                        {
                            commodity2.Num = 1;
                        }
                        dictionary.Add(2, commodity2);
                        flowLayoutPanel1.Controls.Add(item2);
                    }

                    //                    MessageBox.Show("label_2");
                    break;
                case "label_3":
                    sum += commodity3.price;
                    if (dictionary.ContainsKey(3))
                    {
                        dictionary[3].Num++;
                        UpdateData(3);
                    }
                    else
                    {
                        labelSum.Text = "￥" + sum / 100.0;
                        if (commodity3.Num == 0)
                        {
                            commodity3.Num = 1;
                        }
                        dictionary.Add(3, commodity3);
                        flowLayoutPanel1.Controls.Add(item3);
                    }

                    //                    MessageBox.Show("label_3");
                    break;
                case "label_4":
                    sum += commodity4.price;
                    if (dictionary.ContainsKey(4))
                    {
                        dictionary[4].Num++;
                        UpdateData(4);
                    }
                    else
                    {
                        labelSum.Text = "￥" + sum / 100.0;
                        if (commodity4.Num == 0)
                        {
                            commodity4.Num = 1;
                        }
                        dictionary.Add(4, commodity4);
                        flowLayoutPanel1.Controls.Add(item4);
                    }

                    //                    MessageBox.Show("label_4");
                    break;
            }

        }

        private void UpdateData(int index)
        {
            Item temp = item1;
            switch (index)
            {
                case 1:
                    temp = item1;
                    break;
                case 2:
                    temp = item2;
                    break;
                case 3:
                    temp = item3;
                    break;
                case 4:
                    temp = item4;
                    break;
            }

            if (!dictionary.ContainsKey(index))
            {
                //                if (dictionary.Count == 0)
                //                {
                labelSum.Text = "￥" + sum / 100.0;
                //                }
                return;
            }
            temp.LabelGram.Text = dictionary[index].Gram.ToString() + "g";
            temp.LabelNum.Text = dictionary[index].Num.ToString();
            temp.LabelPrice.Text = "￥" + dictionary[index].Price / 100.0;
            labelSum.Text = "￥" + sum / 100.0;
        }

        private void SubmitOrder(object sender, EventArgs e)
        {
            OrderSubmit();
        }

        private void OrderSubmit()
        {
            if (sum == 0)
            {
                return;
            }
            List<OrderItem> lists = new List<OrderItem>();
            foreach (int id in dictionary.Keys)
            {
                OrderItem item = new OrderItem(dictionary[id].Name, dictionary[id].price.ToString(),
                    dictionary[id].id.ToString(), dictionary[id].Num.ToString());
                lists.Add(item);
            }

            Order order = new Order(sum.ToString(), Guid.NewGuid().ToString("N"),
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), lists);
            OrderParam param = new OrderParam("2000", "abc123", order, Guid.NewGuid().ToString("N"));
            string json = JsonConvert.SerializeObject(param);
            byte[] body = Encoding.UTF8.GetBytes(json);
            int length = body.Length;
            byte[] contentLength = BitConverter.GetBytes(length);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(contentLength);
            }

            byte[] bytes = copybyte(contentLength, body);
            socketSend.Send(bytes);
        }

        public static byte[] copybyte(byte[] a, byte[] b)
        {
            byte[] c = new byte[a.Length + b.Length];
            a.CopyTo(c, 0);
            b.CopyTo(c, a.Length);
            return c;
        }



        /// <summary>
        /// 用于接收服务器传来的消息
        /// </summary>
        void Recive()
        {
            while (true)
            {
                byte[] buffer = new byte[1024]; //设置缓存大小
                
                int r = socketSend.Receive(buffer);
                //实际接收到的有效字节数
                if (r == 0)
                {
                    break;
                }
                
//                MessageBox.Show("消费成功！");
                Application.Run(new Tip());
//                byte[] body = new byte[r-4]; //设置缓存大小
//                Array.Copy(buffer, 4, body, 0, r - 4);
                
                //TODO 接收消息
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void ResetColor(object sender, MouseEventArgs e)
        {
            Label label = sender as Label;
            label.ForeColor = System.Drawing.Color.Black;
        }

        private void ChangeColor(object sender, MouseEventArgs e)
        {
            Label label = sender as Label;
            label.ForeColor = System.Drawing.Color.Red;
        }

        public void SubtractItem(object sender, EventArgs e)
        {
            Button button = sender as Button;
            switch (button.Name)
            {
                case "btn1":
                    sum -= commodity1.price;
                    CheckData(1);
                    break;
                case "btn2":
                    sum -= commodity2.price;
                    CheckData(2);
                    break;
                case "btn3":
                    sum -= commodity3.price;
                    CheckData(3);
                    break;
                case "btn4":
                    sum -= commodity4.price;
                    CheckData(4);
                    break;
            }
        }

        public void CheckData(int index)
        {
            if (dictionary.ContainsKey(index))
            {
                dictionary[index].Num--;
                if (dictionary[index].Num == 0)
                {
                    dictionary.Remove(index);
                    flowLayoutPanel1.Controls.Remove(ItemList[index - 1]);
                }
                UpdateData(index);
            }
        }
    }
}
