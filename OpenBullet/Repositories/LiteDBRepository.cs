using System;
using System.Collections.Generic;
using LiteDB;
using RuriLib.Interfaces;
using RuriLib.Models;

namespace OpenBullet.Repositories
{
	// Token: 0x02000059 RID: 89
	public class LiteDBRepository<T> : IRepository<T, Guid> where T : Persistable<Guid>
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000237 RID: 567 RVA: 0x0000A6BE File Offset: 0x000088BE
		// (set) Token: 0x06000238 RID: 568 RVA: 0x0000A6C6 File Offset: 0x000088C6
		public string ConnectionString { get; set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000239 RID: 569 RVA: 0x0000A6CF File Offset: 0x000088CF
		// (set) Token: 0x0600023A RID: 570 RVA: 0x0000A6D7 File Offset: 0x000088D7
		public string Collection { get; set; }

		// Token: 0x0600023B RID: 571 RVA: 0x0000A6E0 File Offset: 0x000088E0
		public LiteDBRepository(string connectionString, string collection)
		{
			this.ConnectionString = connectionString;
			this.Collection = collection;
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000A6F8 File Offset: 0x000088F8
		private LiteDatabase Connect()
		{
			this._db = new LiteDatabase("filename=" + this.ConnectionString + "; Connection=Shared;", null);
			this._coll = this._db.GetCollection<T>(this.Collection, BsonAutoId.ObjectId);
			return this._db;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000A745 File Offset: 0x00008945
		public void Disconnect(bool dispose = false)
		{
			if (dispose)
			{
				this._db.Dispose();
			}
			this._coll = null;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000A75C File Offset: 0x0000895C
		public void Add(T entity)
		{
			using (this.Connect())
			{
				this._coll.Insert(entity);
				this.Disconnect(false);
			}
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000A7A0 File Offset: 0x000089A0
		public void Add(IEnumerable<T> entities)
		{
			using (this.Connect())
			{
				this._coll.InsertBulk(entities, 5000);
				this.Disconnect(false);
			}
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000A7EC File Offset: 0x000089EC
		public IEnumerable<T> Get()
		{
			IEnumerable<T> enumerable;
			using (this.Connect())
			{
				enumerable = this._coll.FindAll();
			}
			return enumerable;
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000A82C File Offset: 0x00008A2C
		public T Get(Guid id)
		{
			T t2;
			using (this.Connect())
			{
				T t = this._coll.FindById(id);
				this.Disconnect(false);
				t2 = t;
			}
			return t2;
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000A878 File Offset: 0x00008A78
		public void Remove(T entity)
		{
			using (this.Connect())
			{
				this._coll.Delete(entity.Id);
				this.Disconnect(false);
			}
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000A8CC File Offset: 0x00008ACC
		public void Remove(IEnumerable<T> entities)
		{
			using (this.Connect())
			{
				foreach (T entity in entities)
				{
					this._coll.Delete(entity.Id);
				}
				this.Disconnect(false);
			}
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000A950 File Offset: 0x00008B50
		public void RemoveAll()
		{
			using (LiteDatabase db = this.Connect())
			{
				db.DropCollection(this.Collection);
				this.Disconnect(false);
			}
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000A994 File Offset: 0x00008B94
		public void Update(T entity)
		{
			using (this.Connect())
			{
				this._coll.Update(entity);
				this.Disconnect(false);
			}
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000A9D8 File Offset: 0x00008BD8
		public void Update(IEnumerable<T> entities)
		{
			using (this.Connect())
			{
				this._coll.Update(entities);
				this.Disconnect(false);
			}
		}

		// Token: 0x040001D6 RID: 470
		private LiteDatabase _db;

		// Token: 0x040001D7 RID: 471
		private ILiteCollection<T> _coll;
	}
}
