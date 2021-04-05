﻿using System.Linq;

namespace HospitalData
{
	public partial class Waiting
	{
		public override string ToString()
		{
			using var db = new HospitalContext();

			var waiting = db.Waitings.Where(w => w.WaitingId == WaitingId).FirstOrDefault();
			var patient = db.Patients.Where(p => p.PatientId == waiting.PatientId).FirstOrDefault();
			var organ = db.Organs.Where(o => o.OrganId == waiting.OrganId).FirstOrDefault();

			return $"Id: {WaitingId} - {patient.Title} {patient.FirstName} {patient.LastName} of Blood Type {patient.BloodType} needs {organ.Name}";
		}

		public override bool Equals(object obj)
		{
			return obj is Waiting waiting &&
				   PatientId == waiting.PatientId &&
				   OrganId == waiting.OrganId &&
				   DateOfEntry == waiting.DateOfEntry;
		}

		public override int GetHashCode()
		{
			return System.HashCode.Combine(WaitingId, PatientId, OrganId, DateOfEntry);
		}
	}

	public partial class DonatedOrgan
	{
		public override string ToString()
		{
			DonatedOrgan donatedOrgan;
			Organ organ;
			using (var db = new HospitalContext())
			{
				donatedOrgan = db.DonatedOrgans.Where(d => d.DonatedOrganId == this.DonatedOrganId).FirstOrDefault();
				organ = db.Organs.Where(o => o.OrganId == donatedOrgan.OrganId).FirstOrDefault();
			}

			var availability = donatedOrgan.IsDonated ? "No" : "Yes";

			return $"Id: {DonatedOrganId} - Availability: {availability} - Organ: {organ.Name} - Blood Type: {BloodType} - Age at Donation: {DonorAge} - Donated on: {DonationDate:dd/MM/yyyy}";
		}

		public override bool Equals(object obj)
		{
			return obj is DonatedOrgan organ &&
				   OrganId == organ.OrganId &&
				   BloodType == organ.BloodType &&
				   DonorAge == organ.DonorAge &&
				   IsDonated == organ.IsDonated;
		}

		public override int GetHashCode()
		{
			return System.HashCode.Combine(DonatedOrganId, OrganId, BloodType, DonorAge, IsDonated);
		}
	}

	public partial class Patient
	{
		public override string ToString()
		{
			return $"{PatientId} - {Title} {LastName} {FirstName} - Blood Type: {BloodType} - {DateOfBirth:dd/MM/yyyy} - {Address}, {City}, {PostCode}";
		}

		public override bool Equals(object obj)
		{
			return obj is Patient patient &&
				   Title == patient.Title &&
				   LastName == patient.LastName &&
				   FirstName == patient.FirstName &&
				   DateOfBirth == patient.DateOfBirth &&
				   Address == patient.Address &&
				   City == patient.City &&
				   PostCode == patient.PostCode &&
				   Phone == patient.Phone &&
				   BloodType == patient.BloodType;
		}

		public override int GetHashCode()
		{
			System.HashCode hash = new System.HashCode();
			hash.Add(PatientId);
			hash.Add(Title);
			hash.Add(LastName);
			hash.Add(FirstName);
			hash.Add(DateOfBirth);
			hash.Add(Address);
			hash.Add(City);
			hash.Add(PostCode);
			hash.Add(Phone);
			hash.Add(BloodType);

			return hash.ToHashCode();
		}
	}

	public partial class Organ
	{
		public override string ToString()
		{
			return Name;
		}

		public override bool Equals(object obj)
		{
			return obj is Organ organ &&
				   Name == organ.Name &&
				   Type == organ.Type &&
				   IsAgeChecked == organ.IsAgeChecked;
		}

		public override int GetHashCode()
		{
			return System.HashCode.Combine(OrganId, Name, Type, IsAgeChecked);
		}
	}

	public partial class MatchedDonation
	{
		public override string ToString()
		{
			Patient patient;
			DonatedOrgan donatedOrgan;
			Organ organ;
			using (var db = new HospitalContext())
			{
				patient = db.Patients.Where(p => p.PatientId == this.PatientId).FirstOrDefault();
				donatedOrgan = db.DonatedOrgans.Where(d => d.DonatedOrganId == this.DonatedOrganId).FirstOrDefault();
				organ = db.Organs.Where(o => o.OrganId == donatedOrgan.OrganId).FirstOrDefault();
			}

			return $"{MatchedDonationId} - {patient.FirstName} {patient.LastName} has received {organ.Name} on {DateOfMatch:dd/MM/yyyy}.";
		}

		public override bool Equals(object obj)
		{
			return obj is MatchedDonation donation &&
				   PatientId == donation.PatientId &&
				   DonatedOrganId == donation.DonatedOrganId &&
				   DateOfMatch == donation.DateOfMatch;
		}

		public override int GetHashCode()
		{
			return System.HashCode.Combine(MatchedDonationId, PatientId, DonatedOrganId, DateOfMatch);
		}
	}
}