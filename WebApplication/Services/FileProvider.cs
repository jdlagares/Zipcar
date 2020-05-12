using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Services
{
    /// <summary>
    /// Serivicio que se encarga de Leer, crear o eliminar archivos en la carpeta wwwroot
    /// </summary>
    public interface IFileProvider
    {
        /// <summary>
        /// Mapea una ruta publica a una ruta fisica en la carpeta wwwroot del servidor
        /// </summary>
        /// <param name="path">Ruta publica</param>
        string MapPath(string path);

        /// <summary>
        /// Crea un archivo imagen dada una ruta publica, su nombre y el formato imagen en la carpeta wwwroot del servidor
        /// </summary>
        /// <param name="directory">Ruta publica</param>
        /// <param name="fileName">Nombre de archivo</param>
        void ImageFileCreate(string directory, string fileName, Stream streamFile);

        /// <summary>
        /// Elimina un archivo imagen en la carpeta wwwroot del servidor dada su ruta publica
        /// </summary>
        /// <param name="path">Ruta publica</param>
        void FileDelete(string path);
    }

    /// <summary>
    /// Serivicio que se encarga de Leer, crear o eliminar archivos en la carpeta wwwroot
    /// </summary>
    public class FileProvider : IFileProvider
    {
        private IWebHostEnvironment _hostingEnvironment;

        public FileProvider(IWebHostEnvironment environment)
        {
            _hostingEnvironment = environment;
        }

        /// <summary>
        /// Mapea una ruta publica a una ruta fisica en la carpeta wwwroot del servidor
        /// </summary>
        /// <param name="path">Ruta publica</param>
        public string MapPath(string path)
        {
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath);
            foreach (var item in path.Substring(1).Split('/'))
            {
                filePath = Path.Combine(filePath, item);
            }
            return filePath;
        }

        /// <summary>
        /// Crea un archivo imagen dada una ruta publica, su nombre y el formato imagen en la carpeta wwwroot del servidor
        /// </summary>
        /// <param name="directory">Ruta publica</param>
        /// <param name="fileName">Nombre de archivo</param>
        public async void ImageFileCreate(string directory, string fileName, Stream streamFile) 
        {
            directory = MapPath(directory);
            Directory.CreateDirectory(directory);

            var fileServerPath = $"{directory}/{fileName}";
            using (var stream = System.IO.File.Create(fileServerPath))
            {
                await streamFile.CopyToAsync(stream);
            }
        }

        /// <summary>
        /// Elimina un archivo imagen en la carpeta wwwroot del servidor dada su ruta publica
        /// </summary>
        /// <param name="path">Ruta publica</param>
        public void FileDelete(string path) 
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            File.Delete(MapPath(path));
        }
    }

}
