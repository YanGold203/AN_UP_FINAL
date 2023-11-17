using System.Collections.Generic;
using AN_UP.DateBase;
using AN_UP.Window;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace AN_UP;

public partial class LoginWindow : Avalonia.Controls.Window
{
    private bool isPatient = false;
    private Doctor doctorAuth;
    private Patient patientAuth;

    public LoginWindow()
    {
        InitializeComponent();
        doctorAuth = null;
    }

    private void BtnAuth_OnClick(object? sender, RoutedEventArgs e)
    {
        if (isPatient)
        {
            if (TBoxPhoneNumber.Text.Length <= 1)
            {
                MessageBoxManager.GetMessageBoxStandard("Ошибка", "Поля не заполнены", ButtonEnum.Ok).ShowAsync();
                return;
            }

            List<Patient> patients = DataBaseManager.GetPatients();
            if (patients.Count == 0)
            {
                MessageBoxManager.GetMessageBoxStandard("Ошибка", "В базе нет сотрудников", ButtonEnum.Ok).ShowAsync();
                return;
            }

            for (int i = 0; i < patients.Count; i++)
            {
                if (patients[i].PhoneNumber == (TBoxPhoneNumber.Text))
                {
                    patientAuth = patients[i];
                    break;
                }
            }

            if (patientAuth == null)
            {
                MessageBoxManager.GetMessageBoxStandard("Ошибка", "Данные не верны", ButtonEnum.Ok).ShowAsync();
                return;
            }
            else
            {
                ProcedureWindow wMeny = new ProcedureWindow(patientAuth);
                wMeny.Show();
                this.Hide();
            }
        }
        else
        {
            if (TBoxLogin.Text.Length <= 1 || TBoxPassword.Text.Length <= 1)
            {
                MessageBoxManager.GetMessageBoxStandard("Ошибка", "Поля не заполнены", ButtonEnum.Ok).ShowAsync();
                return;
            }

            List<Doctor> doctors = DataBaseManager.GetDoctors();
            if (doctors.Count == 0)
            {
                MessageBoxManager.GetMessageBoxStandard("Ошибка", "В базе нет сотрудников", ButtonEnum.Ok).ShowAsync();
                return;
            }

            for (int i = 0; i < doctors.Count; i++)
            {
                if (doctors[i].Login == (TBoxLogin.Text) &&
                    doctors[i].Password == (TBoxPassword.Text))
                {
                    doctorAuth = doctors[i];
                    break;
                }
            }

            if (doctorAuth == null)
            {
                MessageBoxManager.GetMessageBoxStandard("Ошибка", "Данные не верны", ButtonEnum.Ok).ShowAsync();
                return;
            }
            else
            {
                MenyWindow wMeny = new MenyWindow(doctorAuth);
                wMeny.Show();
                this.Hide();
            }
        }
    }

    private void BtnSwitch_OnClick(object? sender, RoutedEventArgs e)
    {
        isPatient = !isPatient;
        if (isPatient)
        {
            PanelClient.IsVisible = true;
            PanelEmployee.IsVisible = false;
        }
        else
        {
            PanelClient.IsVisible = false;
            PanelEmployee.IsVisible = true;
        }
    }

    private void BtnClose_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}