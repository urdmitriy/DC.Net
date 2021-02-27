using System;

namespace DC_dotNet
{
    //класс для списка файлов
    public class Files 
    {
        public string name { get; set; }
        public string date { get; set; }
        public string size { get; set; }
        public Files(string _name, string _date, string _size)
        {
            name = _name;
            date = _date;
            size = _size;
        }
        public Files(string _name)
        {
            name = _name;
        }
    }


        
}
