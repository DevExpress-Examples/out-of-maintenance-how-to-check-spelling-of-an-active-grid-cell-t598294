using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using DevExpress.Utils;
using DevExpress.Utils.Zip;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.SpellChecker;
using DevExpress.XtraRichEdit;
using DevExpress.XtraSpellChecker;

namespace WpfApplication1
{
    public class Employees : System.ComponentModel.INotifyPropertyChanged {
        public int EmployeeID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Title { get; set; }
        public string TitleOfCourtesy { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime HireDate { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string HomePhone { get; set; }
        public string Extension { get; set; }
        public double Salary { get; set; }
        public bool OnVacation { get; set; }
        public byte[] Photo { get; set; }
        public string Notes { get; set; }
        public int ReportsTo { get; set; }

        #region INotifyPropertyChanged Members
        System.ComponentModel.PropertyChangedEventHandler onPropertyChanged;
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged { add { onPropertyChanged += value; } remove { onPropertyChanged -= value; } }
        void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler handler = onPropertyChanged;
            if (handler != null)
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public static class EmployeesData {
        static IList dataSource;

        public static IList DataSource {
            get {
                if (dataSource == null) {
                    dataSource = GetDataSource();
                    DoMistakes(dataSource);
                }
                return dataSource;
            }
        }
        static IList GetDataSource() 
        {
            XmlSerializer s = new XmlSerializer(typeof(List<Employees>), new XmlRootAttribute("NewDataSet"));
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WpfApplication1.nwind.xml");
            return (IList)s.Deserialize(stream);
        }

        static void DoMistakes(IList dataSet) {
            foreach (Employees employee in dataSet) {
                StringBuilder text = new StringBuilder(employee.Notes);
                List<char> charSet = CreateCharSet(text);
                Random random = new Random(Environment.TickCount);
                for (int i = text.Length - 1; i >= 0; i -= 30) {
                    if (!Char.IsLetter(text[i]))
                        continue;
                    char ch = GetRandomChar(charSet);
                    if (Char.IsUpper(text[i]))
                        ch = Char.ToUpper(ch);
                    if (text[i] == ch)
                        text.Remove(i, 1);
                    else
                        text[i] = ch;
                }
                employee.Notes = text.ToString();
            }
        }
        static List<char> CreateCharSet(StringBuilder text) {
            List<char> result = new List<char>();
            int length = text.Length;
            for (int i = 0; i < length; i++) {
                char ch = text[i];
                if (!Char.IsLetter(ch))
                    continue;
                ch = Char.ToLower(ch);
                int index = result.BinarySearch(ch);
                if (index < 0)
                    result.Insert(~index, ch);
            }
            return result;
        }
        static char GetRandomChar(List<char> charSet) {
            Random random = new Random(Environment.TickCount);
            int index = random.Next(0, charSet.Count - 1);
            return charSet[index];
        }
    }
    public class BitmapToBitmapSourceConverter : IValueConverter {
        #region IValueConverter Members
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return GetImageSource((byte[])value);
        }
        public static ImageSource GetImageSource(byte[] bytes) {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            try {
                bi.StreamSource = new MemoryStream(bytes);
            }
            finally {
                bi.EndInit();
            }
            return bi;
        }
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
        #endregion
    }
    public class EmployeeToAddressStringConverter : IValueConverter {
        #region IValueConverter Members
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            Employees employee = value as Employees;
            if (employee == null || typeof(string) != targetType)
                return null;
            return String.Format("{0}, {1}, {2}", employee.Country, employee.City, employee.Address);
        }
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
        #endregion
    }
}
