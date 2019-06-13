using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.dto
{
    class LoginParam
    {
        /// <summary>
        /// 固定值 1000
        /// </summary>
        private string command;
        /// <summary>
        /// 每一个mis一个
        /// </summary>
        private string misId;
        /// <summary>
        /// 如果是连锁店，可以给对应的号码
        /// </summary>
        private string merchantCode;
        /// <summary>
        /// 32位UUID 
        /// </summary>
        private string messageId;

        public LoginParam()
        {
        }

        public LoginParam(string command, string misId, string merchantCode, string messageId)
        {
            this.command = command;
            this.misId = misId;
            this.merchantCode = merchantCode;
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

        public string MerchantCode
        {
            get => merchantCode;
            set => merchantCode = value;
        }

        public string MessageId
        {
            get => messageId;
            set => messageId = value;
        }
    }
}
