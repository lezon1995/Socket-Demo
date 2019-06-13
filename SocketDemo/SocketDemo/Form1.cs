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

namespace SocketDemo
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
            txt_Listen.ReadOnly = true;                                                       //设置不能手动输入
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        //相关变量定义
        Socket socketSend;

        /// <summary>
        /// 连接到服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Connect_Click(object sender, EventArgs e)
        {
            //创建通信的Socket
            socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse(txt_IP.Text);
            IPEndPoint point = new IPEndPoint(ip, Convert.ToInt32(txt_Port.Text));
            //连接到对应的IP地址和端口号
            socketSend.Connect(point);
            ShowMsg("连接成功");
            Thread threadClient = new Thread(Recive);
            threadClient.IsBackground = true; //设置为后台线程
            threadClient.Start();

        }

        /// <summary>
        /// 信息框
        /// </summary>
        /// <param name="str"></param>
        void ShowMsg(string str)
        {
            txt_Listen.AppendText(str + "\r\n");
        }

        /// <summary>
        /// 用于接收服务器传来的消息
        /// </summary>
        void Recive()
        {
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

        /// <summary>
        /// 发送消息到服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Send_Click(object sender, EventArgs e)
        {
            string str = txt_SendMessage.Text.Trim();
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str); //转换成序列形式
            socketSend.Send(buffer);
            txt_SendMessage.Text = ""; //清空输入框
        }

        /// <summary>
        /// 关闭线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            socketSend.Shutdown(SocketShutdown.Both);
            socketSend.Close();
        }

        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SendFile_Click(object sender, EventArgs e)
        {
            string path = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @".\message";
            openFileDialog.Title = "请选择文件";
            openFileDialog.Filter = "所有文件|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog.FileName;  
            }
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
    }
}
