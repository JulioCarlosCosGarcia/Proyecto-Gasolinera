using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto_final
{   
    internal class Clientes
    {
        private string nombres;
        private string apellidos;
        private int NIT;
        private int numero_Telefono;
        private DateTime fecha;
        private DateTime Hora;
        private Clientes siguiente;

        public Clientes()
        {
            this.Nombres = " ";
            this.Apellidos = " ";
            this.NIT1 = 0;
            this.Numero_Telefono = 0;
            this.Fecha = DateTime.Now;
            this.Hora1 = DateTime.Now;
            this.Siguiente = null;
        }

        public string Nombres { get => nombres; set => nombres = value; }
        public string Apellidos { get => apellidos; set => apellidos = value; }
        public int NIT1 { get => NIT; set => NIT = value; }
        public int Numero_Telefono { get => numero_Telefono; set => numero_Telefono = value; }
        public DateTime Fecha { get => fecha; set => fecha = value; }
        public DateTime Hora1 { get => Hora; set => Hora = value; }
        internal Clientes Siguiente { get => siguiente; set => siguiente = value; }
    }
}
