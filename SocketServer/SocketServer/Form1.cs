using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.skinEngine1.SkinFile = System.Environment.CurrentDirectory + "\\MacOS.ssk";  //使用苹果风格
            txtLog.ReadOnly = true;                                                           //设置不能手动输入
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        //相关变量定义
        Socket socketSend;
        string ipAddress;
        Dictionary<string, Socket> dicSocket = new Dictionary<string, Socket>();

        /// <summary>
        /// 启动监听功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Start_Click(object sender, EventArgs e)
        {
            //创建一个Socket对象
            Socket socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //设置指定的端口号和服务器的IP地址
            IPAddress ip = IPAddress.Parse(txt_IP.Text);
            IPEndPoint endpoint = new IPEndPoint(ip, Convert.ToInt32(txt_Port.Text));

            //绑定IPEndPoint
            socketWatch.Bind(endpoint);
            ShowMsg("监听成功");

            //设置监听20个客户端服务器：
            socketWatch.Listen(20);

            //创建线程用来监听;
            //线程创建方法：Thread th = new Thread(Listen);  
            //其中listen为方法名 即实例化
            Thread th = new Thread(Listen);
            th.IsBackground = true;
            th.Start(socketWatch);    //Start里面的参数 是线程方法的参数
        }

        /// <summary>
        /// 显示传输信息到textbox.
        /// </summary>
        /// <param name="str"></param>
        void ShowMsg(string str)
        {
            txtLog.AppendText(str + "\r\n");
        }

        /// <summary>
        /// 等待客户端的连接 并且创建与之通信用的Socket
        /// </summary>
        void Listen(Object o)
        {
            Socket socketWatch = o as Socket;
            while (true)
            {
                try
                {
                    //创建一个新的用于与客户端进行通信的socket对象
                    socketSend = socketWatch.Accept();
                    ipAddress = socketSend.RemoteEndPoint.ToString();   //获得客户端的ip地址和端口号  ipAddress格式例如：127.0.0.1:50000

                    dicSocket.Add(socketSend.RemoteEndPoint.ToString(), socketSend);
                    //192.168...连接成功
                    ShowMsg(ipAddress + ":" + "连接成功");
                    //创建一个新的线程，用来不停的接收客户端发送过来的消息
                    Thread th = new Thread(Recive);
                    th.IsBackground = true;
                    th.Start(socketSend);
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="o"></param>
        void Recive(object o)
        {
            Socket socketSend = o as Socket;
            while (true)
            {
                byte[] buffer = new byte[1024 * 1024 * 10];   //设置缓存大小
                int r = socketSend.Receive(buffer);
                //实际接收到的有效字节数
                if (r == 0)
                {
                    break;
                }
                if (buffer[0] == 0)      //表示收到的是数据
                {
                    string s = Encoding.UTF8.GetString(buffer, 1, r - 1);
                    ShowMsg(socketSend.RemoteEndPoint + ":" + s);
                }
                else if (buffer[0] == 1)   //表示收到的是文件
                {
                    string filePath = "";
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Title = "保存文件";
                    sfd.InitialDirectory = @"C:\Users\Administrator\Desktop";
                    sfd.Filter = "文本文件|*.txt|图片文件|*.jpg|视频文件|*.avi|音乐文件|*.mp3|所有文件|*.*";
                    while (true)
                    {
                        sfd.ShowDialog(this);
                        filePath = sfd.FileName;
                        if (string.IsNullOrEmpty(filePath))
                        {
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                    //保存接收的文件
                    using (FileStream fsWrite = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        fsWrite.Write(buffer, 1, r - 1);
                    }
                    ShowMsg(socketSend.RemoteEndPoint + ": 接收文件成功");
                }
                else if (buffer[0] == 2)
                {
                    Popping();
                }
            }
        }

        /// <summary>
        /// 发送消息到客户端
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Send_Click(object sender, EventArgs e)
        {
            string str = txt_Message.Text;
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str); //将字符串转成二进制格式字节
            List<byte> list = new List<byte>();
            list.Add(0);  //让客户端知道现在发送的文字
            list.AddRange(buffer);
            byte[] newBuffer = list.ToArray();
            dicSocket[ipAddress].Send(newBuffer);  //传输字符串到客户端
            txt_Message.Text = "";
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog.Title = "请选择文件";
            openFileDialog.Filter = "所有文件|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txt_Path.Text = openFileDialog.FileName;  //路径传到txt文本上
            }
        }

        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SendFile_Click(object sender, EventArgs e)
        {
            string path = txt_Path.Text;
            using (FileStream fsRead = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read))
            {
                //1. 第一步：发送一个包，表示文件的长度，让客户端知道后续要接收几个包来重新组织成一个文件
                long length = fsRead.Length;
                byte[] byteLength = Encoding.UTF8.GetBytes(length.ToString());
                //获得发送的信息时候，在数组前面加上一个字节 1
                List<byte> list = new List<byte>();
                list.Add(1);
                list.AddRange(byteLength);
                socketSend.Send(list.ToArray()); //
                //2. 第二步：每次发送一个1MB的包，如果文件较大，则会拆分为多个包
                byte[] buffer = new byte[1024 * 1024 * 200];
                long send = 0; //发送的字节数                  
                while (true)  //大文件断点多次传输
                {
                    int r = fsRead.Read(buffer, 0, buffer.Length);
                    if (r == 0)
                    {
                        break;
                    }
                    socketSend.Send(buffer, 0, r, SocketFlags.None);
                    send += r;
                    ShowMsg(string.Format("{0}: 已发送：{1}/{2}", socketSend.RemoteEndPoint, send, length));
                }
                ShowMsg("发送完成");
            }
        }

        /// <summary>
        /// 发送震动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Popping_Click(object sender, EventArgs e)
        {
            byte[] buffer = new byte[1];
            buffer[0] = 2;  //字节为2表示震动
            dicSocket[ipAddress].Send(buffer);
        }

        /// <summary>
        /// 关闭监听进程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CloseListen_Click(object sender, EventArgs e)
        {
            socketSend.Close();
        }

        /// <summary>
        /// 震动
        /// </summary>
        void Popping()
        {
            Point p = this.Location;
            int x = p.X;
            int y = p.Y;
            int j;
            for (int i = 0; i < 200; i++)
            {
                this.Location = new Point(x, y + 5);
                for (j = 0; j < 10000; j++) ;
                this.Location = new Point(x + 5, y + 5);
                for (j = 0; j < 10000; j++) ;
                this.Location = new Point(x + 5, y);
                for (j = 0; j < 10000; j++) ;
                this.Location = new Point(x, y + 5);
                for (j = 0; j < 10000; j++) ;
            }
        }
    }
}
