using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ApiDataLayer;
using System.Data.Entity;
using IG_API.Models;

namespace IG_API.Controllers
{
	/// <summary>
	/// CRUD operations for Accounts.
	/// </summary>
	public class AccountsController : ApiController
	{
		/// <summary>
		/// Validation Login.
		/// </summary>
		//[HttpGet]
		//public HttpResponseMessage ValidLogin(string username, string password)
		//{
		//	using (IG_DB_Entities entities = new IG_DB_Entities())
		//	{
		//		Account account = entities.Accounts.First(acc => acc.Email == username && acc.Password == password);
		//		if (account == null)
		//		{
		//			return Request.CreateErrorResponse(HttpStatusCode.BadGateway, "Username or password is invalid");
		//		}
		//		return Request.CreateResponse(HttpStatusCode.OK, TokenManager.GenerateToken(username));
		//	}

		//}
		/// <summary>
		/// Validation Login.
		/// </summary>
		[HttpGet]
		public HttpResponseMessage ValidLogin(string username, string password)
		{
			using (IG_DB_Entities entities = new IG_DB_Entities())
			{
				Account account = entities.Accounts.First(acc => acc.Email == username && acc.Password == password);
				if (account == null)
				{
					return Request.CreateErrorResponse(HttpStatusCode.BadGateway, "Username or password is invalid");
				}
				return Request.CreateResponse(HttpStatusCode.OK, account);
			}
		}
		/// <summary>
		/// Returns user.
		/// </summary>

		//[HttpGet]
		//[CustomAuthenticationFilter]
		//public HttpResponseMessage GetUser()
		//{
		//	return Request.CreateResponse(HttpStatusCode.OK, "Successfully valid");
		//}

		/// <summary>
		/// Returns list of accounts.
		/// </summary>
		public IEnumerable<Account> Get()
		{

			using (IG_DB_Entities entities = new IG_DB_Entities())
			{
				return entities.Accounts.ToList();
			}
		}
		/// <summary>
		/// Returns an account by UserID.
		/// </summary>
		public HttpResponseMessage Get(string userId)
		{
			using (IG_DB_Entities entities = new IG_DB_Entities())
			{
				var entity = entities.Accounts.FirstOrDefault(acc => acc.UserId == userId);
								
				if (entity != null)
				{
					return Request.CreateResponse(HttpStatusCode.OK, entity);
				}
				else
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Account with [UserID = " + userId + "] not found.");
				}
			}
		}
		/// <summary>
		/// Creates a new account.
		/// </summary>
		public HttpResponseMessage Post([FromBody] Account account)
		{
			try
			{
				using (IG_DB_Entities entities = new IG_DB_Entities())
				{
					foreach (var acc in entities.Accounts)
					{
						if (acc.Email == account.Email)
						{
							return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Email already exists.");
						}
					}
					entities.Accounts.Add(account);
					entities.SaveChanges();
					var message = Request.CreateResponse(HttpStatusCode.Created, account);
					message.Headers.Location = new Uri(Request.RequestUri + account.IDAccount.ToString());
					return message;
				}
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
			}
		}
		/// <summary>
		/// Deletes an account by ID.
		/// </summary>
		public HttpResponseMessage Delete(int id)
		{
			try
			{
				using (IG_DB_Entities entities = new IG_DB_Entities())
				{
					var entity = entities.Accounts.FirstOrDefault(acc => acc.IDAccount == id);
					if (entity == null)
					{
						return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Account with [ID = " + id.ToString() + "] not found to delete.");
					}

					entities.Accounts.Remove(entity);
					entities.SaveChanges();
					return Request.CreateResponse(HttpStatusCode.OK);
				}
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
			}
		}
		/// <summary>
		/// Updates an account by UserID.
		/// </summary>
		public HttpResponseMessage Put(string userId, [FromBody] Account account)
		{
			try
			{
				using (IG_DB_Entities entities = new IG_DB_Entities())
				{
					var entity = entities.Accounts.FirstOrDefault(acc => acc.UserId == userId);
					if (entity == null)
					{
						return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Account with [UserID = " + userId + "] not found to update.");
					}
					entity.Email = account.Email;
					entity.FullName = account.FullName;
					entity.Language = account.Language;
					entity.DateOfBirth = account.DateOfBirth;
					entity.DateJoined = account.DateJoined;

					entities.SaveChanges();
					return Request.CreateResponse(HttpStatusCode.OK, entity);
				}
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
			}
		}

        /// <summary>
        /// Change password of account by UserID.
        /// </summary>
        [HttpGet]
        public HttpResponseMessage ChangePassword(int id, string password)
        {
            try
            {
                using (IG_DB_Entities entities = new IG_DB_Entities())
                {
                    var account = entities.Accounts.FirstOrDefault(acc => acc.IDAccount == id);
                    if (account == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Account with [ID = " + id + "] not found to update.");
                    }
                    account.Password = password;

                    entities.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, account);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex + "Something went wrong");
            }
        }
        /// <summary>
        /// Confirms an account by UserID.
        /// </summary>
        [HttpPut]
		public HttpResponseMessage ConfirmEmail([FromBody]string userId)
		{
			try
			{
				using (IG_DB_Entities entities = new IG_DB_Entities())
				{
					var entity = entities.Accounts.FirstOrDefault(acc => acc.UserId == userId);
					if (entity == null)
					{
						return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Account with [UserID = " + userId + "] not found to update.");
					}
					entity.Confirmed = true;

					entities.SaveChanges();
					return Request.CreateResponse(HttpStatusCode.OK, entity);
				}
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
			}
		}
	}
}
