using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Newtonsoft.Json;

namespace IndentityServer
{
    public class Config2
    {

        /// <summary>
        /// This list is loaded from a database in production
        /// </summary>
        /// <returns></returns>
        private static List<User> users;
        public static List<User> Users
        {
            get
            {
                if (users == null)
                    users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText("settings" + Path.DirectorySeparatorChar + "users.json"));
                return Users;
            }
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            List<ApiResource> resources = JsonConvert.DeserializeObject<List<ApiResource>>(File.ReadAllText("settings" + Path.DirectorySeparatorChar + "apiresources.json"));
            return resources;
        }

        public static IEnumerable<Client> GetClients()
        {
            List<Client> clients = JsonConvert.DeserializeObject<List<Client>>(File.ReadAllText("settings" + Path.DirectorySeparatorChar + "clients.json"));
            //var act = clients[1].AllowedGrantTypes;
            //Client clie = new Client();
            //clie.AllowedGrantTypes = (GrantTypes)act;
            return clients;
        }

        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
            new TestUser
                {
                    SubjectId = "1",
                    Username = "my@email.com",
                    Password = "mysecretpassword123"
                },
            new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password="password"
                }
            };
        }

    }

    public class Config
    {


        /// <summary>
        /// Users list should be loaded from a database in production
        /// </summary>
        /// <returns></returns>
        private static List<User> users;
        public static List<User> Users
        {
            get
            {
                if (users == null)
                    users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText("settings" + Path.DirectorySeparatorChar + "users.json"));
                return users;
            }
        }

        //Defining the API that will have access to the IdentityServer
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new  ApiResource("Project Proposal, My API")
            };
        }
        //Defining the client
        /**
        * The next step is to define a client that can access this API.
         For this scenario, the client will not have an interactive user and will authenticate using the so-called client secret with IdentityServer4
        **/
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "clientCred",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,  //just secret

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    // scopes that client has access to
                    AllowedScopes = { "Project."}
                },
                new Client
                {
                    ClientId = "Project.Client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                new Secret ("secret".Sha256())
                    },
                    AllowedScopes = { "Project." }
                }
            };
        }
        // The users that can be authenticated.
        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
            new TestUser
                {
                    SubjectId = "1",
                    Username = "my@email.com",
                    Password = "mysecretpassword123"
                },
            new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password="password"
                }
            };
        }
    }


    public class User
    {
        public int UserId { get; set; }
        public string ClientId { get; set; }
        public string Provider { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public bool IsAdminRole { get; set; }
        public List<string> Roles { get; set; }
        public bool IsActive { get; set; }
        public string Password { get; set; }   //stored encrypted
        public string DecryptedPassword
        {
            get { return Decrypt(Password); }
            set { Password = Encrypt(value); }
        }
        private string Decrypt(string cipherText)
        {
            return EntityHelper.Decrypt(cipherText);
        }
        private string Encrypt(string clearText)
        {
            return EntityHelper.Encrypt(clearText);
        }
    }

    public static class EntityHelper
    {

        static readonly String _EncryptionKey = "eid729";

        public static string Encrypt(string clearText)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(_EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string Decrypt(string cipherText)
        {
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(_EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}
