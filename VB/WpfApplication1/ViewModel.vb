Imports System.Collections

Namespace WpfApplication1

    Public Class ViewModel

        Private ReadOnly sourceField As IList = DataSource

        Public ReadOnly Property Source As IList
            Get
                Return sourceField
            End Get
        End Property

        Public Sub New()
        End Sub
    End Class
End Namespace
