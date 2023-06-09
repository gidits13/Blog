using Blog.DAL;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.Services.Interfaces
{
	public interface ICrutch
	{
		public void ClearUserRolesList(int id);
	}
}
