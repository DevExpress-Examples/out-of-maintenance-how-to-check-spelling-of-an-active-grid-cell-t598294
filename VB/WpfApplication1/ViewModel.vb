Imports System
Imports System.Collections
Imports System.Linq

Namespace WpfApplication1
	Public Class ViewModel
'INSTANT VB NOTE: The field source was renamed since Visual Basic does not allow fields to have the same name as other class members:
		Private ReadOnly source_Conflict As IList = EmployeesData.DataSource
		Public ReadOnly Property Source() As IList
			Get
				Return Me.source_Conflict
			End Get
		End Property

		Public Sub New()
		End Sub

	End Class
End Namespace
