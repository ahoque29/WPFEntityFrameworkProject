﻿using System;
using System.Windows;
using HospitalManagement;

namespace HospitalWPF
{
	public partial class MainWindow : Window
	{
		private PatientManager _patientManager = new PatientManager();
		private WaitingListManager _waitingListManager = new WaitingListManager();
		private DonatedOrganManager _donatedOrganManager = new DonatedOrganManager();
		private MatchedDonationManager _matchedDonationManager = new MatchedDonationManager();
		private OrganManager _organManager = new OrganManager();

		public MainWindow()
		{
			InitializeComponent();
			PopulateListBoxPatients();
			PopulateListBoxDonatedOrgans();
			PopulateOrganNameComboBox();
		}

		#region Patient Manager Tab

		private void PopulateListBoxPatients()
		{
			ListBoxPatients.ItemsSource = _patientManager.RetrieveAllPatients();
		}

		private void RegisterPatient_Click(object sender, RoutedEventArgs e)
		{
			// create new patient
			_patientManager.CreatePatient(TitleTextBox.Text,
				LastNameTextBox.Text,
				FirstNameTextBox.Text,
				(DateTime)DobCalendar.SelectedDate,
				AddressTextBox.Text,
				CityTextBox.Text,
				PostCodeTextBox.Text,
				PhoneTextBox.Text,
				BloodTypeComboBoxPM.Text);

			// clear the text box
			ListBoxPatients.ItemsSource = null;

			// repopulate the textbox with the new patient
			PopulateListBoxPatients();

			// reinitialise the textboxes
			TitleTextBox.Text =
				LastNameTextBox.Text =
				FirstNameTextBox.Text =
				AddressTextBox.Text =
				CityTextBox.Text =
				PostCodeTextBox.Text =
				PhoneTextBox.Text =
				BloodTypeComboBoxPM.Text = null;
		}

		#endregion

		#region Donated Organ Manager

		private void PopulateListBoxDonatedOrgans()
		{
			ListBoxDonatedOrgans.ItemsSource = _donatedOrganManager.RetrieveAllDonatedOrgans();
		}

		private void PopulateOrganNameComboBox()
		{
			OrganNameComboBox.ItemsSource = _organManager.RetrieveAllOrgans();
		}

		#endregion
	}
}