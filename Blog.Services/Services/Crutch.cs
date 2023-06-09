using Blog.DAL;
using Blog.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.Services
{
	public class Crutch : ICrutch
	{
		public Crutch(DBContext dbcontext)
		{
			this.dbcontext = dbcontext;
		}

		private DBContext dbcontext { get; }
		public void ClearUserRolesList(int id)
		{
			var sql = $"delete from [Blog].[dbo].[RoleUser] where [UsersId]={id}";
			dbcontext.Database.ExecuteSql($"delete from [Blog].[dbo].[RoleUser] where [UsersId]={id}");
		}
	}
}
