using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.ComponentModel.Com2Interop;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace proyecto_final
{
    public partial class Form1 : Form
    {
        private Control_Distribucion controlDistribucion = new Control_Distribucion();
        private int contador = 0;
        private int numeroDestino;
        private bool esBomba1 = true; // Establecer un valor por defecto
        private float precioDelDia;
        private int codigo_bomba;
        private string tipo_ventas;
        private float capacidadBomba1 = 1000;
        private float capacidadBomba2 = 1000;
        private bool ventaGuardada = false;
        public Form1()
        {
            InitializeComponent();
            this.Load += new System.EventHandler(this.Form1_Load);

            timer1.Interval = 70; // Intervalo de 1 segundo
            timer1.Tick += Timer1_Tick; // Asocia el evento Tick con el manejador de eventos

            timerHora.Interval = 1000; // Intervalo de 1 segundo para actualizar la hora
            timerHora.Tick += TimerHora_Tick; // Asocia el evento Tick con el manejador de eventos

            timerVP.Interval = 70; // Intervalo de 1 segundo para actualizar la hora
            timerVP.Tick += timerVP_Tick; // Asocia el evento Tick con el manejador de eventos
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            TxtFechaVG.Text = DateTime.Now.ToString("dd/MM/yyyy");
            TxtFechaVP.Text = DateAndTime.Now.ToString("dd/MM/yyyy");
            TxtHoraVG.Text = DateTime.Now.ToString("HH:mm:ss");
            TxtHoraVP.Text = DateTime.Now.ToString("HH:mm:ss");
            timerHora.Start();
            ReiniciarBombas();
        }
        private void ReiniciarBombas()
        {
            capacidadBomba1 = 1000;
            capacidadBomba2 = 1000;
            MessageBox.Show("Las capacidades de las bombas han sido reiniciadas a 1000 galones.");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Validar y obtener el número ingresado en TxtCantidadVG
            if (int.TryParse(TxtCantidadVG.Text, out numeroDestino))
            {
                // Reiniciar el contador
                contador = 0;

                // Configurar el Timer
                timer1.Stop(); // Detener el timer si estaba en ejecución
                timer1.Interval = 70; // Intervalo en milisegundos
                timer1.Tick -= Timer1_Tick; // Eliminar el evento anterior para evitar múltiples suscripciones
                timer1.Tick += Timer1_Tick; // Asocia el evento Timer1_Tick al Timer
                timer1.Start(); // Inicia el Timer
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un número válido en el campo de Número de Destino.");
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            tipo_ventas = "Galon";

            if (float.TryParse(TxtPrecioDelDiaVG.Text, out precioDelDia))
            {
                if (contador < numeroDestino && ((esBomba1 && contador < capacidadBomba1) || (!esBomba1 && contador < capacidadBomba2)))
                {
                    contador++;

                    if (esBomba1)
                    {
                        codigo_bomba = 01;
                    }
                    else
                    {
                        codigo_bomba = 02;
                    }

                    TxtCantidadGalonesVG.Text = contador.ToString() + " galones";
                }
                else
                {
                    timer1.Stop();
                    MessageBox.Show("El contador ha llegado al número objetivo o la bomba está vacía.", "Completado");

                    float totalPagar = contador * precioDelDia;
                    TxtTotalVG.Text = "Q " + totalPagar.ToString("F2");

                    if (contador < numeroDestino)
                    {
                        MessageBox.Show($"No se pudo completar la cantidad solicitada. Faltaron {numeroDestino - contador} galones.", "Advertencia");

                        float totalAjustado = contador * precioDelDia;
                        if (!ventaGuardada)
                        {
                            controlDistribucion.GuardarVenta(TxtNombreVG.Text, TxtApellidoVG.Text, TxtNitVG.Text, TxtTelefonoVG.Text,
                                TxtPrecioDelDiaVG.Text, contador.ToString(), tipo_ventas, "0", totalAjustado.ToString("F2"),
                                TxtFechaVG.Text, TxtHoraVG.Text, codigo_bomba.ToString());
                            ventaGuardada = true;
                        }
                    }
                    else
                    {
                        controlDistribucion.GuardarVenta(TxtNombreVG.Text, TxtApellidoVG.Text, TxtNitVG.Text, TxtTelefonoVG.Text,
                            TxtPrecioDelDiaVG.Text, contador.ToString(), tipo_ventas, "0", totalPagar.ToString("F2"),
                            TxtFechaVG.Text, TxtHoraVG.Text, codigo_bomba.ToString());
                    }

                    if (esBomba1)
                    {
                        capacidadBomba1 -= contador;
                    }
                    else
                    {
                        capacidadBomba2 -= contador;
                    }

                    ventaGuardada = false; // Reset ventaGuardada para la próxima venta
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingresa un precio del día válido.", "Error");
                timer1.Stop();
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            TxtNombreVG.Clear();
            TxtApellidoVG.Clear();
            TxtNitVG.Clear();
            TxtTelefonoVG.Clear();
            TxtPrecioDelDiaVG.Clear();
            TxtCantidadVG.Clear();
            TxtCantidadGalonesVG.Clear();
            TxtDescuentoVG.Clear();
            TxtTotalVG.Clear();
            LstFacturaVG.Items.Clear();
        }
        private void BtnCobrar_Click(object sender, EventArgs e)
        {
            // Nombre del archivo donde se registrarán los datos
            string nombrearchivo = "ventas.txt";

            // Validar y obtener el nombre y apellido
            if (string.IsNullOrWhiteSpace(TxtNombreVG.Text) || string.IsNullOrWhiteSpace(TxtApellidoVG.Text))
            {
                MessageBox.Show("Por favor, ingrese el nombre y apellido.");
                return;
            }

            // Validar y obtener el NIT
            if (string.IsNullOrWhiteSpace(TxtNitVG.Text))
            {
                MessageBox.Show("Por favor, ingrese el NIT.");
                return;
            }

            // Validar y obtener el teléfono
            if (string.IsNullOrWhiteSpace(TxtTelefonoVG.Text))
            {
                MessageBox.Show("Por favor, ingrese el teléfono.");
                return;
            }

            // Validar y obtener el precio del día
            if (!float.TryParse(TxtPrecioDelDiaVG.Text, out float precioDia))
            {
                MessageBox.Show("Por favor, ingrese un precio del día válido.");
                return;
            }

            // Validar y obtener el número de galones
            if (!int.TryParse(TxtCantidadVG.Text, out int numeroGalones))
            {
                MessageBox.Show("Por favor, ingrese una cantidad de galones válida.");
                return;
            }

            // Calcular el total a pagar
            float totalPagar = numeroGalones * precioDia;

            // Determinar el código de la bomba
            string codigo_bomba = esBomba1 ? "01" : "02";

            // Construir la línea con los datos
            string linea = $"{TxtNombreVG.Text};{TxtApellidoVG.Text};{TxtNitVG.Text};{TxtTelefonoVG.Text};" +
                           $"{precioDia};{numeroGalones};{totalPagar};{codigo_bomba};{tipo_ventas};" +
                           $"{TxtFechaVG.Text};{TxtHoraVG.Text}";

            // Registrar los datos en el archivo
            using (StreamWriter EscribirArchivo = new StreamWriter(nombrearchivo, true))
            {
                EscribirArchivo.WriteLine(linea);
            }

            MessageBox.Show("Datos agregados correctamente.");

            // Limpiar los TextBox y el ListBox después de registrar los datos
            TxtNombreVG.Clear();
            TxtApellidoVG.Clear();
            TxtNitVG.Clear();
            TxtTelefonoVG.Clear();
            TxtPrecioDelDiaVG.Clear();
            TxtCantidadVG.Clear();
            TxtCantidadGalonesVG.Clear();
            TxtDescuentoVG.Clear();
            TxtTotalVG.Clear();
            LstFacturaVG.Items.Clear();
        }
        //-----------------------------------------------------------------------------------------------------
        private void TimerHora_Tick(object sender, EventArgs e)
        {
            // Actualiza el campo de texto de la hora con la hora actual en tiempo real
            TxtHoraVG.Text = DateTime.Now.ToString("HH:mm:ss");
            TxtHoraVP.Text = DateTime.Now.ToString("HH:mm:ss");
        }
        //------------------------------------------------------------------------------------------------------
        private void BtnMostrarVentas_Click(object sender, EventArgs e)
        {
            string nombrearchivo = "Ventas.txt";
            if (File.Exists(nombrearchivo))
            {
                try
                {
                    string[] lineas = File.ReadAllLines(nombrearchivo);
                    listBox1.Items.Clear(); // Limpiar el ListBox antes de agregar nuevos elementos
                    foreach (string linea in lineas)
                    {
                        listBox1.Items.Add(linea);
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
        }

        private void BtnEstadistica_Click(object sender, EventArgs e)
        {
            Control_Distribucion controlDistribucion = new Control_Distribucion();

            // Obtener la fecha actual en el formato dd/MM/yyyy
            string fechaActual = DateTime.Now.ToString("dd/MM/yyyy");

            // Calcular el total del cierre de caja diario para la fecha actual
            decimal totalCierreCaja = controlDistribucion.CalcularTotalCierreCajaDiario(fechaActual);

            // Mostrar el total del cierre de caja en un ListBox
            listBox2.Items.Add($"Total del cierre de caja para el {fechaActual}: Q {totalCierreCaja:F2}");
        }


        //-----------------------------------------------------------------------------------------------------
        private void timerVP_Tick(object sender, EventArgs e)
        {
            // Intentar convertir el texto del TextBox TxtPrecioDelDiaVP a un float
            if (float.TryParse(TxtPrecioDelDiaVP.Text, out precioDelDia))
            {
                // Calcular el total de galones basado en la cantidad ingresada y el precio por galón
                float galonesCalculados = float.Parse(TxtCantidadVP.Text) / precioDelDia;

                // Actualizar la capacidad de la bomba correspondiente
                if (esBomba1)
                {
                    capacidadBomba1 -= galonesCalculados;
                    codigo_bomba = 01;
                }
                else
                {
                    capacidadBomba2 -= galonesCalculados;
                    codigo_bomba = 02;
                }

                // Mostrar el contador de galones
                TxtCantidadGalonesVP.Text = galonesCalculados.ToString("F2") + " Galones";

                // Calcular la cantidad solicitada
                float cantidadSolicitada = float.Parse(TxtCantidadVP.Text);

                // Calcular la diferencia entre la cantidad solicitada y la cantidad calculada
                float diferencia = cantidadSolicitada - galonesCalculados;

                // Mostrar la diferencia en el TextBox de descuento
                float diferenciaGalones = diferencia / precioDelDia;
                TxtDescuentoVP.Text = diferenciaGalones.ToString("F2") + " Galones";

                // Calcular el nuevo total
                float nuevoTotal = diferenciaGalones * precioDelDia;

                // Mostrar el total en el TextBox TxtTotalVP
                TxtTotalVP.Text = "Q " + nuevoTotal.ToString("F2");

                // Detener el temporizador
                timerVP.Stop();

                // Verificar si la cantidad solicitada fue completamente satisfecha
                if (cantidadSolicitada > galonesCalculados)
                {
                    MessageBox.Show($"No se pudo completar la cantidad solicitada. Faltaron {diferencia.ToString("F2")}", "Advertencia");
                }

                // Guardar la venta
                controlDistribucion.GuardarVenta(TxtNombreVP.Text, TxtApellidoVP.Text, TxtNitVP.Text, TxtTelefonoVP.Text,
                                                  TxtPrecioDelDiaVP.Text, galonesCalculados.ToString("F2"), tipo_ventas, TxtDescuentoVP.Text,
                                                  galonesCalculados.ToString("F2"), TxtFechaVP.Text, TxtHoraVP.Text, codigo_bomba.ToString());
            }
            else
            {
                // Mostrar un mensaje de error si el precio del día no es válido
                MessageBox.Show("Por favor, ingresa un precio del día válido.", "Error");
            }
        }

        private void BtnSeleccionBombaVP_Click(object sender, EventArgs e)
        {
            // Validar y obtener el número ingresado en TxtCantidadVG
            if (int.TryParse(TxtCantidadVP.Text, out numeroDestino))
            {
                // Reiniciar el contador
                contador = 0;

                // Configurar el Timer
                timerVP.Stop(); // Detener el timer si estaba en ejecución
                timerVP.Interval = 70; // Intervalo en milisegundos
                timerVP.Tick -= timerVP_Tick; // Eliminar el evento anterior para evitar múltiples suscripciones
                timerVP.Tick += timerVP_Tick; // Asocia el evento Timer1_Tick al Timer
                timerVP.Start(); // Inicia el Timer
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un número válido en el campo de Número de Destino.");
            }
        }
        private void BtnRealizar_Cobro_Click(object sender, EventArgs e)
        {
            tipo_ventas = "Precio";
            // Nombre del archivo donde se registrarán los datos
            string nombrearchivo = "ventas.txt";

            // Validar y obtener el nombre y apellido
            if (string.IsNullOrWhiteSpace(TxtNombreVP.Text) || string.IsNullOrWhiteSpace(TxtApellidoVP.Text))
            {
                MessageBox.Show("Por favor, ingrese el nombre y apellido.");
                return;
            }

            // Validar y obtener el NIT
            if (string.IsNullOrWhiteSpace(TxtNitVP.Text))
            {
                MessageBox.Show("Por favor, ingrese el NIT.");
                return;
            }

            // Validar y obtener el teléfono
            if (string.IsNullOrWhiteSpace(TxtTelefonoVP.Text))
            {
                MessageBox.Show("Por favor, ingrese el teléfono.");
                return;
            }

            // Validar y obtener el precio del día
            if (!float.TryParse(TxtPrecioDelDiaVP.Text, out float precioDia))
            {
                MessageBox.Show("Por favor, ingrese un precio del día válido.");
                return;
            }

            // Validar y obtener el número de galones
            if (!int.TryParse(TxtCantidadVP.Text, out int numeroDinero))
            {
                MessageBox.Show("Por favor, ingrese una cantidad de galones válida.");
                return;
            }

            // Calcular el total a pagar
            float totalPagar = numeroDinero / precioDia;

            // Determinar el código de la bomba
            string codigo_bomba = esBomba1 ? "01" : "02";

            // Construir la línea con los datos
            string linea = $"{TxtNombreVP.Text};{TxtApellidoVP.Text};{TxtNitVP.Text};{TxtTelefonoVP.Text};" +
                           $"{TxtPrecioDelDiaVP.Text};{numeroDinero};{totalPagar};{codigo_bomba};{tipo_ventas};" +
                           $"{TxtFechaVP.Text};{TxtHoraVP.Text}";

            // Registrar los datos en el archivo
            using (StreamWriter EscribirArchivo = new StreamWriter(nombrearchivo, true))
            {
                EscribirArchivo.WriteLine(linea);
            }

            // Mostrar un mensaje de éxito simple
            MessageBox.Show("Datos agregados correctamente.");

            // Limpiar los TextBox y el ListBox después de registrar los datos
            TxtNombreVP.Clear();
            TxtApellidoVP.Clear();
            TxtNitVP.Clear();
            TxtTelefonoVP.Clear();
            TxtPrecioDelDiaVP.Clear();
            TxtCantidadVP.Clear();
            TxtCantidadGalonesVP.Clear();
            TxtDescuentoVP.Clear();
            TxtTotalVP.Clear();
            LstFacturaVP.Items.Clear();
        }
        private void BtnCancelar_Cobro_Click(object sender, EventArgs e)
        {
            TxtNombreVG.Clear();
            TxtApellidoVG.Clear();
            TxtNitVG.Clear();
            TxtTelefonoVG.Clear();
            TxtPrecioDelDiaVG.Clear();
            TxtCantidadVG.Clear();
            TxtCantidadGalonesVG.Clear();
            TxtDescuentoVG.Clear();
            TxtTotalVG.Clear();
            LstFacturaVG.Items.Clear();
        }
        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }
        private void label9_Click(object sender, EventArgs e)
        {

        }
        private void label31_Click(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void label34_Click(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click_1(object sender, EventArgs e)
        {

        }


    }
}