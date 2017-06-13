using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektPOS
{
    public class Postac
    {
     

        public int Id { get; set; }
        public string IMIE { get; set; }
        public string NAZWISKO { get; set; }
        public Postac() { }
        

        public Postac(int Id, string IMIE, string NAZWISKO)
        {
            this.Id = Id;
            this.IMIE = IMIE;
            this.NAZWISKO = NAZWISKO;
            
        }




    }
}
