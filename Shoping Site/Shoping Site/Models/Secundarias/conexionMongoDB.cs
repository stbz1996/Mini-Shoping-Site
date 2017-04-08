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





        public void eliminarBD(string id)
        {
            var collection = database.GetCollection<archivo>("fs.files");

            var query = collection.AsQueryable<archivo>();
            var res = from c in query
                      where c.filename == id
                      select c;

            foreach (archivo mini in res)
            {
                comunicador.DeleteAsync(mini._id);
            }
        }


        // Actualizar imagen de la base de datos.
        public string actualizarImagen(string id, byte[] imagen)
        {

            try
            {
                eliminarBD(id);
                return insertarBD(id, imagen);
            }
            catch (Exception e)
            {
                return "https://upload.wikimedia.org/wikipedia/commons/d/da/Imagen_no_disponible.svg";
            }

        }


        public class archivo
        {
            public ObjectId _id { get; set; }
            public string filename { get; set; }
            public long length { get; set; }
            public int chunkSize { get; set; }
            public DateTime uploadDate { get; set; }
            public string md5 { get; set; }
        }

    }
}