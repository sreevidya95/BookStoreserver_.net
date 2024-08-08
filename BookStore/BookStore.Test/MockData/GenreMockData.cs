using Hangfire.Storage.Monitoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Test.MockData
{
    internal class GenreMockData
    {
        public static IEnumerable<Entities.Genre> GetGenresNull()
        {
            return null;
          
        }
        public static Entities.Genre GetGenresByIdNull(int id)
        {
            return null;

        }
        public static IEnumerable<Entities.Genre> GetGenresExc(){
            throw new Exception("Connection Exception");
        }
        public static IEnumerable<Entities.Genre> GetGenres()
        {

            return new List<Entities.Genre>
            {
               new Entities.Genre{
                    
                      genre_id=1,
                      genre_name="Science Fiction"

               },
               new Entities.Genre{

                      genre_id=2,
                      genre_name="Mystery"

               },
               new Entities.Genre{

                      genre_id=3,
                      genre_name="Fantasy"

               },
               new Entities.Genre{

                      genre_id=4,
                      genre_name="Romance"

               },
               new Entities.Genre{

                      genre_id=5,
                      genre_name="Horror"

               },

            };
        }
    }
}
