Imports System
Imports System.Collections
Imports System.Linq

Namespace WpfApplication1
    Public Class ViewModel

        Private ReadOnly source_Renamed As IList = EmployeesData.DataSource
        Public ReadOnly Property Source() As IList
            Get
                Return Me.source_Renamed
            End Get
        End Property

        Public Sub New()
        End Sub

    End Class
End Namespace
