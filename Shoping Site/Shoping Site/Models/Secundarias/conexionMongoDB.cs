using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Shoping_Site.Models.Secundarias
{
    public class conexionMongoDB
    {
        private IMongoDatabase database;
        private GridFSBucket comunicador;



        public conexionMongoDB(){
            database = new MongoClient("mongodb://localhost").GetDatabase("proyecto");
            comunicador = new GridFSBucket(database);
        }



        // Retorna el ObjectId en donde insertó en la base de datos.
        public string insertarBD(string id, byte[] imagenEnBytes){
            string newID = id.ToString();
            try{
                var t = Task.Run<ObjectId>(() => {
                    return comunicador.UploadFromBytesAsync(newID, imagenEnBytes);
                });

                string base64 = Convert.ToBase64String(imagenEnBytes);
                return string.Format("data:{0};base64,{1}", ".jpg", base64);
            }
            catch (Exception e){
                // No se insertó adecuadamente
                return null;
            }
        }

       
        // Obtiene de la base de datos la imagen.
        public string obtenerBD(int id){
            string newID = id.ToString();
            try{
                var imagen = comunicador.DownloadAsBytesByNameAsync(newID);
                Task.WaitAll(imagen);
                var bytes = imagen.Result;
                string base64 = Convert.ToBase64String(bytes);
                return string.Format("data:{0};base64,{1}", ".jpg", base64);
            }
            catch (Exception){
                return "https://upload.wikimedia.org/wikipedia/commons/d/da/Imagen_no_disponible.svg";
            }
        }


        // Elimina una imagen de la base de datos.
        public void eliminarBD(string id)
        {
            // no funca la verga.
        }

    }
    }