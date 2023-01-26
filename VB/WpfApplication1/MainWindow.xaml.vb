Imports System.Windows

Namespace WpfApplication1

    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            DataContext = New ViewModel()
            Me.InitializeComponent()
        End Sub
    End Class
End Namespace
