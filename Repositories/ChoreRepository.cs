﻿using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;

namespace Roommates.Repositories
{

    public class ChoreRepository : BaseRepository
    {
        public ChoreRepository(string connectionString) : base(connectionString) { }
        
            public List<Chore> GetAll()
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT Id, Name FROM Chore";

                        SqlDataReader reader = cmd.ExecuteReader();

                        List<Chore> chores = new List<Chore>();

                        while (reader.Read())
                        {
                            int idColumnPosition = reader.GetOrdinal("Id");

                            int idValue = reader.GetInt32(idColumnPosition);

                            int nameColumnPosition = reader.GetOrdinal("Name");
                            string nameValue = reader.GetString(nameColumnPosition);

                           
                  

                            Chore chore = new Chore
                            {
                                Id = idValue,
                                Name = nameValue,

                            };

                            chores.Add(chore);
                        }

                        reader.Close();

                    return chores;
                    }
                }
            }
        public Chore GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Name, FROM Chore WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Chore chore = null;

                    if (reader.Read())
                    {
                        chore = new Chore
                        {
                            Id = id,
                            Name = reader.GetString(reader.GetOrdinal("Name")),

                        };
                    }
                    reader.Close();

                    return chore;
                }
            }
        }

        public void Insert(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Chore (Name)
                                          OUTPUT INSERTED.Id
                                            VALUES (@name)";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    int id = (int)cmd.ExecuteScalar();

                    chore.Id = id;
                }
            }
        }
        public List<Chore> GetUnassignedChores()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT chore.name
                                        from chore
                                    left join roommatechore on roommatechore.choreId = chore.Id
                                    Where roommateId is null";
                    SqlDataReader reader = cmd.ExecuteReader();

                    Chore chore = null;
                    List<Chore> choreList = new List<Chore>();

                    while (reader.Read())
                    {
                        chore = new Chore
                        {
                            //Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),

                        };

                    choreList.Add(chore);
                    }
                    reader.Close();
                    return choreList;
                
                    

                    
                }
            }
        }

        public void AssignChore(int roommateId, int choreId)
        {

        }
   }
}

