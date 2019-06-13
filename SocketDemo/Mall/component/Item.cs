using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mall.dto;
using Mall.Properties;

namespace Mall.component
{
    class Item : Panel
    {
        private Label labelNum;
        private Label labelGram;
        private Label labelPrice;
        private Form1 form;
        public static System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
        public Item(Commodity commodity, Form1 form1)
        {
            this.form = form1;
            Location = new System.Drawing.Point(3, 3);
            Size = new System.Drawing.Size(795, 233);
            this.TabIndex = 1;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            labelNum = new Label
            {
                AutoSize = true,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134))),
                Location = new System.Drawing.Point(686, 83),
                Size = new System.Drawing.Size(44, 48),
                TabIndex = 10,
                Text = commodity.Num.ToString(),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            this.Controls.Add(labelNum);
            Button btnAdd = new Button
            {
                Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold,
                    System.Drawing.GraphicsUnit.Point, ((byte) (134))),
                Location = new System.Drawing.Point(736, 83),
                Size = new System.Drawing.Size(50, 50),
                TabIndex = 9,
                Name = "label_" + commodity.Id.ToString(),
                Text = "+",
                UseVisualStyleBackColor = true
            };
            btnAdd.Click += form.AddToShoppingCart;
            this.Controls.Add(btnAdd);
            Button btnSubtract = new Button
            {
                Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold,
                    System.Drawing.GraphicsUnit.Point, ((byte) (134))),
                Location = new System.Drawing.Point(630, 83),
                Size = new System.Drawing.Size(50, 50),
                TabIndex = 8,
                Name ="btn"+ commodity.Id.ToString(),
                Text = "-",
                UseVisualStyleBackColor = true,
            };
            btnSubtract.Click += form.SubtractItem;
            this.Controls.Add(btnSubtract);
            labelGram = new Label
            {
                Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134))),
                Location = new System.Drawing.Point(407, 188),
                Size = new System.Drawing.Size(61, 26),
                AutoSize = true,
                ForeColor = System.Drawing.Color.Gray,
                TabIndex = 7,
                Text = commodity.Gram + "g"
            };
            this.Controls.Add(labelGram);
            labelPrice = new Label
            {
                Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134))),
                Location = new System.Drawing.Point(304, 126),
                Size = new System.Drawing.Size(84, 26),
                AutoSize = true,
                ForeColor = System.Drawing.Color.Red,
                TabIndex = 6,
                Text = "￥" + commodity.Price / 100.0,
            };
            this.Controls.Add(labelPrice);
            this.Controls.Add(new Label
            {
                Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134))),
                Location = new System.Drawing.Point(304, 79),
                Size = new System.Drawing.Size(164, 26),
                AutoSize = true,
                TabIndex = 5,
                Text = commodity.Name
            });
            this.Controls.Add(new Label
            {
                Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134))),
                Location = new System.Drawing.Point(6, 102),
                Size = new System.Drawing.Size(39, 26),
                AutoSize = true,
                TabIndex = 4,
                Text = "✔"
            });
            Image image = null;
            switch (commodity.Id)
            {
                case 1:
                    image = Resources.img_grapefruit;
                    break;
                case 2:
                    image = Resources.img_orange;
                    break;
                case 3:
                    image = Resources.img_kiwifruit;
                    break;
                case 4:
                    image = Resources.img_apple;
                    break;
                default:
                    break;
            }
            this.Controls.Add(new PictureBox
            {
                Location = new System.Drawing.Point(51, 14),
                Size = new System.Drawing.Size(200, 200),
                BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch,
                Image = image,
                SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage,
                TabIndex = 0,
                TabStop = false
            });
        }

        public Label LabelNum
        {
            get => labelNum;
            set => labelNum = value;
        }

        public Label LabelGram
        {
            get => labelGram;
            set => labelGram = value;
        }

        public Label LabelPrice
        {
            get => labelPrice;
            set => labelPrice = value;
        }

        
    }
}
