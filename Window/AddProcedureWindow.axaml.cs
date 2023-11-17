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

public partial class AddProcedureWindow : Avalonia.Controls.Window
{
    private List<Doctor> _DoctorList { get; set; }
    private List<Status> _StatusList { get; set; }
    private List<DiseaseRecord> _DiseaseRecordsList { get; set; }
    private DiseaseRecord _DiseaseRecord { get; set; }

    public AddProcedureWindow(DiseaseRecord diseaseRecord)
    {
        InitializeComponent();
        _DiseaseRecord = diseaseRecord;
        UpdateComboBox();
    }

    private void UpdateComboBox()
    {
        _StatusList = DataBaseManager.GetStatusList();
        _DoctorList = DataBaseManager.GetDoctors();
        _DiseaseRecordsList = DataBaseManager.GetDiseaseRecords();
        CBoxStatus.ItemsSource = _StatusList;
        CBoxAttendingDoctor.ItemsSource = _DoctorList;
        CBoxDisiaseRecord.ItemsSource = _DiseaseRecordsList;
        CBoxDisiaseRecord.SelectedItem = _DiseaseRecordsList.Where(c => c.Id == _DiseaseRecord.Id).FirstOrDefault();
        CBoxDisiaseRecord.IsEnabled = false;
        CBoxStatus.SelectedItem = _StatusList.Where(c => c.Id == 1).FirstOrDefault();
        CBoxStatus.IsEnabled = false;
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

        DataBaseManager.AddProcedure(new Procedure(
            0,
            (CBoxAttendingDoctor.SelectedItem as Doctor).Id,
            (CBoxDisiaseRecord.SelectedItem as DiseaseRecord).Id,
            TBoxDescription.Text,
            DPickerDateStart.SelectedDate.Value.Date,
            Convert.ToInt32(NUpDownDuration.Value.Value),
            Convert.ToDecimal(NUpDownCost.Value.Value),
            2
        ));
        this.Close();
    }

    private void BtnExit_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}