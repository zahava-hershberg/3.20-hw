using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3._20_Hw.Data
{
    public class ImageRepository
    {
        private readonly string _connectionString;
        public ImageRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Images> GetImages()
        {
            using var context = new ImageDataContext(_connectionString);
            return context.Images.ToList();
        }
        public void AddImage(Images i)
        {
            using var context = new ImageDataContext(_connectionString);
            context.Images.Add(i);
            context.SaveChanges();
        }
        public Images GetImageById(int id)
        {
            using var context = new ImageDataContext(_connectionString);
            return context.Images.FirstOrDefault(p => p.Id == id);
        }
        public void AddLikes(int id)
        {
            using var context = new ImageDataContext(_connectionString);
            Images image = GetImageById(id);
            image.Likes++;
            context.Entry(image).State = EntityState.Modified;
            context.SaveChanges();
        }
        public int GetLikes(int id)
        {
            using var context = new ImageDataContext(_connectionString);
            Images image = context.Images.FirstOrDefault(p=>p.Id==id);
            return image.Likes;

        }
    }
}
