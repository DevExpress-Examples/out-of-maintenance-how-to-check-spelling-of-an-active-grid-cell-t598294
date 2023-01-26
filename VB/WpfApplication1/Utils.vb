Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Globalization
Imports System.IO
Imports System.Reflection
Imports System.Text
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Xml.Serialization

Namespace WpfApplication1

    Public Class Employees
        Implements System.ComponentModel.INotifyPropertyChanged

        Public Property EmployeeID As Integer

        Public Property LastName As String

        Public Property FirstName As String

        Public Property Title As String

        Public Property TitleOfCourtesy As String

        Public Property BirthDate As Date

        Public Property HireDate As Date

        Public Property Address As String

        Public Property City As String

        Public Property Region As String

        Public Property PostalCode As String

        Public Property Country As String

        Public Property HomePhone As String

        Public Property Extension As String

        Public Property Salary As Double

        Public Property OnVacation As Boolean

        Public Property Photo As Byte()

        Public Property Notes As String

        Public Property ReportsTo As Integer

'#Region "INotifyPropertyChanged Members"
        Private onPropertyChanged As System.ComponentModel.PropertyChangedEventHandler

        Public Custom Event PropertyChanged As System.ComponentModel.PropertyChangedEventHandler Implements ComponentModel.INotifyPropertyChanged.PropertyChanged
            AddHandler(ByVal value As System.ComponentModel.PropertyChangedEventHandler)
                onPropertyChanged = [Delegate].Combine(onPropertyChanged, value)
            End AddHandler

            RemoveHandler(ByVal value As System.ComponentModel.PropertyChangedEventHandler)
                onPropertyChanged = [Delegate].Remove(onPropertyChanged, value)
            End RemoveHandler

            RaiseEvent(ByVal sender As Object, ByVal e As ComponentModel.PropertyChangedEventArgs)
                If onPropertyChanged IsNot Nothing Then
                    onPropertyChanged(sender, e)
                End If
            End RaiseEvent
        End Event

        Private Sub RaisePropertyChanged(ByVal propertyName As String)
            Dim handler As System.ComponentModel.PropertyChangedEventHandler = onPropertyChanged
            If handler IsNot Nothing Then handler(Me, New System.ComponentModel.PropertyChangedEventArgs(propertyName))
        End Sub
'#End Region
    End Class

    Public Module EmployeesData

        Private dataSourceField As IList

        Public ReadOnly Property DataSource As IList
            Get
                If dataSourceField Is Nothing Then
                    dataSourceField = GetDataSource()
                    DoMistakes(dataSourceField)
                End If

                Return dataSourceField
            End Get
        End Property

        Private Function GetDataSource() As IList
            Dim s As XmlSerializer = New XmlSerializer(GetType(List(Of Employees)), New XmlRootAttribute("NewDataSet"))
            Dim stream As Stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WpfApplication1.nwind.xml")
            Return CType(s.Deserialize(stream), IList)
        End Function

        Private Sub DoMistakes(ByVal dataSet As IList)
            For Each employee As Employees In dataSet
                Dim text As StringBuilder = New StringBuilder(employee.Notes)
                Dim charSet As List(Of Char) = CreateCharSet(text)
                Dim random As Random = New Random(Environment.TickCount)
                For i As Integer = text.Length - 1 To 0 Step -30
                    If Not Char.IsLetter(text(i)) Then Continue For
                    Dim ch As Char = GetRandomChar(charSet)
                    If Char.IsUpper(text(i)) Then ch = Char.ToUpper(ch)
                    If text(i) = ch Then
                        text.Remove(i, 1)
                    Else
                        text(i) = ch
                    End If
                Next

                employee.Notes = text.ToString()
            Next
        End Sub

        Private Function CreateCharSet(ByVal text As StringBuilder) As List(Of Char)
            Dim result As List(Of Char) = New List(Of Char)()
            Dim length As Integer = text.Length
            For i As Integer = 0 To length - 1
                Dim ch As Char = text(i)
                If Not Char.IsLetter(ch) Then Continue For
                ch = Char.ToLower(ch)
                Dim index As Integer = result.BinarySearch(ch)
                If index < 0 Then result.Insert(Not index, ch)
            Next

            Return result
        End Function

        Private Function GetRandomChar(ByVal charSet As List(Of Char)) As Char
            Dim random As Random = New Random(Environment.TickCount)
            Dim index As Integer = random.Next(0, charSet.Count - 1)
            Return charSet(index)
        End Function
    End Module

    Public Class BitmapToBitmapSourceConverter
        Implements IValueConverter

'#Region "IValueConverter Members"
        Private Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As CultureInfo) As Object Implements IValueConverter.Convert
            Return GetImageSource(CType(value, Byte()))
        End Function

        Public Shared Function GetImageSource(ByVal bytes As Byte()) As ImageSource
            Dim bi As BitmapImage = New BitmapImage()
            bi.BeginInit()
            Try
                bi.StreamSource = New MemoryStream(bytes)
            Finally
                bi.EndInit()
            End Try

            Return bi
        End Function

        Private Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Throw New NotImplementedException()
        End Function
'#End Region
    End Class

    Public Class EmployeeToAddressStringConverter
        Implements IValueConverter

'#Region "IValueConverter Members"
        Private Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As CultureInfo) As Object Implements IValueConverter.Convert
            Dim employee As Employees = TryCast(value, Employees)
            If employee Is Nothing OrElse GetType(String) IsNot targetType Then Return Nothing
            Return String.Format("{0}, {1}, {2}", employee.Country, employee.City, employee.Address)
        End Function

        Private Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Throw New NotImplementedException()
        End Function
'#End Region
    End Class
End Namespace
