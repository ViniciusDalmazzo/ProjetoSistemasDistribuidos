using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Entities
{
    public class Configuracao
    {
        public string IP { get; set; }
        public int Peso { get; set; }

        public bool EhLider { get; set; }

        public Configuracao(string ip, string peso)
        {
            IP = ip;
            Peso = Convert.ToInt32(peso);
            EhLider = false;
        }
    }
}
