using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace proyecto_final
{
    internal class Control_Distribucion
    {
            public void GuardarVenta(string nombre, string apellido, string nit, string telefono, string precioDia,
                                        string codigoBomba, string tipo_ventas, string cantidadGalones, string descuento,string total, string fecha, string hora)
            {
                string nombrearchivo = "Ventas.txt";
                using (StreamWriter EscribirArchivo = new StreamWriter(nombrearchivo, true))
                {
                    string linea = $"{nombre};{apellido};{nit};{telefono};{precioDia};{codigoBomba};{tipo_ventas};{cantidadGalones};{descuento};{total};{fecha};{hora}";
                    EscribirArchivo.WriteLine(linea);
                }
            }
            
    

        // Función para verificar si una venta ya existe en el archivo
        private bool ExisteVenta(string nombreArchivo, string nombre, string apellido, string nit, string telefono, string precioDia,
                                  string codigoBomba, string tipoVentas, string cantidadGalones, string descuento,
                                  string total, string fecha, string hora)
        {
            string[] lineas = File.ReadAllLines(nombreArchivo);
            string ventaActual = $"{nombre};{apellido};{nit};{telefono};{precioDia};{codigoBomba};{tipoVentas};{cantidadGalones};{descuento};{total};{fecha};{hora}";

            return lineas.Contains(ventaActual);
        }
        public decimal CalcularTotalCierreCajaDiario(string fecha)
        {
            string nombreArchivo = "VentasGasolina.txt"; // Cambiar al nombre del archivo correcto si es diferente
            decimal totalCierreCaja = 0;

            if (File.Exists(nombreArchivo))
            {
                try
                {
                    string[] lineas = File.ReadAllLines(nombreArchivo);

                    foreach (string linea in lineas)
                    {
                        string[] datos = linea.Split(';');

                        // Verificar si la línea tiene el formato esperado y corresponde a la fecha especificada
                        if (datos.Length >= 12 && datos[10] == fecha)
                        {
                            // Intentar convertir el total de la venta a decimal
                            if (decimal.TryParse(datos[9], out decimal totalVenta))
                            {
                                totalCierreCaja += totalVenta; // Sumar el total de la venta al total del cierre de caja
                            }
                            else
                            {
                                // Si hay un error al convertir el total de la venta, mostrar un mensaje de advertencia
                                MessageBox.Show($"Error al convertir el total de venta en la línea: {linea}", "Error de conversión");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al leer el archivo de ventas: " + ex.Message, "Error");
                }
            }
            else
            {
                MessageBox.Show("No se encontró el archivo de ventas.", "Error");
            }

            return totalCierreCaja;
        }


    }
}
