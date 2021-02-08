﻿using Microsoft.EntityFrameworkCore;

namespace HospitalData
{
	class HospitalContext : DbContext
	{
		public static HospitalContext Instance { get; } = new HospitalContext();

		public DbSet<Patient> Patients { get; set; }
		public DbSet<Organ> Organs { get; set; }
		public DbSet<Waiting> Waitings { get; set; }
		public DbSet<DonatedOrgan> DonatedOrgans { get; set; }
		public DbSet<MatchedDonation> MatchedDonations { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			if (!options.IsConfigured)
			{
				options.UseSqlServer(@"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = HospitalProject;");
			}
		}

	}
}