using apiRESTAdminUsuarioFull.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace apiRESTAdminUsuarioFull.Controllers
{
    public class UsuarioController : ApiController
    {
        [HttpPost]
        [Route("full/usuario/spinsusuario")]
        public clsApiStatus spInsUsuario([FromBody] clsUsuario modelo)
        {
            // -----------------------
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            // -----------------------
            try
            {
                // Creación del objeto, en base al Modelo
                clsUsuario objUsuario = new clsUsuario(modelo.nombre,
                                                       modelo.apellidoPaterno,
                                                       modelo.apellidoMaterno,
                                                       modelo.usuario,
                                                       modelo.contrasena,
                                                       modelo.ruta,
                                                       modelo.tipo);
                DataSet ds = new DataSet();
                // Ejecución del Método del Modelo (y recepción de datos)
                ds = objUsuario.spInsUsuario();
                // Configuración del objeto de salida
                objRespuesta.statusExec = true;
                objRespuesta.msg = "Usuario registrado exitosamente !";
                objRespuesta.ban =
                         int.Parse(ds.Tables[0].Rows[0][0].ToString());
                jsonResp.Add("msgData", "Usuario registrado exitosamente !");
                objRespuesta.datos = jsonResp;
            }
            catch (Exception e)
            {
                // Configuración del objeto de salida
                objRespuesta.statusExec = false;
                objRespuesta.msg = "Usuario NO registrado ...";
                objRespuesta.ban = -1;
                jsonResp.Add("msgData", e.Message.ToString());
                objRespuesta.datos = jsonResp;
            }

            return objRespuesta;
        }


        // endpoint para validación de acceso spValidarAcceso
        [HttpPost]
        [Route("full/usuario/spvalidaracceso")]
        public clsApiStatus spValidarAcceso([FromBody] clsUsuario modelo)
        {
            // -----------------------------------------
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            // -----------------------------------------
            DataSet ds = new DataSet();
            try
            {
                // Creación del objeto del modelo clsUsuario
                clsUsuario objUsuario = new clsUsuario(modelo.usuario,
                                                       modelo.contrasena);
                ds = objUsuario.spValidarAcceso();
                // Configuración del objeto de salida
                objRespuesta.statusExec = true;
                objRespuesta.ban = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                // Validar el valor recibido en bandera
                if (objRespuesta.ban == 1)
                {
                    objRespuesta.msg = "Usuario validado exitosamente!";
                    jsonResp.Add("usu_nombre_completo", ds.Tables[0].Rows[0][1].ToString());
                    jsonResp.Add("usu_ruta", ds.Tables[0].Rows[0][2].ToString());
                    jsonResp.Add("usu_usuario", ds.Tables[0].Rows[0][3].ToString());
                    jsonResp.Add("tip_descripcion", ds.Tables[0].Rows[0][4].ToString());
                    objRespuesta.datos = jsonResp;
                }
                else
                {
                    objRespuesta.msg = "Acceso denegado, verificar ...";
                    jsonResp.Add("msgData", "Acceso denegado, verificar ...");
                    objRespuesta.datos = jsonResp;
                }

            }    // <<----------fin del try
            catch (Exception ex)
            {
                // Configuración del objeto de salida
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error de conexión con el servicio de datos";
                jsonResp.Add("msgData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;
            }

            // Retorno del obj de Salida objRespuesta
            return objRespuesta;
        }   // <<-------------fin del endpoint


        // Endpoint para consulta de usuarios vwRptUsuario
        [HttpGet]
        [Route("full/usuario/vwrptusuario")]
        public clsApiStatus vwRptUsuario()
        {
            // -----------------------
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            // -----------------------
            DataSet ds = new DataSet();
            try
            {
                clsUsuario objUsuario = new clsUsuario();
                ds = objUsuario.vwRptUsuario();
                // Configuración del objeto de salida
                objRespuesta.statusExec = true;
                objRespuesta.ban = ds.Tables[0].Rows.Count;
                objRespuesta.msg = "Consulta de usuario " +
                                    "realizada exitosamente";
                // Migración del ds(DataSet) al objeto Json
                string jsonString = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                jsonResp = JObject.Parse($"{{\"{ds.Tables[0].TableName}\": {jsonString}}}");
                // DataSet migrado, se envía clsApiStatus
                objRespuesta.datos = jsonResp;
            }
            catch (Exception ex)
            {
                // Configuración del objeto de salida
                objRespuesta.statusExec = false;
                objRespuesta.msg =
                    "Fallo en consulta de reporte - Usuario ...";
                objRespuesta.ban = -1;
                jsonResp.Add("msgData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;
            }
            // Salida  del objeto configurado
            return objRespuesta;
        }

        // Endpoint para consulta de usuarios por tipo vwRptUsuario
        [HttpGet]
        [Route("full/usuario/vwtipousuario")]
        public clsApiStatus vwTipoUsuario()
        {
            // -----------------------
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            // -----------------------
            DataSet ds = new DataSet();
            try
            {
                clsUsuario objUsuario = new clsUsuario();
                ds = objUsuario.vwTipoUsuario();

                // Configuración del objeto de salida
                objRespuesta.statusExec = true;
                objRespuesta.ban = ds.Tables[0].Rows.Count;
                objRespuesta.msg = "Consulta de tipos de usuario " + "realizada exitosamente";
                
                // Migración del ds(DataSet) al objeto Json
                string jsonString = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                jsonResp = JObject.Parse($"{{\"{ds.Tables[0].TableName}\": {jsonString}}}");

                // DataSet migrado, se envía clsApiStatus
                objRespuesta.datos = jsonResp;
            }
            catch (Exception ex)
            {
                // Configuración del objeto de salida
                objRespuesta.statusExec = false;
                objRespuesta.msg =
                    "Fallo en consulta de reporte - Tipo Usuario ...";
                objRespuesta.ban = -1;
                jsonResp.Add("msgData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;
            }
            // Salida  del objeto configurado
            return objRespuesta;
        }


        // Endpoint para consultar usuarios por filtro
        [HttpGet]
        [Route("full/usuario/vwrptusuario")]
        public clsApiStatus vwRptUsuario(string filtro)
        {
            // -----------------------
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            // -----------------------
            DataSet ds = new DataSet();
            try
            {
                clsUsuario objUsuario = new clsUsuario();
                ds = objUsuario.vwRptUsuario(filtro);// filtro
                // Configuración del objeto de salida
                objRespuesta.statusExec = true;
                objRespuesta.ban = ds.Tables[0].Rows.Count;
                objRespuesta.msg = "Consulta de usuario " +
                                    "realizada exitosamente";
                // Migración del ds(DataSet) al objeto Json
                string jsonString = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                jsonResp = JObject.Parse($"{{\"{ds.Tables[0].TableName}\": {jsonString}}}");
                // DataSet migrado, se envía clsApiStatus
                objRespuesta.datos = jsonResp;
            }
            catch (Exception ex)
            {
                // Configuración del objeto de salida
                objRespuesta.statusExec = false;
                objRespuesta.msg =
                    "Fallo en consulta de reporte - Usuario ...";
                objRespuesta.ban = -1;
                jsonResp.Add("msgData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;
            }
            // Salida  del objeto configurado
            return objRespuesta;
        }

        [HttpGet]
        [Route("full/usuario/getusuario")]
        public clsApiStatus getUsuario(string cve)
        {
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            DataSet ds = new DataSet();

            try
            {
                clsUsuario objUsuario = new clsUsuario();
                objUsuario.cve = cve;

                ds = objUsuario.getUsuario();

                objRespuesta.statusExec = true;
                objRespuesta.ban = ds.Tables[0].Rows.Count;
                objRespuesta.msg = "Consulta realizada exitosamente";

                string jsonString = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                jsonResp = JObject.Parse($"{{\"usuario\": {jsonString}}}");

                objRespuesta.datos = jsonResp;
            }
            catch (Exception ex)
            {
                objRespuesta.statusExec = false;
                objRespuesta.msg = "Error al consultar usuario";
                objRespuesta.ban = -1;
                jsonResp.Add("msgData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;
            }

            return objRespuesta;
        }

        [HttpPut]
        [Route("full/usuario/updateusuario")]
        public clsApiStatus updateUsuario([FromBody] clsUsuario modelo)
        {
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();

            try
            {
                clsUsuario objUsuario = new clsUsuario();
                objUsuario.cve = modelo.cve;

                // 🔎 Validar si existe
                DataSet dsCheck = objUsuario.getUsuario();

                if (dsCheck.Tables[0].Rows.Count == 0)
                {
                    objRespuesta.statusExec = false;
                    objRespuesta.msg = "El usuario no existe";
                    objRespuesta.ban = 0;

                    jsonResp.Add("msgData", "No se encontró el usuario");
                    objRespuesta.datos = jsonResp;

                    return objRespuesta;
                }

                // ✏️ Asignar datos para actualizar
                objUsuario.nombre = modelo.nombre;
                objUsuario.apellidoPaterno = modelo.apellidoPaterno;
                objUsuario.apellidoMaterno = modelo.apellidoMaterno;
                objUsuario.usuario = modelo.usuario;
                objUsuario.contrasena = modelo.contrasena;
                objUsuario.ruta = modelo.ruta;
                objUsuario.tipo = modelo.tipo;

                DataSet ds = objUsuario.spUpdUsuario();

                objRespuesta.statusExec = true;
                objRespuesta.msg = "Usuario modificado correctamente";
                objRespuesta.ban = 1;

                jsonResp.Add("msgData", "Usuario modificado correctamente");
                objRespuesta.datos = jsonResp;
            }
            catch (Exception ex)
            {
                objRespuesta.statusExec = false;
                objRespuesta.msg = "Error al modificar usuario";
                objRespuesta.ban = -1;

                jsonResp.Add("msgData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;
            }

            return objRespuesta;
        }

        [HttpDelete]
        [Route("full/usuario/deleteusuario")]
        public clsApiStatus deleteUsuario(string cve)
        {
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();

            try
            {
                clsUsuario objUsuario = new clsUsuario();
                objUsuario.cve = cve;

                // 🔎 Verificar si existe
                DataSet dsCheck = objUsuario.getUsuario();

                if (dsCheck.Tables[0].Rows.Count == 0)
                {
                    objRespuesta.statusExec = false;
                    objRespuesta.msg = "El usuario no existe";
                    objRespuesta.ban = 0;

                    jsonResp.Add("msgData", "No se encontró el usuario");
                    objRespuesta.datos = jsonResp;

                    return objRespuesta;
                }

                // ❌ Eliminar
                DataSet ds = objUsuario.spDelUsuario();

                objRespuesta.statusExec = true;
                objRespuesta.msg = "Usuario eliminado correctamente";
                objRespuesta.ban = 1;

                jsonResp.Add("msgData", "Usuario eliminado correctamente");
                objRespuesta.datos = jsonResp;
            }
            catch (Exception ex)
            {
                objRespuesta.statusExec = false;
                objRespuesta.msg = "Error al eliminar usuario";
                objRespuesta.ban = -1;

                jsonResp.Add("msgData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;
            }

            return objRespuesta;
        }

    }
}
