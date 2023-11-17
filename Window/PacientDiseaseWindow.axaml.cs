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

public partial class PacientDiseaseWindow : Avalonia.Controls.Window
{
    private List<DiseaseRecord> _DataDiseaseRecord { get; set; }
    private List<DiseaseRecord> _ViewDiseaseRecord { get; set; }
    private List<Patient> _PatientList { get; set; }
    private List<Doctor> _DoctorList { get; set; }
    private List<Status> _StatusList { get; set; }
    private List<Disease> _DiseaseList { get; set; }

    public PacientDiseaseWindow()
    {
        InitializeComponent();
        DownloadDataGrid();
        UpdateComboBox();
    }

    public void DownloadDataGrid()
    {
        _DataDiseaseRecord = DataBaseManager.GetDiseaseRecords();
        UpdateDataGrid();
    }

    private void UpdateDataGrid()
    {
        _ViewDiseaseRecord = _DataDiseaseRecord;
        if (SearchBox.Text.Length >= 1)
        {
            string filters = SearchBox.Text.ToLower();
            _ViewDiseaseRecord = _ViewDiseaseRecord.Where(c =>
                c.Id.ToString().Contains(filters) ||
                c.PatientID.ToString().Contains(filters) ||
                c.FinalPrice.ToString().Contains(filters) ||
                c.DateStart.ToString().Contains(filters) ||
                c.DateEnd.ToString().Contains(filters) ||
                c.StatusID.ToString().Contains(filters) ||
                c.AttendingDoctorID.ToString().Contains(filters) ||
                c.DiseaseID.ToString().Contains(filters) ||
                c.OutputPatientFIO.ToLower().Contains(filters) ||
                c.OutputDiseaseName.ToLower().Contains(filters) ||
                c.OutputStatusName.ToLower().Contains(filters) ||
                c.OutputDoctorFIO.ToLower().Contains(filters)
            ).ToList();
        }

        DataGrid.ItemsSource = _ViewDiseaseRecord;
    }

    private void UpdateComboBox()
    {
        _DiseaseList = DataBaseManager.GetDiseases();
        _StatusList = DataBaseManager.GetStatusList();
        _DoctorList = DataBaseManager.GetDoctors();
        _PatientList = DataBaseManager.GetPatients();
        CBoxDisease.ItemsSource = _DiseaseList;
        CBoxPatient.ItemsSource = _PatientList;
        CBoxStatus.ItemsSource = _StatusList;
        CBoxAttendingDoctor.ItemsSource = _DoctorList;
    }

    private void DataGrid_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (DataGrid.SelectedItem == null)
        {
            CBoxPatient.SelectedItem = null;
            NUpDownFinalPrice.Value = 0;
            DPickerDateStart.SelectedDate = DateTimeOffset.Now;
            DPickerDateEnd.SelectedDate = null;
            CBoxStatus.SelectedItem = null;
            CBoxDisease.SelectedItem = null;
            CBoxAttendingDoctor.SelectedItem = null;
        }
        else
        {
            DiseaseRecord value = DataGrid.SelectedItem as DiseaseRecord;
            CBoxPatient.SelectedItem = _PatientList.Where(c => value.PatientID == c.Id).FirstOrDefault();
            NUpDownFinalPrice.Value = value.FinalPrice;
            DPickerDateStart.SelectedDate = value.DateStart;
            DPickerDateEnd.SelectedDate = value.DateEnd;
            CBoxStatus.SelectedItem = _StatusList.Where(c => value.StatusID == c.Id).FirstOrDefault();
            CBoxDisease.SelectedItem = _DiseaseList.Where(c => value.DiseaseID == c.Id).FirstOrDefault();
            CBoxAttendingDoctor.SelectedItem = _DoctorList.Where(c => value.AttendingDoctorID == c.Id).FirstOrDefault();
        }
    }

    private void BtnDelet_OnClick(object? sender, RoutedEventArgs e)
    {
        if (DataGrid.SelectedItem == null)
            return;
        DataBaseManager.RemoveDiseaseRecord(DataGrid.SelectedItem as DiseaseRecord);
        DownloadDataGrid();
    }

    private void BtnRemoveSelect_OnClick(object? sender, RoutedEventArgs e)
    {
        DataGrid.SelectedItem = null;
    }

    private void ResetBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        SearchBox.Text = "";
    }

    private void BtnSavet_OnClick(object? sender, RoutedEventArgs e)
    {
        if (CBoxPatient.SelectedItem == null ||
            DPickerDateStart.SelectedDate == null ||
            CBoxStatus.SelectedItem == null ||
            CBoxAttendingDoctor.SelectedItem == null ||
            CBoxDisease.SelectedItem == null)
        {
            MessageBoxManager.GetMessageBoxStandard("Ошибка", "Данные не заполнены", ButtonEnum.Ok).ShowAsync();
            return;
        }

        if (DataGrid.SelectedItem == null)
        {
            DataBaseManager.AddDiseaseRecord(new DiseaseRecord(
                0,
                (CBoxPatient.SelectedItem as Patient).Id,
                Convert.ToDecimal(NUpDownFinalPrice.Value.Value),
                DPickerDateStart.SelectedDate.Value.Date,
                DPickerDateEnd.SelectedDate.Value.Date,
                (CBoxStatus.SelectedItem as Status).Id,
                (CBoxAttendingDoctor.SelectedItem as Doctor).Id,
                (CBoxDisease.SelectedItem as Disease).Id
            ));
        }
        else
        {
            DataBaseManager.UpdateDiseaseRecord(new DiseaseRecord(
                ((DiseaseRecord)DataGrid.SelectedItem).Id,
                (CBoxPatient.SelectedItem as Patient).Id,
                Convert.ToDecimal(NUpDownFinalPrice.Value.Value),
                DPickerDateStart.SelectedDate.Value.Date,
                DPickerDateEnd.SelectedDate.Value.Date,
                (CBoxStatus.SelectedItem as Status).Id,
                (CBoxAttendingDoctor.SelectedItem as Doctor).Id,
                (CBoxDisease.SelectedItem as Disease).Id
            ));
        }

        DownloadDataGrid();
    }

    private void BtnCreateProcedure_OnClick(object? sender, RoutedEventArgs e)
    {
        if (DataGrid.SelectedItem == null)
            return;
        AddProcedureWindow wAddReport =
            new AddProcedureWindow(DataGrid.SelectedItem as DiseaseRecord);
        wAddReport.ShowDialog(this);
    }

    private void BtnRecover_OnClick(object? sender, RoutedEventArgs e)
    {
        if (DataGrid.SelectedItem == null)
            return;
        DiseaseRecord diseaseRecord = DataGrid.SelectedItem as DiseaseRecord;
        diseaseRecord.StatusID = 4;
        diseaseRecord.DateEnd = DateTime.Now;
        DataBaseManager.UpdateDiseaseRecord(diseaseRecord);
        DownloadDataGrid();
    }

    private void BtnUpdateFinalPrice_OnClick(object? sender, RoutedEventArgs e)
    {
        if (DataGrid.SelectedItem == null)
            return;
        DiseaseRecord diseaseRecord = DataGrid.SelectedItem as DiseaseRecord;
        List<Procedure> procedures = DataBaseManager.GetProcedures().Where(
            p => p.DiseaseRecordID == diseaseRecord.Id).ToList();
        decimal finalPrice = 0;
        foreach (Procedure value in procedures)
        {
            if (value.StatusID == 4)
                finalPrice += value.Cost;
        }

        diseaseRecord.FinalPrice = finalPrice;
        MessageBoxManager.GetMessageBoxStandard("Обновлено", "Итоговая стоимость услуг:" + finalPrice, ButtonEnum.Ok)
            .ShowAsync();
        DataBaseManager.UpdateDiseaseRecord(diseaseRecord);
        DownloadDataGrid();
    }

    private void SearchBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        UpdateDataGrid();
    }
}