using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.dto
{
    class Commodity
    {
        public int id;
        public string name;
        public int price;
        public int num;
        public int gram;

        public Commodity(int id, string name, int price, int num, int gram)
        {
            this.id = id;
            this.name = name;
            this.price = price;
            this.num = num;
            this.gram = gram;
        }

        public int Gram
        {
            get => gram*num;
            set => gram = value;
        }

        public int Id
        {
            get => id;
            set => id = value;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public int Price
        {
            get => price*num;
            set => price = value;
        }

        public int Num
        {
            get => num;
            set => num = value;
        }

        public override string ToString()
        {
            return $"{nameof(Gram)}: {Gram}, {nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Price)}: {Price}, {nameof(Num)}: {Num}";
        }


        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if ((obj.GetType().Equals(this.GetType())) == false)
            {
                return false;
            }
            Commodity temp = null;
            temp = (Commodity)obj;

            return this.Id.Equals(temp.Id);

        }

        //重写GetHashCode方法（重写Equals方法必须重写GetHashCode方法，否则发生警告

        public override int GetHashCode()
        {
            return this.Id.GetHashCode() + this.Id.GetHashCode();
        }
    }
}
