﻿using System.Collections.Generic;
using System.Linq;

namespace HospitalData.Services
{
	public class WaitingService : IWaitingService
	{
		private readonly HospitalContext _context;

		public WaitingService()
		{
			_context = new HospitalContext();
		}

		public WaitingService(HospitalContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Adds the waiting list entry into the database and saves.
		/// </summary>
		/// <param name="waiting">
		/// Waiting to be added to the database.
		/// </param>
		public void AddWaiting(Waiting waiting)
		{
			_context.Add(waiting);
			_context.SaveChanges();
		}

		/// <summary>
		/// Calls the database context to return the waiting list.
		/// </summary>
		/// <returns>
		/// Waiting list.
		/// </returns>
		public List<Waiting> GetWaitingList()
		{
			return _context.Waitings.ToList();
		}

		/// <summary>
		/// Removes the waiting list entry from the database.
		/// </summary>
		/// <param name="waitingId">
		/// Id of the waiting to be removed.
		/// </param>
		public void RemoveWaiting(int waitingId)
		{
			var waitingToBeRemoved = _context.Waitings.Where(w => w.WaitingId == waitingId);
			_context.Waitings.RemoveRange(waitingToBeRemoved);
			_context.SaveChanges();
		}
	}
}