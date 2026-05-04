using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace apiRESTAdminUsuarioFull.Models
{
    public class clsUsuario
    {
        // Definición de atributos
        public string cve { get; set; }
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string usuario { get; set; }
        public string contrasena { get; set; }
        public string ruta { get; set; }
        public string tipo { get; set; }

        // Definición de cadena de Conexión
        private string cadConn = ConfigurationManager.
                    ConnectionStrings["bdControlAcceso"].
                    ConnectionString;

        // Definición de Constructores del Modelo
        public clsUsuario()
        {
            // Código de inicialización posterior ...        
        }
        public clsUsuario(string usuario,
                          string contrasena)
        {
            this.usuario = usuario;
            this.contrasena = contrasena;
        }
        public clsUsuario(string nombre,
                          string apellidoPaterno,
                          string apellidoMaterno,
                          string usuario,
                          string contrasena,
                          string ruta,
                          string tipo)
        {
            this.nombre = nombre;
            this.apellidoPaterno = apellidoPaterno;
            this.apellidoMaterno = apellidoMaterno;
            this.usuario = usuario;
            this.contrasena = contrasena;
            this.ruta = ruta;
            this.tipo = tipo;
        }

        // Definición de Métodos de Proceso
        public DataSet spInsUsuario()
        {
            // Creación del comando SQL
            string cadSql = "CALL spInsUsuario('" + this.nombre + "', '"
                                                  + this.apellidoPaterno + "','"
                                                  + this.apellidoMaterno + "', '"
                                                  + this.usuario + "', '"
                                                  + this.contrasena + "', '"
                                                  + this.ruta + "', "
                                                  + this.tipo + ");";
            // Configuración de los objetosd de conexión a MySQL
            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSql, cnn);
            DataSet ds = new DataSet();
            // Ejecución del Adaptadora de Datos
            da.Fill(ds, "spInsUsuario");
            return ds;
        }

        // Proceso de validación de usuarios (spValidarAcceso)
        public DataSet spValidarAcceso()
        {
            // Crear el comando SQL
            string cadSQL = "";
            cadSQL = "call spValidarAcceso('" + this.usuario + "','"
                                              + this.contrasena + "');";
            // Configuración de objetos de conexión
            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSQL, cnn);
            DataSet ds = new DataSet();
            // Ejecución y salida
            da.Fill(ds, "spValidarAcceso");
            return ds;
        }

        // Proceso de Reporte de usuarios (vwRptUsuario)
        public DataSet vwRptUsuario()
        {
            // Crear el comando SQL
            string cadSQL = "";
            cadSQL = "select * from vwRptUsuario";
            // Configuración de objetos de conexión
            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSQL, cnn);
            DataSet ds = new DataSet();
            // Ejecución y salida
            da.Fill(ds, "vwRptUsuario");
            return ds;
        }


        // Proceso de Reporte de usuarios (vwRptUsuario)
        public DataSet vwRptUsuario(string filtro)
        {
            // 1. La consulta SQL: El @filtro es un "espacio reservado"
            string cadSQL = "SELECT * FROM vwRptUsuario WHERE Nombre LIKE CONCAT('%', @filtro, '%') OR Usuario LIKE CONCAT('%', @filtro, '%') OR Rol LIKE CONCAT('%', @filtro, '%'); ";

            // 2. Definimos los objetos básicos
            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSQL, cnn);
            DataSet ds = new DataSet();

            // 3. Configuramos el filtro (el % es para que busque en cualquier parte del nombre)
            da.SelectCommand.Parameters.AddWithValue("@filtro", "%" + filtro + "%");

            da.Fill(ds, "vwRptUsuario");
            return ds;
        }


        // Proceso de Reporte de Tipos de Usuarios (vwRptUsuario)
        public DataSet vwTipoUsuario()
        {
            // Crear el comando SQL
            string cadSQL = "";
            cadSQL = "select * from vwTipoUsuario";
            // Configuración de objetos de conexión
            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSQL, cnn);
            DataSet ds = new DataSet();
            // Ejecución y salida
            da.Fill(ds, "vwTipoUsuario");
            return ds;
        }

        public DataSet getUsuario()
        {
            string cadSQL = "SELECT * FROM vwRptUsuario WHERE Clave = @cve";

            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSQL, cnn);
            DataSet ds = new DataSet();

            da.SelectCommand.Parameters.AddWithValue("@cve", this.cve);

            da.Fill(ds, "usuario");
            return ds;
        }

        public DataSet spUpdUsuario()
        {
            string cadSql = "CALL spUpdUsuario('" + this.cve + "', '"
                                                  + this.nombre + "', '"
                                                  + this.apellidoPaterno + "', '"
                                                  + this.apellidoMaterno + "', '"
                                                  + this.usuario + "', '"
                                                  + this.contrasena + "', '"
                                                  + this.ruta + "', "
                                                  + this.tipo + ");";

            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSql, cnn);
            DataSet ds = new DataSet();

            da.Fill(ds, "spUpdUsuario");
            return ds;
        }

        public DataSet spDelUsuario()
        {
            string cadSql = "CALL spDelUsuario('" + this.cve + "');";

            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSql, cnn);
            DataSet ds = new DataSet();

            da.Fill(ds, "spDelUsuario");
            return ds;
        }


    }
}