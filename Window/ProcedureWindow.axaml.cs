using System;
using System.Collections.Generic;
using System.Linq;
using AN_UP.DateBase;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace AN_UP.Window;

public partial class ProcedureWindow : Avalonia.Controls.Window
{
    private List<Procedure> _DataProcedure { get; set; }
    private List<Procedure> _ViewProcedure { get; set; }
    private List<Doctor> _DoctorList { get; set; }
    private List<DiseaseRecord> _PatientDisesList { get; set; }
    private List<Status> _StatusList { get; set; }
    private Patient authPatient { get; set; }

    public ProcedureWindow()
    {
        InitializeComponent();
        UpdateComboBox();
        DownloadDataGrid();
    }

    public ProcedureWindow(Patient patient)
    {
        InitializeComponent();
        authPatient = patient;
        PanelAdmin.IsEnabled = false;
        UpdateComboBox();
        DownloadDataGrid();
    }

    public void DownloadDataGrid()
    {
        _DataProcedure = DataBaseManager.GetProcedures();
        UpdateDataGrid();
    }

    private void UpdateDataGrid()
    {
        _ViewProcedure = _DataProcedure;
        if (authPatient != null)
        {
            List<DiseaseRecord> filterDiseaseRecords = DataBaseManager.GetDiseaseRecords();
            filterDiseaseRecords = filterDiseaseRecords.Where(c => c.PatientID == authPatient.Id).ToList();
            List<int> idDisiaseRecrod = new List<int>();
            foreach (DiseaseRecord value in filterDiseaseRecords)
                idDisiaseRecrod.Add(value.Id);
            _ViewProcedure = _ViewProcedure.Where(c => idDisiaseRecrod.Contains(c.DiseaseRecordID)).ToList();
        }

        if (SearchBox.Text.Length > 0)
            _ViewProcedure = _ViewProcedure.Where(c =>
                c.Id.ToString().Contains(SearchBox.Text) ||
                c.DoctorID.ToString().Contains(SearchBox.Text) ||
                c.DiseaseRecordID.ToString().Contains(SearchBox.Text) ||
                c.Description.ToString().Contains(SearchBox.Text) ||
                c.DateStart.ToString().Contains(SearchBox.Text) ||
                c.Duration.ToString().Contains(SearchBox.Text) ||
                c.Cost.ToString().Contains(SearchBox.Text) ||
                c.StatusID.ToString().Contains(SearchBox.Text)
            ).ToList();
        DataGrid.ItemsSource = _ViewProcedure;
    }

    private void UpdateComboBox()
    {
        _StatusList = DataBaseManager.GetStatusList();
        _DoctorList = DataBaseManager.GetDoctors();
        _PatientDisesList = DataBaseManager.GetDiseaseRecords();
        CBoxDisiaseRecord.ItemsSource = _PatientDisesList;
        CBoxStatus.ItemsSource = _StatusList;
        CBoxAttendingDoctor.ItemsSource = _DoctorList;
    }

    private void DataGrid_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (DataGrid.SelectedItem == null)
        {
            CBoxAttendingDoctor.IsEnabled = true;
            CBoxDisiaseRecord.IsEnabled = true;
            DPickerDateStart.IsEnabled = true;
            NUpDownCost.IsEnabled = true;
            CBoxAttendingDoctor.SelectedItem = null;
            CBoxDisiaseRecord.SelectedItem = null;
            TBoxDescription.Text = "";
            DPickerDateStart.SelectedDate = DateTime.Now;
            NUpDownDuration.Value = 0;
            NUpDownCost.Value = 0;
            CBoxStatus.SelectedItem = null;
        }
        else
        {
            CBoxAttendingDoctor.IsEnabled = false;
            CBoxDisiaseRecord.IsEnabled = false;
            DPickerDateStart.IsEnabled = false;
            NUpDownCost.IsEnabled = false;
            Procedure value = DataGrid.SelectedItem as Procedure;
            CBoxAttendingDoctor.SelectedItem = _DoctorList.Where(c => c.Id == value.DoctorID).FirstOrDefault();
            CBoxDisiaseRecord.SelectedItem =
                _PatientDisesList.Where(c => c.Id == value.DiseaseRecordID).FirstOrDefault();
            TBoxDescription.Text = value.Description;
            DPickerDateStart.SelectedDate = value.DateStart;
            NUpDownDuration.Value = value.Duration;
            NUpDownCost.Value = value.Cost;
            CBoxStatus.SelectedItem = _StatusList.Where(c => c.Id == value.StatusID).FirstOrDefault();
        }
    }

    private void ResetBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        SearchBox.Text = "";
    }

    private void BtnDelet_OnClick(object? sender, RoutedEventArgs e)
    {
        if (DataGrid.SelectedItem == null)
            return;
        DataBaseManager.RemoveProcedure(DataGrid.SelectedItem as Procedure);
        DownloadDataGrid();
    }

    private void BtnRemoveSelect_OnClick(object? sender, RoutedEventArgs e)
    {
        DataGrid.SelectedItem = null;
    }

    private void BtnSavet_OnClick(object? sender, RoutedEventArgs e)
    {
        if (CBoxAttendingDoctor.SelectedItem == null ||
            CBoxDisiaseRecord.SelectedItem == null ||
            TBoxDescription.Text.Length <= 1 ||
            DPickerDateStart.SelectedDate == null ||
            NUpDownDuration.Value == 0 ||
            NUpDownCost.Value == 0 ||
            CBoxStatus.SelectedItem == null)
        {
            MessageBoxManager.GetMessageBoxStandard("Ошибка", "Данные не заполнены", ButtonEnum.Ok).ShowAsync();
            return;
        }

        if (DataGrid.SelectedItem == null)
        {
            DataBaseManager.AddProcedure(new Procedure(
                0,
                (CBoxAttendingDoctor.SelectedItem as Doctor).Id,
                (CBoxDisiaseRecord.SelectedItem as DiseaseRecord).Id,
                TBoxDescription.Text,
                DPickerDateStart.SelectedDate.Value.Date,
                Convert.ToInt32(NUpDownDuration.Value.Value),
                Convert.ToDecimal(NUpDownCost.Value.Value),
                (CBoxStatus.SelectedItem as Status).Id
            ));
        }
        else
        {
            DataBaseManager.UpdateProcedure(new Procedure(
                ((Procedure)DataGrid.SelectedItem).Id,
                (CBoxAttendingDoctor.SelectedItem as Doctor).Id,
                (CBoxDisiaseRecord.SelectedItem as DiseaseRecord).Id,
                TBoxDescription.Text,
                DPickerDateStart.SelectedDate.Value.Date,
                Convert.ToInt32(NUpDownDuration.Value.Value),
                Convert.ToDecimal(NUpDownCost.Value.Value),
                (CBoxStatus.SelectedItem as Status).Id
            ));
        }

        DownloadDataGrid();
    }

    private void BtnCreateProcedure_OnClick(object? sender, RoutedEventArgs e)
    {
        if (DataGrid.SelectedItem == null)
            return;
        Procedure procedure = DataGrid.SelectedItem as Procedure;
        procedure.StatusID = 4;
        DataBaseManager.UpdateProcedure(procedure);
        DownloadDataGrid();
    }

    private void SearchBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        UpdateDataGrid();
    }
}