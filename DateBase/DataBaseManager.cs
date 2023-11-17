using System;
using System.Collections.Generic;
using System.Data;
using System.Net.NetworkInformation;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySqlConnector;

namespace AN_UP.DateBase;

public class DataBaseManager
{
    /// Настройки подключения
    public static MySqlConnectionStringBuilder ConnectionString =
        new MySqlConnectionStringBuilder
        {
            Server = "localhost",
            Database = "up_an",
            UserID = "root",
            Password = "tkl909" // "tkl909"//"nrjkby99"
        };

    // Получение
    public static List<Disease> GetDiseases()
    {
        List<Disease> data = new List<Disease>();
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var comand = connection.CreateCommand())
            {
                comand.CommandText = "SELECT * FROM Disease";
                using (var reader = comand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        data.Add(
                            new Disease(
                                reader.GetInt32("ID"),
                                reader.GetString("Name"),
                                reader.GetInt32("DurationLiness")
                            )
                        );
                    }
                }
            }

            connection.Close();
        }

        return data;
    }

    public static List<DiseaseRecord> GetDiseaseRecords()
    {
        List<DiseaseRecord> data = new List<DiseaseRecord>();
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var comand = connection.CreateCommand())
            {
                comand.CommandText =
                    "SELECT \n    DR.ID,\n    DR.PatientID,\n    DR.FinalPrice,\n    DR.DateStart,\n    DR.DateEnd,\n    DR.StatusID,\n    DR.AttendingDoctorID,\n    DR.DiseaseID,\n    CONCAT(P.FirstName, ' ', P.LastName) AS PatientFullName,\n    CONCAT(D.FirstName, ' ', D.LastName) AS DoctorFullName,\n    S.Name AS StatusName,\n    Dis.Name AS DiseaseName\nFROM DiseaseRecord DR\nLEFT JOIN Patient P ON DR.PatientID = P.ID\nLEFT JOIN Doctor D ON DR.AttendingDoctorID = D.ID\nLEFT JOIN Status S ON DR.StatusID = S.ID\nLEFT JOIN Disease Dis ON DR.DiseaseID = Dis.ID\n";
                using (var reader = comand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        data.Add(
                            new DiseaseRecord(
                                reader.GetInt32("ID"),
                                reader.GetInt32("PatientID"),
                                reader.GetDecimal("FinalPrice"),
                                reader.GetDateTime("DateStart"),
                                reader.GetDateTime("DateEnd"),
                                reader.GetInt32("StatusID"),
                                reader.GetInt32("AttendingDoctorID"),
                                reader.GetInt32("DiseaseID"),
                                reader.GetString("PatientFullName"),
                                reader.GetString("StatusName"),
                                reader.GetString("DoctorFullName"),
                                reader.GetString("DiseaseName")
                            )
                        );
                    }
                }
            }

            connection.Close();
        }

        return data;
    }

    public static List<Doctor> GetDoctors()
    {
        List<Doctor> data = new List<Doctor>();
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var comand = connection.CreateCommand())
            {
                comand.CommandText = "SELECT * FROM Doctor";
                using (var reader = comand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        data.Add(
                            new Doctor(
                                reader.GetInt32("ID"),
                                reader.GetString("FirstName"),
                                reader.GetString("LastName"),
                                reader.GetString("Patronymic"),
                                reader.GetDateTime("EmploymentDate"),
                                reader.GetInt32("PositionID"),
                                reader.GetString("Login"),
                                reader.GetString("Password")
                            )
                        );
                    }
                }
            }

            connection.Close();
        }

        return data;
    }

    public static List<Patient> GetPatients()
    {
        List<Patient> data = new List<Patient>();
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var comand = connection.CreateCommand())
            {
                comand.CommandText = "SELECT * FROM Patient";
                using (var reader = comand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        data.Add(
                            new Patient(
                                reader.GetInt32("ID"),
                                reader.GetString("FirstName"),
                                reader.GetString("LastName"),
                                reader.GetString("Patronymic"),
                                reader.GetDateTime("DateBirth"),
                                reader.GetString("PhoneNumber")
                            )
                        );
                    }
                }
            }

            connection.Close();
        }

        return data;
    }

    public static List<Position> GetPositions()
    {
        List<Position> data = new List<Position>();
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var comand = connection.CreateCommand())
            {
                comand.CommandText = "SELECT * FROM Position";
                using (var reader = comand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        data.Add(
                            new Position(
                                reader.GetInt32("ID"),
                                reader.GetString("Name")
                            )
                        );
                    }
                }
            }

            connection.Close();
        }

        return data;
    }

    public static List<Procedure> GetProcedures()
    {
        List<Procedure> data = new List<Procedure>();
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var comand = connection.CreateCommand())
            {
                comand.CommandText =
                    "SELECT \n    PP.ID,\n    PP.DoctorID,\n    PP.DiseaseRecordID,\n    PP.Description,\n    PP.DateStart,\n    PP.Duration,\n    PP.Cost,\n    PP.StatusID,\n    CONCAT(D.FirstName, ' ', D.LastName) AS DoctorFullName,\n    S.Name AS StatusName\nFROM procedurepatient PP\nLEFT JOIN Doctor D ON PP.DoctorID = D.ID\nLEFT JOIN Status S ON PP.StatusID = S.ID";
                using (var reader = comand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        data.Add(new Procedure(
                            reader.GetInt32("ID"),
                            reader.GetInt32("DoctorID"),
                            reader.GetInt32("DiseaseRecordID"),
                            reader.GetString("Description"),
                            reader.GetDateTime("DateStart"),
                            reader.GetInt32("Duration"),
                            reader.GetDecimal("Cost"),
                            reader.GetInt32("StatusID"),
                            reader.GetString("DoctorFullName"),
                            reader.GetString("StatusName")
                        ));
                    }
                }
            }

            connection.Close();
        }

        return data;
    }

    public static List<Receipt> GetReceipts()
    {
        List<Receipt> data = new List<Receipt>();
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var comand = connection.CreateCommand())
            {
                comand.CommandText = "SELECT * FROM Receipt";
                using (var reader = comand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        data.Add(
                            new Receipt(
                                reader.GetInt32("ID"),
                                reader.GetDecimal("Amount"),
                                reader.GetInt32("DiseaseRecrodID"),
                                reader.GetDateTime("DatePayment")
                            )
                        );
                    }
                }
            }

            connection.Close();
        }

        return data;
    }

    public static List<Status> GetStatusList()
    {
        List<Status> data = new List<Status>();
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var comand = connection.CreateCommand())
            {
                comand.CommandText = "SELECT * FROM Status";
                using (var reader = comand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        data.Add(
                            new Status(
                                reader.GetInt32("ID"),
                                reader.GetString("Name")
                            )
                        );
                    }
                }
            }

            connection.Close();
        }

        return data;
    }

    /// Добавление
    public static void AddDisiase(Disease data)
    {
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "INSERT INTO Disease (ID, Name, DurationLiness) VALUES (@ID, @Name, @DurationLiness);";
                command.Parameters.AddWithValue("@ID", data.Id);
                command.Parameters.AddWithValue("@Name", data.Name);
                command.Parameters.AddWithValue("@DurationLiness", data.DurationLiness);
                var rowsCount = command.ExecuteNonQuery();
                if (rowsCount == 0)
                {
                    MessageBoxManager.GetMessageBoxStandard("Ошибка", "Данные не добавлены", ButtonEnum.Ok).ShowAsync();
                    ;
                }
            }

            connection.Close();
        }
    }

    public static void AddDiseaseRecord(DiseaseRecord data)
    {
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "INSERT INTO DiseaseRecord (PatientID, FinalPrice, DateStart, DateEnd, StatusID, AttendingDoctorID, DiseaseID) " +
                    "VALUES (@PatientID, @FinalPrice, @DateStart, @DateEnd, @StatusID, @AttendingDoctorID, @DiseaseID);";
                command.Parameters.AddWithValue("@PatientID", data.PatientID);
                command.Parameters.AddWithValue("@FinalPrice", data.FinalPrice.ToString("#.##"));
                command.Parameters.AddWithValue("@DateStart", data.DateStart);
                command.Parameters.AddWithValue("@DateEnd", data.DateEnd);
                command.Parameters.AddWithValue("@StatusID", data.StatusID);
                command.Parameters.AddWithValue("@AttendingDoctorID", data.AttendingDoctorID);
                command.Parameters.AddWithValue("@DiseaseID", data.DiseaseID);
                var rowsCount = command.ExecuteNonQuery();
                if (rowsCount == 0)
                {
                    MessageBoxManager.GetMessageBoxStandard("Ошибка", "Данные не добавлены", ButtonEnum.Ok).ShowAsync();
                }
            }
        }
    }

    public static void AddDoctor(Doctor data)
    {
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "INSERT INTO Doctor (ID, FirstName, LastName, Patronymic, EmploymentDate, PositionID, Login, Password) " +
                    "VALUES (@ID, @FirstName, @LastName, @Patronymic, @EmploymentDate, @PositionID, @Login, @Password);";
                command.Parameters.AddWithValue("@ID", data.Id);
                command.Parameters.AddWithValue("@FirstName", data.FirstName);
                command.Parameters.AddWithValue("@LastName", data.LastName);
                command.Parameters.AddWithValue("@Patronymic", data.Patronymic);
                command.Parameters.AddWithValue("@EmploymentDate", data.EmploymentDate);
                command.Parameters.AddWithValue("@PositionID", data.PositionID);
                command.Parameters.AddWithValue("@Login", data.Login);
                command.Parameters.AddWithValue("@Password", data.Password);
                var rowsCount = command.ExecuteNonQuery();
                if (rowsCount == 0)
                {
                    MessageBoxManager.GetMessageBoxStandard("Ошибка", "Данные не добавлены", ButtonEnum.Ok).ShowAsync();
                    ;
                }
            }

            connection.Close();
        }
    }

    public static void AddPatient(Patient data)
    {
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "INSERT INTO Patient (ID, FirstName, LastName, Patronymic, DateBirth, PhoneNumber) " +
                    "VALUES (@ID, @FirstName, @LastName, @Patronymic, @DateBirth, @PhoneNumber);";
                command.Parameters.AddWithValue("@ID", data.Id);
                command.Parameters.AddWithValue("@FirstName", data.FirstName);
                command.Parameters.AddWithValue("@LastName", data.LastName);
                command.Parameters.AddWithValue("@Patronymic", data.Patronymic);
                command.Parameters.AddWithValue("@DateBirth", data.DateBirth);
                command.Parameters.AddWithValue("@PhoneNumber", data.PhoneNumber);
                var rowsCount = command.ExecuteNonQuery();
                if (rowsCount == 0)
                {
                    MessageBoxManager.GetMessageBoxStandard("Ошибка", "Данные не добавлены", ButtonEnum.Ok).ShowAsync();
                    ;
                }
            }

            connection.Close();
        }
    }

    public static void AddPosition(Position data)
    {
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Position (ID, Name) VALUES (@ID, @Name);";
                command.Parameters.AddWithValue("@ID", data.Id);
                command.Parameters.AddWithValue("@Name", data.Name);
                var rowsCount = command.ExecuteNonQuery();
                if (rowsCount == 0)
                {
                    MessageBoxManager.GetMessageBoxStandard("Ошибка", "Данные не добавлены", ButtonEnum.Ok).ShowAsync();
                    ;
                }
            }

            connection.Close();
        }
    }

    public static void AddProcedure(Procedure data)
    {
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "INSERT INTO procedurepatient (ID, DoctorID, DiseaseRecordID, Description, DateStart, Duration, Cost, StatusID) " +
                    "VALUES (@ID, @DoctorID, @DiseaseRecordID, @Description, @DateStart, @Duration, @Cost, @StatusID);";
                command.Parameters.AddWithValue("@ID", data.Id);
                command.Parameters.AddWithValue("@DoctorID", data.DoctorID);
                command.Parameters.AddWithValue("@DiseaseRecordID", data.DiseaseRecordID);
                command.Parameters.AddWithValue("@Description", data.Description);
                command.Parameters.AddWithValue("@DateStart", data.DateStart);
                command.Parameters.AddWithValue("@Duration", data.Duration);
                command.Parameters.AddWithValue("@Cost", data.Cost);
                command.Parameters.AddWithValue("@StatusID", data.StatusID);
                var rowsCount = command.ExecuteNonQuery();
                if (rowsCount == 0)
                {
                    MessageBoxManager.GetMessageBoxStandard("Ошибка", "Данные не добавлены", ButtonEnum.Ok).ShowAsync();
                    ;
                }
            }

            connection.Close();
        }
    }

    public static void AddReceipt(Receipt data)
    {
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Receipt (ID, Amount, DiseaseRecrodID, DatePayment) " +
                                      "VALUES (@ID, @Amount, @DiseaseRecrodID, @DatePayment);";
                command.Parameters.AddWithValue("@ID", data.Id);
                command.Parameters.AddWithValue("@Amount", data.Amount);
                command.Parameters.AddWithValue("@DiseaseRecrodID", data.DiseaseRecordID);
                command.Parameters.AddWithValue("@DatePayment", data.DatePayment);
                var rowsCount = command.ExecuteNonQuery();
                if (rowsCount == 0)
                {
                    MessageBoxManager.GetMessageBoxStandard("Ошибка", "Данные не добавлены", ButtonEnum.Ok).ShowAsync();
                    ;
                }
            }

            connection.Close();
        }
    }

    public static void AddStatus(Status data)
    {
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Status (ID, Name) VALUES (@ID, @Name);";
                command.Parameters.AddWithValue("@ID", data.Id);
                command.Parameters.AddWithValue("@Name", data.Name);
                var rowsCount = command.ExecuteNonQuery();
                if (rowsCount == 0)
                {
                    MessageBoxManager.GetMessageBoxStandard("Ошибка", "Данные не добавлены", ButtonEnum.Ok).ShowAsync();
                    ;
                }
            }

            connection.Close();
        }
    }

    /// Удаление
    public static void RemoveDisease(Disease data)
    {
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Disease WHERE ID = @ID;";
                command.Parameters.AddWithValue("@ID", data.Id);
                var rowsCount = command.ExecuteNonQuery();
                if (rowsCount == 0)
                {
                    MessageBoxManager.GetMessageBoxStandard("Ошибка", "Данные не удалены", ButtonEnum.Ok).ShowAsync();
                    ;
                }
            }

            connection.Close();
        }
    }

    public static void RemoveDiseaseRecord(DiseaseRecord data)
    {
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM DiseaseRecord  WHERE ID = @ID;";
                command.Parameters.AddWithValue("@ID", data.Id);
                var rowsCount = command.ExecuteNonQuery();
                if (rowsCount == 0)
                {
                    MessageBoxManager.GetMessageBoxStandard("Ошибка", "Данные не удалены", ButtonEnum.Ok).ShowAsync();
                    ;
                }
            }

            connection.Close();
        }
    }

    public static void RemoveDoctor(Doctor data)
    {
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Doctor  WHERE ID = @ID;";
                command.Parameters.AddWithValue("@ID", data.Id);
                var rowsCount = command.ExecuteNonQuery();
                if (rowsCount == 0)
                {
                    MessageBoxManager.GetMessageBoxStandard("Ошибка", "Данные не удалены", ButtonEnum.Ok).ShowAsync();
                    ;
                }
            }

            connection.Close();
        }
    }

    public static void RemovePatirnt(Patient data)
    {
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Patient WHERE ID = @ID;";
                command.Parameters.AddWithValue("@ID", data.Id);
                var rowsCount = command.ExecuteNonQuery();
                if (rowsCount == 0)
                {
                    MessageBoxManager.GetMessageBoxStandard("Ошибка", "Данные не удалены", ButtonEnum.Ok).ShowAsync();
                    ;
                }
            }

            connection.Close();
        }
    }

    public static void RemovePosition(Position data)
    {
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Position WHERE ID = @ID;";
                command.Parameters.AddWithValue("@ID", data.Id);
                var rowsCount = command.ExecuteNonQuery();
                if (rowsCount == 0)
                {
                    MessageBoxManager.GetMessageBoxStandard("Ошибка", "Данные не удалены", ButtonEnum.Ok).ShowAsync();
                    ;
                }
            }

            connection.Close();
        }
    }

    public static void RemoveProcedure(Procedure data)
    {
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM procedurepatient WHERE ID = @ID;";
                command.Parameters.AddWithValue("@ID", data.Id);
                var rowsCount = command.ExecuteNonQuery();
                if (rowsCount == 0)
                {
                    MessageBoxManager.GetMessageBoxStandard("Ошибка", "Данные не удалены", ButtonEnum.Ok).ShowAsync();
                    ;
                }
            }

            connection.Close();
        }
    }

    public static void RemoveReceipt(Receipt data)
    {
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Receipt WHERE ID = @ID;";
                command.Parameters.AddWithValue("@ID", data.Id);
                var rowsCount = command.ExecuteNonQuery();
                if (rowsCount == 0)
                {
                    MessageBoxManager.GetMessageBoxStandard("Ошибка", "Данные не удалены", ButtonEnum.Ok).ShowAsync();
                    ;
                }
            }

            connection.Close();
        }
    }

    public static void RemoveStatus(Status data)
    {
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Status WHERE ID = @ID;";
                command.Parameters.AddWithValue("@ID", data.Id);
                var rowsCount = command.ExecuteNonQuery();
                if (rowsCount == 0)
                {
                    MessageBoxManager.GetMessageBoxStandard("Ошибка", "Данные не удалены", ButtonEnum.Ok).ShowAsync();
                    ;
                }
            }

            connection.Close();
        }
    }

    /// Обновление
    public static void UpdatePosition(Position data)
    {
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE Position SET Name = @Name WHERE ID = @ID;";
                command.Parameters.AddWithValue("@ID", data.Id);
                command.Parameters.AddWithValue("@Name", data.Name);
                var rowsCount = command.ExecuteNonQuery();
                if (rowsCount == 0)
                {
                    MessageBoxManager.GetMessageBoxStandard("Ошибка", "Данные не Обновлены", ButtonEnum.Ok).ShowAsync();
                    ;
                }
            }

            connection.Close();
        }
    }

    public static void UpdateStatus(Status data)
    {
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE Status SET Name = @Name WHERE ID = @ID;";
                command.Parameters.AddWithValue("@ID", data.Id);
                command.Parameters.AddWithValue("@Name", data.Name);
                var rowsCount = command.ExecuteNonQuery();
                if (rowsCount == 0)
                {
                    MessageBoxManager.GetMessageBoxStandard("Ошибка", "Данные не Обновлены", ButtonEnum.Ok).ShowAsync();
                    ;
                }
            }

            connection.Close();
        }
    }

    public static void UpdateDisease(Disease data)
    {
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "UPDATE Disease SET Name = @Name, DurationLiness = @DurationLiness WHERE ID = @ID;";
                command.Parameters.AddWithValue("@ID", data.Id);
                command.Parameters.AddWithValue("@Name", data.Name);
                command.Parameters.AddWithValue("@DurationLiness", data.DurationLiness);
                var rowsCount = command.ExecuteNonQuery();
                if (rowsCount == 0)
                {
                    MessageBoxManager.GetMessageBoxStandard("Ошибка", "Данные не Обновлены", ButtonEnum.Ok).ShowAsync();
                    ;
                }
            }

            connection.Close();
        }
    }

    public static void UpdatePatient(Patient data)
    {
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "UPDATE Patient SET FirstName = @FirstName, LastName = @LastName, Patronymic = @Patronymic, " +
                    "DateBirth = @DateBirth, PhoneNumber = @PhoneNumber WHERE ID = @ID;";
                command.Parameters.AddWithValue("@ID", data.Id);
                command.Parameters.AddWithValue("@FirstName", data.FirstName);
                command.Parameters.AddWithValue("@LastName", data.LastName);
                command.Parameters.AddWithValue("@Patronymic", data.Patronymic);
                command.Parameters.AddWithValue("@DateBirth", data.DateBirth);
                command.Parameters.AddWithValue("@PhoneNumber", data.PhoneNumber);
                var rowsCount = command.ExecuteNonQuery();
                if (rowsCount == 0)
                {
                    MessageBoxManager.GetMessageBoxStandard("Ошибка", "Данные не Обновлены", ButtonEnum.Ok).ShowAsync();
                    ;
                }
            }

            connection.Close();
        }
    }

    public static void UpdateDoctor(Doctor data)
    {
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "UPDATE Doctor SET FirstName = @FirstName, LastName = @LastName, Patronymic = @Patronymic, " +
                    "EmploymentDate = @EmploymentDate, PositionID = @PositionID, Login = @Login, Password = @Password " +
                    "WHERE ID = @ID;";
                command.Parameters.AddWithValue("@ID", data.Id);
                command.Parameters.AddWithValue("@FirstName", data.FirstName);
                command.Parameters.AddWithValue("@LastName", data.LastName);
                command.Parameters.AddWithValue("@Patronymic", data.Patronymic);
                command.Parameters.AddWithValue("@EmploymentDate", data.EmploymentDate);
                command.Parameters.AddWithValue("@PositionID", data.PositionID);
                command.Parameters.AddWithValue("@Login", data.Login);
                command.Parameters.AddWithValue("@Password", data.Password);
                var rowsCount = command.ExecuteNonQuery();
                if (rowsCount == 0)
                {
                    MessageBoxManager.GetMessageBoxStandard("Ошибка", "Данные не Обновлены", ButtonEnum.Ok).ShowAsync();
                    ;
                }
            }

            connection.Close();
        }
    }

    public static void UpdateDiseaseRecord(DiseaseRecord data)
    {
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "UPDATE DiseaseRecord SET PatientID = @PatientID, FinalPrice = @FinalPrice, DateStart = @DateStart, " +
                    "DateEnd = @DateEnd, StatusID = @StatusID, AttendingDoctorID = @AttendingDoctorID, DiseaseID = @DiseaseID " +
                    " WHERE ID = @ID;";
                command.Parameters.AddWithValue("@ID", data.Id);
                command.Parameters.AddWithValue("@PatientID", data.PatientID);
                command.Parameters.AddWithValue("@FinalPrice", data.FinalPrice);
                command.Parameters.AddWithValue("@DateStart", data.DateStart);
                command.Parameters.AddWithValue("@DateEnd", data.DateEnd);
                command.Parameters.AddWithValue("@StatusID", data.StatusID);
                command.Parameters.AddWithValue("@AttendingDoctorID", data.AttendingDoctorID);
                command.Parameters.AddWithValue("@DiseaseID", data.DiseaseID);
                var rowsCount = command.ExecuteNonQuery();
                if (rowsCount == 0)
                {
                    MessageBoxManager.GetMessageBoxStandard("Ошибка", "Данные не Обновлены", ButtonEnum.Ok).ShowAsync();
                    ;
                }
            }

            connection.Close();
        }
    }

    public static void UpdateProcedure(Procedure data)
    {
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "UPDATE procedurepatient SET DoctorID = @DoctorID, DiseaseRecordID = @DiseaseRecordID, Description = @Description, " +
                    "DateStart = @DateStart, Duration = @Duration, Cost = @Cost, StatusID = @StatusID WHERE ID = @ID;";
                command.Parameters.AddWithValue("@ID", data.Id);
                command.Parameters.AddWithValue("@DoctorID", data.DoctorID);
                command.Parameters.AddWithValue("@DiseaseRecordID", data.DiseaseRecordID);
                command.Parameters.AddWithValue("@Description", data.Description);
                command.Parameters.AddWithValue("@DateStart", data.DateStart);
                command.Parameters.AddWithValue("@Duration", data.Duration);
                command.Parameters.AddWithValue("@Cost", data.Cost);
                command.Parameters.AddWithValue("@StatusID", data.StatusID);
                var rowsCount = command.ExecuteNonQuery();
                if (rowsCount == 0)
                {
                    MessageBoxManager.GetMessageBoxStandard("Ошибка", "Данные не Обновлены", ButtonEnum.Ok).ShowAsync();
                    ;
                }
            }

            connection.Close();
        }
    }

    public static void UpdateReceipt(Receipt data)
    {
        using (var connection = new MySqlConnection(ConnectionString.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "UPDATE Receipt SET Amount = @Amount, DiseaseRecrodID = @DiseaseRecrodID, DatePayment = @DatePayment " +
                    "WHERE ID = @ID;";
                command.Parameters.AddWithValue("@ID", data.Id);
                command.Parameters.AddWithValue("@Amount", data.Amount);
                command.Parameters.AddWithValue("@DiseaseRecrodID", data.DiseaseRecordID);
                command.Parameters.AddWithValue("@DatePayment", data.DatePayment);
                var rowsCount = command.ExecuteNonQuery();
                if (rowsCount == 0)
                {
                    MessageBoxManager.GetMessageBoxStandard("Ошибка", "Данные не Обновлены", ButtonEnum.Ok).ShowAsync();
                    ;
                }
            }

            connection.Close();
        }
    }
}