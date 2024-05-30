using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto_final
{
    internal class Bombas
    {
        private string codigo_bomba;
        private string nombre_Venta;
        private double capacidad;
        private double cantidad_disponible;

        public Bombas()
        {
            this.Codigo_bomba = "";
            this.Nombre_Venta = "";
            this.Capacidad = 0.0;
            this.Cantidad_disponible = 0.0;
        }

        public string Codigo_bomba { get => codigo_bomba; set => codigo_bomba = value; }
        public string Nombre_Venta { get => nombre_Venta; set => nombre_Venta = value; }
        public double Capacidad { get => capacidad; set => capacidad = value; }
        public double Cantidad_disponible { get => cantidad_disponible; set => cantidad_disponible = value; }
        
    }
}
