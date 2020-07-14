using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public interface IDatingRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T> (T entity) where T: class;
         Task<bool> SaveAll();
         Task<PagedList<User>> GetUsers(UserParams userParams);
         Task<User> GetUser(int id);
         Task<Photo> GetPhoto(int id); 

         Task<Photo> GetMainPhotoForUser(int userId);

        // checks if a user hasn't already liked another
         Task<Like> GetLike(int userId, int recipientId);

        // Getting a single message from the database
         Task<Message> GetMessage (int id);
         Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams);

         // this is the method for the conversation between two users that is displayed on the tapped panel inside that detail view of user
         Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId);


    }
}