﻿using HospitalData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HospitalManagement
{
	public class OrganMatchFinder
	{
		private WaitingListManager _waitingListManager = new WaitingListManager();
		private MatchedDonationManager _matchedDonationManager = new MatchedDonationManager();

		// returns a list of donated organs with same OrganId the waiting list entry, if the donated organ is available
		public List<DonatedOrgan> HasOrganList(int waitingId)
		{
			using (var db = new HospitalContext())
			{
				var waiting = db.Waitings.Where(w => w.WaitingId == waitingId).FirstOrDefault();
				var hasOrganList = db.DonatedOrgans.Where(d => d.OrganId == waiting.OrganId && d.IsDonated == false).ToList();

				return hasOrganList;
			}
		}

		// returns true if HasOrganList is not null
		public bool HasOrgan(int waitingId)
		{
			var hasOrgan = HasOrganList(waitingId).Any();

			return hasOrgan;
		}

		// AgeFinder
		public string AgeRangeFinder(int age)
		{
			if (age <= 1)
			{
				return "Newborn or Infant";
			}

			if (age <= 3)
			{
				return "Toddler";
			}

			if (age <= 5)
			{
				return "Preschooler";
			}

			if (age <= 12)
			{
				return "Child";
			}

			if (age <= 19)
			{
				return "Teenager";
			}

			return "Adult";
		}

		// AgeRangeFinder() method overload that takes in date of birth as parameter
		public string AgeRangeFinder(DateTime dateOfBirth)
		{
			int age = DateTime.Today.Year - dateOfBirth.Year;
			return AgeRangeFinder(age);
		}

		// returns list of donated organs where the age ranges match
		public List<DonatedOrgan> AgeCheckList(int waitingId)
		{
			using (var db = new HospitalContext())
			{
				var waiting = db.Waitings.Where(w => w.WaitingId == waitingId).FirstOrDefault();
				var organ = db.Organs.Where(o => o.OrganId == waiting.OrganId).FirstOrDefault();
				var organAgeChecked = organ.IsAgeChecked;
				var donatedOrgans = HasOrganList(waitingId);

				if (organAgeChecked)
				{
					var patient = db.Patients.Where(p => p.PatientId == waiting.PatientId).FirstOrDefault();
					var ageRangePatient = AgeRangeFinder(patient.DateOfBirth);

					List<DonatedOrgan> ageCheckList = new List<DonatedOrgan>();

					foreach (var donatedOrgan in donatedOrgans)
					{
						var ageRangeDonor = AgeRangeFinder((int)donatedOrgan.DonorAge);

						if (ageRangePatient == ageRangeDonor)
						{
							ageCheckList.Add(donatedOrgan);
						}
					}

					return ageCheckList;
				}

				return donatedOrgans;
			}
		}

		// returns true if HasOrganList is not null
		public bool AgeCheck(int waitingId)
		{
			var AgeCheck = AgeCheckList(waitingId).Any();

			return AgeCheck;
		}

		// blood type compatibility check
		public bool BloodTypeCheck(string patientBloodType, string donorBloodType)
		{
			switch (donorBloodType)
			{
				case "O":
					return true;

				case "A":
					if (patientBloodType == "A" || patientBloodType == "AB")
					{
						return true;
					}
					break;

				case "B":
					if (patientBloodType == "B" || patientBloodType == "AB")
					{
						return true;
					}
					break;

				case "AB":
					if (patientBloodType == "AB")
					{
						return true;
					}
					break;
			}
			return false;
		}

		// returns list of donated organs where the blood types is compatible
		public List<DonatedOrgan> BloodTypeMatchList(int waitingId)
		{
			using (var db = new HospitalContext())
			{
				var waiting = db.Waitings.Where(w => w.WaitingId == waitingId).FirstOrDefault();
				var patient = db.Patients.Where(p => p.PatientId == waiting.PatientId).FirstOrDefault();
				var patientBloodType = patient.BloodType;

				var donatedOrgans = AgeCheckList(waitingId);

				List<DonatedOrgan> donatedOrgansCorrectBloodType = new List<DonatedOrgan>();

				foreach (var donatedOrgan in donatedOrgans)
				{
					var donorBloodType = donatedOrgan.BloodType;
					var bloodTypeCheck = BloodTypeCheck(patientBloodType, donorBloodType);

					if (bloodTypeCheck)
					{
						donatedOrgansCorrectBloodType.Add(donatedOrgan);
					}
				}

				return donatedOrgansCorrectBloodType;
			}
		}

		// returns true if BloodTypeMatchList is not null
		public bool BloodTypeMatch(int waitingId)
		{
			var bloodTypeMatch = BloodTypeMatchList(waitingId).Any();

			return bloodTypeMatch;
		}

		// Finding a match
		public bool MatchExists(int waitingId)
		{
			return HasOrgan(waitingId) && AgeCheck(waitingId) && BloodTypeMatch(waitingId);
		}

		// Listing the matches
		public List<DonatedOrgan> ListMatchedOrgans(int waitingId)
		{
			var matchedOrgans = new List<DonatedOrgan>();

			if (MatchExists(waitingId))
			{
				matchedOrgans = BloodTypeMatchList(waitingId);
			}
			
			return matchedOrgans;			
		}

		public void ExecuteMatch(int waitingId, int donatedOrganId)
		{
			if (MatchExists(waitingId))
			{
				using (var db = new HospitalContext())
				{
					// mark the donated organ as donated and save changes
					var donatedOrgan = db.DonatedOrgans.Where(d => d.DonatedOrganId == donatedOrganId).FirstOrDefault();
					donatedOrgan.IsDonated = true;
					db.SaveChanges();

					// add an entry to the matched donations table
					var waiting = db.Waitings.Where(w => w.WaitingId == waitingId).FirstOrDefault();
					_matchedDonationManager.CreateMatchedDonation(waiting.PatientId, donatedOrganId, DateTime.Now);

					// delete the waiting from the database
					_waitingListManager.DeleteWaiting(waitingId);
				}
			}
		}
	}
}