using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace FigurasAccion
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "server=localhost;user=root;password=;database=ventasacos;";
            MySqlConnection connection = new MySqlConnection(connectionString);

            int numero_clientes, codigo_cliente;
            string apellido, nombre, saco_desc;
            double precio;


            Console.WriteLine("Digite el numero de clientes: ");
            numero_clientes = Int32.Parse(Console.ReadLine());

            for (int i = 0; i < numero_clientes; i++)
            {
                Console.WriteLine("Digite el codigo del cliente");
                codigo_cliente = Int32.Parse(Console.ReadLine());
                Console.WriteLine("Apellido del cliente");
                apellido = Console.ReadLine();
                Console.WriteLine("Nombre del cliente");
                nombre = Console.ReadLine();
                Console.WriteLine("Descripcion del saco");
                saco_desc = Console.ReadLine();
                Console.WriteLine("Precio del saco sin IVA");
                precio = Double.Parse(Console.ReadLine());


                string insertQuery = $"INSERT INTO sacos (codigo_cliente, apellido, nombre, saco_desc, precio) VALUES ('{codigo_cliente}', '{apellido}', '{nombre}', '{saco_desc}', '{precio}' )";

                MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection);
                connection.Open();
                insertCommand.ExecuteNonQuery();
                connection.Close();

                while (true)
                {
                    Console.WriteLine("Menu Principal");
                    Console.WriteLine("1.Actualizar cliente");
                    Console.WriteLine("2.Eliminar cliente");
                    Console.WriteLine("3.Lista de clientes");
                    Console.WriteLine("4.Precio con Iva");
                    Console.WriteLine("5.Salir");
                    Console.WriteLine("escoje una opcion");
                    int opcion = Int32.Parse(Console.ReadLine());

                    switch (opcion)
                    {
                        case 1:
                            Console.WriteLine("Digite el nuevo codigo del cliente");
                            string codigoActualizar = Console.ReadLine();
                            Console.WriteLine("Digite el nuevo apellido del cliente");
                            string nuevoApellido = Console.ReadLine();
                            Console.WriteLine("Digite el nuevo nombre del cliente");
                            string nuevoNombre = Console.ReadLine();
                            Console.WriteLine("Digite el nuevo saco");
                            string nuevaSaco = Console.ReadLine();
                            Console.WriteLine("Digite el precio del saco sin IVA");
                            double nuevoPrecio = Double.Parse(Console.ReadLine());

                            ActualizarCliente(codigoActualizar, nuevoApellido, nuevoNombre, nuevaSaco, nuevoPrecio, connection);
                            break;
                        case 2:
                            Console.WriteLine("Digite el codigo del cliente a eliminar:");
                            string codigoEliminar = Console.ReadLine();
                            EliminarCliente(codigoEliminar, connection);
                            break;

                        case 3:
                            ListarCliente(connection);
                            break;

                        case 4:
                            PrecioIva(connection);
                            break;
                        case 5:
                            Console.WriteLine("Saliendo del programa");
                            return;

                        default:
                            Console.WriteLine("Opcion invalida, escoja una opcion valida del menu");
                            break;
                    }
                }
            }
        }

        static void ActualizarCliente(string codigo, string nuevoNombre, string nuevoApellido, string nuevaSaco, double nuevoPrecio, MySqlConnection connection)
        {
            string updateQuery = $"UPDATE sacos SET apellido = '{nuevoApellido}', nombre = '{nuevoNombre}', saco_desc = '{nuevaSaco}', precio = {nuevoPrecio} WHERE codigo_cliente = '{codigo}'";
            MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
            connection.Open();
            updateCommand.ExecuteNonQuery();
            connection.Close();

            Console.WriteLine("Cliente actualizado exitosamente.");
        }

        static void EliminarCliente(string codigo, MySqlConnection connection)
        {
            string deleteQuery = $"DELETE FROM sacos WHERE codigo_cliente = '{codigo}'";

            MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, connection);
            connection.Open();
            deleteCommand.ExecuteNonQuery();
            connection.Close();

            Console.WriteLine("Cliente eliminado exitosamente.");
        }
        static void PrecioIva(MySqlConnection connection)
        {
            string selectQuery = "SELECT precio FROM sacos";
            MySqlCommand command = new MySqlCommand(selectQuery, connection);
            connection.Open();

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                double Precioiva = 0;
                while (reader.Read())
                {
                    double Precio = reader.GetDouble("precio");
                    Precioiva += Precio * 0.19;
                }

                Console.WriteLine("El precio iva es de: " + Precioiva);
            }

            connection.Close();


        }
        static void ListarCliente(MySqlConnection connection)
        {
            string selectQuery = "SELECT * FROM sacos";
            MySqlCommand command = new MySqlCommand(selectQuery, connection);
            connection.Open();

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                Console.WriteLine("Lista de Clientes:");
                while (reader.Read())
                {
                    string codigo = reader.GetString("codigo_cliente");
                    string apellido = reader.GetString("apellido");
                    string nombre = reader.GetString("nombre");
                    string nombreSaco = reader.GetString("nombre_saco");
                    double precio = reader.GetDouble("precio");

                    Console.WriteLine($"codigo_cliente: {codigo}, apellido: {apellido},nombre: {nombre}, nombre_sacos: {nombreSaco}, precio: {precio}");
                }
            }

            connection.Close();
        }

    }
}