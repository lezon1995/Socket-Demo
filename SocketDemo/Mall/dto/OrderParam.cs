using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.dto
{
    class OrderParam
    {
        /// <summary>
        /// 固定值200
        /// </summary>
        private string command;
        /// <summary>
        /// misId
        /// </summary>
        private string misId;
        /// <summary>
        /// 订单
        /// </summary>
        private Order order;
        /// <summary>
        /// messageId
        /// </summary>
        private string messageId;

        public OrderParam()
        {
        }

        public OrderParam(string command, string misId, Order order, string messageId)
        {
            this.command = command;
            this.misId = misId;
            this.order = order;
            this.messageId = messageId;
        }

        public string Command
        {
            get => command;
            set => command = value;
        }

        public string MisId
        {
            get => misId;
            set => misId = value;
        }

        public Order Order
        {
            get => order;
            set => order = value;
        }

        public string MessageId
        {
            get => messageId;
            set => messageId = value;
        }
    }

    class Order
    {
        private string totalPrice;
        private string orderNum;
        private string orderTime;
        private string orderPushedTime;
        private List<OrderItem> lists;


        public Order(string totalPrice, string orderNum, string orderTime, string orderPushedTime, List<OrderItem> lists)
        {
            this.totalPrice = totalPrice;
            this.orderNum = orderNum;
            this.orderTime = orderTime;
            this.orderPushedTime = orderPushedTime;
            this.lists = lists;
        }

        public string TotalPrice
        {
            get => totalPrice;
            set => totalPrice = value;
        }

        public string OrderNum
        {
            get => orderNum;
            set => orderNum = value;
        }

        public string OrderTime
        {
            get => orderTime;
            set => orderTime = value;
        }

        public string OrderPushedTime
        {
            get => orderPushedTime;
            set => orderPushedTime = value;
        }

        public List<OrderItem> Lists
        {
            get => lists;
            set => lists = value;
        }
    }

    class OrderItem
    {
        private string name;
        private string price;
        private string commodityId;
        private string num;

        public OrderItem(string name, string price, string commodityId, string num)
        {
            this.name = name;
            this.price = price;
            this.commodityId = commodityId;
            this.num = num;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public string Price
        {
            get => price;
            set => price = value;
        }

        public string CommodityId
        {
            get => commodityId;
            set => commodityId = value;
        }

        public string Num
        {
            get => num;
            set => num = value;
        }
    }
}
