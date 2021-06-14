using Microsoft.Data.SqlClient;
using Roommates.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roommates.Repositories
{
    
        public class RoommateRespository : BaseRepository
        {
            public RoommateRespository(string connectionString) : base(connectionString) { }

            
            public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                // Note, we must Open() the connection, the "using" block doesn't do that for us.
                conn.Open();

                // We must "use" commands too.
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // Here we setup the command with the SQL we want to execute before we execute it.
                    cmd.CommandText = "SELECT * FROM Roommate";

                    // Execute the SQL in the database and get a "reader" that will give us access to the data.
                    SqlDataReader reader = cmd.ExecuteReader();

                    // A list to hold the rooms we retrieve from the database.
                    List<Roommate> roommates = new List<Roommate>();

                    // Read() will return true if there's more data to read
                    while (reader.Read())
                    {
                        // The "ordinal" is the numeric position of the column in the query results.
                        //  For our query, "Id" has an ordinal value of 0 and "Name" is 1.
                        int idColumnPosition = reader.GetOrdinal("Id");

                        // We user the reader's GetXXX methods to get the value for a particular ordinal.
                        int idValue = reader.GetInt32(idColumnPosition);

                        int firstNameColumnPosition = reader.GetOrdinal("FirstName");
                        string firstNameValue = reader.GetString(firstNameColumnPosition);

                        int lastNameColumnPosition = reader.GetOrdinal("lastName");
                        string lastNameValue = reader.GetString(lastNameColumnPosition);

                        int maxOccupancyColumPosition = reader.GetOrdinal("MaxOccupancy");
                        int maxOccupancy = reader.GetInt32(maxOccupancyColumPosition);

                        // Now let's create a new room object using the data from the database.
                        Roommate roommate = new Roommate
                        {
                            Id = idValue,
                            FirstName = firstNameValue,
                            LastName = lastNameValue
                        };

                        // ...and add that room object to our list.
                        roommates.Add(roommate);
                    }

                    // We should Close() the reader. Unfortunately, a "using" block won't work here.
                    reader.Close();

                    // Return the list of rooms who whomever called this method.
                    return roommates;
                }
            }
        }
    }
            public Roommate GetById(int id)
            {

                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"SELECT roommate.FirstName, roommate.RentPortion, room.name as RoomName
                                from roommate
                                left join room on roommate.roomid = room.id
                                Where room.id = @id;";
                        cmd.Parameters.AddWithValue("@id", id);
                        SqlDataReader reader = cmd.ExecuteReader();

                        Roommate mate = null;

                        if (reader.Read())
                        {
                            mate = new Roommate
                            {
                                //Id = id,
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                //LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                                //MovedInDate = reader.GetDateTime(reader.GetOrdinal("MovedInDate")),
                                //RoomId = reader.GetInt32(reader.GetOrdinal("RoomId")),
                                Room = new Room()
                                {
                                    //Id = reader.GetInt32(reader.GetOrdinal("RoomId")),
                                    Name = reader.GetString(reader.GetOrdinal("RoomName")),
                                    //MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy")),
                                }
                            };
                        }
                        reader.Close();
                        return mate;

                    }
                }
            }
        }
   }
