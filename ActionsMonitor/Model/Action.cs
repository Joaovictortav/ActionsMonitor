using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ActionsMonitor.Model
{
	[Table("action")]
	public sealed class Action
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("ID", TypeName = "INT")] public int Id { get; set; } 
		[Column("Name", TypeName = "VARCHAR(255)"), MaxLength(255), Required] public string? Name { get; set; }
		[Column("Active", TypeName = "BIT"), Required] public bool Active { get; set; }
		[Column("MonitorId", TypeName = "INT"), ForeignKey("Monitor")] public int MonitorId { get; set; } 

		public Action() { }
		
		internal void Delete()
		{
			ActionsMoniotrContext.Get().ActionsSet.Remove(this);
		}

		public static Task<List<Action>> List(int id, bool active)
		{
			return ActionsMoniotrContext.Get().ActionsSet.Where(s => s.Active == active && s.MonitorId == id).ToListAsync();
		}
	}
}