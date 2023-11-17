using System;
using System.Collections.Generic;
using System.Linq;
using AN_UP.DateBase;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AN_UP.Window;

public partial class ReportWindow : Avalonia.Controls.Window
{
    private List<Status> _StatusList { get; set; }
    private List<Doctor> _DoctorsList { get; set; }
    private List<Patient> _PatientsList { get; set; }

    public ReportWindow()
    {
        InitializeComponent();
        UpdateComboBox();
    }

    private void UpdateComboBox()
    {
        _StatusList = DataBaseManager.GetStatusList();
        _DoctorsList = DataBaseManager.GetDoctors();
        _PatientsList = DataBaseManager.GetPatients();
        CBoxStatus.ItemsSource = _StatusList;
        CBoxDoctor.ItemsSource = _DoctorsList;
        CBoxPatient.ItemsSource = _PatientsList;
    }

    private void GenerateReport()
    {
        // Стартовые переменные
        decimal price = 0;
        List<Procedure> procedures = DataBaseManager.GetProcedures();

        // Отчистка формы
        ClearReportForm();

        // Фильрация по ComboBox
        if (CBoxDoctor.SelectedItem != null)
            procedures = procedures.Where(c => c.DoctorID == (CBoxDoctor.SelectedItem as Doctor).Id).ToList();
        if (CBoxPatient.SelectedItem != null)
            procedures = procedures.Where(c => c.DoctorID == (CBoxPatient.SelectedItem as Patient).Id).ToList();
        if (CBoxStatus.SelectedItem != null)
            procedures = procedures.Where(c => c.DoctorID == (CBoxStatus.SelectedItem as Status).Id).ToList();

        // Проверка периода
        if (CBoxPatient.SelectedItem == null)
            TBoxPatient.Text = "Все";
        else
            TBoxPatient.Text = (CBoxPatient.SelectedItem as Patient).FirstName + " " +
                               (CBoxPatient.SelectedItem as Patient).LastName;
        if (CBoxDoctor.SelectedItem == null)
            TBoxDoctor.Text = "Все";
        else
            TBoxDoctor.Text = (CBoxDoctor.SelectedItem as Doctor).FirstName + " " +
                              (CBoxDoctor.SelectedItem as Doctor).LastName;
        if (CBoxStatus.SelectedItem == null)
            TBoxStatus.Text = "Все";
        else
            TBoxStatus.Text = (CBoxStatus.SelectedItem as Status).Name;
        if (DPicerStart.SelectedDate == null)
            DPicerStart.SelectedDate = DateTime.Today.AddYears(-1);
        if (DPicerEnd.SelectedDate == null)
            DPicerEnd.SelectedDate = DateTime.Now;
        if (DPicerEnd.SelectedDate != null && DPicerStart.SelectedDate == null)
            DPicerStart.SelectedDate = DPicerEnd.SelectedDate.Value.Date.AddYears(-1);
        if (DPicerEnd.SelectedDate > DPicerStart.SelectedDate)
        {
            DateTime cash = DPicerEnd.SelectedDate.Value.Date;
            DPicerEnd.SelectedDate = DPicerStart.SelectedDate;
            DPicerStart.SelectedDate = cash;
        }

        TBoxCountProcedure.Text = procedures.Count.ToString();
        TBoxDateSelected.Text =
            DPicerStart.SelectedDate.Value.ToString() + " - " + DPicerEnd.SelectedDate.Value.ToString();
        foreach (Procedure value in procedures)
        {
            TBoxProcedureList.Text += value.Id + " | " + value.DateStart.Date + " | " + value.Cost.ToString() + " | " +
                                      value.OutputStatusName + "\n";
            price += value.Cost;
        }

        TBoxPrice.Text = price.ToString();
    }

    private void ClearReportForm()
    {
        TBoxDoctor.Text = "";
        TBoxProcedureList.Text = "";
        TBoxPrice.Text = "";
        TBoxPatient.Text = "";
        TBoxCountProcedure.Text = "";
        TBoxDateSelected.Text = "";
        TBoxStatus.Text = "";
    }

    private void BtnPatientClear_OnClick(object? sender, RoutedEventArgs e)
    {
        CBoxPatient.SelectedItem = null;
    }

    private void BtnDoctorClear_OnClick(object? sender, RoutedEventArgs e)
    {
        CBoxDoctor.SelectedItem = null;
    }

    private void BtnStatusClear_OnClick(object? sender, RoutedEventArgs e)
    {
        CBoxStatus.SelectedItem = null;
    }

    private void BtnReset_OnClick(object? sender, RoutedEventArgs e)
    {
        CBoxPatient.SelectedItem = null;
        CBoxDoctor.SelectedItem = null;
        CBoxStatus.SelectedItem = null;
        ClearReportForm();
    }

    private void BtnGenerate_OnClick(object? sender, RoutedEventArgs e)
    {
        GenerateReport();
    }

    private void BtnGoPrint_OnClick(object? sender, RoutedEventArgs e)
    {
        GenerateReport();
    }
}